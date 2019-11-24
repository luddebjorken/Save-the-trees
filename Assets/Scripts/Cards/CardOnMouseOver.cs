﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOnMouseOver : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }
}
