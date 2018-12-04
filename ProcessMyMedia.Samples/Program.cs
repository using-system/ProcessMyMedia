﻿namespace ProcessMyMedia.Samples
{
    using System.IO;

    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.override.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            new Samples.IngestFromDirectory(configuration).Execute();
        }
    }
}
