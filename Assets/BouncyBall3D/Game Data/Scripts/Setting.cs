using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] string settingName;
    [SerializeField] GameObject on, off;

    bool isOn = false;

    private void Awake()
    {
        isOn = PlayerPrefs.GetInt(settingName, 1) == 1;

        on.SetActive(isOn);
        off.SetActive(!isOn);
    }

    public void ChangeSetting()
    {
        isOn = !isOn;

        on.SetActive(isOn);
        off.SetActive(!isOn);

        PlayerPrefs.SetInt(settingName, isOn ? 1 : 0);

        SoundManager.Instance.Sounds(isOn);
    }
}
