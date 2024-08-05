using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] GameObject collectedEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(Instantiate(collectedEffect, transform.position, Quaternion.Euler(-90, 0, 0)), 3);
            GameManager.Instance.star++;
            Destroy(gameObject);
        }
    }
}
