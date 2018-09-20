using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class AuthGroup
    {

        [Key("ID")]
        public int ID { get; set; }
        [Key("ETag")]
        public string ETag { get; set; }
        [Key("Name")]
        public string Name { get; set; }
        [Key("Users")]
        public List<string> Users { get; set; }
    }
}
