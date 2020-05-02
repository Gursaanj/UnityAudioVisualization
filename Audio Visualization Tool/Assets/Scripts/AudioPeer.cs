using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    // samples follow Frequency intervals, _samples[0]
    // will be the deepbase at 20hz Min
    // and _samples[512] will be highest pitch at 20 khz max
    public static float[] Samples = new float[512];

    //Split up the 512 channels into 8 different regions for optimized Visualization
    public static float[] FrequencyBands = new float[8];

    [SerializeField] private AudioSource _audioSource = null;

    private const float FREQUENCY_SCALING_FACTOR = 10f;

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBends();
    }

    /// <summary>
    /// Window is the Fourier Alogrithm to reduce leakage
    /// I.e. smaller O() with more complex FFT Window - but reduced speed for method
    /// </summary>
    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
    }

    /// <summary>
    /// Seperate frequencies depending on each Bin (7 definitions)
    /// 20-60 hz || 60 - 250 hz || 250 - 500 hz || 500 - 2000 hz || 2000 - 4000 hz || 4000 - 6000 hz || 6000 - 20000 hz
    /// </summary>
    private void MakeFrequencyBends()
    {
        int count = 0;
        int currentSampleCountTraversed = 0;

        for (int i = 0; i < FrequencyBands.Length; i++)
        {
            float averageCount = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            currentSampleCountTraversed += sampleCount;

            if (i == FrequencyBands.Length - 1 && Samples.Length > currentSampleCountTraversed)
            {
                sampleCount += Samples.Length - currentSampleCountTraversed;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                averageCount += Samples[count] * (count + 1);
                count++;
            }

            averageCount /= count;

            FrequencyBands[i] = averageCount * FREQUENCY_SCALING_FACTOR;
;
        }
    }
}
