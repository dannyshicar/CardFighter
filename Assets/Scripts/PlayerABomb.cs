using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerABomb : MonoBehaviour
{
    private Rigidbody rb;

    private bool hitTarget = false;

    [SerializeField] private GameObject explodePrefabs;
    
    public GameObject myself;
    public int mapLayer = 6;

    private void Start()
    {
        // get rigidbody component
        rb = GetComponent<Rigidbody>();       
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject explode;
        GameObject playerB;
        
        // if (hitTarget) return;
        // else hitTarget = true;
        
        if(collision.gameObject.tag == "PlayerBBody"){
            this.gameObject.SetActive(false);
            explode = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
            playerB = GameObject.Find("PlayerB");
            playerB.GetComponent<PlayerBAnimationAndMovementController>().takeDamage(10);
            Destroy(explode, 2);
            Destroy(this.gameObject, 2);
        }
        if(collision.gameObject.layer == mapLayer){
            this.gameObject.SetActive(false);
            explode = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
            Destroy(explode, 2);
            Destroy(this.gameObject, 2);
        }


    }


    void Update()
    {

    }
}