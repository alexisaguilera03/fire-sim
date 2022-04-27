using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEngine : MonoBehaviour
{
    public bool fireActive = false;
    public bool mute = false;
    private AudioSource fireAudioSource;
    public AudioClip FireAudioClip;

    private bool priority = false;
    private bool checkDuplicatesOnStart = false;

    private List<AudioSource> duplicateAudioSources = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        fireAudioSource = gameObject.GetComponent<AudioSource>();
        FireAudioClip = fireAudioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {
        preventDuplicateFireAudio();
        if (mute)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.isPlaying)
                {
                    StopSound(audioSource);
                }
            }
        }

        if (fireActive && !fireAudioSource.isPlaying)
        {
            PlaySoundEffect(fireAudioSource, true, false);
        }
    }

    void preventDuplicateFireAudio()
    {
        bool found = false;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying && audioSource.clip.name == FireAudioClip.name && !found)
            {
                found = true;
                continue;
            } 
            if (audioSource.isPlaying && audioSource.clip.name == FireAudioClip.name && found)
            {
                audioSource.Stop();
            }
        }
    }
    
    public void PlaySoundEffect(AudioSource sound, bool loop, bool duplicateAllowed) //duplicate allowed is boolean that will play a sound effect multiple times if true
    {
        /*if (!duplicateAllowed)
        {
            duplicateAudioSources.Add(sound);
            if (!checkDuplicatesOnStart)
            {
                checkDuplicatesOnStart = true;
                StartCoroutine(removeDuplicateSounds());
            }
            

        }
        */
        if (!duplicateAllowed && sound.isPlaying) return;
        if (sound.clip.name == "Fire_Sound") fireAudioSource = sound;
        if (priority) return;
        sound.loop = loop;
        sound.PlayOneShot(sound.clip);
    }

    public void PlayMusic(AudioSource sound, bool loop)
    {
        sound.loop = loop;
        sound.PlayOneShot(sound.clip);
    }


    public void StopSound(AudioSource source)
    {
        source.Stop();
    }

    public void PlaySoundEffectPriority(AudioSource sound, bool loop)
    {
        if (sound.isPlaying) return;
        priority = true;
        sound.loop = loop;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }
        sound.PlayOneShot(sound.clip);
    }



}
