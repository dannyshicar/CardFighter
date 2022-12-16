using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAHolyLightCollider : MonoBehaviour
{
    private bool hitTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        GameObject explode;
        GameObject playerB;
        
        // if (hitTarget) return;
        // else hitTarget = true;
        
        if(collision.gameObject.tag == "PlayerBBody"){
            if (hitTarget) return;
            else hitTarget = true;
            playerB = GameObject.Find("PlayerB");
            playerB.GetComponent<PlayerBAnimationAndMovementController>().takeDamage(10);
            Destroy(this.gameObject);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
