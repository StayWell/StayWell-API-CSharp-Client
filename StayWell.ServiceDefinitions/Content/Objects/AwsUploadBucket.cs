using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StayWell.ServiceDefinitions.Content.Objects
{
    public class AwsUploadBucket
    {
        public string Bucket { get; set; }

        public string ObjectKey { get; set; }

        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string SessionToken { get; set; }

        public string SignedUrl { get; set; }

        public string ApiRequestUrl { get; set; }

        public string VideoId { get; set; }
    }
}
