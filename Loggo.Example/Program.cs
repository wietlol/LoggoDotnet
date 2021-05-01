using System;
using Loggo.Example.SystemServiceProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Loggo.Example
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			IServiceProvider serviceProvider = DependencyInjection.ServiceProvider;

			using (IServiceScope scope = serviceProvider.CreateScope())
			{
				scope.ServiceProvider
					.GetRequiredService<ExampleProgram>()
					.Main();
			}

			using (IServiceScope scope = serviceProvider.CreateScope())
			{
				scope.ServiceProvider
					.GetRequiredService<ExampleProgram>()
					.Main();
			}

			using (IServiceScope scope = serviceProvider.CreateScope())
			{
				scope.ServiceProvider
					.GetRequiredService<ExampleProgram>()
					.Main();
			}
		}
	}
}
