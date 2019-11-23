using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Grass,
    Dirt,
    Tree,
    Ash,
    Water
}

public class InteractTile : Interactable
{
    public TileType type;
    public bool HasTree;
    public bool IsBurning;
    public float BurnStart;
    public Transform Child;
    public int x,y;
    public Material Material;
    private MeshFilter mesh;
    
    void Awake()
    {
        Material = GetComponent<Renderer>().material;
        mesh = GetComponent<MeshFilter>();
        /*
        switch(type)
        {
            case TileType.Tree: material.color = Color.green; break;// new Color(23, 135, 30); break;
            case TileType.Ash: material.color = Color.gray; break;//new Color(77,77,77); break;
            default: material.color = Color.yellow; break;//new Color(118, 199, 20); break;
        }
        */
    }

    void Update()
    {
        if(IsBurning)
        {
            if(Time.time - BurnStart > (type == TileType.Tree?World.singleton.TreeBurnTime : type == TileType.Grass ? World.singleton.GrassBurnTime : Mathf.Infinity))
            {
                IsBurning = false;
                if(type == TileType.Tree){
                    World.singleton.TreesAmount--;
                }
                SetType(TileType.Ash);
            }
            else if(Random.Range(0.0f,1.0f) < (type == TileType.Tree?World.singleton.TreeSpreadChance : type == TileType.Grass ? World.singleton.GrassSpreadChance : 0))
            {
                if(type == TileType.Ash)
                {
                    IsBurning = false;
                    return;
                }
                World.singleton.GetTile(GetDirection(Random.Range(0,4)))?.SetFireState(true);
            }
        }
    }


    public override void OnInteractStart()
    {
        SetFireState(true);
    }

    public void SetFireState(bool state)
    {
        if(state == IsBurning || type == TileType.Ash || type == TileType.Dirt) return;
        if(state)
        {
            BurnStart = Time.time;
            Material.color = Color.red;
            if(Child)Child.GetComponent<Renderer>().material.SetColor("Color", Color.red);
        }
        else
        {
            Material.color = Color.white;
            if(Child)Child.GetComponent<Renderer>().material.SetColor("Color", Color.white);
        }
        IsBurning = state;
    }

    public void SetType(TileType newType)
    {
        type = newType;
        switch(type)
        {
            case TileType.Tree: 
                mesh.mesh = World.singleton.GrassBlock;
                if(Child)Destroy(Child);
                Child = Instantiate(World.singleton.TreeModel, transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0,Random.Range(0,360),0), transform);
            break;// new Color(23, 135, 30); break;
            case TileType.Ash: 
                Material.color = Color.white;
                mesh.mesh = World.singleton.AshBlock;
            break;
            case TileType.Grass:
                mesh.mesh = World.singleton.GrassBlock;
                if(Child)Destroy(Child);
                Child = Instantiate(World.singleton.GrassModel, transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0,Random.Range(0,4)*90,0), transform);
            break;
            default:
                mesh.mesh = World.singleton.DirtBlock;
            break;
        }
    }
    

    public Vector2 GetDirection(int dir)
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
