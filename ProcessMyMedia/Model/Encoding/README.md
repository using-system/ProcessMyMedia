

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
</table>
