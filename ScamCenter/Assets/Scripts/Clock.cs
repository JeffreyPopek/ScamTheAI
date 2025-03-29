using System;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private int _hour, _minute;
    private float _second;

    private void Start()
    {
        _hour = 1;
        _minute = 55;
        _second = 0;
    }

    private void Update()
    {
        _second += Time.deltaTime * 3;

        if (_second >= 60)
        {
            _second = 0;
            _minute++;

            if (_minute >= 60)
            {
                _minute = 0;
                _hour++;
            }
        }

        if (_minute < 10)
            timeText.text = _hour + ":0" + _minute + "AM";
        else
            timeText.text = _hour + ":" + _minute + "AM";
    }
}
