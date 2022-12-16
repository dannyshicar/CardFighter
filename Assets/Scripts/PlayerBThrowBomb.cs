using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBThrowBomb : MonoBehaviour
{
    public GameObject bombPrefab;
    GameObject playerB;
    Context context;
    
    Vector3 mousePos;
    // name, skillPrefabIdx, baseDamage, rangeX, rangeY, rangeZ, skillDelay, skillDuration, energyCost
    Skill baseValue = new Skill("ThrowBomb", 1, 10, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        playerB = GameObject.Find("PlayerB");
        mousePos = playerB.GetComponent<PlayerBAnimationAndMovementController>().AIMousePos;
        Throw();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = playerB.GetComponent<PlayerBAnimationAndMovementController>().AIMousePos;
    }
    void Throw(){
        GameObject bomb = Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        mousePos.y += 1;
        Vector3 forceDirection = (mousePos - transform.position) * 3;
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }
}
