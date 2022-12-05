using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "Data", menuName = "Audio SO/AudioSourceSO", order = 1)]
public class AudioSourceSO : ScriptableObject
{
    public AudioSourceClip[] clipPool;
}

[Serializable]
public class AudioSourceClip
{
    public AudioClip clip;
    public UnityEngine.Audio.AudioMixerGroup group;
    public bool Loop = false;

    //TODO ADD
    //flat additive values

    //Curves over sound time.
}
