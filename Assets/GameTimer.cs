using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] Text timerText;

    private float startTime;
    private float elapsedTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
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
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
