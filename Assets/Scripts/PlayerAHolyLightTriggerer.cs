using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAHolyLightTriggerer : MonoBehaviour
{
    private Rigidbody rb;

    private bool hitTarget = false;

    public GameObject holyLightPrefab;
    public GameObject holyLightColliderPrefab;
    GameObject holyLight;
    GameObject holyLight2;
    GameObject holyLightCollider;

    public int mapLayer = 6;
    // Start is called before the first frame update
    void Start()
    {
        // get rigidbody component
        rb = GetComponent<Rigidbody>();  
    }

    void OnTriggerEnter(Collider collision)
    {
        

        
        
       
        if(collision.gameObject.layer == mapLayer){
            if (hitTarget) return;
            else hitTarget = true;

            this.gameObject.SetActive(false);
            Invoke("Cast", 1f);
            
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void Cast(){
        holyLight = Instantiate(holyLightPrefab, transform.position, holyLightPrefab.transform.rotation);
        Destroy(holyLight, 2);
        holyLight2 = Instantiate(holyLightPrefab, transform.position, holyLightPrefab.transform.rotation);
        Destroy(holyLight2, 2);
        Destroy(this.gameObject, 2);
        Vector3 colliderPos = transform.position;
        colliderPos.y += holyLightColliderPrefab.GetComponent<Transform>().localScale.y / 2;
        holyLightCollider = Instantiate(holyLightColliderPrefab, colliderPos, holyLightColliderPrefab.transform.rotation);
        Destroy(holyLightCollider, 2);
    }
}
