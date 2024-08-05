using UnityEngine;
using System;
using System.Collections.Generic;

public class SpectrumEffects : MonoBehaviour
{
    [Tooltip("12 objects")]
    public List<GameObject> cubeList = new List<GameObject>();

    public float height;
    public float width;

    void Start()
    {
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);
        processor.onSpectrum.AddListener(onSpectrum);
    }

    void onOnbeatDetected()
    {
        Debug.Log("Beat!!!");
    }

    void onSpectrum(float[] spectrum)
    {
        for (int i = 0; i < spectrum.Length; i++)
        {
            cubeList[i].transform.localScale = new Vector3(width, Mathf.Lerp(cubeList[i].transform.localScale.y, spectrum[i] * height, 0.2f), width);
        }
    }
}
