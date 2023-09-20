using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip buttonSound;

    public void PlayButton()
    {
        audio.clip = buttonSound;
        audio.Play();
    }
}
