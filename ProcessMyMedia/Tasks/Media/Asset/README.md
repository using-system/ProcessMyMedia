﻿# Asset Tasks
 
## IngestFileTask

Create a new asset and upload the file specified

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>AssetName</td>
  <td>string</td>
  <td>The asset name to create.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>AssetDescription</td>
  <td>string</td>
  <td>Specify an asset description.</td>
  <td>No</td>
 </tr>
 <tr>
  <td>AssetFilePath</td>
  <td>string</td>
  <td>The full path of the file to upload</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>StorageAccountName</td>
  <td>string</td>
  <td>If not specified, the task upload the file into the primary storage account associated to the Media Services account</td>
  <td>No</td>
 </tr>
 <tr>
  <td>Metadata</td>
  <td>Dictionary</td>
  <td>Metadata informations to associate with the asset</td>
  <td>No</td>
 </tr>
</table>

<table>
 <caption>Output</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>Asset</td>
  <td>AssetEntity</td>
  <td>The asset created or updated by the task</td>
 </tr>
</table>

## IngestFilesTask

Create a new asset and upload the files specified

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>AssetName</td>
  <td>string</td>
  <td>The asset name to create.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>AssetDescription</td>
  <td>string</td>
  <td>Specify an asset description.</td>
  <td>No</td>
 </tr>
 <tr>
  <td>AssetFiles</td>
  <td>List Of String</td>
  <td>List of the full path of the files to upload</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>StorageAccountName</td>
  <td>string</td>
  <td>If not specified, the task upload the file into the primary storage account associated to the Media Services account</td>
  <td>No</td>
 </tr>
 <tr>
  <td>Metadata</td>
  <td>Dictionary</td>
  <td>Metadata informations to associate with the asset</td>
  <td>No</td>
 </tr>
</table>

<table>
 <caption>Output</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>Asset</td>
  <td>AssetEntity</td>
  <td>The asset created or updated by the task</td>
 </tr>
</table>

## IngestFromDirectoryTask

Create a new asset and upload the files presents in a directory

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>AssetName</td>
  <td>string</td>
  <td>The asset name to create.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>AssetDescription</td>
  <td>string</td>
  <td>Specify an asset description.</td>
  <td>No</td>
 </tr>
 <tr>
  <td>AssetDirectoryPath</td>
  <td>string</td>
  <td>List of the full path of the directory to upload</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>SearchPattern</td>
  <td>string</td>
  <td>Search pattern to specify the files to upload</td>
  <td>No</td>
 </tr>
 <tr>
  <td>TopDirectoryOnly</td>
  <td>bool</td>
  <td>Specifies to get files only on the top directory. Default value : true.</td>
  <td>No</td>
 </tr>
 <tr>
  <td>StorageAccountName</td>
  <td>string</td>
  <td>If not specified, the task upload the file into the primary storage account associated to the Media Services account</td>
  <td>No</td>
 </tr>
 <tr>
  <td>Metadata</td>
  <td>Dictionary</td>
  <td>Metadata informations to associate with the asset</td>
  <td>No</td>
 </tr>
</table>

<table>
 <caption>Output</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>Asset</td>
  <td>AssetEntity</td>
  <td>The asset created or updated by the task</td>
 </tr>
</table>

## DownloadAssetTask

Download asset to a local directory

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>AssetName</td>
  <td>string</td>
  <td>The asset name to download.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>DirectoryToDownload</td>
  <td>string</td>
  <td>The directory to download the asset.</td>
  <td>Yes</td>
 </tr>
</table>

## DeleteAssetTask

Delete an asset

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>AssetName</td>
  <td>string</td>
  <td>The asset name to delete.</td>
  <td>Yes</td>
 </tr>
</table>
