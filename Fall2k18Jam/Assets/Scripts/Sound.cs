using UnityEngine;

[System.Serializable] // makes classes that don't inherit from MonoBehaviour show up in the inspector
public class Sound {

    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
