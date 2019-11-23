using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Transform TilePrefab;
    public Transform WorldParent;
    public Vector2 Size;
    public InteractTile[,] tiles;
    public int TreesToPlace;
    public float TreeSpreadChance;
    public float GrassSpreadChance;
    public float GrassBurnTime;
    public float TreeBurnTime;
    public static World singleton;
    public List<InteractTile> TreesPlaced;
    
    // Start is called before the first frame update
    void Start()
    {
        this.TreesPlaced = new List<InteractTile>();
        if(!singleton) singleton = this;
        tiles = new InteractTile[(int)Size.x,(int)Size.y];
        for(int x = 0; x < (int)Size.x; x++)
        {
            for(int y = 0; y < (int)Size.y; y++)
            {
                tiles[x,y] = Instantiate(TilePrefab, new Vector3(x,0,y), transform.rotation, WorldParent).GetComponent<InteractTile>();
                tiles[x,y].x = x;
                tiles[x,y].y = y;
            }
        }

        //Spreads random trees from 4 random locations
        for(int i = 0; i < 4; i++)
        {
            InteractTile tile = GetTile(new Vector2(Random.Range(0,(int)Size.x),Random.Range(0,(int)Size.y)));
            tile.SetType(TileType.Tree);
            TreesPlaced.Add(tile);
        }
        while(TreesPlaced.Count < TreesToPlace)
        {
            InteractTile tile = World.singleton.GetTile(TreesPlaced[Random.Range(0,TreesPlaced.Count)].GetDirection(Random.Range(0,4)));
            if(tile && tile.type == TileType.Grass)
            {
                tile.SetType(TileType.Tree);
                TreesPlaced.Add(tile);
            }
        }


    }

    public InteractTile GetTile(Vector2 pos)
    {
        if(pos.x >= Size.x || pos.y >= Size.y || pos.x < 0 || pos.y < 0) return null;
        return tiles[(int)pos.x, (int)pos.y];
    }

    // Update is called once per frame
    void Update()
    {
    }
}
