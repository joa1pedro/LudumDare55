using System;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    
    /// <summary>
    /// Level Select Method to be used on Button hookups inside the Scene
    /// </summary>
    /// <param name="level"></param>
    public void SelectLevel(String level)
    {
        LoadLevel(level);
    }

    private void LoadLevel(String levelName)
    {
        // TODO Load the Level from file
    }
}