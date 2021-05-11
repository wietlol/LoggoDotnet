using System;
using System.Collections.Generic;
using System.IO;
using Loggo.Api;

namespace Loggo.Core.Streams
{
	public class FileOutputStream : ILogOutputStream<String>
	{
		public Func<FileInfo> FileProvider { get; }

		public FileOutputStream(Func<FileInfo> fileProvider)
		{
			FileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider), $"'{nameof(fileProvider)}' is not allowed to be null.");
		}

		public void Write(String log)
		{
			FileInfo file = FileProvider();
			using StreamWriter streamWriter = file.AppendText();
			streamWriter.WriteLine(log);
		}

		public void WriteAll(IReadOnlyCollection<String> logs)
		{
			FileInfo file = FileProvider();
			using StreamWriter streamWriter = file.AppendText();
			foreach (String log in logs)
				streamWriter.WriteLine(log);
		}

		public void Flush()
		{
			// nothing to do
		}

		public void Dispose()
		{
			// nothing to do
		}
	}
}
