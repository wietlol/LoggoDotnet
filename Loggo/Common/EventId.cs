using System;

namespace Loggo.Common
{
	public struct EventId
	{
		public Int32 Id { get; }
		public String Name { get; }

		public EventId(Int32 id, String name)
		{
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name), $"'{nameof(name)}' is not allowed to be null.");
		}

		public override String ToString() =>
			$"<{Id}::{Name}>";

		// ReSharper disable MemberCanBePrivate.Global
		// ReSharper disable BuiltInTypeReferenceStyle
		// ReSharper disable ConvertIfStatementToReturnStatement
		// ReSharper disable ArrangeThisQualifier

		public bool Equals(EventId other)
		{
			return Id == other.Id && Name == other.Name;
		}

		public override bool Equals(object obj)
		{
			return obj is EventId other && Equals(other);
		}

		public static bool operator ==(EventId? left, EventId? right) =>
			left?.Equals(right) ?? ReferenceEquals(null, right);

		public static bool operator !=(EventId? left, EventId? right) =>
			!left?.Equals(right) ?? !ReferenceEquals(null, right);

		public override int GetHashCode()
		{
			unchecked
			{
				return (Id * 397) ^ (Name != null ? Name.GetHashCode() : 0);
			}
		}
		// ReSharper restore ArrangeThisQualifier
		// ReSharper restore ConvertIfStatementToReturnStatement
		// ReSharper restore BuiltInTypeReferenceStyle
		// ReSharper restore MemberCanBePrivate.Global
	}
}
