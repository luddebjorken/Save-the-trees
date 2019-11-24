using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWave : CardBase
{
    private class WaveFrame
    {
        List<Transform> blocks;
        public WaveFrame(Vector2 center, int radius)
        {
            int r2 = radius * radius;
            for(int x = -(int)radius; x <= radius; x++)
            {
                int y = (int)(Mathf.Sqrt(r2 - x*x) + 0.5f);
                Transform A = World.singleton.GetTile(center + new Vector2(x,y))?.transform;
                Transform B = World.singleton.GetTile(center + new Vector2(x,-y))?.transform;
                if(A)blocks.Add(A);
                if(B)blocks.Add(B);
            }

            IEnumerator FadeRoutine()
            {
                float start = Time.time;
                while(Time.time - start < 1)
                {
                    //foreach(Transform transform in )
                    yield return 0;
                }
            }
        }
    }

    public float Radius;
    public float Range;
    private List<InteractTile> selectedTiles;
    public override void Use(InteractTile tile)
    {
        if(selectedTiles.Count <= 0) return;
        Vector3 windForward = -Vector3.Cross(InteractComponent.singleton.GetDirection(), Vector3.up);
        Destroy(Instantiate(World.singleton.WaveModel, tile.transform.position + new Vector3(0,5,0), Quaternion.LookRotation(windForward,Vector3.up)).gameObject,1);
        Destroy(Instantiate(World.singleton.WaveModel, tile.transform.position + new Vector3(0,3,0) + windForward*Radius, Quaternion.LookRotation(windForward,Vector3.up)).gameObject,1);
        Destroy(Instantiate(World.singleton.WaveModel, tile.transform.position + new Vector3(0,3,0) - windForward*Radius, Quaternion.LookRotation(windForward,Vector3.up)).gameObject,1);

        if(!Currency.singleton.Pay(Price)) return;
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
        bool HasWater = false;
        
        Collider[] HitColliders = Physics.OverlapCapsule(center.transform.position, center.transform.position + InteractComponent.singleton.GetDirection() * Range, Radius, 1 << 8);
        foreach(Collider HitCollider in HitColliders){
            InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
            if(TileComponent){
                if(TileComponent.type == TileType.Water) 
                {
                    HasWater = true;
                }
                ret.Add(TileComponent);
            }
        }
        return HasWater ? ret : new List<InteractTile>();
    }
}
