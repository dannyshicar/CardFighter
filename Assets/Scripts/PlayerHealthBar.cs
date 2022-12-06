using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void setHealth(int _health) 
    {
        healthSlider.value = _health;
    }
}