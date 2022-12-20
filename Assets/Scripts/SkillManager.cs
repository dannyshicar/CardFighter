using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public SkillDatabase skillDB;

    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] Image DisplayImage;
    [SerializeField] TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] Slider displayStatSlider1; // damage
    [SerializeField] Slider displayStatSlider2; // range
    [SerializeField] Slider displayStatSlider3; // energy cost
    [SerializeField] Slider displayStatSlider4; // support
    [SerializeField] TMPro.TextMeshProUGUI countRemainSkillSelection;

    const int MAX_SKILL_SELECT = 5;
    bool allowMultipleSelect = false;
    int max_selection_per_skill = 2;
    public static int[] selectedSkills = new int[MAX_SKILL_SELECT];
    [SerializeField] Image[] selectedSkillImage = new Image[MAX_SKILL_SELECT];
    int currentSkillSelect = 0;
    int currentDisplaySkill = 0;
    // Start is called before the first frame update
    void Start()
    {
        displayStatSlider1.maxValue = SkillStat.MAX_BASE_DAMAGE;
        displayStatSlider2.maxValue = SkillStat.MAX_RANGE;
        displayStatSlider3.maxValue = SkillStat.MAX_ENERGY_COST;
        displayStatSlider4.maxValue = SkillStat.MAX_SUPPORT_SCORE;
        UpdateSkill();
    }

    private int getSkillCount(int skillID)
    {
        int count = 0;
        for (int i = 0; i < currentSkillSelect; i++)
        {
            if (selectedSkills[i] == skillID)
            {
                count++;
            }
        }
        return count;
    }

    private bool IsSkillSelected(int skillID)
    {
        if(allowMultipleSelect) return false;
        int count = 0;
        for (int i = 0; i < currentSkillSelect; i++)
        {
            if (selectedSkills[i] == skillID)
            {
                count++;
                if (count >= max_selection_per_skill) return true;
            }
        }
        return false;
    }
    public void NextOption()
    {
        do
        {
            currentDisplaySkill = (currentDisplaySkill + 1) % skillDB.SkillCount;
        } while (IsSkillSelected(currentDisplaySkill) || skillDB.GetSkill(currentDisplaySkill).is_available != 1);
        UpdateSkill();
    }
    public void BackOption()
    {
        do
        {
            currentDisplaySkill = (currentDisplaySkill - 1 + skillDB.SkillCount) % skillDB.SkillCount;
        } while (IsSkillSelected(currentDisplaySkill) || skillDB.GetSkill(currentDisplaySkill).is_available != 1);
        UpdateSkill();
    }
    public void SelectOption()
    {
        selectedSkills[currentSkillSelect] = currentDisplaySkill;
        selectedSkillImage[currentSkillSelect].sprite = skillDB.GetSkill(currentDisplaySkill).skillSprite;
        currentSkillSelect++;
        currentDisplaySkill = skillDB.SkillCount - 1;
        NextOption();
        if (currentSkillSelect == MAX_SKILL_SELECT)
        {
            StartGame();
        }
    }

    private void UpdateSkill()
    {
        SkillStat skillStat = skillDB.GetSkill(currentDisplaySkill);
        nameText.text = skillStat.skillName;
        DisplayImage.sprite = skillStat.skillSprite;
        descriptionText.text = skillStat.skillDescription;

        displayStatSlider1.value = skillStat.baseDamage;
        displayStatSlider2.value = skillStat.range;
        displayStatSlider3.value = skillStat.energyCost;
        displayStatSlider4.value = skillStat.supportScore;
        countRemainSkillSelection.text = (max_selection_per_skill - getSkillCount(currentDisplaySkill)).ToString();
    }

    private void StartGame()
    {
        // Debug.Log("Starting Game");
        // load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
