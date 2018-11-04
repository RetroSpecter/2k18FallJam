using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    [Range(0.0f, 1.0f)]
    public static float masterVolume = 1;
    public static float musicVolume = 1;
    public static float sfxVolume = 1;

    // Use this for initialization
    void Awake() {
        // Singleton pattern (making sure object carries through levels as well as not resetting on every scene)
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

		foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * masterVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}
	
	public void Play(string name)
    {
        // lambda expressions are something i picked up in python, but learned can be applied in c# too
        // basically "sound" is the name of the Sound object we're trying to access, kinda like in a foreach loop
        // sound.name is the value we're comparing to the name we were passed in the paramenter. 
        // Arry.Find will check the sounds array to see if any Sound object's name matches the name parameter
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            return s.source.isPlaying;
        else
            return false;
    }

    void Update()
    {
        // set up the volume for each sound so they adjust with the sliders.
        foreach(Sound s in sounds)
        {
            s.source.volume = (s.source.loop) ? musicVolume * masterVolume : sfxVolume * masterVolume;
        }
    }

    // loop through sounds and make sure they stop playing.
    public void StopAllSounds()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }
}
