using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void BTNStart()
    {
        SceneManager.LoadScene("InGame");
    }

    public void BTNExit()
    {
        Application.Quit();
    }
    public void BTNFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void BTNTeamTrees()
    {
        Application.OpenURL("https://teamtrees.org/");
    }
}
