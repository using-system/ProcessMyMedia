# Analyzing Tasks

## AnalyzeFileTask

Annalyse a media file. This task produce an asset the analysing result.
The primary output file of analyzing videos is called insights.json. 
This file contains insights about your video. You can find description of elements found in the json file in the [Media intelligence article](https://docs.microsoft.com/fr-fr/azure/media-services/latest/analyzing-video-audio-files-concept).

<table>
 <caption>Input</caption>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
  <th>Is Required ?</th>
 </tr>
 <tr>
  <td>FilePath</td>
  <td>string</td>
  <td>Full path of the file to analyse.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>AnalyzingParameters</td>
  <td>AnalyzingParameters</td>
  <td>Analyse options</td>
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
  <td>Result</td>
  <td>AnalyzingResult</td>
  <td>Get the asset id of the analyzing result.</td>
 </tr>
</table>

## AnalyzeAssetTask

Annalyse a media asset. This task produce an asset the analysing result.
The primary output file of analyzing videos is called insights.json. 
This file contains insights about your video. You can find description of elements found in the json file in the [Media intelligence article](https://docs.microsoft.com/fr-fr/azure/media-services/latest/analyzing-video-audio-files-concept).

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
  <td>Asset name to analyse.</td>
  <td>Yes</td>
 </tr>
 <tr>
  <td>AnalyzingParameters</td>
  <td>AnalyzingParameters</td>
  <td>Analyse options</td>
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
  <td>Result</td>
  <td>AnalyzingResult</td>
  <td>Get the asset id of the analyzing result.</td>
 </tr>
</table>

