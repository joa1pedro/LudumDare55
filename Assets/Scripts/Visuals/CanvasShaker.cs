using System.Collections;
using UnityEngine;

public class CanvasShaker : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeAmount = 10f;
    [SerializeField] public int Context;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void ShakeCanvas()
    {
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeAmount;
            transform.position = new Vector3(randomPoint.x, randomPoint.y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
}