using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float runTime = 0;
    private float minutes = 0;
    private float seconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        runTime = 0;
    }

    private void SetTime()
    {
        runTime += Time.deltaTime;
        seconds += Time.deltaTime;

        if (seconds >= 60)
        {
            minutes++;
            seconds = 0;
        }

        if (timerText)
        {
            timerText.SetText(ClockFormat(minutes) + ":" + ClockFormat(seconds));
        }
    }

    private string ClockFormat(float val)
    {
        
        int num = Mathf.FloorToInt(val);
        string clockString = num.ToString();

        if (val < 10)
        {
            clockString = "0" + num;
        }

        return clockString;
    }

    // Update is called once per frame
    void Update()
    {
        SetTime();
    }
}
