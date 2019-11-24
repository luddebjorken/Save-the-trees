using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    public static Currency singleton;
    public Text CurrencyFadeText;
    public float CurrencyFadeTextDuration;
    public Text CurrencyText;
    public int CurrentCurrency;
    public int MaxCurrencyPerTick;
    public float tickLength;
    private Coroutine CoinRoutine;
    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        CoinRoutine = StartCoroutine("CoinTick");
    }

    void Update()
    {
        CurrencyText.text = CurrentCurrency.ToString();
    }

    IEnumerator CoinTick()
    {
        while(enabled)
        {   
            yield return new WaitForSeconds(tickLength);
            int increase = (int)(((float)World.singleton.TreesAmount/World.singleton.TreesToPlace)*MaxCurrencyPerTick);
            increase = Mathf.Max(1,increase);
            CurrentCurrency += increase;
            StartCoroutine(FadeText(Instantiate(CurrencyFadeText, CurrencyText.transform.position, Quaternion.identity, CurrencyText.transform), increase));
        }
    }

    IEnumerator FadeText(Text textObject, int ammount)
    {
        textObject.transform.localPosition = Vector3.zero;
        textObject.text = "+" + ammount.ToString();
        float startTime = Time.time;
        while(Time.time - startTime < CurrencyFadeTextDuration)
        {
            yield return 0;
            textObject.color = Color.Lerp(Color.white, Color.clear, (Time.time - startTime) / CurrencyFadeTextDuration);
            textObject.transform.Translate(new Vector3(0,0.2f*CurrencyFadeTextDuration/(Time.time - startTime)), Space.Self);
        }
        Destroy(textObject.gameObject);
    }

    public bool CanAfford(int ammount)
    {
        if(CurrentCurrency - ammount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Pay(int ammount)
    {
        if(CurrentCurrency - ammount >= 0)
        {
            CurrentCurrency -= ammount;
            return true;
        }
        else
        {
            return false;
        }
    }


}
