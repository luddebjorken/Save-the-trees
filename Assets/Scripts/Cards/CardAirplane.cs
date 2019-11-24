using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAirplane : CardBase
{
    public float Range;
    private List<InteractTile> selectedTiles;
    public override void Use(InteractTile tile)
    {
        SoundHandler.singleton.CardSource.PlayOneShot(UseSound[Random.Range(0,UseSound.Length)]);
        if(selectedTiles == null) Debug.LogError("NO TILES WERE FOUND!");
        foreach(InteractTile selectedTile in selectedTiles)
        {
            if(selectedTile.IsBurning) 
            {
                selectedTile.SetFireState(false);
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

        Collider[] HitColliders = Physics.OverlapCapsule(center.transform.position, center.transform.position + InteractComponent.singleton.GetDirection() * Range, 1f, 1 << 8);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                ret.Add(TileComponent);
            }
        }
        return ret;
    }
}
