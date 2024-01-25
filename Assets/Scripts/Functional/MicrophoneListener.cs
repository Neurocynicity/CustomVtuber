using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneListener : MonoBehaviour
{
    public List<MicUser> MicrophoneUsers = new();

    public float sampleRate = 50f;
    float _lastSampleTime;
    public float Sensitivity;
    public float MinThreshold;

    AudioClip _microphoneInput;
    string _currentDeviceName;
    private float _level;

    public int lerpSpeed = 3;

    private float[] _previousLevels;
    int _previousLevelsIndex = 0;

    private void Awake()
    {
        _previousLevels = new float[lerpSpeed];
        UpdateAudioDevice(Microphone.devices[0]);
    }

    // for some reason this is set up to use strings??
    // fuck it we ball
    public void UpdateAudioDevice(string deviceName)
    {
        if (deviceName != "")
            Microphone.End(deviceName);

        _currentDeviceName = deviceName;
        _microphoneInput = Microphone.Start(deviceName, true, 10, 44100);
    }

    private int _sampleWindow = 64;

    void Update()
    {
        float timeBetweenSamples = 1 / sampleRate;

        if (Time.time < _lastSampleTime + timeBetweenSamples)
            return;

        _lastSampleTime = Time.time;

        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(_currentDeviceName) - _sampleWindow;
        _microphoneInput.GetData(waveData, micPosition);

        float totalLoudness = 0;

        // Getting a peak on the last samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        totalLoudness /= _sampleWindow;

        totalLoudness *= Sensitivity;

        if (totalLoudness < MinThreshold)
            totalLoudness = 0;

        _previousLevels[_previousLevelsIndex % _previousLevels.Length] = totalLoudness;
        _previousLevelsIndex++;

        _level = _previousLevels.Average();

        foreach (MicUser user in MicrophoneUsers)
            user.NewMicLevel(_level);
    }

    public void SetMinTheshold(float value)
    {
        MinThreshold = value;
    }

    public void SetSensitivity(float value)
    {
        Sensitivity = value;
    }
}
