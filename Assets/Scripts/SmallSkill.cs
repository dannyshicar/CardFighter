using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSkill : MonoBehaviour
{
    private int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnＴriggerEnter(Collider other)
    {
        Debug.Log("### 1");
    }

    void OnTriggerStay(Collider other)
    {
        if(cnt == 0){
            Debug.Log("### 2");
            Debug.Log(other.gameObject.name);
            cnt++;
        }
    }

    void OnColliderEnter(Collider other)
    {
        Debug.Log("### 3");
    }
}
