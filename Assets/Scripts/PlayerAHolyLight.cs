using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAHolyLight : MonoBehaviour
{
    public GameObject holyLightEffectPrefab;
    private GameObject holyLightEffect;
    // Start is called before the first frame update
    void Start()
    {
        holyLightEffect = Instantiate(holyLightEffectPrefab, transform.position, holyLightEffectPrefab.transform.rotation);
        Destroy(holyLightEffect, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
