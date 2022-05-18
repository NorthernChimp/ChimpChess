using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryScript : MonoBehaviour
{
    public static List<CardReference> cardLibrary;//basically every card
    public static List<CardReference> spellLibrary;//every proper spell that can be bought or put into your starting deck normally
    public static List<CardReference> cardlessSpellLibrary;// spells that are indirectly produced. like playing "Behold" shuffles the card "My stuff" into your discard. "My stuff" isn't produceable otherwise so its reference is here
    public static List<UnitReference> unitLibrary;//library of all proper units that can be bought or put into your starting deck normally
    public static List<UnitReference> buildingLibrary;//library of all proper buildings you can buy or put in your start deck
    public static List<UnitReference> cardlessBuildingLibrary;//library of all proper buildings you can buy or put in your start deck

    public static List<CardReference> startingDeck;// the normative starting deck
    public static List<CardReference> cardLibraryProper;//all proper cards
    public static List<CardReference> unitAbilityLibrary;//list of abilities available to units like Doge Deals Damage, a spell card available when you have doge selected
    public static List<UnitReference> cardlessUnitLibrary; // for minions that are produced through spells or other means than playing a minion card, like the barrels produced from "its raining barrels"
    public static EventListener dieAtEndOfTurnListener;
    public static CardReference moveUnit;
    public static CardReference attackUnit;
    public static BuildingReference enemyBuildingRef;
    public BarracksScript barracksScript;
    public HospitalScript hospitalScript;
    public EngineeringBayScript engineeringBayScript;
    public static List<BuildingReference> buildingRefLibrary;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void CreateLibrary()
    {
        //dieAtEndOfTurnListener = new EventListener(MainScript.nullUnit, new List<GameEventType>() { GameEventType.endTurn });
        //dieAtEndOfTurnListener.listenerName = "dieAtTheEndOfTurn";
        buildingRefLibrary = new List<BuildingReference>();
        //Debug.Log("creating Library");
        enemyBuildingRef = new BuildingReference();
        enemyBuildingRef.buildingRefName = "enemy";
        attackUnit = new CardReference(CardType.attackUnit);
        moveUnit = new CardReference(CardType.moveUnit);
        cardLibraryProper = new List<CardReference>();
        spellLibrary = new List<CardReference>();
        cardLibrary = new List<CardReference>();
        buildingLibrary = new List<UnitReference>();
        
        unitLibrary = new List<UnitReference>()
        { 
            //new UnitReference("Minion", 2, 6, 2, 1, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.bishop, 4, "basicMinion"), 
            new UnitReference("Cheems", 2, 3, 2, 2, "engineerPrefab", 4, ChessPieceMovementAbilityType.bishop, 1, "cheemsCard",true,false,CardTargetType.emptyBoardPiece,AttackType.melee),    
            new UnitReference("Doge", 3, 2, 2, 4, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.rook, 4, "dogeCard",false,true,CardTargetType.requiresNoTarget,AttackType.shoot) ,
            new UnitReference("Plague Virologist", 3, 2, 2, 4, "plagueDoctorPrefab", 4, ChessPieceMovementAbilityType.rook, 2, "plagueDoctorCard",false,true,CardTargetType.requiresNoTarget,AttackType.shoot) ,
            new UnitReference("Candy Stripe Nurse", 3, 2, 2, 4, "candyStripeNursePrefab", 4, ChessPieceMovementAbilityType.rook, 1, "candyStripeNurseCard",false,true,CardTargetType.requiresNoTarget,AttackType.melee) ,
            //new UnitReference("Walter", 8, 8, 2, 2, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.queen, 10, "walterCard",new List<GameEventType>(){ GameEventType.attackUnit,GameEventType.moveAttack}),
            //new UnitReference("Perro", 3, 41, 2, 2, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.queen, 5, "perroCard",new List<GameEventType>(){ GameEventType.attackUnit,GameEventType.moveAttack}),
            //new UnitReference("Caesar", 4, 14, 2, 2, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.queen, 1, "caesarCard",new List<GameEventType>(){ GameEventType.destroyMinion}),
            //new UnitReference("Doge", 6, 13, 2, 4, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.rook, 2, "dogeCard",false,true,CardTargetType.requiresNoTarget) ,
            new UnitReference("Uncle Murphy", 2, 15, 0, 0, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.rook, 2, "uncleMurphyCard",false,true,CardTargetType.targetsBoardPiece,AttackType.shoot) 
        };
        foreach (UnitReference u in unitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            cardLibraryProper.Add(currentReference);
        }
        enemyBuildingRef.unitLibrary = new List<UnitReference>()
        {
            //new UnitReference("Enemy Marine",2,8,2,2,"enemyMarinePrefab",3,ChessPieceMovementAbilityType.rook,3,"enemyDogeCard",AttackType.shoot)
        };
        enemyBuildingRef.unitAbilityLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.targetsUnit, 3, "Doge Deals Damage", "dogeDealsDamage", true)
        };
        foreach (UnitReference u in enemyBuildingRef.unitLibrary)
        {
            u.buildingRefParent = enemyBuildingRef;
            CardReference currentReference = new CardReference(u);
            //Debug.Log("setting up the unit reference " + u.unitName + " with the card reference " + currentReference.cardName);
            currentReference.buildingRefParent = enemyBuildingRef;
            cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            cardLibraryProper.Add(currentReference);
        }
        cardlessUnitLibrary = new List<UnitReference>() { 
            new UnitReference("Barrel", 0, 5, 0, 0, "barrelPrefab", 0, ChessPieceMovementAbilityType.none, 0, "barrelCard", true),
            
            new UnitReference("Walking Mine",1,2,2,1,"walkingMinePrefab",2,ChessPieceMovementAbilityType.rook,1,"walkingMineUnitCard",AttackType.shoot)
        };
        foreach (UnitReference u in cardlessUnitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
        }
        spellLibrary = new List<CardReference>()
        {
            (new CardReference(CardType.spell, CardTargetType.requiresNoTarget, cardlessUnitLibrary[0],"Its Raining Barrels", "itsRainingBarrels")),
            (new CardReference(CardType.spell, CardTargetType.targetsUnit, 6, "Fireball", "fireball")),
            new CardReference(CardType.spell,CardTargetType.targetsUnit,4,"Youngling Slayer 3000","younglingSlayerCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Behold","beholdCard"),

        };
        foreach (CardReference c in spellLibrary){cardLibrary.Add(c);cardLibraryProper.Add(c);}
        cardlessSpellLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,3,"My Stuff","myStuffCard"),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,3,"If you throw another moon at me","throwAnotherMoonCard"),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Push Units Outward","pushUnitsOutwardCard")
        };
        unitAbilityLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.targetsUnit, 3, "Doge Deals Damage", "dogeDealsDamage", true),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard",true),
            new CardReference(CardType.spell,CardTargetType.emptyBoardPiece,2,"Deploy Walking Mine", "walkingMineDeployCard"),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,1,"Toxic Flask","toxicFlaskCard"),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,1,"Wall of Toxin","wallOfToxinCard")

        };
        foreach (CardReference c in unitAbilityLibrary)
        {
            cardLibrary.Add(c);
        }
        buildingLibrary = new List<UnitReference>()
        {
            new UnitReference("Barracks",2,20,3,5,"barracksPrefab",0,ChessPieceMovementAbilityType.none,0,"barracksCard",AttackType.shoot),
            new UnitReference("Command Center",2,20,3,5,"commandCenterPrefab",0,ChessPieceMovementAbilityType.none,0,"commandCenterCard",AttackType.shoot),
            new UnitReference("Engineering Bay",2,20,3,5,"engineeringBayPrefab",0,ChessPieceMovementAbilityType.none,0,"engineeringBayCard",AttackType.shoot),
            new UnitReference("Hospital",2,20,3,5,"hospitalPrefab",0,ChessPieceMovementAbilityType.none,0,"hospitalCard",AttackType.shoot)
        };
        
        //MainScript.nullUnitReference.buildingRefParent = barr
        
        foreach (UnitReference r in buildingLibrary)
        {
            CardReference temp = CardReference.GetBuilding(r);
            cardLibrary.Add(temp);
            cardLibraryProper.Add(temp);
            r.cardReference = temp;
            r.storeFront = GetBuildingStorefront(r);
            r.researchStoreFront = GetBuildingResearchableCards(r);
            r.cardReference.ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playBeforeAnything, 0, 0, 0, 10) };
            r.cardReference.ai[0].aiChildren = new List<AIChildComponent>() { new AIChildComponent(AIChildComponentType.dontPlayIfYouHaveThisUnit, new List<CardReference>() { r.cardReference }, false, -100, 0, 0, 0) };
            r.cardReference.ai[0].aiChildren.Add(new AIChildComponent(AIChildComponentType.playClosestToenemyZone,new List<CardReference>(), true, 2, 6, 0, 0));
        }
        barracksScript.SetupBarracksLibrary();
        hospitalScript.SetupHospitalLibrary();
        engineeringBayScript.SetupEngineeringBayLibrary();
        cardlessBuildingLibrary = new List<UnitReference>()
        {
            new UnitReference("Icarus Mortar Installation", 2 ,12,0,2,"icarusMortarPrefab",0,ChessPieceMovementAbilityType.none,0,"icarusMortarCard",AttackType.shoot)
        };
        foreach (UnitReference r in cardlessBuildingLibrary)
        {
            CardReference temp = CardReference.GetBuilding(r);
            cardLibrary.Add(temp);
            //cardLibraryProper.Add(temp);
            r.cardReference = temp;
            r.storeFront = GetBuildingStorefront(r);
            r.researchStoreFront = GetBuildingResearchableCards(r);
        }
        //startingDeck = new List<CardReference>() { cardLibraryProper[0], cardLibraryProper[0], cardLibraryProper[0], cardLibraryProper[0], cardLibraryProper[1], cardLibraryProper[1], cardLibraryProper[1], cardLibraryProper[1], buildingLibrary[0].cardReference, buildingLibrary[0].cardReference };
        startingDeck = new List<CardReference>()
        {
            /*buildingLibrary[0].cardReference,
            unitLibrary[2].cardReference,
            unitLibrary[2].cardReference,
            //unitLibrary[2].cardReference,
            //unitLibrary[3].cardReference,
            //unitLibrary[3].cardReference,
            //unitLibrary[3].cardReference,
            //buildingLibrary[1].cardReference,
            //buildingLibrary[2].cardReference,
            //buildingLibrary[3].cardReference,
            cardLibraryProper[0],
            cardLibraryProper[0],
            cardLibraryProper[1],
            cardLibraryProper[1]
            /*buildingLibrary[0].cardReference,
            buildingLibrary[1].cardReference,
            buildingLibrary[2].cardReference,
            buildingLibrary[3].cardReference*/
        };
        //startingDeck.AddRange(barracksScript.barracksRef.cardLibrary);
        //startingDeck.AddRange(engineeringBayScript.engineeringBayRef.cardLibrary);
        //startingDeck.AddRange(hospitalScript.hospitalRef.cardLibrary);
        
        startingDeck = new List<CardReference>();
        foreach(UnitReference u in BarracksScript.barracksRef.unitLibrary)
        {
            startingDeck.Add(u.cardReference);
        }
        foreach(CardReference c in BarracksScript.barracksRef.spellLibrary)
        {
            //startingDeck.Add(c);
        }
        foreach (UnitReference u in HospitalScript.hospitalRef.unitLibrary)
        {
            startingDeck.Add(u.cardReference);
        }
        foreach (UnitReference u in EngineeringBayScript.engineeringBayRef.unitLibrary)
        {
            startingDeck.Add(u.cardReference);
        }
        //startingDeck.AddRange(barracksScript.barracksRef.cardLibrary);
        //startingDeck.AddRange(barracksScript.barracksRef.cardLibrary);
    }
    List<CardReference> GetBuildingStorefront(UnitReference r)
    {
        switch (r.unitName)
        {
            case "Barracks":
                return new List<CardReference>() { unitLibrary[0].cardReference,unitLibrary[1].cardReference,unitLibrary[2].cardReference };
            case "Engineering Bay":
                return new List<CardReference>() { cardLibrary[3], cardLibrary[4], cardLibrary[5] };
            case "Hospital":
                return new List<CardReference>() { cardLibrary[6], cardLibrary[7], cardLibrary[9] };
            case "Command Center":
                List<CardReference> temp = new List<CardReference>();
                foreach(UnitReference u in buildingLibrary)
                {
                    //temp.Add(u.cardReference);
                }
                return temp;
        }
        return new List<CardReference>();
    }
    public static List<CardReference> GetBuildingResearchableCards(UnitReference u)
    {
        switch (u.unitName)
        {
            case "Engineering Bay":
                return new List<CardReference>() { cardlessSpellLibrary[1] };
        }
        return new List<CardReference>();
    }
    public static List<CardReference> GetUnitAbilities(UnitReference unit)
    {
        switch (unit.unitName)
        {
            case "Doge":
                return new List<CardReference>() { unitAbilityLibrary[0]};
            case "Cheems":
                return new List<CardReference>() { spellLibrary[0] ,cardlessBuildingLibrary[0].cardReference,unitAbilityLibrary[2]};
            case "Icarus Mortar Installation":
                return new List<CardReference>() { cardlessSpellLibrary[2] };
            case "Walking Mine":
                return new List<CardReference>() { cardlessSpellLibrary[3] };
        }
        return new List<CardReference>();
    }
    public static void ApplyModToCards(List<CardReference> refs, List<CardModifier> cardMods,List<UnitReferenceModifier> unitMods)
    {
        bool debugging = false;

        if (debugging) Debug.Log("researching upgrade barracks defence");
        foreach(CardReference r in refs)
        {
            if (debugging) Debug.Log("CARD REFERENCE : " + r.cardName);
            foreach(CardModifier cardMod in cardMods)
            {
                if (debugging) Debug.Log("ADDING THE CARD MOD " + cardMod.type);
                r.modifiers.Add(cardMod);
                bool cardRequiresSystematicModification = false;
                if (cardMod.type == CardModifierType.changeCost) { cardRequiresSystematicModification = true; }
                if(cardRequiresSystematicModification)//this should really be any card mod that needs to be immediately updated, like a change in cost or unit stats
                {
                    if (debugging) Debug.Log("requires systematicModification");
                    foreach (Player p in MainScript.currentGame.allPlayers)
                    {
                        if (p != MainScript.neutralPlayer)//this makes sure we dont check neutral players hand discard pile etc cause he doesn't have those. neutral player mods apply to all players and will be applied on actual player cards
                        {
                            if (debugging) Debug.Log("checking for player of type " + p.theType);
                            bool applyToAllCards = cardMod.modOwner == MainScript.neutralPlayer;
                            bool playerIsModOwner = cardMod.modOwner.playerNumber == p.playerNumber;
                            if (applyToAllCards || playerIsModOwner)
                            {
                                if (debugging) Debug.Log("player is either the owner or it applies to all cards (owner is neutral player");
                                List<Card> allPlayerCards = new List<Card>();
                                allPlayerCards.AddRange(p.hand.cardsInHand);
                                allPlayerCards.AddRange(p.discardPile.cardsInDiscard);
                                allPlayerCards.AddRange(p.deck.cardsInDeck);
                                if(cardMod.type == CardModifierType.changeCost)
                                {
                                    foreach (Card c in allPlayerCards)
                                    {
                                        if(c.cardReference == r)
                                        {
                                            Debug.Log("changinc the cost of " + c.cardName + " his defence now is " + c.cost + "and afterward should be " + c.cardReference.GetCost(p));
                                            c.cost = c.cardReference.GetCost(p);
                                            if(p.theType == PlayerType.localHuman && p.hand.cardsInHand.Contains(c)){ c.ResetUnitStats(); }
                                            //c.ResetUnitStats();
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(r.cardType == CardType.minion ||r.cardType == CardType.building)
            {
                //Debug.Log("we found a minion/building it is " + r.cardName);
                foreach (UnitReferenceModifier mod in unitMods)
                {
                    if (debugging) Debug.Log("APPLYING THE UNIT MOD " + mod.theType);
                    r.reference.modifiers.Add(mod);//add the modifier to the reference
                    bool cardRequiresSystematicModification = false;//do we need to change all the cards? defaults no
                    if (mod.theType == UnitReferenceModifierType.addStats) { cardRequiresSystematicModification = true; }//if we are adding stats we have the change the old cards made without the mod
                    if (cardRequiresSystematicModification)//again this is for when you need to go through all cards and change them for some reason. otherwise cards built with the reference will add the upgrade automatically (I hope)
                    {
                        if (debugging) Debug.Log("this mod requires systematicmodification");
                        foreach (Player p in MainScript.currentGame.allPlayers)
                        {
                            if (debugging) Debug.Log("checking for player number " + p.playerNumber + " and they are of type " + p.theType);
                            if (p != MainScript.neutralPlayer)//this makes sure we dont check neutral players hand discard pile etc cause he doesn't have those. neutral player mods apply to all players and will be applied on actual player cards
                            {
                                bool applyToAllCards = mod.owner== MainScript.neutralPlayer;
                                bool playerIsModOwner = mod.owner.playerNumber == p.playerNumber;
                                if (applyToAllCards || playerIsModOwner)
                                {
                                    if (debugging) Debug.Log("player is either the owner or it applies to all cards (owner is neutral player");
                                    List<Card> allPlayerCards = new List<Card>();
                                    allPlayerCards.AddRange(p.hand.cardsInHand);
                                    allPlayerCards.AddRange(p.discardPile.cardsInDiscard);
                                    allPlayerCards.AddRange(p.deck.cardsInDeck);
                                    foreach (Card c in allPlayerCards)
                                    {
                                        if(c.cardReference == r)
                                        {
                                            if (debugging) Debug.Log("found a copy of the card reference in the players cards");
                                            if (mod.theType == UnitReferenceModifierType.addStats)
                                            {
                                                c.attack = c.reference.GetAttackForOwner(p);
                                                //Debug.Log("current defence is " + c.defence);
                                                c.defence = c.reference.GetDefenceForOwner(p);
                                                //Debug.Log("now defence is " + c.defence);
                                                c.cost = c.cardReference.GetCost(p);
                                                if (p.theType == PlayerType.localHuman)
                                                {
                                                    
                                                    if (p.hand.cardsInHand.Contains(c)) { c.ResetUnitStats(); if (debugging) Debug.Log("it is in their hand and they are local human"); }
                                                }
                                            }
                                        }
                                        if(c.cardType == CardType.minion || c.cardType == CardType.building)
                                        {
                                            //c.reference.modifiers.Add(mod);
                                            
                                        }
                                    }
                                    foreach(Chesspiece u in p.currentUnits)
                                    {
                                        if (u.alive)
                                        {
                                            if(u.unitRef.cardReference == r)
                                            {
                                                //Debug.Log("health for owner " + u.unitRef.GetDefenceForOwner(u.owner));
                                                u.currentHealth += mod.health;
                                                u.CalculateBuffs();
                                                //u.attack += mod.attack;
                                                if(u.attack < 0) { u.attack = 0; }
                                                //u.maxHealth += mod.health;
                                                
                                                if(u.maxHealth < 1) { u.maxHealth = 1; }
                                                if(u.currentHealth > u.maxHealth) { u.currentHealth = u.maxHealth; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            
        }
        /*UnitReferenceModifier mod = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), owner);
        mod.SetHealth(1);
        foreach (UnitReference u in barracksRef.unitLibrary)
        {
            //Debug.Log("adding buff to unit reference " + u.unitName);
            u.modifiers.Add(mod);
        }
        foreach (Chesspiece u in owner.currentUnits)
        {
            if (u.alive && u.unitRef.cardReference.buildingRefParent == barracksRef && u.unitRef.cardReference.cardType == CardType.minion)
            {
                u.maxHealth += mod.health;
                if (u.maxHealth < 1) { u.maxHealth = 1; }//do you want a negative heatlh buff to kill a minion? if so remove this line
                if (mod.health > 0) { u.currentHealth += mod.health; } else { if (u.currentHealth > u.maxHealth) { u.currentHealth = u.maxHealth; } }

                //if(u.unitRef.cardReference.buildingRefParent == barracksRef) { u.unitRef.defence = u.unitRef.cardReference.GetHealth(owner);u.unitRef.cardReference.defence = u.unitRef.defence; u.maxHealth = u.unitRef.defence; u.currentHealth++; }
            }
        }
        List<Card> allCards = new List<Card>();
        allCards.AddRange(c.owner.deck.cardsInDeck);
        allCards.AddRange(c.owner.hand.cardsInHand);
        allCards.AddRange(c.owner.discardPile.cardsInDiscard);
        foreach (Card card in allCards)
        {
            if (card.cardType == CardType.minion && card.cardReference.buildingRefParent == barracksRef)
            {
                card.attack = card.reference.GetAttackForOwner(card.owner);
                card.defence = card.reference.GetDefenceForOwner(card.owner);
                if (card.owner.hand.cardsInHand.Contains(card))
                {
                    card.ResetUnitStats();
                }
            }
        }*/
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class HandOfCards
{
    public List<Card> cardsInHand;
    public HandOfCards()
    {
        cardsInHand = new List<Card>();
    }
}
public class DeckOfCards
{
    public List<Card> cardsInDeck;
    public DeckOfCards()
    {
        cardsInDeck = new List<Card>();
    }
    public Card GetRandomCard()
    {
        int i = (int)Random.Range(0f, (float)cardsInDeck.Count);
        return (cardsInDeck[i]);
    }
}
public class DiscardPile
{
    public List<Card> cardsInDiscard;
    public Player owner;
    public DiscardPile(Player owns)
    {
        owner = owns;
        cardsInDiscard = new List<Card>();
    }
    public void AddToDiscardFromPurchase(List<Card> purchasedCard)
    {
        foreach(Card c in purchasedCard)
        {
            //Debug.Log("adding card from purchase it is " + c.cardName + " and the owner is type " + owner.theType);
            cardsInDiscard.Add(new Card(c.cardReference, owner));
        }
    }
}
public class BuildingStoreFront
{
    public List<CardReference> cardsThatCanBeDiscarded;
    public BuildingStoreFront(List<CardReference> cards)
    {
        cardsThatCanBeDiscarded = cards;
    }
}
public class CardModifier
{
    public Player modOwner = MainScript.neutralPlayer;
    public int amount = 0;
    public int degree = 0;
    public int speed = 0;
    public CardModifierType type;
    public CardModifier(CardModifierType t, Player owns)
    {
        modOwner = owns;
        type = t;
    }
    public void SetAmount(int am) { amount = am; }
    public void SetDegree(int de) { degree = de; }
    public void SetSpeed(int sp) { speed = sp; }
}
public enum CardModifierType { changeCost,drawCardWhenPlayed}