using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject optionsScreen;
    
    /// <summary>
    /// Scene Loader Method to be used on Button hookups inside the Main Menu
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowSummonsSelect()
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);
        optionsScreen.SetActive(false);
    }
    
    public void ShowMainMenu()
    {
        loadingScreen.SetActive(false);
        mainMenu.SetActive(true);
        optionsScreen.SetActive(false);
    }
    
    public void ShowOptionsMenu()
    {
        loadingScreen.SetActive(false);
        mainMenu.SetActive(false);
        optionsScreen.SetActive(true);
    }
}