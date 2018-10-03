using Artesian.SDK.Service;

namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ActualTimeSerieException : ArtesianSdkClientException
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