namespace Artesian.SDK.Factory
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AuctionTimeSerieException : ArtesianFactoryException
    {
        public AuctionTimeSerieException(string message)
            : base(message)
        {
        }

        public AuctionTimeSerieException(string format, params object[] args)
            : base(format,args)
        {
        }
    }
}