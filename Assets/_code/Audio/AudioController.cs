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

        public void Init()
        {
            if (musicSource.volume > 0)
            {
                musicSource.loop = true;
                musicSource.Play();
            }
        }
    }
}
