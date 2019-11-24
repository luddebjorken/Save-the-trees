using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public RectTransform Menu;
    public RectTransform Credits;
    bool isTransitioning = false;
    public void BTNStart()
    {
        if(isTransitioning) return;
        SceneManager.LoadScene("InGame");
    }
    public void BTNExit()
    {
        if(isTransitioning) return;
        Application.Quit();
    }
    public void BTNFullScreen()
    {
        if(isTransitioning) return;
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void BTNTeamTrees()
    {
        if(isTransitioning) return;
        Application.OpenURL("https://teamtrees.org/");
    }
    public void BTNCredits()
    {
        if(isTransitioning) return;
        StartCoroutine(Transition(Credits,new Vector3(-2560,0,0), Vector3.zero, 1));
    }

    IEnumerator Transition(RectTransform obj,Vector3 p1, Vector3 p2, float time)
    {
        isTransitioning = true;
        float startTime = Time.time;
        while(Time.time - startTime < time)
        {
            obj.position = Vector3.Lerp(p1,p2,(Time.time- startTime)/time);
            yield return 0;
        }
        isTransitioning = false;
    }
}
