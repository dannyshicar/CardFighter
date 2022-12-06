using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnergy 
{
    float _currentEnergy;
    float _currentMaxEnergy;
    float _energyRegenSpeed; // amount per second
    bool _pauseEnergyRegen;

    public UnitEnergy(float energy, float maxEnergy, float energyRegenSpeed)
    {
        this._currentEnergy = energy;
        this._currentMaxEnergy = maxEnergy;
        this._energyRegenSpeed = energyRegenSpeed;
        this._pauseEnergyRegen = false;
    }
    public float Energy{
        get
        {
            return _currentEnergy;
        }
        set
        {
            _currentEnergy = value;
        }
    }
    public float MaxEnergy{
        get
        {
            return _currentMaxEnergy;
        }
        set
        {
            _currentMaxEnergy = value;
        }
    }
    public float EnergyRegenSpeed{
        get
        {
            return _energyRegenSpeed;
        }
        set
        {
            _energyRegenSpeed = value;
        }
    }

    public void useEnergy(float energyCost)
    {
        if(_currentEnergy - energyCost <= 0)
        {
            _currentEnergy = 0;
        }
        else
        {
            _currentEnergy -= energyCost;
        }
    }

    public void regenEnergy()
    {
        if(_currentEnergy < _currentMaxEnergy && !_pauseEnergyRegen)
        {
            _currentEnergy += _energyRegenSpeed;
        }
    }
}
