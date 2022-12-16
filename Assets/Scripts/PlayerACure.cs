using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerACure : MonoBehaviour
{
    public int healAmount = 10;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("PlayerA").GetComponent<PlayerAAnimationAndMovementController>().heal(healAmount);
        Invoke("kill", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 healPos = GameObject.Find("PlayerA").GetComponent<Transform>().position;
        healPos.y += 1f;
        healPos.z = 0f;
        transform.position = healPos;
    }
    private void kill(){
        Destroy(this.gameObject);
    }
}
