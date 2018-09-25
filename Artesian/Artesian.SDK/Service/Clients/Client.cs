// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Flurl.Http;
using JWT.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Polly;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    internal sealed class Client : IDisposable
    {
        private readonly MediaTypeFormatterCollection _formatters;

        private readonly AuthenticationApiClient _auth0;
        private readonly ClientCredentialsTokenRequest _credentials;
        private readonly IFlurlClient _client;

        private readonly JsonMediaTypeFormatter _jsonFormatter;
        private readonly MessagePackFormatter _msgPackFormatter;
        private readonly LZ4MessagePackFormatter _lz4msgPackFormatter;

        private readonly object _gate = new { };

        private readonly string _url;

        private readonly Polly.Caching.Memory.MemoryCacheProvider _memoryCacheProvider
           = new Polly.Caching.Memory.MemoryCacheProvider(new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()));

        private readonly Policy<(string AccessToken, DateTimeOffset ExpiresOn)> _cachePolicy;
        /// <summary>
        /// IArtesianServiceConfig Config
        /// </summary>
        public IArtesianServiceConfig Config { get; private set; }
        /// <summary>
        /// Client constructor Auth credentials / ApiKey can be passed through config
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="Url">string</param>
        public Client(IArtesianServiceConfig config, string Url)
        {
            this.Config = config;
            this._url = Url;

            var cfg = new JsonSerializerSettings();
            cfg = cfg.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            cfg = cfg.ConfigureForDictionary();
            cfg = cfg.ConfigureForNodaTimeRanges();
            cfg.Formatting = Formatting.Indented;
            cfg.ContractResolver = new DefaultContractResolver();
            cfg.Converters.Add(new StringEnumConverter());
            cfg.TypeNameHandling = TypeNameHandling.None;
            cfg.ObjectCreationHandling = ObjectCreationHandling.Replace;

            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = cfg
            };
            _jsonFormatter = jsonFormatter;

            _msgPackFormatter = new MessagePackFormatter(CustomCompositeResolver.Instance);
            _lz4msgPackFormatter = new LZ4MessagePackFormatter(CustomCompositeResolver.Instance);
            //Order of formatters important for correct weight in accept header
            var formatters = new MediaTypeFormatterCollection();
            formatters.Clear();
            formatters.Add(_lz4msgPackFormatter);
            formatters.Add(_msgPackFormatter);
            formatters.Add(_jsonFormatter);
            _formatters = formatters;

            if (config.ApiKey==null) {
                _auth0 = new AuthenticationApiClient($"{Config.Domain}");
                _credentials = new ClientCredentialsTokenRequest()
                {
                    Audience = Config.Audience,
                    ClientId = Config.ClientId,
                    ClientSecret = Config.ClientSecret,
                };

                _cachePolicy = Policy.CacheAsync(_memoryCacheProvider.AsyncFor<(string AccessToken, DateTimeOffset ExpiresOn)>(), new ResultTtl<(string AccessToken, DateTimeOffset ExpiresOn)>(r => new Ttl(r.ExpiresOn - DateTimeOffset.Now, false)));
            }

            _client = new FlurlClient(_url);
            _client.WithTimeout(TimeSpan.FromMinutes(ArtesianConstants.ServiceRequestTimeOutMinutes));

        }

        public async Task<TResult> Exec<TResult, TBody>(HttpMethod method, string resource, TBody body = default, CancellationToken ctk = default)
        {
            try
            {
                var req = _client.Request(resource).WithAcceptHeader(_formatters).AllowAnyHttpStatus();

                if (Config.ApiKey != null)
                    req = req.WithHeader("X-Api-Key", Config.ApiKey);
                else
                {
                    var (token, _) = await _getAccessToken();
                    req = req.WithOAuthBearerToken(token);
                }

                ObjectContent content = null;

                try
                {
                    if (body != null)
                        content = new ObjectContent(typeof(TBody), body, _lz4msgPackFormatter);
                
                    using (var res = await req.SendAsync(method, content: content, cancellationToken: ctk))
                    {
                        if (res.StatusCode == HttpStatusCode.NoContent || res.StatusCode == HttpStatusCode.NotFound)
                            return default;

                        if (!res.IsSuccessStatusCode)
                        {
                            var responseText = await res.Content.ReadAsStringAsync();

                            throw new ArtesianSdkRemoteException("Failed handling REST call to WebInterface {0} {1}. Returned status: {2}. Content: \n{3}", method, Config.BaseAddress + _url + resource, res.StatusCode, responseText);
                        }

                        return await res.Content.ReadAsAsync<TResult>(_formatters, ctk);
                    }
                }
                finally
                {
                    content?.Dispose();
                }
            }
            catch (ArtesianSdkRemoteException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ArtesianSdkClientException($"Failed handling REST call to WebInterface: {method} " + Config.BaseAddress + _url + resource, e);
            }
        }

        public async Task Exec(HttpMethod method, string resource, CancellationToken ctk = default)
            => await Exec<object, object>(method, resource, null, ctk);

        public Task<TResult> Exec<TResult>(HttpMethod method, string resource, CancellationToken ctk = default)
            => Exec<TResult, object>(method, resource, null, ctk);

        public async Task Exec<TBody>(HttpMethod method, string resource, TBody body, CancellationToken ctk = default)
            => await Exec<object, TBody>(method, resource, body, ctk);

        #region private methods
        private async Task<(string AccessToken, DateTimeOffset ExpiresOn)> _getAccessToken()
        {
            var res = await _cachePolicy.ExecuteAsync(async (ctx) =>
            {
                var result = await _auth0.GetTokenAsync(_credentials);
                var decode = new JwtBuilder()
                    .DoNotVerifySignature()
                    .Decode<IDictionary<string, object>>(result.AccessToken);

                var exp = (long)decode["exp"];

                return (result.AccessToken, DateTimeOffset.FromUnixTimeSeconds(exp) - TimeSpan.FromMinutes(2));

            }, new Context("_getAccessToken"));

            return res;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        #endregion private methods
    }
    /// <summary>
    /// Flurl Extension
    /// </summary>
    public static class FlurlExt
    {
        /// <summary>
        /// Flurl request extension to return Accept headers
        /// </summary>
        /// <param name="request">IFlurlRequest</param>
        /// <param name="formatters">MediaTypeFormatterCollection</param>
        /// <returns></returns>
        public static IFlurlRequest WithAcceptHeader(this IFlurlRequest request, MediaTypeFormatterCollection formatters)
        {
            var cnt = formatters.Count;
            var step = 1.0 / (cnt + 1);
            var sb = new StringBuilder();
            var headers = formatters.Select((x, i) => new MediaTypeWithQualityHeaderValue(x.SupportedMediaTypes.First().MediaType, 1 - (step * i)));

            return request.WithHeader("Accept", string.Join(",", headers));
        }
    }
}
