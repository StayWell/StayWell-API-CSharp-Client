using System;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class ContentRatingResponse
    {
        public Guid ContentId { get; set; }
        public ContentType ContentType { get; set; }
        public Guid LicenseId { get; set; }
        public double Rating { get; set; }
        public long RatingsCount { get; set; }
        public long ViewCount { get; set; }
    }
}
