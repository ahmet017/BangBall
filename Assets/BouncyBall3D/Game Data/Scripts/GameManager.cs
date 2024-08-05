using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int score = 0;
    public int bestScore = 0;
    public int star = 0;

    private int gameSpeed = 1;
    public float GameSpeed => ((gameSpeed - 1) * 0.2f) + 1;
    public GameState CurrentGameState => gameState;

    GameState gameState;
    Player player;
    float songProgress = 0;

    [Header("UI")]
    [SerializeField] Image levelProgress;
    [SerializeField] Text scoreText;
    [SerializeField] Animator scoreAnim;
    [SerializeField] Animator reviveAnim;
    [SerializeField] GameObject revivePanel, playButton;
    [Space]
    [SerializeField] Text songName;
    [SerializeField] Text levelScore;
    [SerializeField] Image[] stars = new Image[3];
    [SerializeField] Color activeStars, inactiveStars;

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
        bestScore = PlayerPrefs.GetInt("bestScore", 0);

        if (ServicesManager.instance != null)
        {
            ServicesManager.instance.InitializeAdmob();
            ServicesManager.instance.InitializeUnityAds();
        }
    }

    private void Start()
    {
        scoreText.text = score.ToString();
        if (scoreAnim.isActiveAndEnabled)
            scoreAnim.SetTrigger("Up");
    }

    public void PlayerFailed()
    {
        if (ServicesManager.instance != null)
        {
            ServicesManager.instance.ShowInterstitialAdmob();
            ServicesManager.instance.ShowInterstitialUnityAds();
        }
        gameState = GameState.Lost;
        SoundManager.Instance.StopTrack();
        revivePanel.SetActive(true);
        UIManager.Instance.ShowHUD(false);

        if (LevelGenerator.Instance.currentSong.stars < star)
        {
            LevelGenerator.Instance.currentSong.stars = star;
            LevelGenerator.Instance.currentSong.SaveData();
        }

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("bestScore", score);
        }
        ShowLevelProgress();

        PlayerPrefs.Save();
    }

    void ShowLevelProgress()
    {
        songName.text = LevelGenerator.Instance.currentSong.name;
        levelScore.text = score.ToString();
        for (int i = 0; i < 3; i++)
        {
            if (i < star)
                stars[i].color = activeStars;
            else
                stars[i].color = inactiveStars;
        }
    }

    public void Revive()
    {
        if (ServicesManager.instance != null)
        {
            ServicesManager.instance.ShowRewardedVideoAdAdmob();
            ServicesManager.instance.ShowRewardedVideoUnityAds();
        }
    }

    public void ReviveSucceed(bool completed)
    {
        if (completed)
        {
            player.Revive();
            revivePanel.SetActive(false);
            playButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        if (CurrentGameState == GameState.Menu)
        {
            star = 0;
            gameState = GameState.Gameplay;
            player.StartMoving();
            SoundManager.Instance.PlayMusicFromBeat(player.platformHitCount);
        }
        else if (CurrentGameState == GameState.Lost)
        {
            gameState = GameState.Gameplay;
            player.StartMoving();
            SoundManager.Instance.PlayMusicFromBeat(player.platformHitCount);
        }
        UIManager.Instance.ShowHUD(true);
    }

    public void IncreaseGameSpeed()
    {
        if (gameSpeed < 5)
            gameSpeed++;
    }

    public void AddScore(bool perfect)
    {
        if (perfect)
            score += 10;
        else
            score += 5;

        scoreText.text = score.ToString();
        scoreAnim.SetTrigger("Up");
    }

    public void UpdateSongProgress(float value)
    {
        songProgress = value;
        levelProgress.fillAmount = Mathf.Lerp(levelProgress.fillAmount, value, 0.1f);
    }

    public void NoThanks()
    {
        reviveAnim.SetTrigger("No");
        
        if (ServicesManager.instance != null)
        {
            ServicesManager.instance.ShowInterstitialAdmob();
            ServicesManager.instance.ShowInterstitialUnityAds();
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
