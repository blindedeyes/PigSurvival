using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public int poolSize = 10;
    private static AudioMaster activeInstance = null;
    public static AudioMaster Instance
    {
        get
        {
            if(activeInstance  == null)
            {
                GameObject go = new GameObject("AudioMaster");
                go.AddComponent<AudioMaster>().Init();
                DontDestroyOnLoad(go);
                
            }
            return activeInstance;
        }
    }

    private void OnDestroy()
    {
        activeInstance = null;
    }

    private List<AudioSourceObject> AllSounds = new List<AudioSourceObject>();
    private Queue<AudioSourceObject> unusedaudioSourcePool = new Queue<AudioSourceObject>();

    // Start is called before the first frame update
    void Init()
    {
        //Setup
        if (activeInstance != null)
        {
            Destroy(this);
            return;
        }
        else
            activeInstance = this;


        PopulatePool();
    }


    /// <summary>
    /// Plays sound, returning the audio source used.
    /// If the audio source is a bgm, it must be manually freed.
    /// </summary>
    /// <param name="audio"></param>
    /// <returns></returns>
    public AudioSourceObject PlaySound(AudioSourceSO audio)
    {
        //Play a random clip in the SO
        var clipToPlay = audio.clipPool[UnityEngine.Random.Range(0, audio.clipPool.Length)];

        var sourceObject = unusedaudioSourcePool.Dequeue();

        sourceObject.PlayClip(clipToPlay);

        return sourceObject;
    }


    public void FreeAudioSource(AudioSourceObject source)
    {
        if (!unusedaudioSourcePool.Contains(source))
        {
            unusedaudioSourcePool.Enqueue(source);

            source.transform.parent = this.transform;
            source.transform.localPosition = Vector3.zero;
        }
    }

    private void PopulatePool()
    {
        for (int i = 0; i < poolSize; ++i)
        {

            GameObject newObject = new GameObject();

            var source = newObject.AddComponent<AudioSource>();
            var sourceObj = newObject.AddComponent<AudioSourceObject>();

            DontDestroyOnLoad(newObject);

            AllSounds.Add(sourceObj);
            unusedaudioSourcePool.Enqueue(sourceObj);
        }
    }



}
