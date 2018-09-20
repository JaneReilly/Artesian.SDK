using MessagePack;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Dto
{
    public static class ApiKey
    {
        [MessagePackObject]
        public class Input
        {
            [Key(0)]
            public int Id { get; set; }
            [Key(1)]
            public string ETag { get; set; }
            [Key(2)]
            public int? UsagePerDay { get; set; }
            [Key(3)]
            public Instant? ExpiresAt { get; set; }
            [Key(4)]
            public string Description { get; set; }

        }

        [MessagePackObject]
        public class Output
        {
            public Output() { }

            [Key(0)]
            public int Id { get; set; }
            [Key(1)]
            public string ETag { get; set; }
            [Key(2)]
            public int? UsagePerDay { get; set; }
            [Key(3)]
            public Instant? ExpiresAt { get; set; }
            [Key(4)]
            public string Description { get; set; }
            [Key(5)]
            public string UserId { get; set; }
            [Key(6)]
            public string Key { get; set; }
            [Key(7)]
            public Instant CreatedAt { get; set; }
        }
    }

    public static class ApiKeyExt
    {
        public static void Validate(this ApiKey.Input apiKey)
        {
            if (apiKey.Id != 0)
                throw new ArgumentException("ApiKey Id must be 0");
        }
    }
}
