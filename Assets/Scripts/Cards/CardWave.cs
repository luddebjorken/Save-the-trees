using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWave : CardBase
{
    public float Radius;
    private List<InteractTile> selectedTiles;
    public override void Use(InteractTile tile)
    {
        if(selectedTiles == null) Debug.LogError("NO TILES WERE FOUND!");
        foreach(InteractTile selectedTile in selectedTiles)
        {
            selectedTile.SetFireState(false);
        }
    }

    public override void HoverStart(InteractTile tile)
    {
        if(tile != LastTile)
        {
            selectedTiles = GetTiles(tile);
            InteractComponent.singleton.HighlightTiles(selectedTiles);
        }
    }

    public override void HoverEnd(InteractTile tile)
    {
        selectedTiles.Clear();
    }

    private List<InteractTile> GetTiles(InteractTile center)
    {
        List<InteractTile> ret = new List<InteractTile>();
        bool containsWater = false;
        Collider[] HitColliders = Physics.OverlapSphere(center.transform.position,Radius);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                if(TileComponent.type == TileType.Water) containsWater = true;
                ret.Add(TileComponent);
            }
        }
        return containsWater ? ret : new List<InteractTile>();
    }
}
