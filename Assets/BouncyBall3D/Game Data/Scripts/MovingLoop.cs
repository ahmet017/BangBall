using UnityEngine;

public class MovingLoop : MonoBehaviour
{
    public float range = 3f;
    public float speed = 2f;

    float xPos;
    float offset;
    Vector3 newPos;

    private void OnEnable()
    {
        offset = Random.Range(0f, 1f) / speed;
        speed *= Random.Range(0f, 1f) > 0.5 ? 1 : -1;
    }

    private void Update()
    {
        float t = (Mathf.Cos((Time.time + offset) * speed) + 1) / 2;
        xPos = Mathf.Lerp(-range, range, t);
        newPos = new Vector3(xPos, transform.position.y, transform.position.z);

        transform.position = newPos;
    }
}
