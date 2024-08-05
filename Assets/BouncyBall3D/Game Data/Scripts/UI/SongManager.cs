using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] GameObject songHolderPrefab;
    [SerializeField] Transform contentHolder;
    [SerializeField] Song[] songs;

    private void Awake()
    {
        songs = Resources.LoadAll<Song>("Songs");

        foreach (Song song in songs)
        {
            Instantiate(songHolderPrefab, contentHolder).GetComponent<SongHolder>().SetSong(song);
        }
    }
}
