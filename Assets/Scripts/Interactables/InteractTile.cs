using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTile : Interactable
{
    public bool HasTree;
    public bool IsBurning;
    public float Burntime;
    private Material material;
    public int x,y;
    
    void Awake()
    {
        material = GetComponent<Renderer>().material;
        material.color = Color.green;
    }

    void Update()
    {
        if(IsBurning && Random.Range(0.0f,1.0f) < World.singleton.SpreadChance)
        {
            World.singleton.GetTile(getDirection(Random.Range(0,4)))?.SetFireState(true);
        }
    }


    public override void OnInteractStart()
    {
        SetFireState(true);
    }

    public void SetFireState(bool state)
    {
        if(state == IsBurning) return;
        if(state)
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.green;
        }
        IsBurning = state;
    }

    

    private Vector2 getDirection(int dir)
    {
        switch(dir)
        {
            case 0: return new Vector2(x,y+1);
            case 1: return new Vector2(x+1,y);
            case 2: return new Vector2(x,y-1);
            case 3: return new Vector2(x-1,y);
        }
        return new Vector2();
    }
}
