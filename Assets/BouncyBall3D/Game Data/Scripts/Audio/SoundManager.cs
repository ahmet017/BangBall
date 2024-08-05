using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource SFX;
    [SerializeField] AudioSource track;
    [SerializeField] AudioMixer mainMixer;

    [SerializeField] AudioClip blip, blop;
    int bitCount = 0;

    private void Start()
    {
        Sounds(PlayerPrefs.GetInt("Sound", 1) == 1 ? true : false);
    }

    public void BitSound()
    {
        if (bitCount == 0)
            SFX.PlayOneShot(blip);
        else
            SFX.PlayOneShot(blop);

        bitCount++;
        if (bitCount > 3)
            bitCount = 0;
    }

    private void Update()
    {
        mainMixer.SetFloat("Music Pitch", GameManager.Instance.GameSpeed);
        GameManager.Instance.UpdateSongProgress(track.time / track.clip.length);
    }

    public void PlayMusic()
    {
        track.clip = LevelGenerator.Instance.currentSong.song;
        track.Play();
    }

    public void PlayMusicFromBeat(int beat)
    {
        track.clip = LevelGenerator.Instance.currentSong.song;
        track.time = LevelGenerator.Instance.currentSong.TimeFromBeat(beat - 1);
        track.Play();
    }

    public void StopTrack()
    {
        track.Pause();
    }

    public void Sounds(bool on)
    {
        mainMixer.SetFloat("Music Volume", on ? 0 : -80);
    }
}
