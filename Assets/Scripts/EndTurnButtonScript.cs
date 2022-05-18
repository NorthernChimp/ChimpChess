using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndTurnButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public static bool endTurnHasBeenClicked = false;
    bool mouseOver = false;
    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("over end turn");
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("leaving end turn");
        mouseOver = false;
    }
    public void SetupEndturnButton()
    {
        //Debug.Log("setting up here");
        //transform.GetComponent<BoxCollider2D>().size = (Vector2)MainScript.cardScale * 0f;
        transform.localScale = MainScript.cardScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !MainScript.currentGame.isPaused)
        {
            
            if (mouseOver)
            {
                
                endTurnHasBeenClicked = true;
            }

        }
    }
}
