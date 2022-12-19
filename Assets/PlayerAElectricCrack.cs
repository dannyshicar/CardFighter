using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAElectricCrack : MonoBehaviour
{
    public GameObject electricCrackTriggererPrefab;
    GameObject electricCrackTriggerer;
    // Start is called before the first frame update
    void Start()
    {
        electricCrackTriggererPrefab.GetComponent<PlayerAElectricCrackTriggerer>().cnt = 0;
        electricCrackTriggerer = Instantiate(electricCrackTriggererPrefab, transform.position, electricCrackTriggererPrefab.transform.rotation);
        Destroy(electricCrackTriggerer, 2);
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
