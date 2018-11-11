

# BuiltInPreset

BuiltInPreset enumeration to use with the tasks EncodeFileBuiltInPresetTask or EncodeFileBuiltInPresetsTask

<table>
 <tr>
  <th>Name</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>AACGoodQualityAudio</td>
  <td>Produces a single MP4 file containing only stereo audio encoded at 192 kbps.</td>
 </tr>
 <tr>
  <td>AdaptiveStreaming</td>
  <td>Produces a set of GOP aligned MP4 files with H.264 video and stereo AAC audio. Auto-generates a bitrate ladder based on the input resolution and bitrate. The auto-generated preset will never exceed the input resolution and bitrate. For example, if the input is 720p at 3 Mbps, output will remain 720p at best, and will start at rates lower than 3 Mbps. The output will will have video and audio in separate MP4 files, which is optimal for adaptive streaming.</td>
 </tr>
 <tr>
  <td>H264MultipleBitrate1080p</td>
  <td>Produces a set of 6 GOP-aligned MP4 files, ranging from 3400 kbps to 400 kbps, and stereo AAC audio. Resolution starts at 720p and goes down to 360p.</td>
 </tr>
 <tr>
  <td>H264MultipleBitrate720p</td>
  <td>Produces a set of 6 GOP-aligned MP4 files, ranging from 3400 kbps to 400 kbps, and stereo AAC audio. Resolution starts at 720p and goes down to 360p.</td>
 </tr>
 <tr>
  <td>H264MultipleBitrateSD</td>
  <td>Produces a set of 5 GOP-aligned MP4 files, ranging from 1600kbps to 400 kbps, and stereo AAC audio. Resolution starts at 480p and goes down to 360p.</td>
 </tr>
  <tr>
  <td>H264SingleBitrate1080p</td>
  <td>Produces an MP4 file where the video is encoded with H.264 codec at 6750 kbps and a picture height of 1080 pixels, and the stereo audio is encoded with AAC-LC codec at 64 kbps.</td>
 </tr>
   <tr>
  <td>H264SingleBitrate720p</td>
  <td>Produces an MP4 file where the video is encoded with H.264 codec at 4500 kbps and a picture height of 720 pixels, and the stereo audio is encoded with AAC-LC codec at 64 kbps.</td>
 </tr>
    <tr>
  <td>H264SingleBitrateSD</td>
  <td>Produces an MP4 file where the video is encoded with H.264 codec at 2200 kbps and a picture height of 480 pixels, and the stereo audio is encoded with AAC-LC codec at 64 kbps.</td>
 </tr>
</table>

# H264VideoCodec

<table>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>KeyFrameInterval</td>
  <td>string</td>
  <td>Key frame interval</td>
 </tr>
 <tr>
  <td>Complexity</td>
  <td>string</td>
  <td>Possible values : Speed, Balanced, Quality</td>
 </tr>
 <tr>
  <td>SceneChangeDetection</td>
  <td>bool?</td>
  <td>Enable schene changhe detection</td>
 </tr>
 <tr>
  <td>CreationDate</td>
  <td>DateTime</td>
  <td>Creation date of the asset</td>
 </tr>
 <tr>
  <td>Layers</td>
  <td>ListOfH264VideoLayer</td>
  <td>Video codec layers</td>
 </tr>
</table>

