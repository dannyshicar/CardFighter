using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerALightningLaser : MonoBehaviour
{
    public GameObject myself;
    public Context context;
    public Vector3 mousePos;
    public GameObject lightningBoltPrefab;
    public GameObject lightningLaserColliderPrefab;
    private GameObject lightningBolt, lightningLaserCollider;
    Vector3 StartPosition, EndPosition;
    // Start is called before the first frame update
    void Start()
    {
        context = new Context();
        mousePos = context.GetMousePos();
        StartPosition = transform.position;
        EndPosition = mousePos;

        Invoke("Cast", 1);
        // // lightningBolt = Instantiate(lightningBoltPrefab, transform.position, lightningBoltPrefab.transform.rotation);
        // lightningLaserCollider = Instantiate(lightningLaserColliderPrefab, (StartPosition + EndPosition)/2, Quaternion.LookRotation(EndPosition - StartPosition));
        // lightningLaserCollider.transform.localScale = new Vector3(0.5f, 0.5f, (EndPosition - StartPosition).magnitude);
        // Invoke("kill", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Cast(){
        // lightningBolt = Instantiate(lightningBoltPrefab, transform.position, lightningBoltPrefab.transform.rotation);
        lightningLaserCollider = Instantiate(lightningLaserColliderPrefab, (StartPosition + EndPosition)/2, Quaternion.LookRotation(EndPosition - StartPosition));
        lightningLaserCollider.GetComponent<PlayerALightningCollider>().startPosition = StartPosition;
        lightningLaserCollider.GetComponent<PlayerALightningCollider>().endPosition = EndPosition;
        lightningLaserCollider.transform.localScale = new Vector3(0.5f, 0.5f, (EndPosition - StartPosition).magnitude);
    }
    // private void Kill(){
    //     // Destroy(lightningBolt);
    //     // Destroy(lightningLaserCollider);
    //     Destroy(myself);
    // }
}
