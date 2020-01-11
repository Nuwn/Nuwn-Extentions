using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nuwn.Extensions;
using System;

public class MicrophoneInput : Singleton<MicrophoneInput>
{
    AudioSource audioSource;

    public float soundMultiplier = 1.5f;
    public float Vol { get; private set; } = 0;
    public float Frequency { get; private set; } = 0;
    [HideInInspector]public List<string> Options = new List<string>();

    public string microphone { get; set; }

#pragma warning disable 0414
    float minThreshold = 0;
    int audioSampleRate = 44100;
    public FFTWindow fftWindow;
    private int samples = 8192;
#pragma warning restore 0414
    public float DebugMaxVolume = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            Options.Add(Microphone.devices[i]);
        }

        microphone = PlayerPrefs.GetString("Microphone").Empty() ? Options[0] : PlayerPrefs.GetString("Microphone");
    }

    public void StartMicrophone()
    {
        UpdateMicrophoneInput();
    }
    public void StopMicrophone()
    {
        audioSource.Stop();
    }

    public void SetMicrophone(string microphone)
    {
        this.microphone = microphone;
        PlayerPrefs.SetString("Microphone", microphone);
        UpdateMicrophoneInput();
    }

    private void UpdateMicrophoneInput()
    {
        audioSource.Stop();

        audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
        audioSource.loop = true;

        if (Microphone.IsRecording(microphone))
        { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
            while (!(Microphone.GetPosition(microphone) > 0))
            {
            } // Wait until the recording has started. 

            Debug.Log("recording started with " + microphone);

            // Start playing the audio source
            audioSource.Play();
        }
        else
        {
            //microphone doesn't work for some reason
            Debug.Log(microphone + " doesn't work!");
        }
    }
    
    public float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audioSource.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
    public float GetFundamentalFrequency()
    {
        float[] data = new float[samples];
        audioSource.GetSpectrumData(data, 0, fftWindow);
        float s = 0.0f;
        int i = 0;
        for (int j = 1; j < samples; j++)
        {
            if (data[j] > minThreshold) // volumn must meet minimum threshold
            {
                if (s < data[j])
                {
                    s = data[j];
                    i = j;
                }
            }
        }
        float fundamentalFrequency = i * audioSampleRate / samples;
        return fundamentalFrequency;
    }

    private void Update()
    {
        Vol = GetAveragedVolume() * soundMultiplier;
        Frequency = GetFundamentalFrequency();

        if (Vol > DebugMaxVolume) DebugMaxVolume = Vol;

        if (Input.GetKey(KeyCode.RightShift))
        {
            Vol = 0.50f;
        }
    }
}
