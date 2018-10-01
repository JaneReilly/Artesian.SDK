using Artesian.SDK.Service;

namespace Artesian.SDK.Factory
{
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