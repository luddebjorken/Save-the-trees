using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public CardBase currentCard;
    List<InteractTile> highlitTiles;
    public static InteractComponent singleton;
    void Start()
    {
        singleton = this;
        highlitTiles = new List<InteractTile>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray interactRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(interactRay.origin,interactRay.direction * 1000, Color.red);
        if(Physics.Raycast(interactRay, out RaycastHit hit, Mathf.Infinity))
        {
            if(hit.transform.tag.Equals("Tile"))
            {
                InteractTile interactObject = hit.transform.GetComponent<InteractTile>();
                if(interactObject && currentCard)
                {
                    currentCard.HoverStart(interactObject);
                    if(Input.GetButton("Fire1"))
                    {
                        currentCard.Use(interactObject);
                    }
                }
            }
        }
    }

    public void HighlightTiles(List<InteractTile> tiles)
    {
        foreach(InteractTile tile in highlitTiles)
        {
            tile.Material.SetFloat("_Highlit", 0.0f);
        }

        highlitTiles = tiles;
        
        foreach(InteractTile tile in highlitTiles)
        {
            tile.Material.SetFloat("_Highlit", 1.0f);
        }
    }
}
