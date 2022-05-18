using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    public Transform attackTransform;
    public Transform healthTransform;
    public Transform costTransform;
    public List<Transform> cardNumberTransforms;
    public CardNumber attack;
    public CardNumber health;
    public CardNumber cost;
    public Sprite moveUnitSprite;
    public Sprite attackUnitSprite;
    public bool mouseIsOverImage = false;
    public bool mouseIsWithinCardWidth = false;
    public bool requiresSecondTarget = false;
    public GameObject purchaseNotifier;
    public List<Sprite> purchaseNotifierSprites;
    public UnityEngine.UI.Image notifier;
    
    // Start is called before the first frame update
    public Card thisCard = MainScript.nullCard;
    void Start()
    {
        transform.GetComponent<BoxCollider2D>().size = new Vector2(50f,100f);
    }
    public void SetupCard(Card theCard, Player theOwner,bool isUnitCard)
    {
        
        thisCard = theCard;
        //if(thisCard.owner.playerNumber != theOwner.playerNumber) { Debug.Log("the cards owner and the owner from the function are not the same" + theCard.cardName); }
        RectTransform rt = GetComponent<RectTransform>();
        //rt.localPosition = Vector3.zero;
        thisCard.transform = transform;
        thisCard.rectTransform = rt;
        thisCard.owner = theOwner;
        thisCard.render = GetComponent<UnityEngine.UI.Image>();
        if (thisCard.cardType == CardType.minion || thisCard.cardType == CardType.building)
        {
            //Debug.Log("setting up the card " + theCard.cardName + " of card type " + thisCard.cardType + " with an attack of " + thisCard.attack + " and a defence of " + thisCard.defence);
            //thisCard.attack = thisCard.reference.attack;
            //thisCard.defence = thisCard.reference.defence;
        }
        thisCard.rectTransform.localScale = MainScript.cardScale;
        if (theOwner.deck.cardsInDeck.Contains(thisCard)) { theOwner.deck.cardsInDeck.Remove(thisCard); }
        if (!isUnitCard) { theOwner.hand.cardsInHand.Add(thisCard); }//if it is not a unit card, like a card a unit controls, add it to the player's hand
        ApplyCardGraphic(); //does nothing right now
        //Debug.Log("immediately before setup number the defence value is " + thisCard.defence);
        attack = attackTransform.GetComponent<CardNumberScript>().SetupNumber(thisCard.attack);
        health = healthTransform.GetComponent<CardNumberScript>().SetupNumber(thisCard.defence);
        cost = costTransform.GetComponent<CardNumberScript>().SetupNumber(thisCard.cost);
        if(thisCard.cardType == CardType.moveUnit || thisCard.cardType == CardType.attackUnit)
        {
            cost.Disable();
        }
        else
        {
            cost.Enable();
            thisCard.cost = thisCard.cardReference.GetCost(theOwner);
            cost.SetInt(thisCard.cost);
        }
        if (thisCard.cardType == CardType.minion)
        {
            foreach(UnitReferenceModifier mod in thisCard.reference.modifiers)
            {
                if(thisCard.owner.playerNumber == mod.owner.playerNumber)
                {
                    //thisCard.app
                }
            }
            /*attack.Enable();
            health.Enable();*/
            //attack.SetInt(attack.number);
        }
        else
        {
            attack.Disable();
            health.Disable();
        }
    }
    public void ChangeNotifierSprite(int i) 
    { 
        //Debug.Log("checking for value " + i + " and the capacity is " + purchaseNotifierSprites.Count); 
        notifier.sprite = purchaseNotifierSprites[i]; 
    }
    public void SetupPurchasableCard(Card theCard, Player theOwner, bool isUnit)
    {
        SetupCard(theCard, theOwner, isUnit);
        notifier = Instantiate(purchaseNotifier, transform.position + (Vector3.up * MainScript.cardHeight * 65f), Quaternion.identity).transform.GetComponent<UnityEngine.UI.Image>();
        notifier.transform.SetParent(transform);
        theCard.isPurchaseCard = true;
        ChangeNotifierSprite(0);
    }
    public void SetupResearchableCard(Card theCard, Player theOwner, bool isUnit)//this is for setting up a research card which enables the card to be available on a unit or available to purchase at a building, rather than being shuffled into discard
    {
        SetupCard(theCard, theOwner, isUnit);
        theCard.isResearchableCard = true ;
        //Debug.Log("about to change notifiersprite");
        notifier = Instantiate(purchaseNotifier, transform.position + (Vector3.up * MainScript.cardHeight * 65f), Quaternion.identity).transform.GetComponent<UnityEngine.UI.Image>();
        notifier.transform.SetParent(transform);
        ChangeNotifierSprite(2);
    }
    public void MakeVisible()
    {
        if (notifier) { notifier.enabled = true; }
        thisCard.render.enabled = true;
        attack.Enable();
        health.Enable();
        cost.Enable();
    }
    public void MakeInvisible()
    {
        if (notifier) { notifier.enabled = false; }
        thisCard.render.enabled = false;
        attack.Disable();
        health.Disable();
        cost.Disable();
    }
    public void OnPointerOver()
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
    public void ChangeAttack(int newAtk)
    {
        attack.SetInt(newAtk);
    }
    public void ChangeCost(int newCost)
    {
        cost.SetInt(newCost);
    }
    public void ChangeHealth(int newHealth)
    {
        health.SetInt(newHealth);
    }
    public void ApplyCardGraphic()
    {
        //applies the appropriate graphics for a card based on the thisCard variable
        if(thisCard.cardType == CardType.moveUnit) { thisCard.render.sprite = moveUnitSprite; }
        else if(thisCard.cardType == CardType.attackUnit) { thisCard.render.sprite = attackUnitSprite; } else
        {
            //Debug.Log("applying the sprite name from resources load " + thisCard.cardSpriteName);
            thisCard.render.sprite = Resources.Load<Sprite>("CardSprites/" + thisCard.cardSpriteName) ;
        }

        //if()
    }
    // Update is called once per frame
    void Update()
    {
        if (mouseIsOverImage)
        {
            if (Mathf.Abs(Input.mousePosition.x - transform.position.x) < MainScript.cardHeight * 25f) { thisCard.mouseOver = true; } else thisCard.mouseOver = false;
        }
        else
        {
            thisCard.mouseOver = false;
        }
    }
}
public class Card
{
    public bool drawRange = false;
    public int drawRangeDistance = 0;
    public bool drawLineToSecondTarget = false;
    public bool isPurchaseCard = false;
    public bool isResearchableCard = false;
    public Transform transform;
    public RectTransform rectTransform;
    public UnityEngine.UI.Image render;
    public bool mouseOver = false;
    public bool requiresSecondTarget = false;
    public int attack = 0;
    public int defence = 0;
    public int utility = 0;
    public int cost = 0;
    public Player owner;
    public bool exileFromGameWhenPlayed = false;
    public bool removeFromGameWhenPlayed = false;
    public bool hasNoTransform = false;
    public string cardName = "none";
    public string cardSpriteName = "none";
    public CardReference cardReference;
    public CardTargetType targetType;
    public CardState state = CardState.inLibrary;
    public CardType cardType;
    public UnitReference reference;
    public List<CardAIComponent> aiComponent;
    public List<CardModifier> modifiers;
    public Card(int atk,int def,int util,int cos, Player theOwner,CardTargetType typeToTarget, CardType theType, string nameOfCard,Transform t,string nameOfSprite)
    {
        modifiers = new List<CardModifier>();
        aiComponent = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly) };
        foreach (CardAIComponent a in aiComponent) { a.parentCard = this; }
        transform = t;
        render = t.GetComponent<UnityEngine.UI.Image>();
        rectTransform = t.GetComponent<RectTransform>();
        cardName = nameOfCard;
        attack = atk;
        defence = def;
        utility = util;
        cost = cos;
        owner = theOwner;
        targetType = typeToTarget;
        cardType = theType;
        SetupAIComponent();
        if (cardType == CardType.minion)
        {
            Debug.Log("base health is " + defence);
            foreach (UnitReferenceModifier mod in reference.modifiers)
            {
                if (mod.owner.playerNumber == theOwner.playerNumber)
                {
                    ApplyModification(mod);
                    Debug.Log("applying modification health is " + defence);
                }
            }   
        }
        foreach (CardModifier mod in modifiers)
        {
            bool modAppliesFromOwner = mod.modOwner.playerNumber == theOwner.playerNumber;
            if (mod.modOwner == MainScript.neutralPlayer) { modAppliesFromOwner = true; }
            if (mod.type == CardModifierType.changeCost && modAppliesFromOwner)
            {
                cost += mod.amount;
                if (cost < 0) { cost = 0; }
            }
        }
    }
    public static Card GetCardWithNoTransform(CardReference cardRef, Player theOwner)
    {
        Card c = new Card(cardRef, theOwner);
        c.hasNoTransform = true;
        return c;
    }
    public Card(CardReference cardRef,Player theOwner)      //Transform t)
    {
        modifiers = cardRef.modifiers;
        //Debug.Log("creating card " + cardRef.cardName + " of target type " + cardRef.targetType + " for an owner of type " + theOwner.theType);
        reference = cardRef.reference;

        aiComponent = cardRef.ai;
        SetupAIComponent();
        drawRange = cardRef.drawRangeWhenTargeting;
        drawRangeDistance = cardRef.drawRangeDistance;
        //transform = t;
        drawLineToSecondTarget = cardRef.drawLineToSecondTarget;
        requiresSecondTarget = cardRef.requiresSecondtarget;
        cardName = cardRef.cardName;
        cardType = cardRef.cardType;
        attack = cardRef.attack;
        defence = cardRef.defence;
        utility = cardRef.utility;
        cost = cardRef.GetCost(theOwner); ;
        if (cost < 0) { cost = 0; }
        owner = theOwner;
        targetType = cardRef.targetType;
        cardSpriteName = cardRef.cardSpriteName;
        cardReference = cardRef;
        SetupAIComponent();
        if (cardType == CardType.minion)
        {
            //Debug.Log("base health is " + defence);
            foreach (UnitReferenceModifier mod in reference.modifiers)
            {
                if (mod.owner.playerNumber == theOwner.playerNumber)
                {
                    ApplyModification(mod);
                    //Debug.Log("applying modification health is " + defence);
                }
            }
        }
        foreach(CardModifier mod in modifiers)
        {
            bool modAppliesFromOwner = mod.modOwner.playerNumber == theOwner.playerNumber;
            if(mod.modOwner == MainScript.neutralPlayer) { modAppliesFromOwner = true; }
            if(mod.type == CardModifierType.changeCost && modAppliesFromOwner)
            {
                //Debug.Log("changing cost on card " + cardRef.cardName);
                //cost += mod.amount;
                
            }
        }
    }
    public void ResetUnitStats()
    {
        if(cardType == CardType.minion || cardType == CardType.building && !hasNoTransform)
        {
            transform.SendMessage("ChangeAttack", attack);
            transform.SendMessage("ChangeHealth", defence);
            transform.SendMessage("ChangeCost", cost);
        }
    }
    void ApplyModification(UnitReferenceModifier mod)
    {
        switch (mod.theType)
        {
            case UnitReferenceModifierType.addStats:
                //Debug.Log("applying mod ownere is " + mod.owner.theType);
                attack += mod.attack;
                defence += mod.health;
                cost += mod.cost;
                if(cost < 0) { cost = 0; }
                //reference.attack = attack;
                //reference.defence = defence;
            break;
        }
    }
    void SetupAIComponent()
    {
        foreach(CardAIComponent c in aiComponent)
        {
            c.parentCard = this;
        }
    }
    public Card(Player theOwner, CardType theType)
    {
        owner = theOwner;
        cardType = theType;
    }
    public void MakeInvisible() { transform.SendMessage("MakeInvisible"); }
    public void MakeVisible() { transform.SendMessage("MakeVisible"); }
    public Card(UnitReference unit,Player theOwner)// for unit cards
    {
        aiComponent = unit.cardReference.ai;
        SetupAIComponent();
        //Debug.Log(unit.unitName);
        owner = theOwner;
        reference = unit;
        cardSpriteName = unit.unitCardSpriteName;
        cardName = unit.unitPrefabName;
        targetType = CardTargetType.emptyBoardPiece;
        cardType = CardType.minion;
        cardReference = unit.cardReference;
    }
    public void FadeAfterBeingPlayed()
    {
        
    }
}
public class CardReference
{
    public bool isUnitAbility = false;
    public bool drawRangeWhenTargeting = false;
    public int drawRangeDistance = 0;
    public bool drawLineToSecondTarget = false;
    public bool requiresSecondtarget = false;
    public BuildingReference buildingRefParent;
    public int attack = 0;
    public int defence = 0;
    public int utility = 0;
    public int cost = 0;
    public string cardName = "none";
    public CardTargetType targetType;
    public CardType cardType;
    public UnitReference reference;
    public bool requiresAnimationOnPlay = true;
    public List<CardAIComponent> ai;
    public string cardSpriteName = "none";
    public List<CardModifier> modifiers;
    public CardReference(int atk, int def, int util, int cos, CardTargetType typeToTarget, CardType theType,string nameOfCard,string cardSprite)
    {
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly)};
        reference = MainScript.nullUnitReference;
        cardName = nameOfCard;
        attack = atk;
        defence = def;
        utility = util;
        cost = cos;
        targetType = typeToTarget;
        cardType = theType;
        cardSpriteName = cardSprite;
        modifiers = new List<CardModifier>();
    }
    public void CheckAttackHealth()
    {
        //attack = GetAttack();
        
    }
    public static List<CardReference> GetCardReferencesFromUnitReferences(List<UnitReference> units)
    {
        List<CardReference> tempList = new List<CardReference>();
        foreach(UnitReference u in units) { tempList.Add(u.cardReference); }
        return tempList;
    }
    public int GetAttack(Player ownerPlayer)//get the attack of a unit factoring in the unit reference modifiers a player has, neutral if they affect both
    {
        int baseAttack = reference.attack;
        foreach(UnitReferenceModifier u in reference.modifiers)
        {
            if(u.theType == UnitReferenceModifierType.addStats)
            {
                //Debug.Log("found an add stats modifier player " + u.owner.theType + " attack is " + u.attack + " and the owner player set to get attack is " + ownerPlayer.theType);

                if(u.owner.playerNumber == ownerPlayer.playerNumber)
                {
                    //Debug.Log("adding to attack");
                    baseAttack += u.attack;//attack defaults to zero so it changes nothing if its only buffing health for example
                }
            }
        }
        return baseAttack;
    }
    public int GetHealth(Player ownerPlayer)//get the attack of a unit factoring in the unit reference modifiers a player has, neutral if they affect both
    {
        int baseAttack = reference.defence;
        foreach (UnitReferenceModifier u in reference.modifiers)
        {
            if (u.theType == UnitReferenceModifierType.addStats)
            {
                if (u.owner.playerNumber == ownerPlayer.playerNumber)
                {
                    //Debug.Log("adding health and the owner type is " + ownerPlayer.theType + " this specific unit reference modifier is to " + u.owner.theType);
                    baseAttack += u.health;//attack defaults to zero so it changes nothing if its only buffing attack for example
                }
            }
        }
        return baseAttack;
    }
    public int GetCost(Player ownerPlayer)
    {
        int baseCost = cost;
        foreach(CardModifier mod in modifiers)
        {
            bool appliesToAllPlayers = mod.modOwner == MainScript.neutralPlayer;
            bool appliesToThisPlayerSpecifically = mod.modOwner.playerNumber == ownerPlayer.playerNumber;
            bool modApplies = (appliesToAllPlayers || appliesToThisPlayerSpecifically);
            if(mod.type == CardModifierType.changeCost && modApplies)
            {
                //Debug.Log("changinc the cost locally on card ref " + cardName);
                baseCost += mod.amount;
                if (baseCost < 0) { baseCost = 0; }
            }
        }
        if(cardType == CardType.minion || cardType == CardType.building)
        {
            foreach(UnitReferenceModifier mod in reference.modifiers)
            {
                bool appliesToAllPlayers = mod.owner == MainScript.neutralPlayer;
                bool appliesToThisPlayerSpecifically = mod.owner.playerNumber == ownerPlayer.playerNumber;
                bool modApplies = (appliesToAllPlayers || appliesToThisPlayerSpecifically);
                if (mod.theType == UnitReferenceModifierType.addStats && modApplies)
                {
                    baseCost += mod.cost;
                    if(baseCost < 0) { baseCost = 0; }
                }
            }
        }
        return baseCost;
    }
    public static CardReference GiveCardReferenceDrawLineToSecondTarget(CardReference c) { c.drawLineToSecondTarget = true;return c; }
    public CardReference(CardType ty,CardTargetType typeToTarget,int atk,string nameOfCard, string spriteName)
    {
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly)};
        cardSpriteName = spriteName;
        cardType = ty;
        attack = atk;
        cost = atk;
        cardName = nameOfCard;
        targetType = typeToTarget;
        modifiers = new List<CardModifier>();
    }
    public CardReference(CardType ty, CardTargetType typeToTarget, int atk, string nameOfCard, bool secondTarget, string spriteName)
    {
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly)};
        cardSpriteName = spriteName;
        cardType = ty;
        attack = atk;
        requiresSecondtarget = secondTarget;
        cardName = nameOfCard;
        targetType = typeToTarget;
        modifiers = new List<CardModifier>();
    }
    public CardReference(CardType ty, CardTargetType typeToTarget, int atk, string nameOfCard, string spriteName,bool withAnimation)
    {
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly) };
        cardSpriteName = spriteName;
        cardType = ty;
        attack = atk;
        cardName = nameOfCard;
        targetType = typeToTarget;
        requiresAnimationOnPlay = withAnimation;
        modifiers = new List<CardModifier>();
    }
    public CardReference(UnitReference unit)
    {
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly) };
        requiresSecondtarget = unit.requiresSecondTarget;
        buildingRefParent = unit.buildingRefParent;
        //requiresSecondtarget = unit.sec
        cardName = unit.unitName;
        reference = unit;
        attack = unit.attack;
        defence = unit.defence;
        utility = unit.utility;
        cost = unit.cost;
        targetType = CardTargetType.playUnit;
        cardType = CardType.minion;
        cardSpriteName = unit.unitCardSpriteName;

        modifiers = new List<CardModifier>();
    }
    public static CardReference AddModifiers(CardReference cardToAddModsTo, List<CardModifier> mods)
    {
        cardToAddModsTo.modifiers.AddRange(mods);
        return cardToAddModsTo;
    }
    public static CardReference DrawRange(CardReference r, int range)
    {
        r.drawRangeDistance = range;
        r.drawRangeWhenTargeting = true;
        return r;
    }
    public static CardReference GetBuilding(UnitReference unit)
    {
        unit.canAttack = false;
        unit.canMove = false;
        CardReference temp = new CardReference(unit);
        temp.cardType = CardType.building;
        
        return temp;

    }
    public CardReference(CardType theType,CardTargetType ty, UnitReference u, string theName,string spriteName)
    {
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly) };
        cardName = theName;
        cardType = theType;
        targetType = ty;
        reference = u;
        cardSpriteName = spriteName;
        modifiers = new List<CardModifier>();
    }

        public CardReference(CardType theType)
    {
        modifiers = new List<CardModifier>();
        cardType = theType;
        ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly) };
        if (cardType == CardType.moveUnit)
        {
            targetType = CardTargetType.unitMovement;
            cardSpriteName = "MoveMinion";
        }
        else if (cardType == CardType.attackUnit)
        {
            cardSpriteName = "AttackMinion";
            targetType = CardTargetType.unitAttack;
        }
        else
        {
            
        }
    }
    
}
public enum CardTargetType { requiresNoTarget,targetsUnit,targetsBoardPiece,emptyBoardPiece,unitMovement,unitAttack,playUnit,targetsFriendlyUnit,targetsEnemyUnit}
public enum CardState { inDeck,inHand,inDiscardPile,inLibrary}
public enum CardType { minion,spell,equipment,nullCard,moveUnit,attackUnit,building}