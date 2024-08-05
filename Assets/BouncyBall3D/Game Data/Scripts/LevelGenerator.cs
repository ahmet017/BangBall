using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    #region Fields
    [HideInInspector]
    public Song currentSong;
    [Space]
    [HideInInspector] public int platformsPassed = 0;
    [SerializeField] int platformsDrawn = 7;
    [SerializeField] float platformTurnStep = 0.5f;
    [SerializeField] float levelWidth = 10;
    [Range(0f, 1f)]
    [SerializeField] float movingPlatformChance = 0.2f;
    [Space]
    [SerializeField] Player player;
    [SerializeField] Pool platformPool;
    [SerializeField] Pool movingPlatformPool;
    [SerializeField] GameObject starPrefab;

    int[] starIDs = new int[3];
    int songLevel = 0;
    int platformCount;
    bool nextPlatformIsStart = false;
    float lastPlatformZ = 0;
    List<GameObject> platformList = new List<GameObject>();

    public int beatPerSong => (int)((currentSong.song.length / 60f) * currentSong.BPM);
    float distanceBetweenPlatforms => currentSong.song.length / beatPerSong * (player == null ? 10 : player.speed);
    public bool PathIsValid => platformList.Count > 2;

    float TurnStep
    {
        get
        {
            float value = 0;

            if (Random.Range(0f, 1f) > 0.1f)
                value = Random.Range(platformTurnStep / 2, platformTurnStep);
            else
                value = platformTurnStep * 2;

            return value;
        }
    }

    public Transform GetSpecificPlatform(int id)
    {
        return platformList[id].transform;
    }

    public Transform GetNextPlatform
    {
        get
        {
            GameObject platform = platformList[2];
            if (platformList[0].tag == "Moving")
                movingPlatformPool.ReturnItem(platformList[0]);
            else
                platformPool.ReturnItem(platformList[0]);
            platformList.RemoveAt(0);

            GameObject newPlatform = null;
            if (Random.Range(0f, 1f) <= movingPlatformChance && platformCount != beatPerSong - 1)
                newPlatform = movingPlatformPool.GetItem;
            else
                newPlatform = platformPool.GetItem;

            newPlatform.GetComponent<Animator>().SetTrigger("Spawn");
            if (nextPlatformIsStart)
            {
                nextPlatformIsStart = false;
                newPlatform.name = "Start";
            }

            Reposition(newPlatform, platformCount);
            platformList.Add(newPlatform);
            platformCount++;
            platformsPassed++;

            if (platformsPassed >= beatPerSong)
            {
                IncreaseDificulty();
            }

            return platform.transform;
        }
    }
    #endregion

    public void StartWithSong()
    {
        platformCount = 0;
        SetStarIDs();

        for (int i = 0; i < platformsDrawn; i++)
        {
            GameObject newPlatform = platformPool.GetItem;

            Reposition(newPlatform, platformCount);
            platformList.Add(newPlatform);

            platformCount++;
            platformsPassed++;
        }

    }

    void SetStarIDs()
    {
        starIDs[0] = currentSong.BeatFromTime(0.3f);
        starIDs[1] = currentSong.BeatFromTime(0.64f);
        starIDs[2] = currentSong.BeatFromTime(0.99f);
    }

    void IncreaseDificulty()
    {
        platformsPassed = 0;
        lastPlatformZ += 40;
        nextPlatformIsStart = true;
    }

    bool CheckForStar()
    {
        for (int i = 0; i < 3; i++)
        {
            if (platformCount == starIDs[i])
                return true;
        }

        return false;
    }

    public void Reposition(GameObject platform, int id)
    {
        float posX = id > 3 ? Random.Range(-levelWidth, levelWidth) : 0;

        platform.transform.position = new Vector3(posX, platform.transform.position.y, lastPlatformZ);
        platform.transform.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

        lastPlatformZ += distanceBetweenPlatforms;

        if (CheckForStar())
        {
            GameObject star = Instantiate(starPrefab, platform.transform);
        }
    }

    void OnDrawGizmos()
    {
        if (currentSong == null)
            return;
        else if (currentSong.song == null)
            return;

        for (int i = 0; i < beatPerSong; i++)
        {
            Gizmos.DrawSphere(new Vector3(0, 0, distanceBetweenPlatforms * i), 0.5f);
        }
    }
}