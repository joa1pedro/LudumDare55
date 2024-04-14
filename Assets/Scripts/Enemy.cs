using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float dyingPosition = -3.0f;

    private Animator animator;
    private bool walk = true;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (walk)
        {
            this.transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
            if (transform.position.x <= dyingPosition)
            {
                // Optionally, notify the controller or another component that this enemy has "died"
                Debug.Log("Enemy has crossed the map and will be destroyed!");
                Destroy(gameObject); // Self-destruct when crossing the threshold
            }
        }
    }

    public void StopWalk()
    {
        walk = false;
    }

    public void Killed()
    {
        Destroy(gameObject);
    }
}
