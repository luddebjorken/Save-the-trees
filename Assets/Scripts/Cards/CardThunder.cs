using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThunder : CardBase
{
    public AudioClip[] LightningClip;
    public float Radius;
    public float LightningstrikeChance;
    public float LightningStrikeRadius;
    private List<InteractTile> selectedTiles;
    public override void Use(InteractTile tile)
    {
        if(!Currency.singleton.Pay(Price)) return;
        InteractComponent.singleton.RainFade();
        if(selectedTiles == null) Debug.LogError("NO TILES WERE FOUND!");
        foreach(InteractTile selectedTile in selectedTiles)
        {
            selectedTile.SetFireState(false);
        }
        if(Random.Range(0.0f,1.0f) < LightningstrikeChance)
        {
            InteractComponent.singleton.ThunderFade();
            SoundHandler.singleton.CardSource.PlayOneShot(LightningClip[Random.Range(0,LightningClip.Length)]);
            InteractTile lightningTile = selectedTiles[Random.Range(0,selectedTiles.Count)];
            Destroy(Instantiate(World.singleton.LightningModel, lightningTile.transform.position + new Vector3(0,1f,0),Quaternion.Euler(0,45,0)).gameObject,1);
            Collider[] HitColliders = Physics.OverlapSphere(lightningTile.transform.position, Random.Range(0.0f,LightningStrikeRadius), 1 << 8);
            foreach(Collider HitCollider in HitColliders)
            {
                InteractTile hitTile = HitCollider.GetComponent<InteractTile>();
                if(hitTile)
                {
                    hitTile.SetFireState(true);
                }
            }
        }
        else
        {
            GameObject rain = Instantiate(World.singleton.RainModel, tile.transform.position + new Vector3(0,1f,0),Quaternion.Euler(0,45,0)).gameObject;
            rain.transform.localScale *= 1.2f;
            Destroy(rain,1);
            SoundHandler.singleton.CardSource.PlayOneShot(UseSound[Random.Range(0,UseSound.Length)]);
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

        Collider[] HitColliders = Physics.OverlapSphere(center.transform.position,Radius, 1 << 8);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                ret.Add(TileComponent);
            }
        }
        return ret;
    }

}
