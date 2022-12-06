using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBDropIce : MonoBehaviour
{
    public GameObject icePrefab;
    public GameObject icePreviewPrefab;
    GameObject icePreview;
    GameObject playerB;
    Context context;
    Vector3 mousePos;

    // name, skillPrefabIdx, baseDamage, rangeX, rangeY, rangeZ, skillDelay, skillDuration, energyCost
    Skill baseValue;
    // Start is called before the first frame update
    void Start()
    {
        playerB = GameObject.Find("PlayerB");
        mousePos = playerB.GetComponent<PlayerBAnimationAndMovementController>().AIMousePos;
        icePreview = Instantiate(icePreviewPrefab, transform.position, icePreviewPrefab.transform.rotation);
        Invoke("Drop", 1f);
        
        // Drop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Drop(){
        Destroy(icePreview);
        GameObject bomb = Instantiate(icePrefab, transform.position, icePrefab.transform.rotation);
        Destroy(this.gameObject);
        // Rigidbody rb = bomb.GetComponent<Rigidbody>();
        // Vector3 forceDirection = (mousePos - transform.position);
        // rb.AddForce(forceDirection, ForceMode.Impulse);
    }
}
