using System;

namespace StayWell.ServiceDefinitions.Common.Objects
{
    // Needed for core logic
    // used for both client and domain users
    public class UserDetailsResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public UserType Type { get; set; }
    }
}
