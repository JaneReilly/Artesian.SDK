namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class VersionedTimeSerieException : ArtesianFactoryException
    {
        public VersionedTimeSerieException(string message)
            : base(message)
        {
        }

        public VersionedTimeSerieException(string format, params object[] args)
            : base(format,args)
        {
        }
    }
}