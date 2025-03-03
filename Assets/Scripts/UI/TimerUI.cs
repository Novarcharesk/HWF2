using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public class TimerUI : MonoBehaviour
{
    [SerializeField] private float _startTime;

    private float _timer;
    private TextMeshProUGUI _timerTMP;

    private void Start()
    {
        _timerTMP = GetComponent<TextMeshProUGUI>();

        _timerTMP.text = _startTime.ToString("00");
        _timer = _startTime;
    }

    private void Update()
    {
        if (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            _timerTMP.text = _timer.ToString("00");
        }
        else
        {
            EventManager.OnTimerEnd?.Invoke();
        }
    }
}
