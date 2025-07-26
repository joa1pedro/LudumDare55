using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningController : MonoBehaviour
{
    [SerializeField] List<Summon> Summons = new List<Summon>(); 

    public void PerformSummon(int index)
    {
        Summons[index].SummonSelf(index);
    }
}
