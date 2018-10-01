using Artesian.SDK.Service;

namespace Artesian.SDK.Factory
{
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