using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableLink : MonoBehaviour
{
    public string URL;
    public void BTN()
    {
        Application.OpenURL(URL);
    }
}
