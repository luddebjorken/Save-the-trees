using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [Header("Blocks")]
    public Mesh AshBlock;
    public Mesh GrassBlock;
    public Mesh DirtBlock;
    public Mesh WaterBlock;
    [Header("Props")]
    public Transform TreeModel;
    public Transform HouseModel;
    public Transform GrassModel;
    [Header("Tile reference")]
    public Transform TilePrefab;
    public Transform WaterTilePrefab;
    public Transform WorldParent;
    [Header("Spawn settings")]
    public Vector2 Size;
    public int TreesToPlace;
    public int GrassToPlace;
    public InteractTile[,] tiles;
    [Header("Fire spread settings")]
    public float TreeSpreadChance;
    public float GrassSpreadChance;
    public float GrassBurnTime;
    public float TreeBurnTime;
    public static World singleton;
    [Header("Fire start settings")]
    public float RandomFireStartChance;
    public float RandomFireStartRadius;
    public float RandomFireStartMaxCount;
    public List<InteractTile> TreesPlaced;
    public int TreesAmount;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(!singleton) singleton = this;
    }
    void Start()
    {
        TreesAmount = TreesToPlace;
        this.TreesPlaced = new List<InteractTile>();

        //Places tiles
        tiles = new InteractTile[(int)Size.x,(int)Size.y];
        for(int x = 0; x < (int)Size.x; x++)
        {
            for(int y = 0; y < (int)Size.y; y++)
            {
                tiles[x,y] = Instantiate(TilePrefab, new Vector3(x,0,y), Quaternion.Euler(0,Random.Range(0,3)*90,0), WorldParent).GetComponent<InteractTile>();
                tiles[x,y].x = x;
                tiles[x,y].y = y;
            }
        }

        //Border water tiles
        for(int x = (int)(-Size.x*0.5f) - 1; x < Size.x*1.5 + 3; x++)
        {
            float test = (x<Size.x/2?(int)(Size.y)+x:(Size.y-x+Size.x/2));
            for(int y = (int)(x < Size.x/2 ? - x : -Size.y + x)-1; y < (x<Size.x/2?(int)(Size.y)+x:(Size.y-x+Size.x)) + 3; y++)
            {
                if(x >= 0 && y >= 0 && x < Size.x && y < Size.y) continue;
                Instantiate(WaterTilePrefab, new Vector3(x,-0.9f,y), Quaternion.Euler(0,Random.Range(0,3)*90,0), WorldParent);
            }
        }

        //Spreads random trees from 4 random locations
        for(int i = 0; i < 4;)
        {
            InteractTile tile = GetRandomTile();
            if(tile.type == TileType.Dirt)
            {
                tile.SetType(TileType.Tree);
                TreesPlaced.Add(tile);
                TreesAmount++;
                i++;    
            }
        }
        for(int i = 0; i < TreesToPlace*100 && TreesPlaced.Count < TreesToPlace; i++)
        {
            InteractTile tile = GetTile(TreesPlaced[Random.Range(0,TreesPlaced.Count)].GetDirection(Random.Range(0,4)));
            if(tile && tile.type == TileType.Dirt)
            {
                tile.SetType(TileType.Tree);
                TreesPlaced.Add(tile);
            }
        }


        //Spreads random grass 
        List<InteractTile> grassPlaced = new List<InteractTile>();
        for(int i = 0; i < 4;)
        {
            InteractTile tile = GetRandomTile();
            if(tile.type == TileType.Dirt)
            {
                tile.SetType(TileType.Grass);
                grassPlaced.Add(tile);
                i++;
            }
        }
        for(int i = 0; i < GrassToPlace*100 && grassPlaced.Count < GrassToPlace; i++)
        {
            InteractTile tile = GetTile(grassPlaced[Random.Range(0,grassPlaced.Count)].GetDirection(Random.Range(0,4)));
            if(tile && tile.type == TileType.Dirt)
            {
                tile.SetType(TileType.Grass);
                grassPlaced.Add(tile);
            }
        }

        //Places random houses
        int HousesToPlace = Random.Range(0,10);
        int HousesPlaced = 0;
        while(HousesPlaced < HousesToPlace)
        {
            InteractTile tile = GetRandomTile();
            if(tile && tile.type == TileType.Dirt)
            {
                tile.Child = Instantiate(HouseModel, tile.transform.position + new Vector3(0,0.5f,0), Quaternion.Euler(0,Random.Range(0,4)*90,0));
                HousesPlaced++;
            }
        }
    }

    public InteractTile GetTile(Vector2 pos)
    {
        if(pos.x >= Size.x || pos.y >= Size.y || pos.x < 0 || pos.y < 0) return null;
        return tiles[(int)pos.x, (int)pos.y];
    }

    public InteractTile GetRandomTile()
    {
        return GetTile(new Vector2(Random.Range(0,(int)Size.x),Random.Range(0,(int)Size.y)));
    }

    void Update()
    {
        if(Random.Range(0.0f,1.0f) < RandomFireStartChance)
        {
            for(int i = 0; i < Random.Range(0,RandomFireStartMaxCount); i++)
            {
                Collider[] HitColliders = Physics.OverlapSphere(GetRandomTile().transform.position, Random.Range(0.0f, RandomFireStartRadius));
                foreach(Collider HitCollider in HitColliders){
                    InteractTile TileComponent = HitCollider.GetComponent<InteractTile>();
                    if(TileComponent){
                        TileComponent.SetFireState(true);
                    }
                }
            }
        }
    }
}
