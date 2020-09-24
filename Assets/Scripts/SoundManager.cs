using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if(Instance != null)
            DestroyImmediate(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private AudioSource soundSource;
    private AudioSource musicSource;
    
    [SerializeField] private AudioClip[] musicThemes = null;
    
    private int musicIndex = 0;
    
    private void Start()
    {
        InitializeAudioSources();
        if (musicThemes.Length > 0)
            PlayNextMusic();
        SwitchAudio(GameStatus.Sound);
        GameStatus.OnSoundToggle += SwitchAudio;
    }

    private void PlayNextMusic()
    {
        musicIndex = musicIndex != musicThemes.Length - 1 ?
            UnityEngine.Random.Range(0, musicThemes.Length) : 0;
        StartCoroutine(FadeInPlayMusic(musicSource, musicThemes[musicIndex], 2f));
        Invoke("PlayNextMusic", musicSource.clip.length);
    }

    private void InitializeAudioSources()
    {
        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.playOnAwake = false;
        soundSource.loop = false;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void MuteSound(bool status)
    {
        soundSource.mute = status;
        musicSource.mute = status;
        Debug.Log("Mute:" + status);
    }
    
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (!clip)
            return;
        soundSource.pitch = 1;
        soundSource.PlayOneShot(clip, volume);
    }
    
    public void PlaySoundWithPitch(AudioClip clip, float pitch, float volume = 1f)
    {
        soundSource.pitch = pitch;
        soundSource.PlayOneShot(clip, volume);
    }
    
    public void Vibrate(int ms)
    {
        Vibration.Vibrate(ms);
    }

    IEnumerator FadeInPlayMusic(AudioSource source, AudioClip clip, float time)
    {
        float step = .1f;
        source.volume = 0;
        source.clip = clip;
        source.Play();
        while (source.volume < 0.3f)
        {
            source.volume += step * Time.deltaTime / time;
            yield return null;
        }
        source.volume = 0.3f;
    }

    private void SwitchAudio(bool status)
    {
        musicSource.mute = !status;
        soundSource.mute = !status;
    }
    
    private void OnDisable()
    {
        GameStatus.OnSoundToggle -= SwitchAudio;
    }
}
