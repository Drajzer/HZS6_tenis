using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private float Volume = 1.0f;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip[] clips;
    int i = 0;

    // Update is called once per frame
    void Update()
    {
        source.volume = Volume;
        if (!source.isPlaying && Time.timeScale > 0)
        {
            source.clip = clips[i];
            source.Play();
            i++;
            if (i == clips.Length)
            {
                i = 0;
            }
        }
    }
}
