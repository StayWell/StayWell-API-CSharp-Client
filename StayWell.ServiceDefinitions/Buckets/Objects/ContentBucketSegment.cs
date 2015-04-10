﻿using System;

namespace StayWell.ServiceDefinitions.Buckets.Objects
{
	public class ContentBucketSegment
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Slug { get; set; }
		public bool Required { get; set; }
	}
}
