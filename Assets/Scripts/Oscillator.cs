using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    public double increment;
    public double phase;
    public double sampling_frequency = 48000.0;

    public float gain;
    public float volume = 0.1f;

    public float[] frequencies;
    public int thisFreq;

    private void Start()
    {
        frequencies = new float[] { 261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f, 523.25f };
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));

            if(channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > Mathf.PI * 2)
            {
                phase = -Mathf.PI * 2;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gain = volume;
            frequency = frequencies[thisFreq];
            thisFreq += 1;
            thisFreq %= frequencies.Length;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            gain = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            gain = volume;
            frequency = frequencies[thisFreq];
            thisFreq -= 2;
            thisFreq = (thisFreq + frequencies.Length) % frequencies.Length;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            gain = 0.0f;
        }
    }
}
