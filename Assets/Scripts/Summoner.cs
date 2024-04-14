using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Summoner : MonoBehaviour
{
    [SerializeField] public int HP = 100;
    [SerializeField] Text hpText;

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
            // TODO END GAME
            Debug.Log("Summoner has been defeated!");
            SceneManager.LoadScene("Menu");
        }
    }

    void UpdateHPText()
    {
        hpText.text = "HP: " + HP;
    }
}