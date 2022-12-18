using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillDatabase : ScriptableObject
{
    public SkillStat[] skillStats;

    public int SkillCount
    {
        get
        {
            return skillStats.Length;
        }
    }

    public SkillStat GetSkill(int idx)
    {
        return skillStats[idx];
    }
}
