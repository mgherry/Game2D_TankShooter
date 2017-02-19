using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuGuiManager : IManagerBase<MenuGuiManager>
{
    //Called when a player wins and the HOME button is pressed, or if escape is pressed.
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);  //Loads the menu level.
    }



}
