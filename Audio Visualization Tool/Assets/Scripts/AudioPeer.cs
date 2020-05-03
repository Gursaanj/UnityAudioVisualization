using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    // samples follow Frequency intervals, _samples[0]
    // will be the deepbase at 20hz Min
    // and _samples[512] will be highest pitch at 20 khz max
    public static float[] Samples = new float[512];

    //Decides how many bands should the 512 samples be broken up into
    public static int NUMBER_OF_BANDS = 8; 

    //Split up the 512 channels into different regions for optimized Visualization
    public static float[] AudioBands = null;
    //Buffer floats handle smooth transistions of the y-scale 
    public static float[] AudioBandBuffers = null;

    [SerializeField] private AudioSource _audioSource = null;
    private float[] _buffersDecrease = null;
    private float[] _frequencyBands = null;
    private float[] _bandBuffers = null;
    private float[] _frequencyBandMaxValues = null;

    private const float FREQUENCY_SCALING_FACTOR = 10f;
    private const float BAND_BUFFER_DECREASE = 0.00005f;
    private const float BAND_BUFFER_INCREASE_SCALE = 1.2f;


    private void Awake()
    {
        AudioBands = new float[NUMBER_OF_BANDS];
        AudioBandBuffers = new float[NUMBER_OF_BANDS];
        _frequencyBands = new float[NUMBER_OF_BANDS];
        _bandBuffers = new float[NUMBER_OF_BANDS];
        _buffersDecrease = new float[NUMBER_OF_BANDS];
        _frequencyBandMaxValues = new float[NUMBER_OF_BANDS];
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBends();
        BandBuffering();
        CreateAudioBands();
    }

    /// <summary>
    /// Window is the Fourier Alogrithm to reduce leakage
    /// I.e. smaller O() with more complex FFT Window - but reduced speed for method
    /// </summary>
    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
    }

    private void BandBuffering()
    {
        for (int i = 0; i < NUMBER_OF_BANDS; i++)
        {
            if (_frequencyBands[i] > _bandBuffers[i])
            {
                _bandBuffers[i] = _frequencyBands[i];
                _buffersDecrease[i] = BAND_BUFFER_DECREASE;
            }
            if (_frequencyBands[i] < _bandBuffers[i])
            {
                _bandBuffers[i] -= _buffersDecrease[i];
                _buffersDecrease[i] *= BAND_BUFFER_INCREASE_SCALE;
            }
        }
    }

    /// <summary>
    /// Seperate frequencies depending on each Bin (7 definitions)
    /// 20-60 hz || 60 - 250 hz || 250 - 500 hz || 500 - 2000 hz || 2000 - 4000 hz || 4000 - 6000 hz || 6000 - 20000 hz
    /// </summary>
    private void MakeFrequencyBends()
    {
        int count = 0;
        int currentSampleCountTraversed = 0;

        for (int i = 0; i < NUMBER_OF_BANDS; i++)
        {
            float averageCount = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            currentSampleCountTraversed += sampleCount;

            if (i == _frequencyBands.Length - 1 && Samples.Length > currentSampleCountTraversed)
            {
                sampleCount += Samples.Length - currentSampleCountTraversed;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                averageCount += Samples[count] * (count + 1);
                count++;
            }

            averageCount /= count;

            _frequencyBands[i] = averageCount * FREQUENCY_SCALING_FACTOR;
        }
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < NUMBER_OF_BANDS; i++)
        {
            if (_frequencyBands[i] > _frequencyBandMaxValues[i])
            {
                _frequencyBandMaxValues[i] = _frequencyBands[i];
            }

            AudioBands[i] = (_frequencyBands[i] / _frequencyBandMaxValues[i]);
            AudioBandBuffers[i] = (_bandBuffers[i] / _frequencyBandMaxValues[i]);
        }
    }
}
