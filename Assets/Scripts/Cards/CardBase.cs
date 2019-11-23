using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    public int Price;
    protected InteractTile LastTile;
    public virtual void Use(InteractTile tile){}
    public virtual void HoverStart(InteractTile tile){}
    public virtual void HoverEnd(InteractTile tile){}
}
