namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Add TimeSerie OperationResult enums
    /// </summary>
    public enum AddTimeSerieOperationResult
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ValueAdded = 0
      , TimeAlreadyPresent = 1
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Add Auction TimeSerie OperationResult enums
    /// </summary>
    public enum AddAuctionTimeSerieOperationResult
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ValueAdded = 0
      , TimeAlreadyPresent = 1
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Add Assessment OperationResult enums
    /// </summary>
    public enum AddAssessmentOperationResult
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        AssessmentAdded = 0
      , ProductAlreadyPresent = 1
      , IllegalReferenceDate = 2
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
