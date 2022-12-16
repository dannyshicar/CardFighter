using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBIce : MonoBehaviour
{
    private Rigidbody rb;

    private bool hitTarget = false;

    [SerializeField] private GameObject explodePrefabs;
    
    public GameObject myself;
    public int enemyLayer = 14;

    private void Start()
    {
        // get rigidbody component
        rb = GetComponent<Rigidbody>();       
    }

    private void OnTriggerEnter(Collider collision)
    {

        // if (hitTarget) return;
        // else hitTarget = true;

        GameObject explode;
        GameObject playerA;

        if(collision.gameObject.tag == "PlayerABody"){
            this.gameObject.SetActive(false);
            explode = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
            playerA = GameObject.Find("PlayerA");
            playerA.GetComponent<PlayerAAnimationAndMovementController>().takeDamage(15);
            Destroy(explode, 2);
            Destroy(this.gameObject, 2);
        }


    }

    void Update()
    {
        if (transform.position.y < -20f) Destroy(this.gameObject);
    }
}