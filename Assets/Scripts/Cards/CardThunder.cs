using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThunder : CardBase
{
    public float Radius;
    public float LightningstrikeChance;
    public float LightningStrikeRadius;
    private List<InteractTile> selectedTiles;
    public override void Use(InteractTile tile)
    {
        if(selectedTiles == null) Debug.LogError("NO TILES WERE FOUND!");
        foreach(InteractTile selectedTile in selectedTiles)
        {
            selectedTile.SetFireState(false);
        }
        if(Random.Range(0.0f,1.0f) < LightningstrikeChance)
        {
            Collider[] HitColliders = Physics.OverlapSphere(selectedTiles[Random.Range(0,selectedTiles.Count)].transform.position, Random.Range(0.0f,LightningStrikeRadius));
            foreach(Collider HitCollider in HitColliders)
            {
                InteractTile hitTile = HitCollider.GetComponent<InteractTile>();
                if(hitTile)
                {
                    hitTile.SetFireState(true);
                }
            }
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

        Collider[] HitColliders = Physics.OverlapSphere(center.transform.position,Radius);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                ret.Add(TileComponent);
            }
        }
        return ret;
    }
}
