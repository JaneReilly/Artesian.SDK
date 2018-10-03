using Artesian.SDK.Service;

namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MarketAssessmentException : ArtesianSdkClientException
    {
        public MarketAssessmentException(string message)
            : base(message)
        {
        }

        public MarketAssessmentException(string format, params object[] args)
            : base(format,args)
        {
        }
    }
}