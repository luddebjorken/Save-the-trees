using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    public int Price;
    public virtual void Use(Interactable tile){}
}
