using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public float DeathTime;
    public static GameFlow singleton;
    void Awake()
    {
        singleton = this;
        enabled = false;
    }

    void Update()
    {
        if(World.singleton.TreesAmount <= 0)
        {
            //Disables fire spawning
            World.singleton.enabled = false;
        }
    }

    IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(DeathTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
