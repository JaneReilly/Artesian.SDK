namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MarketAssessmentException : ArtesianFactoryException
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