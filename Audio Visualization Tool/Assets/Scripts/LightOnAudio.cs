using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightOnAudio : MonoBehaviour
{
    [SerializeField] private Light _lightSource;

    [Range(0, 8)]
    [SerializeField] private int _frequencyBand;
    [SerializeField] private int _minIntensity;
    [SerializeField] private int _maxIntensity;


    private void Update()
    {
        _lightSource.intensity = (AudioPeer.AudioBandBuffers[_frequencyBand > AudioPeer.NUMBER_OF_BANDS ? AudioPeer.NUMBER_OF_BANDS - 1 : _frequencyBand] * (_maxIntensity - _minIntensity)) + _minIntensity;
    }
}
