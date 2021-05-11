using System;
using System.Collections.Generic;
using System.Linq;

namespace Loggo.Api
{
	public class LogSource
	{
		public IReadOnlyList<String> Names { get; }

		public LogSource(String name)
			: this(new[] {name})
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name), $"'{nameof(name)}' is not allowed to be null.");
		}

		public LogSource(IEnumerable<String> names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names), $"'{nameof(names)}' is not allowed to be null.");
			Names = names.ToList();
		}

		public LogSource Plus(String name) =>
			new LogSource(Names.Concat(new[] {name}));

		public LogSource Plus(IEnumerable<String> names) =>
			new LogSource(Names.Concat(names));

		public LogSource Plus(LogSource source) =>
			new LogSource(Names.Concat(source.Names));

		public override String ToString() =>
			"[" + String.Join(", ", Names) + "]";

		// ReSharper disable MemberCanBePrivate.Global
		// ReSharper disable BuiltInTypeReferenceStyle
		// ReSharper disable ConvertIfStatementToReturnStatement
		// ReSharper disable ArrangeThisQualifier
		protected bool Equals(LogSource other)
		{
			return Names.SequenceEqual(other.Names);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((LogSource) obj);
		}

		public static bool operator ==(LogSource left, LogSource right) =>
			left?.Equals(right) ?? ReferenceEquals(null, right);

		public static bool operator !=(LogSource left, LogSource right) =>
			!left?.Equals(right) ?? !ReferenceEquals(null, right);

		public override int GetHashCode()
		{
			return Names != null ? Names.GetHashCode() : 0;
		}
		// ReSharper restore ArrangeThisQualifier
		// ReSharper restore ConvertIfStatementToReturnStatement
		// ReSharper restore BuiltInTypeReferenceStyle
		// ReSharper restore MemberCanBePrivate.Global
	}
}
