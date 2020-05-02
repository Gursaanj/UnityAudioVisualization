using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    // samples follow Frequency intervals, _samples[0]
    // will be the deepbase at 20hz Min
    // and _samples[512] will be highest pitch at 20 khz max
    public static float[] Samples = new float[512];

    [SerializeField] private AudioSource _audioSource = null;

    private void Update()
    {
        GetSpectrumAudioSource();
    }

    private void GetSpectrumAudioSource()
    {
        /// Window is the Fourier Alogrithm to reduce leakage
        /// I.e. smaller O() with more complex FFT Window - but reduced speed for method
        _audioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
    }
}
