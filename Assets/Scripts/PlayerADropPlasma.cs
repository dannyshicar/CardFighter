using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerADropIce : MonoBehaviour
{
    public GameObject icePrefab;
    public GameObject icePreviewPrefab;
    public GameObject icePreview;
    Context context;
    Vector3 mousePos;

    // name, skillPrefabIdx, baseDamage, rangeX, rangeY, rangeZ, skillDelay, skillDuration, energyCost
    Skill baseValue;
    // Start is called before the first frame update
    void Start()
    {
        context = new Context();
        mousePos = context.GetMousePos();
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
        // Rigidbody rb = bomb.GetComponent<Rigidbody>();
        // Vector3 forceDirection = (mousePos - transform.position);
        // rb.AddForce(forceDirection, ForceMode.Impulse);
        Destroy(this.gameObject);
    }
}
