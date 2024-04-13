using UnityEngine;
using UnityEngine.UI;

public class Summoner : MonoBehaviour
{
    public int HP = 100;
    public Text hpText;

    void Start()
    {
        UpdateHPText();
    }

    public void ReduceHP(int damage)
    {
        HP -= damage;
        UpdateHPText();
        if (HP <= 0)
        {
            Debug.Log("Summoner has been defeated!");
        }
    }

    void UpdateHPText()
    {
        hpText.text = "HP: " + HP;
    }
}