using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] Summoner summoner;

    private float startTime;
    private float elapsedTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (summoner.GameEnded) return;
        elapsedTime = Time.time - startTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(elapsedTime);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
