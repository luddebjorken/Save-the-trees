﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public CardBase currentCard;
    public Image FlashImage;
    public int direction;
    List<InteractTile> highlitTiles;
    public static InteractComponent singleton;
    public Color HighlightTint, FireHighlightTint;
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
                    if(Input.GetButtonUp("Fire1"))
                    {
                        currentCard.Use(interactObject);
                        currentCard = null;
                        HighlightTiles(new List<InteractTile>());
                    }
                }
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {
            direction = (direction+1)%4;
        }

        if(Input.GetButtonUp("Fire1"))
        {
            currentCard = null;
            HighlightTiles(new List<InteractTile>());
        }
    }

    public Vector3 GetDirection()
    {
        switch(direction)
        {
            case 0: return new Vector3(1,0,0); 
            case 1: return new Vector3(0,0,-1); 
            case 2: return new Vector3(-1,0,0);
            case 3: return new Vector3(0,0,1);
        }
        return Vector3.zero;
    }

    public void HighlightTiles(List<InteractTile> tiles)
    {
        foreach(InteractTile tile in highlitTiles)
        {
            tile.Material.SetColor("_Color", new Color(1,1,1,1));   
            if(tile.Child)tile.Child.GetComponent<Renderer>().material.SetColor("_Color", new Color(1,1,1,1));
        }

        highlitTiles = tiles;
        
        foreach(InteractTile tile in highlitTiles)
        {
            tile.Material.SetColor("_Color", HighlightTint);
            if(tile.Child)tile.Child.GetComponent<Renderer>().material.SetColor("_Color", HighlightTint);
        }
    }
}
