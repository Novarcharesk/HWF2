using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI _scoreTMP;
    private int _score;

    private void Start()
    {
        _scoreTMP = GetComponent<TextMeshProUGUI>();

        _scoreTMP.text = "Score: 0";
        _score = 0;
    }

    private void IncrementScore()
    {
        _score++;
        _scoreTMP.text = $"Score: {_score}";
    }

    private void OnEnable()
    {
        EventManager.OnScored += IncrementScore;
    }
    private void OnDisable()
    {
        EventManager.OnScored -= IncrementScore;
    }

}
