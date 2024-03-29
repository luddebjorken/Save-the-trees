﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
    public int Price;
    public AudioClip[] UseSound;
    protected InteractTile LastTile;
    public virtual void Use(InteractTile tile){}
    public virtual void HoverStart(InteractTile tile){}
    public virtual void HoverEnd(InteractTile tile){}
    protected Image graphic;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    void Awake()
    {
        graphic = GetComponent<Image>();
        GetComponentInChildren<Text>().text = Price.ToString();
    }

    void Update()
    {
        graphic.color = Currency.singleton.CanAfford(Price) ? Color.white : new Color(0.3f,0.3f,0.3f,1);
    }

    public void Select()
    {
        if(!Currency.singleton.CanAfford(Price))return;
        InteractComponent.singleton.HighlightTiles(new List<InteractTile>());
        InteractComponent.singleton.currentCard = this;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        InteractComponent.singleton.CustomCursor = true;
    }
}
