using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Music { Menu, Barn, Hydro, ColdStorage, Credits, Comic}

public class MusicManager : MonoBehaviour
{


    public static MusicManager instance;

    public AudioSource musicPlayer;

    public List<EnumToMusic> musicMappings;

    public Dictionary<Music, AudioClip> musicDict = new Dictionary<Music, AudioClip>();

    public Music levelMusic = Music.Menu;


    bool switchSong, switching;

    Music switchMusicTo;

    [System.Serializable]
    public struct EnumToMusic
    {
        public Music music;
        public AudioClip audio;
    }


    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    SwitchMusic(Music.ColdStorage);
        //}
        


        if(switchSong)
        {
            switching = true;
            switchSong = false;
        }

        if(switching)
        {


            if(musicPlayer.volume > 0f)
            {
                musicPlayer.volume -= Time.deltaTime * 0.6f;
            }

            if (musicPlayer.volume == 0f)
            {
                musicPlayer.volume = 1f;
                musicPlayer.clip = musicDict[switchMusicTo];
                musicPlayer.Play();
                switching = false;
            }
        }
    }


    private void Awake()
    {
        instance = this;


        musicPlayer = GetComponent<AudioSource>();


        foreach (var item in musicMappings)
        {
            if (!musicDict.ContainsKey(item.music))
                musicDict.Add(item.music, item.audio);
        }


        DontDestroyOnLoad(gameObject);
        PlaySong(Music.Menu);

        switchSong = false;
    }


    public void PlaySong(Music music)
    {
        musicPlayer.clip = musicDict[music];
        musicPlayer.Play();
    }


    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SwitchMusic(Music music)
    {
        if (musicPlayer.clip == musicDict[music]) return;

        switchMusicTo = music;
        switchSong = true;
    }



}
