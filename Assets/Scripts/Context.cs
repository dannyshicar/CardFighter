using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Context
{

    public Vector3 screenPosition;
    public Vector3 mousePos;
    public Context(){}

    
    public Vector3 GetMousePos() {
        screenPosition = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(screenPosition);
        mousePos.z = 0;
        return mousePos;
    }
}
