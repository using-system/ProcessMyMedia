# ProcessMyMedia
Build your Azure Media Services workflow (V3 API version) and Azure Data Factory (V2 API version) in .NET Core. 

ProcessMyMedia lib is based on [Workflow Core](https://github.com/danielgerlag/workflow-core) . Workflow Core is a light weight workflow engine targeting .NET Standard. It supports pluggable persistence and concurrency providers to allow for multi-node clusters. 

Contributors welcome !

## Installation

Install-Package ProcessMyMedia -Version 1.0.0.56

**Startup : **

```c#
public void ConfigureServices(IServiceCollection services)
{
	//Configure Azure Media Services tasks
	services.AddMediaServices(configuration: new AmsConfiguration()
	{
		ArmEndpoint = this.Configuration["MediaServices:ArmEndpoint"],
		SubscriptionId = this.Configuration["MediaServices:SubscriptionId"],
		MediaAccountName = this.Configuration["MediaServices:MediaAccountName"],
		ResourceGroup = this.Configuration["MediaServices:ResourceGroup"],
		AadTenantId = this.Configuration["MediaServices:AadTenantId"],
		AadClientId = this.Configuration["MediaServices:AadClientId"],
		AadSecret = this.Configuration["MediaServices:AadSecret"]
	});

	//Configure Azure Data Factory tasks
	services.AddDataFactoryServices(configuration: new AdfConfiguration()
	{
		ArmEndpoint = this.Configuration["DataFactory:ArmEndpoint"],
		SubscriptionId = this.Configuration["DataFactory:SubscriptionId"],
		FactoryName = this.Configuration["DataFactory:FactoryName"],
		ResourceGroup = this.Configuration["DataFactory:ResourceGroup"],
		AadTenantId = this.Configuration["DataFactory:AadTenantId"],
		AadClientId = this.Configuration["DataFactory:AadClientId"],
		AadSecret = this.Configuration["DataFactory:AadSecret"]
	});
}
```
**appsettings.json : **

```json
{
  "MediaServices": {
    "ArmEndpoint": "https://management.azure.com/",
    "SubscriptionId": "00000000-0000-0000-0000-000000000000",
    "ResourceGroup": "amsResourceGroup",
    "MediaAccountName": "amsaccount",
    "AadTenantId": "00000000-0000-0000-0000-000000000000",
    "AadClientId": "00000000-0000-0000-0000-000000000000",
    "AadSecret": "00000000-0000-0000-0000-000000000000"
  },
  "DataFactory": {
    "ArmEndpoint": "https://management.azure.com/",
    "SubscriptionId": "00000000-0000-0000-0000-000000000000",
    "ResourceGroup": "adfResourceGroup",
    "FactoryName": "adfaccount",
    "AadTenantId": "00000000-0000-0000-0000-000000000000",
    "AadClientId": "00000000-0000-0000-0000-000000000000",
    "AadSecret": "00000000-0000-0000-0000-000000000000"
  }
}
```

## Documentation

### Tasks documentation

* [Asset Tasks](ProcessMyMedia/Tasks/Media/Asset)
* [Encoding Tasks](ProcessMyMedia/Tasks/Media/Encoding)
* [Analyzing Tasks](ProcessMyMedia/Tasks/Media/Analyzing)
* [Data Factory Tasks](ProcessMyMedia/Tasks/Data)

### Model documentation

* [Asset Model](ProcessMyMedia/Model/Asset)
* [Encoding Model](ProcessMyMedia/Model/Encoding)
* [Data Factory Model](ProcessMyMedia/Model/Data)

### Samples

* [Asset Samples](ProcessMyMedia.Samples/Samples/Asset)
* [Encoding Samples](ProcessMyMedia.Samples/Samples/Encoding)
* [Analyzing Samples](ProcessMyMedia.Samples/Samples/Analyzing)
* [Data Factory Samples](ProcessMyMedia.Samples/Samples/Data)

## License

This project is licensed under the MIT License - see the [LICENSE file](LICENSE)  for details
