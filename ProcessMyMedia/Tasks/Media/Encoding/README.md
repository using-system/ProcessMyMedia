# Encoding
 
## EncodeFileBuiltInPresetTask

Encode a file with a BuiltInPreset

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>Preset</td>
  <td>string</td>
  <td>BuiltInPreset name</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>FilePath</td>
  <td>string</td>
  <td>Full path of the file to encode.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>CleanupResources</td>
  <td>bool</td>
  <td>Clean all azure media services resources (job, transform, file ingested) used by the task to encode the file</td>
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
  <td>Job</td>
  <td>JobEntity</td>
  <td>Get the encoding job informations.</td>
 </tr>
</table>

## EncodeFileBuiltInPresetTask

Encode a file with a a list of BuiltInPreset

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>Presets</td>
  <td>ListOfString</td>
  <td>List of BuiltInPreset names</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>FilePath</td>
  <td>string</td>
  <td>Full path of the file to encode.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>CleanupResources</td>
  <td>bool</td>
  <td>Clean all azure media services resources (job, transform, file ingested) used by the task to encode the file</td>
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
  <td>Job</td>
  <td>JobEntity</td>
  <td>Get the encoding job informations.</td>
 </tr>
</table>
