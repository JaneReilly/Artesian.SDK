using System;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Factory Exception
    /// </summary>
    public class ArtesianFactoryException : Exception
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ArtesianFactoryException(string message)
            : base(message)
        {
        }

        public ArtesianFactoryException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }

        public ArtesianFactoryException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
