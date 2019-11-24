using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public CardBase currentCard;
    public Image FlashImage;
    public Image RainImage;
    public int direction;
    List<InteractTile> highlitTiles;
    public static InteractComponent singleton;
    public Color HighlightTint, FireHighlightTint;
    private Coroutine RainFadeRoutine;
    private Coroutine LightningRoutine;
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

    public void RainFade()
    {
        if(RainFadeRoutine != null) StopCoroutine(RainFadeRoutine);
        RainFadeRoutine = StartCoroutine("RainRoutine");
    }
    public void ThunderFade()
    {
        if(LightningRoutine != null) StopCoroutine(LightningRoutine);
        LightningRoutine = StartCoroutine("FlashRoutine");
    }

    IEnumerator RainRoutine()
    {
        float startTime = Time.time;
        Color startColor = RainImage.color;
        while(Time.time - startTime< 0.5f)
        {
            RainImage.color = Color.Lerp(startColor,new Color(0.07008722f,0.1733249f,0.2358491f,0.3f), (Time.time - startTime)/0.5f);
            yield return 0;

        }
        startTime = Time.time;
        while(Time.time - startTime < 0.5f)
        {
            yield return 0;
        }
        startTime = Time.time;
        while(Time.time - startTime < 2f)
        {
            RainImage.color = Color.Lerp(new Color(0.07008722f,0.1733249f,0.2358491f,0.3f), new Color(0.07008722f,0.1733249f,0.2358491f,0), (Time.time - startTime)/2);
            yield return 0;

        }
    }
    
    IEnumerator FlashRoutine()
    {
        float startTime = Time.time;
        while(Time.time - startTime< 0.3f)
        {
            FlashImage.color = Color.Lerp(Color.white, new Color(1,1,1,0), (Time.time - startTime)/0.3f);
            yield return 0;

        }
    }
}
