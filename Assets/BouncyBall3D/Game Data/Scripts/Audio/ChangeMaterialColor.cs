using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    [Header("Size")]
    [SerializeField] Transform targetTransform;
    [SerializeField] float sizeMultiplyier;
    float currenSize;
    float startSize;

    [Header("Color")]
    [SerializeField] Material targetMaterial;
    [SerializeField] Color[] colors;

    //private void Awake()
    //{
    //    startSize = targetTransform.localScale.x;
    //}

    //private void Update()
    //{
    //    currenSize = Mathf.Lerp(currenSize, startSize, 0.2f);
    //    targetTransform.localScale = Vector3.one * currenSize;
    //}

    public void ChangeColor()
    {
        targetMaterial.color = colors[Random.Range(0, colors.Length)];
        //currenSize = startSize * sizeMultiplyier;
    }
}
