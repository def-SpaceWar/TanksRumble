using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
        public static AudioManager Instance { get; private set; }

        public Sound[] sounds;

        private void Awake()
        {
                if (Instance == null)
                        Instance = this;
                else
                {
                        DestroySelf();
                        return;
                }

                DontDestroyOnLoad(gameObject);

                foreach (var sound in sounds)
                {
                        sound.source = gameObject.AddComponent<AudioSource>();

                        sound.Initialize();
                }
        }

        private void DestroySelf()
        {
                Destroy(gameObject);
                Destroy(this);
        }

        public void Play(string name)
        {
                var s = Array.Find(sounds, sound => sound.name == name);

                s.source.Play();
        }
}
