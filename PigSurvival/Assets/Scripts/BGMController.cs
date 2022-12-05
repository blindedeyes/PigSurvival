using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMController : MonoBehaviour
{
    public static bool Init = false;
    public AudioSourceSO bgm;
    private AudioSourceObject bgmGO;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        if (Init) { Destroy(gameObject); return; }

        bgmGO = AudioMaster.Instance.PlaySound(bgm);
        DontDestroyOnLoad(gameObject);

        UpdateVolumeLevels();
        Init= true;
    }

    public void UpdateVolumeLevels()
    {
        //mixer.SetFloat("BGMVOL", PlayerPrefs.GetFloat("VOLUME"));
        float vol = PlayerPrefs.GetFloat("VOLUME");
        vol = Mathf.Lerp(-20, 20, vol);
        mixer.SetFloat("MASTERVOL", vol);
    }

}
