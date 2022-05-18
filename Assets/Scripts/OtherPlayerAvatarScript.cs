using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerAvatarScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Player thisPlayer;
    void Start()
    {
        
    }
    public void DrawCard(int cardsToDraw)
    {
        //Debug.Log("drawing card for other player");
        int remainingCards = 0;
        if(cardsToDraw > thisPlayer.deck.cardsInDeck.Count) 
        {
            remainingCards = cardsToDraw - thisPlayer.deck.cardsInDeck.Count;
        }
        for(int i = 0; i < cardsToDraw && thisPlayer.deck.cardsInDeck.Count >0; i++)
        {
            Card c = thisPlayer.deck.GetRandomCard();
            thisPlayer.hand.cardsInHand.Add(c);
            thisPlayer.deck.cardsInDeck.Remove(c);
        }
        if(remainingCards > 0)
        {
            thisPlayer.deck.cardsInDeck.AddRange(thisPlayer.discardPile.cardsInDiscard);
            thisPlayer.discardPile.cardsInDiscard = new List<Card>();
            if(remainingCards > thisPlayer.hand.cardsInHand.Count)
            {
                remainingCards = thisPlayer.hand.cardsInHand.Count;
            }
            for (int i = 0; i < remainingCards && thisPlayer.deck.cardsInDeck.Count > 0; i++)
            {
                Card c = thisPlayer.deck.GetRandomCard();
                thisPlayer.hand.cardsInHand.Add(c);
                thisPlayer.deck.cardsInDeck.Remove(c);
            }
        }
    }
    public void UpdatePlayer()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
