using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAThrowBomb : MonoBehaviour
{
    public GameObject bombPrefab;
    Context context;
    
    Vector3 mousePos;
    // name, skillPrefabIdx, baseDamage, rangeX, rangeY, rangeZ, skillDelay, skillDuration, energyCost
    Skill baseValue = new Skill("ThrowBomb", 1, 10, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f);

    // Start is called before the first frame update
    void Start()
    {
        context = new Context();
        mousePos = context.GetMousePos();
        Throw();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = context.GetMousePos();
    }
    void Throw(){
        GameObject bomb = Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        Vector3 forceDirection = (mousePos - transform.position);
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }
}
