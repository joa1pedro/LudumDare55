using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ComboSequence Father;

    public void PlayStarAnimation()
    {
        this.gameObject.SetActive(true);
    }

    // Method called from the Timeline
    void EndStarAnimation()
    {
        this.gameObject.SetActive(false);
    }

}
