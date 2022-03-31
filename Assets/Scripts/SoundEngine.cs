using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEngine : MonoBehaviour
{
    public bool fireActive = false;
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
}
