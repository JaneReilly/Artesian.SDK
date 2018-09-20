using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    public static class AuthorizationPath
    {
        [MessagePackObject]
        public class Input
        {
            [Key(0)]
            public string Path { get; set; }
            [Key(1)]
            public IEnumerable<AuthorizationPrincipalRole> Roles { get; set; }
        }

        [MessagePackObject]
        public class Output : Input
        {
        }
    }

    public class AclPath
    {
        public string Path { get; set; }
        public string ETag { get; set; }
        public List<AclPathRule> Rules { get; set; } = new List<AclPathRule>();
    }

    public class AclPathRule
    { 
        public Principal Principal { get; set; }
        public string Role { get; set; }
    }
}

