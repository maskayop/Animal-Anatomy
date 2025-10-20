using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AnimalAnatomy
{
    [Serializable]
    public class MusicSample
    {
        public string name;
        public AudioClip clip;
    }

    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance;

        [Header("Music")]
        public AudioSource musicSource;
        public AudioMixerGroup musicMixer;
        public float fadeTime = 1.0f;
        public List<MusicSample> musicSamples = new List<MusicSample>();

        [Header("UI")]
        public AudioSource UISource;
        public AudioMixerGroup UIMixer;

        [Header("SFX")]
        public AudioSource SFXSource;
        public AudioMixerGroup SFXMixer;

        [Header("Music")]
        public AudioSource voiceSource;
        public AudioMixerGroup voiceMixer;

        int currentMusic = -1;
        int prevMusicId;

        float currentMusicFadeValue;

        bool isRandomPlaying = true;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create AudioController");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (musicSource.clip != null)
            {
                if (musicSource.isPlaying && musicSource.timeSamples >= musicSource.clip.samples - 1)
                {
                    if (musicSource.loop)
                        return;
                
                    if (isRandomPlaying)
                        PlayRandomMusicClip();
                    else
                        PlayNextMusicClip();
                }
            }
        }

        public void Init()
        {
            if (musicSource.volume > 0)
                PlayRandomMusicClip();
        }

        void PlayMusicClip()
        {
            if (currentMusic < 0)
                currentMusic = musicSamples.Count - 1;
            else if (currentMusic >= musicSamples.Count)
                currentMusic = 0;

            musicSource.Stop();
            musicSource.clip = musicSamples[currentMusic].clip;
            musicSource.Play();
        }

        public void PlayNextMusicClip()
        {
            prevMusicId = currentMusic;

            if (isRandomPlaying)
                PlayRandomMusicClip();
            else
            {
                currentMusic++;
                PlayMusicClip();
            }
        }

        public void PlayPrevMusicClip()
        {
            prevMusicId = currentMusic;
            currentMusic--;
            PlayMusicClip();
        }

        void PlayRandomMusicClip()
        {
            int randomValue = UnityEngine.Random.Range(0, musicSamples.Count);

            if (randomValue == currentMusic)
                PlayNextMusicClip();
            else
            {
                currentMusic = randomValue;
                PlayMusicClip();
            }
        }

        public void SetMusicLoopPlaying(bool state)
        {
            musicSource.loop = state;
        }

        public void PlayCurrentMusic()
        {
            musicSource.UnPause();
        }

        public void PauseCurrentMusic()
        {
            musicSource.Pause();
        }

        public int GetCurrentMusicId()
        {
            return currentMusic;
        }

        public void SetRandomPlaying(bool state)
        {
            isRandomPlaying = state;
        }
    }
}
