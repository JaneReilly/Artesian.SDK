using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Artesian.SDK.Service
{
    public class ArtesianPolicyConfig
    {
        public static AsyncCircuitBreakerPolicy _circuitBreakerPolicy { get; set; }

        public static AsyncRetryPolicy _retryPolicy { get; set; }

        public static AsyncPolicyWrap _resilienceStrategy { get; set; }

        public static AsyncBulkheadPolicy _bulkheadPolicy { get; set; }

        public const int MaxParallelism = 10;
        public const int MaxQueuingActions = 15;
        public const int MaxExceptions = 2;
        public const int RetryWaitTime = 200;
        public const int RetryCount = 3;
        public const int DurationOfBreak = 3;

        public  ArtesianPolicyConfig()
        {
            RetryPolicyConfig();
            CircuitBreakerPolicyConfig();
            BulkheadPolicyConfig();
            ResillianceStrategy();
        }


        public void RetryPolicyConfig(int retryCount = RetryCount, int retryWaitTime = RetryWaitTime)
        {
            _retryPolicy = Policy
                .Handle<AggregateException>(x =>
                {
                    var result = x.InnerException is HttpRequestException;
                    return result;
                })
                .WaitAndRetryAsync(retryCount, retryAttempt =>
                    TimeSpan.FromMilliseconds(Math.Pow(retryWaitTime,
                    retryAttempt)
                 ));
        }

        public void CircuitBreakerPolicyConfig(int maxExceptions = MaxExceptions, int durationOfBreak = DurationOfBreak)
        {
            _circuitBreakerPolicy = Policy
                .Handle<AggregateException>(x =>
                {
                    var result = x.InnerException is HttpRequestException;
                    return result;
                })
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: maxExceptions,
                    durationOfBreak: TimeSpan.FromSeconds(durationOfBreak)
                );
        }

        public void BulkheadPolicyConfig(int maxParallelism = MaxParallelism, int maxQueuingActions = MaxQueuingActions)
        {
            _bulkheadPolicy = Policy
                .BulkheadAsync(
                maxParallelism, 
                maxQueuingActions
                );
        }

        public AsyncPolicyWrap ResillianceStrategy()
        {
            _resilienceStrategy = _circuitBreakerPolicy.WrapAsync(_retryPolicy.WrapAsync(_bulkheadPolicy));

            return _resilienceStrategy;
        }

    }
}
