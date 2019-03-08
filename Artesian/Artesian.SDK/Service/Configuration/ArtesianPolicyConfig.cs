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
        private  AsyncCircuitBreakerPolicy _circuitBreakerPolicy { get; set; }

        private  AsyncRetryPolicy _retryPolicy { get; set; }

        private  AsyncBulkheadPolicy _bulkheadPolicy { get; set; }

        private const int MaxParallelism = 10;
        private const int MaxQueuingActions = 15;
        private const int MaxExceptions = 2;
        private const int RetryWaitTime = 200;
        private const int RetryCount = 3;
        private const int DurationOfBreak = 3;

        public  ArtesianPolicyConfig()
        {
            RetryPolicyConfig().
            CircuitBreakerPolicyConfig().
            BulkheadPolicyConfig().
            ResillianceStrategy();
        }

        public ArtesianPolicyConfig RetryPolicyConfig(int retryCount = RetryCount, int retryWaitTime = RetryWaitTime)
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

            return this;
        }

        public ArtesianPolicyConfig CircuitBreakerPolicyConfig(int maxExceptions = MaxExceptions, int durationOfBreak = DurationOfBreak)
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

            return this;
        }

        public ArtesianPolicyConfig BulkheadPolicyConfig(int maxParallelism = MaxParallelism, int maxQueuingActions = MaxQueuingActions)
        {
            _bulkheadPolicy = Policy
                .BulkheadAsync(
                maxParallelism, 
                maxQueuingActions
                );

            return this;
        }

        public AsyncPolicy ResillianceStrategy()
        {
            return _circuitBreakerPolicy.WrapAsync(_retryPolicy.WrapAsync(_bulkheadPolicy));
        }

    }
}
