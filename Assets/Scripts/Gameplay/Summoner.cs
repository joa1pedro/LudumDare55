using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Summoner : MonoBehaviour
{
    public bool GameEnded = false;
    [SerializeField] public int HP = 100;
    [SerializeField] Text hpText;
    [SerializeField] GameObject defeatScreen;
    [SerializeField] float timeToEnd = 5.0f;


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
            GameEnded = true;
            defeatScreen.SetActive(true);
            Debug.Log("Summoner has been defeated!");
            Invoke("ReturnMenu", timeToEnd);
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