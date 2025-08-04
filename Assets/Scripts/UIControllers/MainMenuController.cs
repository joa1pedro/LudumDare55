using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject optionsScreen;

    [SerializeField] private GameObject enableButton;
    [SerializeField] private GameObject disableButton;

	private string currentScene;

    private void Start()
    {
        ShowMainMenu();
    }

    public void Return(){
        if(currentScene == "SummonSelect"){
		    ShowMainMenu();
        }
        if(currentScene == "Options"){
            ShowMainMenu();
        }
	}

    public void LoadScene()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void ShowSummonsSelect()
    {
        currentScene = "Options";
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);
        optionsScreen.SetActive(false);
    }
    
    public void ShowMainMenu()
    {
        currentScene = "MainMenu";
        loadingScreen.SetActive(false);
        mainMenu.SetActive(true);
        optionsScreen.SetActive(false);
    }
    
    public void ShowOptionsMenu()
    {
        currentScene = "Options";
        loadingScreen.SetActive(false);
        mainMenu.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void EnableTyping(bool enable)
    {
        disableButton.SetActive(!enable);
        enableButton.SetActive(enable);
    }
}