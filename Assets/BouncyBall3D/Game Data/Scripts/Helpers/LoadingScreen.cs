using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private AsyncOperation currentLoadingOperation;
    private bool isLoading;

    [SerializeField]
    private Image barFillRectTransform;

    [SerializeField]
    private Text percentLoadedText;

    private void Start()
    {
        Show(SceneManager.LoadSceneAsync(1));
    }

    private void Update()
    {
        if (isLoading)
        {
            SetProgress(currentLoadingOperation.progress);

            if (currentLoadingOperation.isDone)
            {
                Hide();
            }
        }
    }

    private void SetProgress(float progress)
    {
        barFillRectTransform.fillAmount = progress;
        percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
    }

    public void Show(AsyncOperation loadingOperation)
    {
        gameObject.SetActive(true);

        currentLoadingOperation = loadingOperation;
        SetProgress(0f);

        isLoading = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        currentLoadingOperation = null;
        isLoading = false;
    }
}