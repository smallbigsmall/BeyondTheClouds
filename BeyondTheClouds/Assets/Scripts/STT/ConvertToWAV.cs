using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ConvertToWAV : MonoBehaviour
{
    public static byte[] AudioClipToWav(AudioClip clip) {
        MemoryStream stream = new MemoryStream();
        EncodeAsWav(stream, clip);

        return stream.ToArray();
    }

    public static void EncodeAsWav(Stream stream, AudioClip clip) {
        int frequency = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        float[] samplesFloat = new float[clip.samples * channels];
        clip.GetData(samplesFloat, 0);

        stream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
        stream.Write(BitConverter.GetBytes(36 + samples * channels * 2), 0, 4);
        stream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);

        stream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4); 
        stream.Write(BitConverter.GetBytes(16), 0, 4);
        stream.Write(BitConverter.GetBytes((short)1), 0, 2);
        stream.Write(BitConverter.GetBytes((short)channels), 0, 2);
        stream.Write(BitConverter.GetBytes(frequency), 0, 4);
        stream.Write(BitConverter.GetBytes(frequency*channels*2), 0, 4);
        stream.Write(BitConverter.GetBytes((short)channels*2), 0, 2);
        stream.Write(BitConverter.GetBytes((short)16), 0, 2);

        stream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
        stream.Write(BitConverter.GetBytes(samples*channels*2), 0, 4);

        foreach (var sample in samplesFloat) {
            byte[] byteSample = BitConverter.GetBytes((short)(sample * short.MaxValue));
            stream.Write(byteSample, 0, byteSample.Length);
        }
    }
}
