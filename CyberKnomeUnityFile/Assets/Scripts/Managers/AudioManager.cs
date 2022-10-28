using System;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    void Awake()
    {
        foreach (var sound in Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;
        }
    }

    public void Play(string name)
    {
        foreach (var sound in Sounds)
        {
            if(sound.name.Equals(name)) sound.source.Play();
        }
    }
}
