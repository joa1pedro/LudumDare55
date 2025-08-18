using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _optionsScreen;

    [SerializeField] private GameObject _enableButton;
    [SerializeField] private GameObject _disableButton;

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
        _loadingScreen.SetActive(true);
        _mainMenu.SetActive(false);
        _optionsScreen.SetActive(false);
    }
    
    public void ShowMainMenu()
    {
        currentScene = "MainMenu";
        _loadingScreen.SetActive(false);
        _mainMenu.SetActive(true);
        _optionsScreen.SetActive(false);
    }
    
    public void ShowOptionsMenu()
    {
        currentScene = "Options";
        _loadingScreen.SetActive(false);
        _mainMenu.SetActive(false);
        _optionsScreen.SetActive(true);
    }

    public void EnableTyping(bool enable)
    {
        _disableButton.SetActive(enable);
        _enableButton.SetActive(!enable);
    }
}