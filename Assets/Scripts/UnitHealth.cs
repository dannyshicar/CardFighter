using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    int _currentHealth;
    int _currentMaxHealth;

    public UnitHealth(int health, int maxHealth)
    {
        this._currentHealth = health;
        this._currentMaxHealth = maxHealth;
    }

    public int Health
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return _currentMaxHealth;
        }
        set
        {
            _currentMaxHealth = value;
        }
    }

    public void dmgUnit(int dmgAmount)
    {
        if(_currentHealth - dmgAmount <= 0)
        {
            _currentHealth = 0;
        }
        else
        {
            _currentHealth -= dmgAmount;
        }
    }

    public void healUnit(int healAmount)
    {
        if(_currentHealth + healAmount >= _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
        else
        {
            _currentHealth += healAmount;
        }
    }


}
