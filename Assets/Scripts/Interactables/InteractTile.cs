using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Grass,
    Dirt,
    Tree,
    Ash,
    Water,
    House
}

public class InteractTile : Interactable
{
    public TileType type;
    public bool HasTree;
    public bool IsBurning;
    public float BurnStart;
    public int x,y;
    public Material Material;
    public Transform Child;
    private Transform Fire;
    private MeshFilter mesh;
    
    void Awake()
    {
        Material = GetComponent<Renderer>().material;
        mesh = GetComponent<MeshFilter>();
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
                    Quaternion rotation = Child.rotation;
                    if(Child)Destroy(Child.gameObject);
                    Child = Instantiate(World.singleton.StumpModel, transform.position + new Vector3(0,0.5f,0), rotation, transform);
                }
                else if(type == TileType.Grass)
                {
                    if(Child)Destroy(Child.gameObject);
                }
                SetType(TileType.Ash);
            }
            else if(Random.Range(0.0f,1.0f) < (type == TileType.Grass ? World.singleton.GrassSpreadChance : World.singleton.TreeSpreadChance))
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


    public void SetFireState(bool state)
    {
        if(state == IsBurning || type == TileType.Ash || type == TileType.Dirt || type == TileType.Water) return;
        if(state)
        {
            BurnStart = Time.time;
            if(Fire)Destroy(Fire.gameObject);
            Fire = Instantiate(World.singleton.FireModel, transform.position + new Vector3(0,1.77f,0),Quaternion.Euler(0,45,0), transform);
        }
        else
        {
            if(Fire)Destroy(Fire.gameObject);
        }
        IsBurning = state;
    }

    public bool SetType(TileType newType)
    {
        if(type == newType) return false;
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
                if(Fire)Destroy(Fire.gameObject);
            break;
            case TileType.Grass:
                mesh.mesh = World.singleton.GrassBlock;
                if(Child)Destroy(Child);
                Child = Instantiate(World.singleton.GrassModel, transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0,Random.Range(0,360),0), transform);
            break;
            case TileType.Water:
                mesh.mesh = World.singleton.WaterBlock;
                transform.Translate(0,-0.9f,0);
                GetComponent<BoxCollider>().center = new Vector3(0,0.9f,0);
            break;
            default:
                mesh.mesh = World.singleton.DirtBlock;
            break;
        }
        return true;
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
