using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    [SerializeField] Text pointsText;

    private float currentPoints;

    private void Start()
    {
        currentPoints = 0;
    }

    private void Update()
    {        
        UpdatePointsText();
    }

    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = currentPoints.ToString();
        }
    }

    public void ReceivePoints(float pointsToReceive)
    {
        currentPoints += pointsToReceive;
    }
}
