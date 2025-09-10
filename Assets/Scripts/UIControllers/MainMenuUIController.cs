using System;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuScenes
{
    Main,
    Options,
    Inventory,
}
public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private MenuScenes _initialMenu;
    [Header("Screens")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _inventoryScreen;
    [SerializeField] private GameObject _optionsScreen;

    [Header("Options Screen")]
    [SerializeField] private GameObject _enableButton;
    [SerializeField] private GameObject _disableButton;
    
    [Header("Radial menu")]
    [SerializeField] private RadialMenuController _radialMenu;
    
    [SerializeField] private MainMenuTypingSystem _mainMenuTypingSystem;

	private MenuScenes _currentScene;
    public bool EnabledTyping = true;

    private void Start()
    {
        switch (_initialMenu)
        {
            case MenuScenes.Main:
                ShowMainMenu();
                break;
            case MenuScenes.Options:
                ShowOptionsMenu();
                break;
            case MenuScenes.Inventory:
                ShowInventory();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Return(){
        if(_currentScene == MenuScenes.Inventory){
		    ShowMainMenu();
        }
        if(_currentScene == MenuScenes.Options){
            ShowMainMenu();
        }
	}

    public void LoadScene()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void ShowInventory()
    {
        _currentScene = MenuScenes.Inventory;
        _inventoryScreen.SetActive(true);
        _radialMenu.SetActive(true);
        
        _mainMenu.SetActive(false);
        _optionsScreen.SetActive(false);
        _mainMenuTypingSystem.SetActive(false, true);
    }
    
    public void ShowMainMenu()
    {
        _currentScene = MenuScenes.Main;
        _inventoryScreen.SetActive(false);
        _mainMenu.SetActive(true);
        _optionsScreen.SetActive(false);
        _radialMenu.SetActive(false);
        _mainMenuTypingSystem.SetActive(true);
    }
    
    public void ShowOptionsMenu()
    {
        _currentScene = MenuScenes.Options;
        _inventoryScreen.SetActive(false);
        _mainMenu.SetActive(false);
        _optionsScreen.SetActive(true);
        _radialMenu.SetActive(false);
    }

    public void EnableTyping(bool enable)
    {
        _disableButton.SetActive(enable);
        _enableButton.SetActive(!enable);
    }
}