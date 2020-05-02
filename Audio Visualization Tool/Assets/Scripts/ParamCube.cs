using UnityEngine;

public class ParamCube : MonoBehaviour
{
    [SerializeField] private int _band;
    [SerializeField] private float _startScale;
    [SerializeField] private float _scaleMultiplier;


    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.FrequencyBands[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
    }
}
