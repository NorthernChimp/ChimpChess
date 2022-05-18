using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public static ChessboardPiece GetTargetUnderMouse(CardTargetType type) 
    {
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform t = null;
        LayerMask tempMask = LayerMask.GetMask();
        switch (type)
        {
            case CardTargetType.requiresNoTarget:
                return MainScript.nullBoardPiece;
                //use a different function to check if we are over the board. this function is for getting boardpieces or units. (if it is a unit then it sends the boardpiece that unit stands on. the unit can be accessed from that)
            case CardTargetType.unitAttack:
            case CardTargetType.unitMovement:
            case CardTargetType.targetsBoardPiece: //regardless of whether it targets a board piece or a unit it ultimately just takes teh board piece that it operates on
            case CardTargetType.targetsUnit:
            case CardTargetType.targetsEnemyUnit:
            case CardTargetType.targetsFriendlyUnit:
                tempMask = LayerMask.GetMask("BoardPiece", "Unit");
                break;
            case CardTargetType.playUnit:
            case CardTargetType.emptyBoardPiece:
                tempMask = LayerMask.GetMask("BoardPiece");
                break;
        }
        /*if (currentlySelectedCard.isPurchaseCard)
        {
            tempMask = LayerMask.GetMask("Board");
            //Debug.Log("we are switching to board"); 
        }//if its a purchase card it really just needs to be played like a card that requires no target as its just beind discarded*/
        //** this must be moved to whereever we run this function from and change the input type to board if the card we're using for a target type is a research card (i.e. target type of the card is irrelevant you are researching it not playing it)
        if (Physics.Raycast(ray, out hit, tempMask))
        {
            t = hit.transform;
            //Debug.Log(t.tag);
        }
        if (t == null)//the raycast did not hit anything so there is no possible target we are hovering over
        {
            //Debug.Log("hits nothing");
            return MainScript.nullBoardPiece;
        }
        else
        {
            ChessboardPiece tempPiece = MainScript.nullBoardPiece;// check if the raycast landed on a boardpiece or a minion and get the board piece that minion is on or the piece itself
            if (t.tag == "Unit") { tempPiece = t.GetComponent<ChesspieceScript>().thisChessPiece.currentBoardPiece; }//if unit get its current board piece
            else if (t.tag == "BoardPiece") { tempPiece = t.GetComponent<ChessboardPieceScript>().thisChessboardPiece; }//if boardpiece just get teh boardpiece directly
            //if(type == CardTargetType.playUnit || type == CardTargetType.emptyBoardPiece){if (tempPiece.hasChessPiece) { return MainScript.nullBoardPiece; }}//keeping this for reference if this messes up
            if(type == CardTargetType.emptyBoardPiece){if (tempPiece.hasChessPiece) { return MainScript.nullBoardPiece; }}
            //Debug.Log(tempPiece.xPos);
            return tempPiece;
        }
    }
    public static bool GetTargetingBoard()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform t = null;
        LayerMask tempMask = LayerMask.GetMask("Board");
        if(Physics.Raycast(ray,out hit, tempMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
