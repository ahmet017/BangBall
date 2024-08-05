using UnityEngine;
using UnityEngine.UI;

public class SongHolder : MonoBehaviour
{
    [SerializeField] Song song;
    [SerializeField] Text songName;
    [SerializeField] Image[] stars = new Image[3];
    [SerializeField] Color activeStars, inactiveStars;

    public void SetSong(Song newSong)
    {
        song = newSong;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        song.LoadData();

        for (int i = 0; i < 3; i++)
            stars[i].color = i < song.stars ? activeStars : inactiveStars;

        songName.text = song.name;
    }

    public void PlaySong()
    {
        LevelGenerator.Instance.currentSong = song;
        LevelGenerator.Instance.StartWithSong();
        UIManager.Instance.CloseMenu();
    }
}
