using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    [SerializeField] private GameObject _sampleCubePrefab =  null;

    [Range(1000, 100000)]
    [SerializeField] private float _maxScale = 1000f;

    private GameObject[] _sampleCubes = new GameObject[512];

    private const string CUBE_NAME = "SampleCube";
    private const float ANGLE_OF_SAMPLE_IN_CIRCLE = 360f / 512f;
    private const float DISTANCE_BETWEEN_CUBES = 100f;
    private const float CUBE_SCALE = 10f;


    private void Start()
    {
        for (int i = 0; i < _sampleCubes.Length; i++)
        {
            GameObject instantiatedCube = (GameObject)Instantiate(_sampleCubePrefab);
            instantiatedCube.transform.position = this.transform.position;
            instantiatedCube.transform.parent = this.transform;
            instantiatedCube.name = string.Format("{0}{1}", CUBE_NAME, i);

            this.transform.eulerAngles = new Vector3(0, -i * ANGLE_OF_SAMPLE_IN_CIRCLE, 0);
            instantiatedCube.transform.position = Vector3.forward * DISTANCE_BETWEEN_CUBES;

            _sampleCubes[i] = instantiatedCube;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _sampleCubes.Length; i++)
        {
            if (_sampleCubes[i] != null)
            {
                _sampleCubes[i].transform.localScale = new Vector3(CUBE_SCALE, AudioPeer.Samples[i] * _maxScale, CUBE_SCALE);
            }
        }
    }

}
