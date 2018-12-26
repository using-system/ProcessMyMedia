# Streaming
 
## StreamTask

Stream an asset

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
  <td>The asset name to stream</td>
  <td>Yes</td>
 </tr>
  <tr>
  <td>Options</td>
  <td>StreamingOptions</td>
  <td>Streaming options (Specify Start date and End date of the stream)</td>
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
  <td>LocatorName</td>
  <td>string</td>
  <td>The streaming locator name.</td>
 </tr>
 <tr>
  <td>StreamingUrls</td>
  <td>List Of string</td>
  <td>Get the streaming urls.</td>
 </tr>
</table>
