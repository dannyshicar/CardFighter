using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider _energySlider;

    public void setMaxEnergy(float maxEnergy)
    {
        _energySlider.maxValue = maxEnergy;
        _energySlider.value = maxEnergy;
    }

    public void setEnergy(float energy)
    {
        _energySlider.value = energy;
    }
}
