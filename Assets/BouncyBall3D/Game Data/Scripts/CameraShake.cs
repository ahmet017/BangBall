using UnityEngine;
using System.Collections;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField] private Transform camTransform = null;
    [SerializeField] private float shakeDuration = 0f;
    [SerializeField] private float shakeAmount = 0.7f;
    [SerializeField] private float decreaseFactor = 1.0f;

    private Vector3 originalPos;

    protected override void Awake()
    {
        base.Awake();

        if (camTransform == null)
        {
            Debug.LogWarning("[CameraShake] camTransform no assigned! Will be taken the MainCamera's transform.");
            camTransform = Camera.main.transform;
        }
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount * shakeDuration, 0.3f);

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void Shake(float duration)
    {
        shakeDuration = duration;
    }
}