// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Client Exception
    /// </summary>
    public class ArtesianSdkClientException : Exception
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ArtesianSdkClientException(string message)
            : base(message)
        {
        }

        public ArtesianSdkClientException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }

        public ArtesianSdkClientException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
