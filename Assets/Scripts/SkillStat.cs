using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillStat
{
    [SerializeField] int _skillIdx;
    [SerializeField] string _skillName;
    [SerializeField] Sprite _skillSprite;
    [SerializeField] int _baseDamage;
    [SerializeField] float _rangeX;
    [SerializeField] float _rangeY;
    [SerializeField] float _rangeZ;
    [SerializeField] float _skillDelay;
    [SerializeField] float _skillDuration;
    [SerializeField] float _energyCost;
    [SerializeField] string _skillDescription;

    public int skillIdx
    {
        get
        {
            return _skillIdx;
        }
    }

    public string skillName
    {
        get
        {
            return _skillName;
        }
    }

    public Sprite skillSprite
    {
        get
        {
            return _skillSprite;
        }
    }

    public int baseDamage
    {
        get
        {
            return _baseDamage;
        }
    }

    public float rangeX
    {
        get
        {
            return _rangeX;
        }
    }

    public float rangeY
    {
        get
        {
            return _rangeY;
        }
    }

    public float rangeZ
    {
        get
        {
            return _rangeZ;
        }
    }

    public int range // average range for display on skill-selection-menu
    {
        get
        {
            return (int)((rangeX + rangeY + rangeZ) / 3);
        }
    }

    public float skillDelay
    {
        get
        {
            return _skillDelay;
        }
    }

    public float skillDuration
    {
        get
        {
            return _skillDuration;
        }
    }

    public int energyCost
    {
        get
        {
            return (int)_energyCost;
        }
    }

    public string skillDescription
    {
        get
        {
            const int MAX_LENGTH = 100;
            string padding = "    ";
            if(_skillDescription.Length > MAX_LENGTH - (3 + padding.Length))
            {
                return padding + _skillDescription.Substring(0, MAX_LENGTH - 3) + "...";
            }
            else
            {
                return padding + _skillDescription;
            }
        }
    }


}
