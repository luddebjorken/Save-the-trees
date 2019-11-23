using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public float DeathTime;
    public static GameFlow singleton;
    public HealthBar HPBar;
    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        HPBar = FindObjectOfType<HealthBar>();
    }

    void Update()
    {
        HPBar.updateHealth((float)World.singleton.TreesAmount/(World.singleton.TreesToPlace + 4));
        if(World.singleton.TreesAmount <= 0)
        {
            //Disables fire spawning
            World.singleton.enabled = false;
            enabled = false;
            StartCoroutine("ResetTimer");
        }

    }

    IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(DeathTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
