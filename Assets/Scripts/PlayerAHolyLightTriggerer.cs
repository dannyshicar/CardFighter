using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAHolyLightTriggerer : MonoBehaviour
{
    private Rigidbody rb;

    private bool hitTarget = false;

    public GameObject holyLightPrefab;
    public GameObject holyLightColliderPrefab;

    public int mapLayer = 6;
    // Start is called before the first frame update
    void Start()
    {
        // get rigidbody component
        rb = GetComponent<Rigidbody>();  
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject holyLight;
        GameObject holyLightCollider;

        
        
       
        if(collision.gameObject.layer == mapLayer){
            if (hitTarget) return;
            else hitTarget = true;

            this.gameObject.SetActive(false);
            holyLight = Instantiate(holyLightPrefab, transform.position, holyLightPrefab.transform.rotation);
            Destroy(holyLight, 2);
            Destroy(this.gameObject, 2);
            Vector3 colliderPos = transform.position;
            colliderPos.y += holyLightColliderPrefab.GetComponent<Transform>().localScale.y / 2;
            holyLightCollider = Instantiate(holyLightColliderPrefab, colliderPos, holyLightColliderPrefab.transform.rotation);
            Destroy(holyLightCollider, 2);
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
