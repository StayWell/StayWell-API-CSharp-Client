namespace StayWell.ServiceDefinitions.Common.Objects
{
	public class ItemReference
	{
		public string IdOrSlug { get; set; }
		public string BucketIdOrSlug { get; set; }

		public ItemReference() { }

		public ItemReference(string bucketIdOrSlug, string idOrSlug)
		{
			BucketIdOrSlug = bucketIdOrSlug;
			IdOrSlug = idOrSlug;
		}

		protected bool Equals(ItemReference other)
		{
			return string.Equals(IdOrSlug, other.IdOrSlug) && string.Equals(BucketIdOrSlug, other.BucketIdOrSlug);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((ItemReference)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((IdOrSlug != null ? IdOrSlug.GetHashCode() : 0) * 397) ^ (BucketIdOrSlug != null ? BucketIdOrSlug.GetHashCode() : 0);
			}
		}

		public static bool operator ==(ItemReference left, ItemReference right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ItemReference left, ItemReference right)
		{
			return !Equals(left, right);
		}
	}
}
