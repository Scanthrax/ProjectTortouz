using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Music { Menu, Barn, Hydro, ColdStorage }

public class MusicManager : MonoBehaviour
{


    public static MusicManager instance;

    public AudioSource musicPlayer;

    public List<EnumToMusic> musicMappings;

    public Dictionary<Music, AudioClip> musicDict = new Dictionary<Music, AudioClip>();

    public Music levelMusic = Music.Menu;

    [System.Serializable]
    public struct EnumToMusic
    {
        public Music music;
        public AudioClip audio;
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

}
