using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCounter : MonoBehaviour
{
    void Start()
    {
        World.singleton.FireCount++;
        int count = 0;
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, 5, 1 << 8);
        foreach(Collider collider in collidersHit)
        {
            InteractTile tile = collider.GetComponent<InteractTile>();
            if(tile && tile.IsBurning) count++;            
        }
        SoundHandler.SpawnFire(this, count);
    }

    void OnDestroy()
    {
        World.singleton.FireCount--;
    }
}
