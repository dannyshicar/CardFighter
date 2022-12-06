using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider _energySlider;
    [SerializeField] private TMPro.TextMeshProUGUI _energyText;
    private string energyText = "N";
    private string maxEnergyText = "A";

    public void setMaxEnergy(float maxEnergy)
    {
        _energySlider.maxValue = maxEnergy;
        _energySlider.value = maxEnergy;
        maxEnergyText = ((int)maxEnergy).ToString();
        _energyText.text = energyText + " / " + maxEnergyText;
    }

    public void setEnergy(float energy)
    {
        _energySlider.value = energy;
        energyText = ((int)energy).ToString();
        _energyText.text = energyText + " / " + maxEnergyText;
    }
}
