// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Net.Http;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Policy Config
    /// </summary>
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
        /// <summary>
        /// Artesian Policy Config
        /// </summary>
        public  ArtesianPolicyConfig()
        {
            RetryPolicyConfig().
            CircuitBreakerPolicyConfig().
            BulkheadPolicyConfig().
            ResillianceStrategy();
        }
        /// <summary>
        /// Wait and Retry Policy Config
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="retryWaitTime"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Circuit Breaker Policy
        /// </summary>
        /// <param name="maxExceptions"></param>
        /// <param name="durationOfBreak"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Bulkhead Policy Config
        /// </summary>
        /// <param name="maxParallelism"></param>
        /// <param name="maxQueuingActions"></param>
        /// <returns></returns>
        public ArtesianPolicyConfig BulkheadPolicyConfig(int maxParallelism = MaxParallelism, int maxQueuingActions = MaxQueuingActions)
        {
            _bulkheadPolicy = Policy
                .BulkheadAsync(
                maxParallelism, 
                maxQueuingActions
                );

            return this;
        }
        /// <summary>
        /// Policy Resiliance Strategy
        /// </summary>
        /// <returns></returns>
        public AsyncPolicy ResillianceStrategy()
        {
            return _circuitBreakerPolicy.WrapAsync(_retryPolicy.WrapAsync(_bulkheadPolicy));
        }

    }
}
