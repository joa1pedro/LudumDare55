using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Summoner : MonoBehaviour
{
    [SerializeField] public int HP = 100;
    [SerializeField] Text hpText;
    [SerializeField] GameObject defeatScreen;

    void Start()
    {
        UpdateHPText();
    }

    public void ReduceHP(int damage)
    {
        HP -= damage;
        UpdateHPText();
        if (HP <= 90)
        {
            // TODO END GAME
            defeatScreen.SetActive(true);
            Debug.Log("Summoner has been defeated!");
            Invoke("ReturnMenu", 2.0f);
        }
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void UpdateHPText()
    {
        hpText.text = "HP: " + HP;
    }
}