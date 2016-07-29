using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource effectsSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

	void Awake () {

        // singleton check
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        // sustains singleton between scenes
        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingle ( AudioClip clip )
    {
        effectsSource.clip = clip;
        effectsSource.Play();
    }

    public void RandomizeSoundEffects ( params AudioClip [] clips )
    {
        // Set random pitch
        effectsSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        // Pick random clip
        effectsSource.clip = clips[Random.Range(0, clips.Length)];
        effectsSource.Play();
    }
}
