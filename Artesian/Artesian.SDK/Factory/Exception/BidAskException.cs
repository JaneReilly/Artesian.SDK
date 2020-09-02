﻿namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BidAskException : ArtesianFactoryException
    {
        public BidAskException(string message)
            : base(message)
        {
        }

        public BidAskException(string format, params object[] args)
            : base(format,args)
        {
        }
    }
}