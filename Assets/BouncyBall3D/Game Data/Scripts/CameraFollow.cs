using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothDamp = 0.5f;

    Vector3 offset, velocity;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothDamp);
    }
}
