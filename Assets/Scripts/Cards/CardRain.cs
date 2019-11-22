using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRain : CardBase
{
    public float Radius;
    public override void Use(Interactable tile)
    {
        Collider[] HitColliders = Physics.OverlapSphere(tile.transform.position,Radius);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                TileComponent.SetFireState(false);
            }
        }
    }
}
