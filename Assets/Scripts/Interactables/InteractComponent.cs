using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public CardBase currentCard;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            Ray interactRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(interactRay.origin,interactRay.direction * 1000, Color.red);
            if(Physics.Raycast(interactRay, out RaycastHit hit, Mathf.Infinity))
            {
                Interactable interactObject = hit.transform.GetComponent<Interactable>();
                if(interactObject && currentCard)
                {
                    currentCard.Use(interactObject);
                }
            }
        }
    }
}
