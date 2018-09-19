using MessagePack;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class CustomFilter
    {
        [MessagePack.Key(0)]
        public int Id { get; set; }

        [MessagePack.Key(1)]
        public string Name { get; set; }

        [MessagePack.Key(2)]
        public string SearchText { get; set; }

        [MessagePack.Key(3)]
        public Dictionary<string, List<string>> Filters { get; set; }

        [MessagePack.Key(4)]
        public string ETag { get; set; }
    }

    public static class CustomFilterExt
    {
        public static void Validate(this CustomFilter customfilter)
        {
            if (String.IsNullOrWhiteSpace(customfilter.Name))
                throw new ArgumentException("CustomFilter Name must be valorized");

            if (String.IsNullOrWhiteSpace(customfilter.SearchText) && customfilter.Filters == null)
                throw new ArgumentException("Either filter text or filter key values must be provided");
        }

    }
}
