using UnityEngine;

public class SavingHandler : Singleton<SavingHandler>
{
    public int bestScore = 0;

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    public void LoadData()
    {
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
    }

    public void SaveData()
    {
        PlayerPrefs.GetInt("bestScore", bestScore);
    }
}
