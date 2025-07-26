using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadingScreen;
    
    /// <summary>
    /// Scene Loader Method to be used on Button hookups inside the Main Menu
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowLevelSelect()
    {
        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void ShowMainMenu()
    {
        loadingScreen.SetActive(false);
        mainMenu.SetActive(true);
    }
}