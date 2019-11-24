using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWind : CardBase
{
    public float Radius;
    public float ExtinguishRange;
    private List<InteractTile> selectedTiles;
    public override void Use(InteractTile tile)
    {
        if(selectedTiles == null) Debug.LogError("NO TILES WERE FOUND!");
        int TilesToLight = 0;
        Vector3 windForward = -Vector3.Cross(InteractComponent.singleton.GetDirection(), Vector3.up);
        Destroy(Instantiate(World.singleton.WindModel, tile.transform.position + new Vector3(0,2,0) + InteractComponent.singleton.GetDirection() * 10, Quaternion.LookRotation(windForward,Vector3.up)).gameObject,1);

        foreach(InteractTile selectedTile in selectedTiles)
        {
            if(Vector3.Distance(tile.transform.position, selectedTile.transform.position) < ExtinguishRange)
            {
                if(selectedTile.IsBurning) 
                {
                    selectedTile.SetFireState(false);
                    TilesToLight++;
                }
            }
            else if(TilesToLight >= 2 && (selectedTile.type == TileType.Grass || selectedTile.type == TileType.Tree))
            {
                selectedTile.SetFireState(true);
                TilesToLight-=2;
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

        Collider[] HitColliders = Physics.OverlapCapsule(center.transform.position, center.transform.position + InteractComponent.singleton.GetDirection() * World.singleton.Size.magnitude, Radius, 1 << 8);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                ret.Add(TileComponent);
            }
        }
        return ret;
    }
}
