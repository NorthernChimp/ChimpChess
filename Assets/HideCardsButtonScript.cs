using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HideCardsButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public static bool mouseOver = false;
    public UnityEngine.UI.Image render;
    void Start()
    {
        
    }
    public void OnPointerOver()
    {
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        //if (thisCard != MainScript.nullCard) { thisCard.mouseOver = true;  }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        //if (thisCard != MainScript.nullCard) { thisCard.mouseOver = true; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
