using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public float DeathTime;
    public static GameFlow singleton;
    public Transform GameOverScreen;
    public HealthBar HPBar;
    public Text TimerText;
    public float time;
    public int TreeThreshold;
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
        int minutes = (int)Mathf.Floor(time / 60);
        int seconds = (int)Mathf.RoundToInt(time%60);
        
        TimerText.text = (minutes<10? "0" + minutes.ToString() : minutes.ToString()) + ":" + (seconds<10? "0" + seconds.ToString() : seconds.ToString());

        time += Time.deltaTime;
        HPBar.updateHealth((float)World.singleton.TreesAmount/(World.singleton.TreesToPlace + 4));
        if(World.singleton.TreesAmount <= TreeThreshold)
        {
            GameOverScreen.gameObject.SetActive(true);
            GameOverScreen.GetComponentInChildren<Text>().text = TimerText.text;
            //Disables fire spawning
            World.singleton.enabled = false;
            enabled = false;
            //StartCoroutine("ResetTimer");
        }

    }

    IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(DeathTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
