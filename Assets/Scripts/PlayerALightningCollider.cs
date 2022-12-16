using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerALightningCollider : MonoBehaviour
{

    private bool hitTarget = false;
    public Context context;
    public GameObject lightningPrefab;
    public GameObject myself;
    private GameObject lightning;
    public int enemyLayer = 14;
    public Vector3 startPosition, endPosition;
    GameObject playerB;


    private void Start()
    {
        // context = new Context();
        // mousePos = context.GetMousePos();
        // Vector3 StartPosition = transform.position;
        // Vector3 EndPosition = mousePos;
        lightning = Instantiate(lightningPrefab, transform.position, lightningPrefab.transform.rotation);
        lightning.GetComponent<LightningBoltScript>().StartPosition = startPosition;
        lightning.GetComponent<LightningBoltScript>().EndPosition = endPosition;
        Invoke("kill", 1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if (hitTarget) return;
        // else hitTarget = true;


        // this.gameObject.SetActive(false);
        if(collision.gameObject.tag == "PlayerBBody"){
            if (hitTarget) return;
            else hitTarget = true;
            Debug.Log("lightning hit");
            playerB = GameObject.Find("PlayerB");
            playerB.GetComponent<PlayerBAnimationAndMovementController>().takeDamage(10);
            // collision.gameObject.GetComponent<PlayerBAnimationAndMovementController>().takeDamage(10);
            kill();

        }
        
    }
    void Update()
    {

    }
    private void kill(){
        Destroy(lightning);
        Destroy(this.gameObject);
    }
    
}