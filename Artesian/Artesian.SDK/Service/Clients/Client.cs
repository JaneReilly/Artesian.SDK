﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Flurl;
using Flurl.Http;
using JWT.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Polly;
using Polly.Caching;
using Polly.Wrap;
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

        private readonly AsyncPolicyWrap _resilienceStrategy;

        private readonly Polly.Caching.Memory.MemoryCacheProvider _memoryCacheProvider
           = new Polly.Caching.Memory.MemoryCacheProvider(new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()));

        private readonly AsyncPolicy<(string AccessToken, DateTimeOffset ExpiresOn)> _cachePolicy;

        private readonly string _apiKey;

        /// <summary>
        /// Client constructor Auth credentials / ApiKey can be passed through config
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="Url">String</param>
        public Client(IArtesianServiceConfig config, string Url, ArtesianPolicyConfig policy)
        {
            _url = config.BaseAddress.ToString().AppendPathSegment(Url);
            _apiKey = config.ApiKey;

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

            _resilienceStrategy = policy.ResillianceStrategy();


            if (config.ApiKey == null)
            {
                _auth0 = new AuthenticationApiClient($"{config.Domain}");
                _credentials = new ClientCredentialsTokenRequest()
                {
                    Audience = config.Audience,
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
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

                if (_apiKey != null)
                    req = req.WithHeader("X-Api-Key", _apiKey);
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

                    using (var res =  await _resilienceStrategy.ExecuteAsync ( () =>  req.SendAsync(method, content: content, completionOption: HttpCompletionOption.ResponseContentRead, cancellationToken: ctk)))
                    {
                        if (res.StatusCode == HttpStatusCode.NoContent || res.StatusCode == HttpStatusCode.NotFound)
                            return default;

                        if (!res.IsSuccessStatusCode)
                        {
                            var responseText = await res.Content.ReadAsStringAsync();
                            var exceptionMessage = $"Failed handling REST call to WebInterface {method} {_url + resource}. Returned status: {res.StatusCode}. Content: \n";

                            if (res.StatusCode == HttpStatusCode.BadRequest)
                            {
                                var responseDeserialized = await res.Content.ReadAsAsync<object>(_formatters, ctk);
                                responseText = _tryDecodeText(responseDeserialized);

                                throw new ArtesianSdkValidationException(exceptionMessage + responseText);
                            }

                            if (res.StatusCode == HttpStatusCode.Conflict || res.StatusCode == HttpStatusCode.PreconditionFailed)
                                throw new ArtesianSdkOptimisticConcurrencyException(exceptionMessage + responseText);

                            if (res.StatusCode == HttpStatusCode.Forbidden)
                                throw new ArtesianSdkForbiddenException(exceptionMessage + responseText);

                            throw new ArtesianSdkRemoteException(exceptionMessage + responseText);
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
                throw new ArtesianSdkClientException($"Failed handling REST call to WebInterface: {method} " + _url + resource, e);
            }
        }

        private string _tryDecodeText(object responseDeserialized)
        {
            switch (responseDeserialized)
            {
                case Dictionary<object, object> dc:
                    {
                        if (dc.Count > 0)
                        {
                            if (dc.ContainsKey("ErrorMessage"))
                            {
                                return dc["ErrorMessage"].ToString();
                            }
                        }
                        break;
                    }
                case String st:
                    {
                        return st;
                    }
                case Int32 i:
                    {
                        return i.ToString();
                    }
                default:
                    return "Not parsed error message";
            }

            return null;
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
