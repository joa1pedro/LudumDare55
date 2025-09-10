using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public enum Scene
    {
        MainMenu,
        Options,
        Inventory
    }
    
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _optionsScreen;

    [SerializeField] private GameObject _enableButton;
    [SerializeField] private GameObject _disableButton;

	private Scene _currentScene;

    private void Start()
    {
        ShowMainMenu();
    }

    public void Return(){
        if(_currentScene == Scene.Inventory){
		    ShowMainMenu();
        }
        if(_currentScene == Scene.Options){
            ShowMainMenu();
        }
	}

    public void LoadScene()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void ShowSummonsSelect()
    {
        _currentScene = Scene.Inventory;
        _loadingScreen.SetActive(true);
        _mainMenu.SetActive(false);
        _optionsScreen.SetActive(false);
    }   
    
    public void ShowMainMenu()
    {
        _currentScene = Scene.MainMenu;
        _loadingScreen.SetActive(false);
        _mainMenu.SetActive(true);
        _optionsScreen.SetActive(false);
    }
    
    public void ShowOptionsMenu()
    {
        _currentScene = Scene.Options;
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