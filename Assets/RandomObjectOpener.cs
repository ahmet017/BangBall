using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectOpener : MonoBehaviour
{
    public List<GameObject> objectsToOpen;
    private List<GameObject> shuffledObjects;
    private int currentIndex = 0;

    void Start()
    {
        // Objelerin s�ras�n� kar��t�r
        ShuffleObjects();

        // �lk objeyi a�
        OpenNextObject();
    }

    void ShuffleObjects()
    {
        shuffledObjects = new List<GameObject>(objectsToOpen);
        int n = shuffledObjects.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            GameObject temp = shuffledObjects[k];
            shuffledObjects[k] = shuffledObjects[n];
            shuffledObjects[n] = temp;
        }
    }

    public void OpenNextObject()
    {
        if (currentIndex < shuffledObjects.Count)
        {
            GameObject nextObject = shuffledObjects[currentIndex];
            nextObject.SetActive(true);
            currentIndex++;
        }
        else
        {
            Debug.Log("T�m objeler a��ld�!");
        }
    }
}
