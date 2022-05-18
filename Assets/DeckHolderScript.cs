using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckHolderScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<Card> cardsInDeck;//cards that are already in the deck
    public List<Card> cardsIncoming;//cards that are coming into the deck;
    public GameObject cardBackPrefab;
    public List<Transform> cardBacks;
    List<Vector3> cardBackPositions;
    int previousNumberOfCards = 0;
    public DeckHolderType deckType;
    public bool mouseIsOverImage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOverImage = true;
        //if (thisCard != MainScript.nullCard) { thisCard.mouseOver = true;  }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOverImage = false;
        //if (thisCard != MainScript.nullCard) { thisCard.mouseOver = true; }
    }
    public void SetupDeck(DeckHolderType ty, List<Card> cardDeck)
    {
        deckType = ty;
        cardBacks = new List<Transform>();
        cardsInDeck = cardDeck;
        SetupCardBacks();
        cardsIncoming = new List<Card>();
    }
    public void SetupCardBacks()
    {
        
        int numberOfCards = cardsInDeck.Count;
        
        int amountToAdd = numberOfCards - previousNumberOfCards;
        //Debug.Log("num of cards " + numberOfCards + " previous " + previousNumberOfCards + " and amot to add " + amountToAdd);
        if (amountToAdd > 0)
        {
            for (int i = 0; i < amountToAdd; i++)
            {
                cardBacks.Add(Instantiate(cardBackPrefab, transform.position, Quaternion.identity, transform).transform);
            }
        }else if (amountToAdd < 0)
        {
            for(int i = 0; i < Mathf.Abs(amountToAdd); i++)
            {
                if(cardBacks.Count > 0)
                {
                    Transform t = cardBacks[0];
                    cardBacks.Remove(t);
                    Destroy(t.gameObject);
                }
                
            }
        }
        
        foreach(Transform t in cardBacks) { t.localScale = MainScript.cardScale * 0.25f; }
        GetPositionsForCardBacks();
        previousNumberOfCards = numberOfCards;
    }
    void MakeEmptyCardMovingToDiscard(Card c)
    {
        cardsIncoming.Add(c);
    }
    void GetPositionsForCardBacks()
    {
        cardBackPositions = new List<Vector3>();
        float totalWidth = transform.localScale.x * 102.4f;
        Vector3 origin = transform.position + (Vector3.left * totalWidth * 0.35f) + (Vector3.down * totalWidth * 0.035f);
        float distancePerCard = totalWidth / cardBacks.Count;
        for (int i = 0; i < cardBacks.Count;i++)
        {
            Transform t = cardBacks[i];
            cardBackPositions.Add(origin + (Vector3.right * distancePerCard * i) + (Vector3.up * distancePerCard * i  *0.1f));
        }
    }
    public void UpdateDeckHolder()
    {
        for(int i = 0; i < cardBacks.Count; i++)
        {
            Transform t = cardBacks[i];
            Vector3 pos = cardBackPositions[i];
            if(t.position != pos)
            {
                Vector3 rawDirection = pos - t.position;
                float distance = rawDirection.magnitude;
                float distanceToMove = MainScript.defaultCardSpeed * Time.deltaTime;
                if(distance < distanceToMove)
                {
                    t.position = pos;
                }
                else
                {
                    t.Translate(distanceToMove * rawDirection.normalized);
                }
            
            }
        }
        for (int i = 0; i < cardsIncoming.Count; i++)
        {
            Card c = cardsIncoming[i];
            Vector3 directToThis = transform.position - c.transform.position;
            float distance = directToThis.magnitude;
            float distanceToMove = MainScript.defaultCardSpeed * Time.deltaTime * 3.75f;
            if (distanceToMove > distance)
            {
                c.transform.position = transform.position;
                c.MakeInvisible();
                cardsIncoming.Remove(c);
                Destroy(c.transform.gameObject);
                i--;
            }
            else
            {
                c.transform.Translate(directToThis.normalized * distanceToMove);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum DeckHolderType { deck,discardPile,graveyard}