using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    // fixed attributes
    public string name;
    public int baseDamage;
    public float rangeX;
    public float rangeY;
    public float rangeZ;
    public float skillDelay;
    public float skillDuration;
    public float energyCost;
    public int skillPrefabIdx; // to get the prefab from the prefab list


    public Skill(string name, int skillPrefabIdx, int baseDamage, float rangeX, float rangeY, float rangeZ, float skillDelay, float skillDuration, float energyCost)
    {
        this.name = name;
        this.skillPrefabIdx = skillPrefabIdx;
        this.baseDamage = baseDamage;
        this.rangeX = rangeX;
        this.rangeY = rangeY;
        this.rangeZ = rangeZ;
        this.skillDelay = skillDelay;
        this.skillDuration = skillDuration;
        this.energyCost = energyCost;
    }

    public bool InSkillRange(Vector3 skillPos, Vector3 targetPos)
    {
        if(Mathf.Abs(skillPos.x - targetPos.x) <= rangeX && Mathf.Abs(skillPos.y - targetPos.y) <= rangeY && Mathf.Abs(skillPos.z - targetPos.z) <= rangeZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isReady(float currentEnergy){
        if(currentEnergy >= energyCost){
            return true;
        }
        else{
            return false;
        }
    }
}
