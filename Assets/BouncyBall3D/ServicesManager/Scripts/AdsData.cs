using UnityEngine;

public class AdsData : MonoBehaviour
{
    public static bool AdmobIsEnable
    {
        get
        {
            if (PlayerPrefs.GetString("AdmobIsEnable") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value == true)
                PlayerPrefs.SetString("AdmobIsEnable", "true");
            else
                PlayerPrefs.SetString("AdmobIsEnable", "false");
            PlayerPrefs.Save();
        }
    }
    public static bool UnityAdsIsEnable
    {
        get
        {
            if (PlayerPrefs.GetString("UnityAdsIsEnable") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value == true)
                PlayerPrefs.SetString("UnityAdsIsEnable", "true");
            else
                PlayerPrefs.SetString("UnityAdsIsEnable", "false");
            PlayerPrefs.Save();
        }
    }
    public static bool FacebookIsEnable
    {
        get
        {
            if (PlayerPrefs.GetString("FacebookIsEnable") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value == true)
                PlayerPrefs.SetString("FacebookIsEnable", "true");
            else
                PlayerPrefs.SetString("FacebookIsEnable", "false");
            PlayerPrefs.Save();
        }
    }
    public static bool IAPIsEnable
    {
        get
        {
            if (PlayerPrefs.GetString("IapIsEnable") == "true")
                return true;
            else
                return false;
        }
        set
        {
            if (value == true)
                PlayerPrefs.SetString("IapIsEnable", "true");
            else
                PlayerPrefs.SetString("IapIsEnable", "false");
            PlayerPrefs.Save();
        }
    }
}
