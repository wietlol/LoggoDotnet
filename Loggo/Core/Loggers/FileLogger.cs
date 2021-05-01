using System;
using System.Collections.Generic;
using System.IO;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class FileLogger : ILogger<String>
	{
		public Func<FileInfo> FileProvider { get; }

		public FileLogger(Func<FileInfo> fileProvider)
		{
			FileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider), $"'{nameof(fileProvider)}' is not allowed to be null.");
		}

		public void Log(String log)
		{
			FileInfo file = FileProvider();
			using StreamWriter streamWriter = file.AppendText();
			streamWriter.WriteLine(log);
		}

		public void LogAll(IReadOnlyCollection<String> logs)
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
