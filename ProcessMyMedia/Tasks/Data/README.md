# Data Factory Tasks
 
## CreateLinkedServiceTask

Create or update an Azure Data Factory linked service.

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>LinkedServiceToCreate</td>
  <td>LinkedServiceEntity</td>
  <td>Linked service to create or update.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>CleanupResources</td>
  <td>bool</td>
  <td>Clean all azure resources (Pipeline, dataset) used by the task. Default value : true.</td>
  <td>No</td>
 </tr>
</table>

## CopyTask

Copy files from any sources to any sources ! 

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>SourcePath</td>
  <td>DataPath</td>
  <td>Source properties.</td>
  <td>Yes</td>
 </tr>
  <tr>
  <td>DestinationPath</td>
  <td>DataPath</td>
  <td>Destination properties.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>CleanupResources</td>
  <td>bool</td>
  <td>Clean all azure resources (Pipeline, dataset) used by the task. Default value : true.</td>
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
  <td>InputDataset</td>
  <td>DatasetEntity</td>
  <td>Azure Input dataset used for the copy.</td>
 </tr>
  <tr>
  <td>OutputDataset</td>
  <td>DatasetEntity</td>
  <td>Azure Output dataset used for the copy.</td>
 </tr>
   <tr>
  <td>Run</td>
  <td>DataPipelineRunEntity</td>
  <td>Azure Pipeline Run used for the copy.</td>
 </tr>
</table>