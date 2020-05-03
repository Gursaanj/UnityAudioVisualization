using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    [SerializeField] private GameObject _sampleCubePrefab =  null;

    [Range(1000, 100000)]
    [SerializeField] private float _maxScale = 1000f;

    private GameObject[] _sampleCubes = null;

    private const string CUBE_NAME = "SampleCube";
    private float _anglebetweenSamples;
    private const float DISTANCE_BETWEEN_CUBES = 100f;
    private const float CUBE_SCALE = 10f;

    #region TryoutVariables

    [SerializeField] private float _distanceBetweenCubes = 20f;
    [SerializeField] private float _cubeScale = 5f;

    #endregion


    private void Start()
    {
        _sampleCubes = new GameObject[AudioPeer.Samples.Length];
        _anglebetweenSamples = 360f / AudioPeer.Samples.Length;

        for (int i = 0; i < _sampleCubes.Length; i++)
        {
            GameObject instantiatedCube = (GameObject)Instantiate(_sampleCubePrefab);
            instantiatedCube.transform.position = this.transform.position;
            instantiatedCube.transform.parent = this.transform;
            instantiatedCube.name = string.Format("{0}{1}", CUBE_NAME, i);

            this.transform.eulerAngles = new Vector3(0, -i * _anglebetweenSamples, 0);
            instantiatedCube.transform.position = Vector3.forward * _distanceBetweenCubes;

            _sampleCubes[i] = instantiatedCube;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _sampleCubes.Length; i++)
        {
            if (_sampleCubes[i] != null)
            {
                //_sampleCubes[i].transform.localScale = new Vector3(_cubeScale, AudioPeer.Samples[i] * _maxScale, _cubeScale);
                _sampleCubes[i].transform.localScale = new Vector3(AudioPeer.Samples[i] * _maxScale, _cubeScale, _cubeScale);
            }
        }
    }

}
