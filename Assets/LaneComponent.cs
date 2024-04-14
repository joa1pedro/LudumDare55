using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneComponent : MonoBehaviour
{
    [SerializeField] List<GameObject> Effects = new List<GameObject>();

    public void Initialize()
    {
        foreach (GameObject go in Effects)
        {
            go.SetActive(false);
        }
    }

    public void ActivateEffect(int index)
    {
        Effects[index].SetActive(true);
    }

}
