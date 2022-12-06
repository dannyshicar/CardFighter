using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    const int MAX_SKILL_NUM = 3;
    [SerializeField] List<Image> skillIcons;
    [SerializeField] List<Image> skillIconPreviews;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < MAX_SKILL_NUM; i++)
        {
            skillIcons[i].fillAmount = 0;
        }
    }

    public void SetImage(List<Sprite> skillIconSprites)
    {
        for(int i = 0; i < MAX_SKILL_NUM; i++)
        {
            skillIcons[i].GetComponent<Image>().sprite = skillIconSprites[i];
            skillIconPreviews[i].GetComponent<Image>().sprite = skillIconSprites[i];
        }
    }

    public void updateFillAmount(int skillIdx, float playerEnergy, float skillEnergyCost)
    {
        if(playerEnergy >= skillEnergyCost)
        {
            skillIcons[skillIdx].fillAmount = 1;
        }
        else
        {
            skillIcons[skillIdx].fillAmount = playerEnergy / skillEnergyCost;
        }
    }
}
