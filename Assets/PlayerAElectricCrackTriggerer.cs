using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAElectricCrackTriggerer : MonoBehaviour
{

    private Rigidbody rb;

    private bool hitTarget = false;

    public GameObject electricCrackPrefab;
    public GameObject electricCrackColliderPrefab;
    public GameObject electricCrackTriggererPrefab;
    GameObject electricCrackTriggerer;
    public int cnt;

    public int mapLayer = 6;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("created");
        // get rigidbody component
        rb = GetComponent<Rigidbody>();  
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject electricCrack;
        GameObject electricCrackCollider;

        
        
       
        if(collision.gameObject.layer == mapLayer){
            if (hitTarget) return;
            else hitTarget = true;

            this.gameObject.SetActive(false);
            electricCrack = Instantiate(electricCrackPrefab, transform.position, electricCrackPrefab.transform.rotation);
            Destroy(electricCrack, 2);
            Destroy(this.gameObject, 2);
            Vector3 colliderPos = transform.position;
            colliderPos.y += electricCrackColliderPrefab.GetComponent<Transform>().localScale.y / 2;
            electricCrackCollider = Instantiate(electricCrackColliderPrefab, colliderPos, electricCrackColliderPrefab.transform.rotation);
            Destroy(electricCrackCollider, 2);

            if (cnt < 3){
                electricCrackTriggererPrefab.GetComponent<PlayerAElectricCrackTriggerer>().cnt = cnt+1;
                Vector3 pos = transform.position;
                pos.x += 1f;
                pos.y += 1f;
                electricCrackTriggerer = Instantiate(electricCrackTriggererPrefab, pos, electricCrackTriggererPrefab.transform.rotation);
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
