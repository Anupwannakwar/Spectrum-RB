using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public bool IsPlayerHealthBar = true;

    public Slider slider;

    public Image image;

    private float _health = 1f;

    private void Update()
    {
        if(image != null)
        {
            if (image.fillAmount > _health && IsPlayerHealthBar) image.fillAmount -= 0.01f;
        }
    }

    public void setHealth(float health)
    {
        _health = health / 100f;
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setBossHealth(int health)
    {
        slider.value = health;
    }

    public void setPowerBar(int value)
    {
        slider.value = value;
    }

    public void setMaxPowerBar(int value)
    {
        slider.maxValue = value;
    }
}
