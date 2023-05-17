using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject bar;
    private Image image;
   
    private void SetHealth(int health)
    {
        slider.value = health;
    }

    private void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    private void ChangeHealthColor()
    {
        if (slider.value <= 25)
        {
            image.color = Color.red;
        }
        else if (slider.value > 50)
        {
            image.color = Color.green;
        }
        else
        {
            image.color = Color.yellow;
        }
    }

    void Start()
    {
         text.text = slider.value.ToString();
         image = bar.GetComponent<Image>();
        //image.enabled = true;
       
    }

    void Update()
    {
        image.enabled = true;
        text.text = slider.value.ToString();
        ChangeHealthColor();
        
    }
}
