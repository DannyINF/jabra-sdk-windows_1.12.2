using System;
using System.Collections.Generic;
using System.IO;
using JabraSDK;
using NAudio.Wave;

namespace Jabra_SDK_Demo.Helpers
{
  /// <summary>
  /// Audio utility
  /// </summary>
  public static class AudioUtilities
  {
    /// <summary>
    /// Converts mp3/wav file to uncompressed wav file format
    /// </summary>
    /// <param name="fileName">Source file</param>
    /// <param name="fileparam">file parameters for conversion</param>
    /// <param name="maxByteLength">maximum bytes for conversion</param>
    /// <param name="outputFileName">destination file</param>
    /// <remarks>Resamples audio if needed</remarks>
    public static void ConvertToWav(string fileName, IAudioFile fileparam, int maxByteLength, string outputFileName)
    {
      Stream outputStream = File.Open(outputFileName, FileMode.Create);
      var newFormat = new WaveFormat(fileparam.SampleRate, fileparam.BitsPerSample, fileparam.NumberOfChannels);
      using (MediaFoundationReader reader = new MediaFoundationReader(fileName))
      using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, newFormat))
      {
        byte[] bytes = Resample(reader, newFormat);
        byte[] wavData = TruncateBytes(bytes, maxByteLength);
        waveFileWriter.Write(wavData, 0, wavData.Length);
        waveFileWriter.Flush();
      }
    }

    static byte[] TruncateBytes(byte[] bytes, int byteCount)
    {

      if (bytes.Length > byteCount)
      {
        var truncatedBytes = new Byte[byteCount];
        Array.Copy(bytes, truncatedBytes, byteCount);

        return truncatedBytes;
      }

      return bytes;
    }

    static byte[] Resample(IWaveProvider sourceProvider, WaveFormat outputFormat)
    {
      var buffer = new List<byte>();
      using (var resampler = new MediaFoundationResampler(sourceProvider, outputFormat))
      {
        var readBuffer = new byte[4096];
        for (; ; )
        {
          var cnt = resampler.Read(readBuffer, 0, readBuffer.Length);
          if (cnt == 0)
            break;

          buffer.AddRange(readBuffer);
        }
      }

      return buffer.ToArray();
    }
  }
}
