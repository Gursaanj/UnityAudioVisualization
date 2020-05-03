using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private int _startScale = 1;

    [Range(40,80)]
    [SerializeField] private int _scaleMultiplier = 50;

    [SerializeField] private bool _useBandBuffer = true;
    [SerializeField] private GameObject[] _paramCubes = null;
    [SerializeField] private Color _cubeColor = new Color(1,1,0,1);

    private Material[] _cubeMaterials = null;
    private Color _diffuseColor;

    private void Start()
    {
        if (_paramCubes.Length != AudioPeer.NUMBER_OF_BANDS)
        {
            Debug.LogError("The Number of frequency bands and the number of Param Cubes do not match, please Address");
        }
        else
        {
            _cubeMaterials = new Material[AudioPeer.NUMBER_OF_BANDS];

            for (int i = 0; i < _paramCubes.Length; i++)
            {
                _cubeMaterials[i] = _paramCubes[i].GetComponent<MeshRenderer>().materials[0];
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _paramCubes.Length; i++)
        {
            Vector3 localScale = _paramCubes[i].transform.localScale;

            _paramCubes[i].transform.localScale = new Vector3(localScale.x, ((_useBandBuffer? AudioPeer.AudioBandBuffers[i] : AudioPeer.AudioBands[i]) * _scaleMultiplier) + _startScale, localScale.z);
            _diffuseColor = new Color(_cubeColor.r * AudioPeer.AudioBandBuffers[i], _cubeColor.g * AudioPeer.AudioBandBuffers[i], _cubeColor.b * AudioPeer.AudioBandBuffers[i]);

            _cubeMaterials[i].SetColor("_EmissionColor", _diffuseColor);
        }
    }
}
