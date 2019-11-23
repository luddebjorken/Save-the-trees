using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public float FillSpeed = 0.5f;
    [SerializeField]
    private float targetHealth = 1;
    
    private void Awake(){
        slider = gameObject.GetComponent<Slider>();
    }
    
    // Start is called before the first frame update
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value < targetHealth && slider.value + 0.01f < targetHealth){
            slider.value += FillSpeed * Time.deltaTime;
        } else if (slider.value > targetHealth &&slider.value - 0.01f > targetHealth){
            slider.value -= FillSpeed * Time.deltaTime;
        }
        
    }

    public void updateHealth(float newHealth){
        targetHealth = newHealth;
    }
}


