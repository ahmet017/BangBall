using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject HUDPanel;
    [SerializeField] Animator menuAnim;
    [SerializeField] GameObject gameUI;
    [SerializeField] Text bestScoreText;

    public void CloseMenu()
    {
        menuAnim.SetTrigger("Close");
        gameUI.SetActive(true);
    }

    private void OnEnable()
    {
        bestScoreText.text = PlayerPrefs.GetInt("bestScore", 0).ToString();
    }

    public void ShowHUD(bool value)
    {
        HUDPanel.SetActive(value);
    }
}
