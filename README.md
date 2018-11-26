# ProcessMyMedia
Build your Azure Media Services workflow (V3 API version) and Azure Data Factory (V2 API version) in .NET Core. 

ProcessMyMedia lib is based on [Workflow Core](https://github.com/danielgerlag/workflow-core) . Workflow Core is a light weight workflow engine targeting .NET Standard. It supports pluggable persistence and concurrency providers to allow for multi-node clusters. 

Contributors welcome !

## Installation

Install-Package ProcessMyMedia -Version 1.0.0.56

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
