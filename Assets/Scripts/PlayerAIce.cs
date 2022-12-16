using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIce : MonoBehaviour
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
        GameObject explode;
        GameObject playerB;

        if(collision.gameObject.tag == "PlayerBBody"){
            this.gameObject.SetActive(false);
            explode = Instantiate(explodePrefabs, transform.position, explodePrefabs.transform.rotation);
            playerB = GameObject.Find("PlayerB");
            playerB.GetComponent<PlayerBAnimationAndMovementController>().takeDamage(10);
            Destroy(explode, 2);
            Destroy(this.gameObject, 2);
        }
    }

    void Update()
    {
        if (transform.position.y < -20f) Destroy(this.gameObject);
    }
}