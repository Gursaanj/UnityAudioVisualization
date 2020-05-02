﻿using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private int _startScale = 1;

    [Range(10,30)]
    [SerializeField] private int _scaleMultiplier = 10;

    [SerializeField] private bool _useBandBuffer = true;
    [SerializeField] private Transform[] _paramCubes = null;

    private void Start()
    {
        if (_paramCubes.Length != AudioPeer.FrequencyBands.Length || _paramCubes.Length != AudioPeer.BandBuffers.Length)
        {
            Debug.LogError("The Number of frequency bands and the number of Param Cubes do not match, please Address");
        }
    }

    private void Update()
    {
        for (int i = 0; i < _paramCubes.Length; i++)
        {
            Vector3 localScale = _paramCubes[i].transform.localScale;

            _paramCubes[i].localScale = new Vector3(localScale.x, ((_useBandBuffer? AudioPeer.BandBuffers[i] : AudioPeer.FrequencyBands[i]) * _scaleMultiplier) + _startScale, localScale.z);
        }
    }
}