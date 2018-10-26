using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ProcessMyMedia.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.override.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            new Samples.Ingest(configuration).Execute();
            Console.ReadLine();
        }
    }
}
