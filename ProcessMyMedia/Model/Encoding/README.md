

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
  <td>FilenamePattern</td>
  <td>string</td>
  <td>Specify the file pattern to use for generate the video</td>
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

# H264VideoLayer

<table>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>Bitrate</td>
  <td>int?</td>
  <td>Video bitrate</td>
 </tr>
 <tr>
  <td>Width</td>
  <td>string</td>
  <td>Set the Width of the video</td>
 </tr>
 <tr>
  <td>Height</td>
  <td>string</td>
  <td>Set the Height of the video</td>
 </tr>
 <tr>
  <td>MaxBitrate</td>
  <td>int?</td>
  <td>The maximum bitrate (in bits per second), at which the VBV buffer should be assumed to refill. If not specified, defaults to the same value as bitrate.</td>
 </tr>
 <tr>
  <td>BFrames</td>
  <td>int?</td>
  <td>the number of B-frames to be used when encoding this layer. If not specified, the encoder chooses an appropriate number based on the video profile and level.</td>
 </tr>
 <tr>
  <td>FrameRate</td>
  <td>string</td>
  <td>The frame rate (in frames per second) at which to encode this layer. The value can be in the form of M/N where M and N are integers (For example, 30000/1001), or in the form of a number (For example, 30, or 29.97). The encoder enforces constraints on allowed frame rates based on the profile and level. If it is not specified, the encoder will use the same frame rate as the input video.</td>
 </tr>
  <tr>
  <td>Slices</td>
  <td>int?</td>
  <td>the number of slices to be used when encoding this layer. If not specified, default is zero, which means that encoder will use a single slice for each frame.</td>
 </tr>
  <tr>
  <td>AdaptiveBFrame</td>
  <td>bool?</td>
  <td>whether or not adaptive B-frames are to be used when encoding this layer. If not specified, the encoder will turn it on whenever the video profile permits its use.</td>
 </tr>
 <tr>
  <td>Profile</td>
  <td>string</td>
  <td>Specify which profile of the H.264 standard should be used when encoding this layer. Default is Auto. Possible values include: 'Auto', 'Baseline', 'Main', 'High', 'High422', 'High444'</td>
 </tr>
 <tr>
  <td>Level</td>
  <td>string</td>
  <td>Specify which level of the H.264 standard should be used when encoding this layer. The value can be Auto, or a number that matches the H.264 profile. If not specified, the default is Auto, which lets the encoder choose the Level that is appropriate for this layer.</td>
 </tr>
  <tr>
  <td>BufferWindow</td>
  <td>string</td>
  <td>The VBV buffer window length. The value should be in ISO 8601 format. The value should be in the range [0.1-100] seconds. The default is 5 seconds (for example, PT5S).</td>
 </tr>
  <tr>
  <td>ReferenceFrames</td>
  <td>int?</td>
  <td>The number of reference frames to be used when encoding this layer. If not specified, the encoder determines an appropriate number based on the encoder complexity setting.</td>
 </tr>
   <tr>
  <td>EntropyMode</td>
  <td>string</td>
  <td>Specify the entropy mode to be used for this layer. If not specified, the encoder chooses the mode that is appropriate for the profile and level. Possible values include: 'Cabac', 'Cavlc'</td>
 </tr>
  <tr>
  <td>Label</td>
  <td>string</td>
  <td>Layer label.</td>
 </tr>
</table>

# AacAudioCodec

<table>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
 <tr>
  <td>FilenamePattern</td>
  <td>string</td>
  <td>Specify the file pattern to use for generate the audio</td>
 </tr>
  <td>Profile</td>
  <td>string</td>
  <td>Specify the encoding profile to be used when encoding audio with AAC. Possible values include: 'AacLc', 'HeAacV1', 'HeAacV2'</td>
 </tr>
 <tr>
  <td>Channels</td>
  <td>int?</td>
  <td>Specify the number of channels in the audio.</td>
 </tr>
 <tr>
  <td>SamplingRate</td>
  <td>int?</td>
  <td>Specify the sampling rate to use for encoding in hertz.</td>
 </tr>
 <tr>
  <td>Bitrate</td>
  <td>int?</td>
  <td>The bitrate, in bits per second, of the output encoded audio.</td>
 </tr>
</table>

# ThumbnailsOptions

<table>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>FilenamePattern</td>
  <td>string</td>
  <td>Specify the file pattern to use for generate the thumbnails</td>
 </tr>
 <tr>
  <td>GeneratePng</td>
  <td>bool?</td>
  <td>Define if the preset generate png thumbnails.</td>
 </tr>
 <tr>
  <td>GenerateJpg</td>
  <td>bool?</td>
  <td>Define if the preset generate jpg thumbnails.</td>
 </tr>
 <tr>
  <td>Start</td>
  <td>string</td>
  <td>The position in the input video from where to start generating thumbnails. The value can be in absolute timestamp (ISO 8601, e.g: PT05S), or a frame count (For example, 10 for the 10th frame), or a relative value (For example, 1%). Also supports a macro {Best}, which tells the encoder to select the best thumbnail from the first few seconds of the video. </td>
 </tr>
 <tr>
  <td>Step</td>
  <td>string</td>
  <td>The intervals at which thumbnails are generated. The value can be in absolute timestamp (ISO 8601, e.g: PT05S for one image every 5 seconds), or a frame count (For example, 30 for every 30 frames), or a relative value (For example, 1%).</td>
 </tr>
 <tr>
  <td>Range</td>
  <td>string</td>
  <td>The position in the input video at which to stop generating thumbnails. The value can be in absolute timestamp (ISO 8601, e.g: PT5M30S to stop at 5 minutes and 30 seconds), or a frame count (For example, 300 to stop at the 300th frame), or a relative value (For example, 100%).</td>
 </tr>
  <tr>
  <td>Width</td>
  <td>string</td>
  <td>the width of the output video for this layer. The value can be absolute (in pixels) or relative (in percentage). For example 50% means the output video has half as many pixels in width as the input.</td>
 </tr>
  <tr>
  <td>Height</td>
  <td>string</td>
  <td>The height of the output video for this layer. The value can be absolute (in pixels) or relative (in percentage). For example 50% means the output video has half as many pixels in height as the input.</td>
 </tr>
</table>
