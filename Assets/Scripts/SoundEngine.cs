using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEngine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundEffect(AudioSource sound, bool loop)
    {
        sound.loop = loop;
        sound.Play();
    }

    public void PlayMusic(AudioSource sound, bool loop)
    {
        sound.loop = loop;
        sound.Play();
    }


    public void StopSound(AudioSource source)
    {
        source.Stop();
    }
}
