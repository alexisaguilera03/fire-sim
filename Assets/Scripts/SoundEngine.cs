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

    public void PlaySoundEffect(AudioSource sound)
    {
        sound.Play();
    }

    public void PlayMusic(AudioSource sound)
    {
        sound.Play();
    }
}
