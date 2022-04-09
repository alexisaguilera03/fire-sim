using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEngine : MonoBehaviour
{
    public bool fireActive = false;

    private bool priority = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundEffect(AudioSource sound, bool loop, bool duplicateAllowed) //duplicate allowed is boolean that will play a sound effect multiple times if true
    {
        if (!duplicateAllowed && sound.isPlaying) return;
        if (priority) return;
        sound.loop = loop;
        sound.PlayOneShot(sound.clip); ;
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

    void waitUntilDone(AudioSource sound)
    {
        while (sound.isPlaying)
        {
            //do nothing
        }

        priority = false;
        //todo: restore sound effects?
    }

    public bool checkPlaying(AudioSource sound)
    {
        return sound.isPlaying;
    }
}
