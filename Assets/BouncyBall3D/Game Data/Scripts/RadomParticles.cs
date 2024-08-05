using UnityEngine;

public class RadomParticles : MonoBehaviour
{
    //[SerializeField] float beatSizeMultiplyier = 1.5f;
    [SerializeField] GameObject[] gos;
    //ParticleSystem currentParticles;
    //float startSize;
    //float currentSize;
    //ParticleSystem.MainModule main;

    private void Awake()
    {
        int r = Random.Range(0, gos.Length);
        gos[r].SetActive(true);

        //currentParticles = gos[r].GetComponent<ParticleSystem>();
        //main = currentParticles.main;
        //startSize = main.startSize.constantMax;
        //currentSize = startSize;
    }

    //private void Update()
    //{
    //    if (currentSize > startSize)
    //    {
    //        currentSize = Mathf.Lerp(currentSize, startSize, 0.1f);
    //        main.startSize = new ParticleSystem.MinMaxCurve(1, currentSize);
    //    }
    //}

    //public void OnBeat()
    //{
    //    currentSize = startSize * beatSizeMultiplyier;
    //}
}
