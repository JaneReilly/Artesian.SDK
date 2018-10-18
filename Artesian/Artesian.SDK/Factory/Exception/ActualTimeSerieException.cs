namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ActualTimeSerieException : ArtesianFactoryException
    {
        public ActualTimeSerieException(string message)
            : base(message)
        {
        }

        public ActualTimeSerieException(string format, params object[] args)
            : base(format,args)
        {
        }
    }
}