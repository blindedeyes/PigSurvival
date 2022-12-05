using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceObject : MonoBehaviour
{
    AudioSource source;
    bool waitingForPlayingFinish = false;
    private void Init()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioSourceClip clipToPlay)
    {
        if(source == null)
        {
            Init();
        }
        waitingForPlayingFinish = true;
        source.clip = clipToPlay.clip;
        source.loop = clipToPlay.Loop;
        source.outputAudioMixerGroup = clipToPlay.group;
        source.Play();

    }

    private void Update()
    {
        if (waitingForPlayingFinish && !source.isPlaying)
        {
            waitingForPlayingFinish = false;
            AudioMaster.Instance.FreeAudioSource(this);
        }
    }
}
