# Asset Tasks
 
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
</table>
