using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTemplate
{
    // fixed attributes
    public string name;
    public int baseDamage;
    public float skillDelay;
    public float skillDuration;
    public float energyCost;

    public SkillTemplate(string name, int baseDamage, float skillDelay, float skillDuration, float energyCost){
        this.name = name;
        this.baseDamage = baseDamage;
        this.skillDelay = skillDelay;
        this.skillDuration = skillDuration;
        this.energyCost = energyCost;

    }

    
}
