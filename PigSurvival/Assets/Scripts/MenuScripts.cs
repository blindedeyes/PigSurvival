using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScripts : MonoBehaviour
{
    public Slider volumeSlider;

    public void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("VOLUME");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void VolumeSliderUpdate()
    {
        PlayerPrefs.SetFloat("VOLUME", volumeSlider.value);
        PlayerPrefs.Save();
    }
}
