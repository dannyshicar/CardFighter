using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATeleport : MonoBehaviour
{
    public GameObject teleportEffectPrefab;
    private GameObject teleportEffect;
    Context context;
    Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        context = new Context();
        mousePos = context.GetMousePos();
        // teleportEffect = Instantiate(teleportEffectPrefab, transform.position, teleportEffectPrefab.transform.rotation);
        // GameObject.Find("PlayerA").GetComponent<PlayerAAnimationAndMovementController>().isTeleport = true;
        // GameObject.Find("PlayerA").GetComponent<PlayerAAnimationAndMovementController>().transform.position = mousePos;
        // Debug.Log(mousePos + " " + GameObject.Find("PlayerA").GetComponent<PlayerAAnimationAndMovementController>().transform.position);
        Destroy(teleportEffect, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
