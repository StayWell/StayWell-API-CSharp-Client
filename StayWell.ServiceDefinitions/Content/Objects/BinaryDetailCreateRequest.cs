using System;
using StayWell.ServiceDefinitions.Buckets.Objects;
using StayWell.ServiceDefinitions.Content.Interfaces;

namespace StayWell.ServiceDefinitions.Content.Objects
{

	// not exposed via the api
	public class BinaryDetailCreateRequest : BinaryDetailRequest, IContentCreateRequest
	{
		public Guid Id { get; set; }
		public FormatType Format { get; set; }

		// set in code instead?
		public FileStatus Status { get; set; }

		public string MasterId { get; set; }

		public string LanguageCode { get; set; }

		// need to fix the updatedetails paradigm so this can live here
		//public string Slug { get; set; }
	}
}
