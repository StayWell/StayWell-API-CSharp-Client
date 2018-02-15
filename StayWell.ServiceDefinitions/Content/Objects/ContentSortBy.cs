using System;

namespace StayWell.ServiceDefinitions.Content.Objects
{
	public enum ContentSortBy
	{
		Relevance,
		TitleAscending,
		TitleDescending,
		DatePublishedAscending,
		DatePublishedDescending,
		LastReviewedDateAscending,
		LastReviewedDateDescending,
		PostingDateAscending,
		PostingDateDescending,
		ViewCountAscending,
		ViewCountDescending,
		RatingAscending,
		RatingDescending,
	    NativeTitleAscending,
	    NativeTitleDescending,

        [Obsolete("Use Relevance")]
		Relavence, // spelling!
	}
}
