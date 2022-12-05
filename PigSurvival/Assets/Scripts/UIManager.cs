using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider expSlider;
    public TextMeshProUGUI expText;
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetExp(int expNum, int maxExp)
    {
        if (expSlider)
        {
            expSlider.value = expNum;
            expSlider.maxValue = maxExp;
        }

        expText.text = expNum + "/" + maxExp;
    }
}
