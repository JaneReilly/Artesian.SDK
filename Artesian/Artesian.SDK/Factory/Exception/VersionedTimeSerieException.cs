using Artesian.SDK.Service;

namespace Artesian.SDK.Factory
{
    public class VersionedTimeSerieException : ArtesianSdkClientException
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