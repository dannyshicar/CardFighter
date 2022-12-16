using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerASkill : MonoBehaviour
{
    [SerializeField] private List<GameObject> skillPrefabs;

    public Context context;

    // throwBomb
    const int throwBombDamage = 10;
    const float throwBombDelay = 0f;
    const int throwBombEnergyCost = 10;
    const float throwBombDuration = 0f;

    // dropIce
    const int dropIceDamage = 20;
    const float dropIceDelay = 1f;
    const int dropIceEnergyCost = 10;
    const float dropIceDuration = 0f;
    
    // lightiningLaser
    const int lightningLaserDamage = 5;
    const float lightningLaserDelay = 0f;
    const int lightningLaserEnergyCost = 10;
    const float lightningLaserDuration = 0f;
    
    // heal
    const int healAmount = 10;
    const float healDelay = 0f;
    const int healEnergyCost = 10;
    const float healDuration = 0f;

    // electric crack
    const int electricCrackDamage = 5;
    const float electricCrackDelay = 0.3f;
    const int electricCrackEnergyCost = 30;
    const float electricCrackDuration = 0f;

    // holy light
    const int holyLightDamage = 10;
    const float holyLightDelay = 0.5f;
    const int holyLightEnergyCost = 10;
    const float holyLightDuration = 0f;

    // // teleport
    // const int teleportDamage = 0;
    // const float teleportDelay = 0.5f;
    // const int teleportEnergyCost = 20;
    // const float teleportDuration = 0f;

    // string name, int baseDamage, float skillDelay, float skillDuration, float energyCost
    public List<SkillTemplate> skills= new List<SkillTemplate>(){
        new SkillTemplate("ThrowBomb", throwBombDamage, throwBombDelay, throwBombDuration, throwBombEnergyCost),
        new SkillTemplate("DropIce", dropIceDamage, dropIceDelay, dropIceDuration, dropIceEnergyCost),
        new SkillTemplate("LightningLaser", lightningLaserDamage, lightningLaserDelay, lightningLaserDuration, lightningLaserEnergyCost),
        new SkillTemplate("Heal", healAmount, healDelay, healDuration, healEnergyCost),
        new SkillTemplate("ElectricCrack", electricCrackDamage, electricCrackDelay, electricCrackDuration, electricCrackEnergyCost),
        new SkillTemplate("HolyLight", holyLightDamage, holyLightDelay, holyLightDuration, holyLightEnergyCost)
    };

    Vector3 playerPosition;

    private Vector3 skillPos, mousePos;
    GameObject skillPrefab, skillObj;

    // Start is called before the first frame update
    void Awake()
    {
        context = new Context();
        mousePos = context.GetMousePos();
        playerPosition = GameObject.Find("PlayerA").GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = context.GetMousePos();
        playerPosition = GameObject.Find("PlayerA").GetComponent<Transform>().position;
    }

    public bool Cast(int skillIdx, float currentEnergy){
        if(!isReady(skillIdx, currentEnergy)) return false;
        GameObject.Find("PlayerA").GetComponent<PlayerAAnimationAndMovementController>().useEnergy(skills[skillIdx].energyCost);
        if(skillIdx == 0){
            //ThrowBomb
            //Debug.Log(skills[skillIdx].name);

            skillPos = playerPosition;
            skillPos.y += 1f;
            skillPos.z = 0f;

            skillPrefab = skillPrefabs[skillIdx];
  
            skillObj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
        }
        else if(skillIdx == 1){
            //DropIce
            //Debug.Log(skills[skillIdx].name);

            skillPos = mousePos;
            skillPrefab = skillPrefabs[skillIdx];
            skillObj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);


        }
        else if(skillIdx == 2){
            //LightningLaser
            //Debug.Log(skills[skillIdx].name);

            skillPos = playerPosition;
            skillPos.y += 1f;
            skillPos.z = 0f;

            skillPrefab = skillPrefabs[skillIdx];
            skillObj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
        }
        else if(skillIdx == 3){
            //Heal
            //Debug.Log(skills[skillIdx].name);
            skillPos = playerPosition;
            skillPos.y += 1f;
            skillPos.z = 0f;

            skillPrefab = skillPrefabs[skillIdx];
            skillObj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);

        }
        else if(skillIdx == 4){

        }
        else if(skillIdx == 5){
            skillPos = mousePos;
            skillPrefab = skillPrefabs[skillIdx];
            skillObj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
        }
        // else if(skillIdx == 6){
        //     skillPos = playerPosition;
        //     // skillPos.y += 1f;
        //     skillPos.z = 0f;

        //     skillPrefab = skillPrefabs[skillIdx];
        //     skillObj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
        // }
        return true;
    }
    public bool isReady(int skillIdx, float currentEnergy){
        if(currentEnergy >= skills[skillIdx].energyCost) return true;
        return false;
    }

}
