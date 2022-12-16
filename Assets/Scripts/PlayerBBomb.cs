using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBBomb : MonoBehaviour
{
    private Rigidbody rb;

    private bool hitTarget = false;

    [SerializeField] private GameObject explodePrefabs;
    
    public GameObject myself;
    private int mapLayer = 6;

    private void Start()
    {
        // get rigidbody component
        rb = GetComponent<Rigidbody>();       
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject explode;
        GameObject playerA;

        // if (hitTarget) return;
        // else hitTarget = true;


        if(collision.gameObject.tag == "PlayerABody"){
            this.gameObject.SetActive(false);
            explode = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
            playerA = GameObject.Find("PlayerA");
            playerA.GetComponent<PlayerAAnimationAndMovementController>().takeDamage(10);
            Destroy(explode, 2);
            Destroy(this.gameObject, 2);
        }
        if(collision.gameObject.layer == mapLayer){
            this.gameObject.SetActive(false);
            explode = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
            Destroy(explode, 2);
            Destroy(this.gameObject, 2);
        }

        // // enemy hit
        // if (collision.gameObject.GetComponent<BasicEnemy>() != null)
        // {
        //     BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();

        //     // deal damage to the enemy
        //     enemy.TakeDamage(damage);

        //     // spawn hit effect (if assigned)
        //     if (hitEffect != null)
        //         Instantiate(hitEffect, transform.position, Quaternion.identity);
            
        //     // destroy projectile
        //     if (!isExplosive && destroyOnHit)
        //         Invoke(nameof(DestroyProjectile), 0.1f);
        // }

        // // explode projectile if it's explosive
        // if (isExplosive)
        // {
        //     Explode();
        //     return;
        // }

        // // make sure projectile sticks to surface
        // rb.isKinematic = true;

        // // make sure projectile moves with target
        // transform.SetParent(collision.transform);
    }

    // public void Throw(Vector3 dir){
    //     Debug.Log("here to throw");
    //     Debug.Log(dir);
    //     rb = GetComponent<Rigidbody>();
    //     rb.AddForce(dir, ForceMode.Impulse);
    // }
    // Update is called once per frame
    void Update()
    {

    }
}