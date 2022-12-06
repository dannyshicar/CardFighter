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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj;
        // if (hitTarget) return;
        // else hitTarget = true;


        this.gameObject.SetActive(false);
        obj = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
        Debug.Log("Layer = " + collision.gameObject.layer);
        if(collision.gameObject.layer == enemyLayer){
            Debug.Log("hit");
            collision.gameObject.GetComponent<PlayerAAnimationAndMovementController>().takeDamage(15);

        }
        Destroy(obj, 2);
        Destroy(this.gameObject, 2);
    }

    void Update()
    {
        if (transform.position.y < -20f) Destroy(this.gameObject);
    }
}