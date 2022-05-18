using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksScript : MonoBehaviour
{
    public static BuildingReference barracksRef;
    public UnitReference barracksUnitReference;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetupBarracksLibrary()
    {
        barracksRef = new BuildingReference();
        LibraryScript.buildingLibrary[0].buildingRefParent = barracksRef;
        LibraryScript.buildingLibrary[0].cardReference.buildingRefParent = barracksRef;
        LibraryScript.buildingRefLibrary.Add(barracksRef);
        barracksRef.buildingRefName = "barracks";
        barracksRef.cardLibrary = new List<CardReference>();
        barracksRef.unitLibrary = new List<UnitReference>()
        {
            new UnitReference("Doge", 1, 3, 2, 1, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.rook, 4, "dogeCard",false,true,CardTargetType.requiresNoTarget,AttackType.shoot),
            new UnitReference("Marauder", 3, 6, 2, 4, "largeBasicUnitPrefab", 4, ChessPieceMovementAbilityType.rook, 4, "marauderCard",false,false,CardTargetType.requiresNoTarget,AttackType.melee),
            new UnitReference("Uncle Murphy", 2, 5, 0, 5, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.rook, 2, "uncleMurphyCard",false,true,CardTargetType.targetsUnit,AttackType.shoot),
            new UnitReference("Rocket Man", 2, 5, 0, 5, "rocketManPrefab", 4, ChessPieceMovementAbilityType.rook, 1, "rocketManCard",false,false,CardTargetType.requiresNoTarget,AttackType.melee)
        };
        barracksRef.cardlessUnitLibrary = new List<UnitReference>()
        {
            new UnitReference("Roadblock",0,5,0,1,"roadblockPrefab",0,ChessPieceMovementAbilityType.none,0,"roadblockUnitCard",AttackType.melee)
        };
        barracksRef.researchStorefront = new List<CardReference>()
        {
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Upgrade Barracks Attack","UpgradeBarracksAttackCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Upgrade Barracks Defence","UpgradeBarracksDefenceCard")

        };
        foreach(CardReference r in barracksRef.researchStorefront) { r.buildingRefParent = barracksRef; }
        foreach (UnitReference u in barracksRef.unitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = barracksRef;
            barracksRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = barracksRef;
            
        }
        foreach (UnitReference u in barracksRef.cardlessUnitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = barracksRef;
            barracksRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = barracksRef;

        }
        barracksRef.unitLibrary[0].cardReference.ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly, 0, 0, 0, 8) };

        barracksRef.unitLibrary[1].cardReference.ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly, 0, 0, 0, 3) };
        //barracksRef.unitLibrary[1].cardReference.ai[0].aiChildren = new List<AIChildComponent>() { new AIChildComponent(AIChildComponentType.playNextToUnit, new List<CardReference>() { EngineeringBayScript.engineeringBayRef.cardlessBuildingLibrary[0].cardReference }, false, 8, 3, 3, 3) };
        barracksRef.unitLibrary[2].cardReference.ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playBeforeAttackingWithUnit, 0, 0, 0, 2) };
        barracksRef.unitLibrary[2].cardReference.ai[0].aiChildren = new List<AIChildComponent>() { new AIChildComponent(AIChildComponentType.playNextToUnit, new List<CardReference>() { }, true, 2, 3, 3, 3) };
        barracksRef.unitLibrary[2].cardReference.ai[0].aiChildren[0].grandChildComponents = new List<AIChildComponent>() { new AIChildComponent(AIChildComponentType.targetUnitCanAttackAMinion, new List<CardReference>(), true, 2, 2, 0, 0) };
        barracksRef.spellLibrary = new List<CardReference>()
        {
            //(new CardReference(CardType.spell, CardTargetType.targetsUnit, 6, "Fireball", "fireball")),
            (new CardReference(CardType.spell, CardTargetType.requiresNoTarget, 6, "The Power Of Friendship", "powerOfFriendshipCard")),
            new CardReference(CardType.spell,CardTargetType.targetsFriendlyUnit,4,"Youngling Slayer 3000","younglingSlayerCard"),
            //new CardReference(CardType.spell,CardTargetType.requiresNoTarget,0,"Pick A Random Unit","younglingSlayerCard")
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Behold","beholdCard"),
            new CardReference(CardType.spell,CardTargetType.targetsFriendlyUnit,1,"Phallic Science Rifle","phallicScienceRifleCard")
        };
        foreach (CardReference c in barracksRef.spellLibrary) { barracksRef.cardLibrary.Add(c); c.buildingRefParent = barracksRef; }
        barracksRef.unitAbilityLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.targetsEnemyUnit, 3, "Doge Deals Damage", "dogeDealsDamage", true),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard",true),
            new CardReference(CardType.spell, CardTargetType.targetsUnit, 4,"Nobody Tosses a Dwarf", "nobodyTossesADwarfCard",true),
            new CardReference(CardType.spell,CardTargetType.targetsBoardPiece,3,"Setup Roadblock","setupRoadblockCard",true),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,3,"Rocket Leap", "rocketLeapCard"),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,3,"Surprise", "surpriseCard")
        };
        foreach(CardReference c in barracksRef.unitAbilityLibrary)
        {
            c.buildingRefParent = barracksRef;
            c.isUnitAbility = true;
        }
        barracksRef.cardlessBuildingLibrary = new List<UnitReference>()
        {
            new UnitReference("RoadBlock",0,5,0,5,"roadblockPrefab",0,ChessPieceMovementAbilityType.none,0,"roadblockUnitCard",AttackType.melee)
        };
        foreach (UnitReference r in barracksRef.cardlessBuildingLibrary)
        {
            CardReference temp = CardReference.GetBuilding(r);
            r.cardReference = temp;
            r.cardReference.cardType = CardType.building;
            temp.buildingRefParent = barracksRef;
            r.buildingRefParent = barracksRef;
            r.storeFront = new List<CardReference>();// dont think any non carded buildings are gonna have their own storefronts
            r.researchStoreFront = new List<CardReference>();
        }
        barracksRef.cardlessSpellLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard"),
        };
        foreach(CardReference c in barracksRef.cardlessSpellLibrary)
        {
            c.buildingRefParent = barracksRef;
        }
        barracksRef.storefront = new List<CardReference>()
        {
            barracksRef.unitLibrary[0].cardReference,
            barracksRef.spellLibrary[0],
            barracksRef.spellLibrary[1],
            //barracksRef.spellLibrary[2]
        };
        //barracksRef.researchStorefront = new List<CardReference>();
    }
    public bool DoesExecutingThisEventActivateListenever(List<EventListener> l, List<GameEvent> currentEvents)//for when an event has been confirmed for executing, rather than checking for an interuption. if you interupt attacking and a unit has a listener that responds to attacking the listener will activate twice, so this function is necessary for when an action is literally about to happen
    {
        switch (l[0].listenerName)
        {
            case "Walter":
                //Debug.Log("checking walters listener for activation");
                l[0].unitOwner.currentHealth += 2;
                l[0].unitOwner.maxHealth += 2;
                currentEvents.Insert(1, new GameEvent(l[0]));
                return false;//pretty sure just the game event of attack unit, moveunit attack and deal damage should have walter covered
        }
        return false;
    }
    public List<GameEvent>Surprise(Chesspiece rocketMan, ChessboardPiece moveTarget)
    {
        List<GameEvent> tempList = new List<GameEvent>();
        List<Chesspiece> stunTargets = new List<Chesspiece>();
        List<ChessboardPiece> randomStunNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, moveTarget);
        foreach (ChessboardPiece ranStun in randomStunNeighbours)
        {
            if (ranStun.hasChessPiece)
            {
                Chesspiece ranStunUnit = ranStun.currentChessPiece;
                if(ranStunUnit.owner.playerNumber != rocketMan.owner.playerNumber && ranStunUnit.owner != MainScript.neutralPlayer)
                {
                    stunTargets.Add(ranStunUnit);
                }
            }
        }
        if (rocketMan.alive && stunTargets.Count > 0)
        {
            if (!moveTarget.hasChessPiece)
            {
                GameEvent tossEvent = GameEvent.GetTossUnitEvent(rocketMan, moveTarget);
                tempList.Add(tossEvent);
            }
            int rando = (int)Random.Range(0f, stunTargets.Count);
            Chesspiece stunTarget = stunTargets[rando];
            //stunTarget.stunned = true;
            UnitBuff testBuff = new UnitBuff(0, 0, true, false);
            testBuff.remainingStunnedTurns = 1;
            stunTarget.buffs.Add(testBuff);
            stunTarget.CalculateBuffs();
        }
        
        return tempList;
    }
    public List<GameEvent> PlaySpell(Card c, List<ChessboardPiece> targets, List<Chesspiece> units)//if a target is not required simply use nullboardpiece
    {
        //Debug.Log("playing spell " + c.cardName);
        switch (c.cardName)
        {
            case "Surprise":
                return Surprise(units[0], targets[0]);
            case "Fireball":
                return (Fireball(c, MainScript.currentBoard, units[0]));
            case "Doge Deals Damage":
                break;
            //return DogeDealsDamage(units[0], c);
            case "Youngling Slayer 3000":
            return YounglingSlayer(units[0]);
            case "Stimpak":
                break;
            case "The Power Of Friendship":
                return PowerOfFriendship(c);
            case "Nobody Tosses a Dwarf":
                return NobodyTossesADwarf(c, units[1].currentBoardPiece, units[0]);
            //return Stimpak(units[0]);
            case "Setup Roadblock":
                return SetupRoadblock(c, targets[0], units[0]);
            case "Pick A Random Unit":
                List<CardReference> un = new List<CardReference>();
                foreach(UnitReference u in barracksRef.unitLibrary) { un.Add(u.cardReference); }
                foreach(UnitReference u in barracksRef.cardlessUnitLibrary) { un.Add(u.cardReference); }
                un.AddRange(barracksRef.spellLibrary);
                return new List<GameEvent>() { GameEvent.GetChooseFromCardsEvent(un, c,c.owner,true) };
            case "Rocket Leap":
                return RocketLeap( units[0], targets[0]);
            case "Phallic Science Rifle":
                return ScienceRifle(units[0]);
                break;
        }
        return new List<GameEvent>();
    }
    List<GameEvent> ScienceRifle(Chesspiece target)
    {
        List<GameEvent> tempList = new List<GameEvent>() { GameEvent.GetBuffEvent(new UnitBuff(3, 0, false, true), new List<Chesspiece>() { target }) };
        return tempList;
    }
    List<GameEvent> RocketLeap(Chesspiece rocketMan,ChessboardPiece target)
    {
        return new List<GameEvent>() { GameEvent.GetTossUnitEvent(rocketMan, target) };
    }
    public List<GameEvent> YounglingSlayer(Chesspiece targetUnit)
    {
        List<GameEvent> tempList = new List<GameEvent>() { GameEvent.GetBuffEvent(new UnitBuff(3,3,false,false),new List<Chesspiece>() { targetUnit })};
        //targetUnit.
        return tempList;
    }
    public List<GameEvent> GetChooseCardEvent(Card cardChosen,CardReference originalCard, ChessboardPiece target, ChessboardPiece secondaryTarget,Chesspiece selectedUnit,Player targetPlayer)
    {
        switch (originalCard.cardName)
        {
            case "Pick A Random Unit":
                cardChosen.owner = targetPlayer;
                targetPlayer.hand.cardsInHand.Add(cardChosen);
                //cardChosen);
                break;

        }
        return new List<GameEvent>();
    }
    public List<CardReference> GetStorefront()
    {
        return new List<CardReference>() { barracksRef.unitLibrary[0].cardReference, barracksRef.unitLibrary[1].cardReference, barracksRef.unitLibrary[2].cardReference, barracksRef.unitLibrary[3].cardReference, barracksRef.spellLibrary[1], barracksRef.spellLibrary[0] };
    }
    public List<CardReference> GetUnitAbilities(UnitReference unit)
    {
        switch (unit.unitName)
        {
            case "Doge":
                return new List<CardReference>() { barracksRef.unitAbilityLibrary[0], barracksRef.unitAbilityLibrary[3] };
            case "Marauder":
                return new List<CardReference>() { barracksRef.unitAbilityLibrary[2] };
            case "Rocket Man":
                return new List<CardReference>() { barracksRef.unitAbilityLibrary[4] , barracksRef.unitAbilityLibrary[5] };
        }
        return new List<CardReference>();
    }
    public List<CardReference> GetBuildingResearchableCards()
    {
        return new List<CardReference>() { barracksRef.cardlessSpellLibrary[0] ,barracksRef.researchStorefront[0],barracksRef.researchStorefront[1]};
    }
    public List<ChessboardPiece> GetOnPlayTargets(Card c, ChessboardPiece target, Chesspiece selectedUnit)
    {
        //Debug.Log("we are getting onplay targets in barracksscript for " + c.cardName);
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        switch (c.cardName)
        {
            case "Surprise":
                List<ChessboardPiece> allPiecesNearRocketMan = MainScript.currentBoard.GetAllPiecesWithinDistance(3, false, selectedUnit.currentBoardPiece);
                foreach(ChessboardPiece nearRocket in allPiecesNearRocketMan)
                {
                    if (!nearRocket.hasChessPiece)//if you can move to it (throw yourself there
                    {
                        List<ChessboardPiece> neighboursOfNearRocket = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, nearRocket);//get all the neighbours
                        bool haveFoundANeighbourWithEnemy = false;
                        for (int i = 0; i < neighboursOfNearRocket.Count && !haveFoundANeighbourWithEnemy; i++)
                        {
                            ChessboardPiece currentNeighbour = neighboursOfNearRocket[i];
                            if (currentNeighbour.hasChessPiece)
                            {
                                Chesspiece unitNeighbour = currentNeighbour.currentChessPiece;
                                if(unitNeighbour.owner.playerNumber != selectedUnit.owner.playerNumber && unitNeighbour.owner != MainScript.neutralPlayer && unitNeighbour.unitRef.cardReference.cardType != CardType.building)
                                {
                                    haveFoundANeighbourWithEnemy = true;
                                }
                            }
                        }
                        if (haveFoundANeighbourWithEnemy) { tempList.Add(nearRocket); }
                    }
                    
                }
                break;
            case "Rocket Leap":
                ChessboardPiece p = selectedUnit.currentBoardPiece;
                int originX = p.xPos - 3;
                int originY = p.yPos - 3;
                List<ChessboardPiece> rocketLeapTargets = new List<ChessboardPiece>() { MainScript.nullBoardPiece };
                MainScript.possibleAttackLocations = new List<ChessboardPiece>();
                for(int x = 0; x < 2; x++)
                {
                    for(int y = 0; y < 2; y++)
                    {
                        int currentX = originX + (x * 6);
                        int currentY = originY + (y * 6);
                        if (MainScript.currentBoard.IsThisAValidPiecePosition(currentX, currentY))
                        {
                            p = MainScript.currentBoard.chessboardPieces[currentX, currentY];
                            if (!p.hasChessPiece)
                            {
                                rocketLeapTargets.Add(p);
                            }
                            else
                            {
                                MainScript.possibleAttackLocations.Add(p);
                            }
                        }
                    }
                }
                return rocketLeapTargets;
            case "Uncle Murphy":
                List<ChessboardPiece> murph = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, target);
                List<ChessboardPiece> murphDone = new List<ChessboardPiece>() { MainScript.nullBoardPiece};
                List<ChessboardPiece> couldPlayButCant = new List<ChessboardPiece>();
                foreach (ChessboardPiece m in murph)
                {
                    if (m.hasChessPiece)
                    {
                        if(m.currentChessPiece.owner == c.owner) { murphDone.Add(m); } else { couldPlayButCant.Add(m); }
                    }
                    else
                    {
                        couldPlayButCant.Add(m);
                    }
                }
                MainScript.possibleAttackLocations = couldPlayButCant;
                return murphDone;
            case "Nobody Tosses a Dwarf":
                bool thereIsASpotToTossAMinion = false;
                ChessboardPiece unitPosition = selectedUnit.currentBoardPiece;
                int differenceInYPos = 3;
                if(selectedUnit.owner.theType == PlayerType.AI) { differenceInYPos = -3; }
                int fourSpacesForward = unitPosition.yPos + differenceInYPos;
                
                List<ChessboardPiece> possibleTargets = new List<ChessboardPiece>();
                List<ChessboardPiece> possibleOnPlayTargets = new List<ChessboardPiece>();

                if (MainScript.currentBoard.IsThisAValidPiecePosition(0, fourSpacesForward))
                {
                    int twoLeftOfTarget = unitPosition.xPos - 2;
                    if (twoLeftOfTarget < 0) { twoLeftOfTarget = 0; }
                    for (int i = twoLeftOfTarget; i < unitPosition.xPos + 3; i++)
                    {
                        if (MainScript.currentBoard.IsThisAValidPiecePosition(i, fourSpacesForward))
                        {
                            ChessboardPiece currentPieceToCheck = MainScript.currentBoard.chessboardPieces[i, fourSpacesForward];
                            if (!currentPieceToCheck.hasChessPiece)
                            {
                                thereIsASpotToTossAMinion = true;
                                possibleTargets.Add(currentPieceToCheck);
                            }
                        }
                    }
                }
                MainScript.possibleUnitMoveLocations = possibleTargets;
                MainScript.possibleAttackLocations = new List<ChessboardPiece>();
                List<ChessboardPiece> unitNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, unitPosition);
                foreach (ChessboardPiece piece in unitNeighbours)
                {
                    if (piece.hasChessPiece)
                    {
                        if (((piece.currentChessPiece.owner == selectedUnit.owner || piece.currentChessPiece.owner == MainScript.neutralPlayer) && piece.currentChessPiece.unitRef.cardReference.cardType != CardType.building)&& thereIsASpotToTossAMinion)
                        {
                            possibleOnPlayTargets.Add(piece);
                            //Debug.Log("found a target" + possibleOnPlayTargets.Count);
                        }
                        else
                        {
                            MainScript.possibleAttackLocations.Add(piece);
                        }
                    }
                    else
                    {
                        MainScript.possibleAttackLocations.Add(piece);
                    }
                }
                if (possibleOnPlayTargets.Count > 0) { return possibleOnPlayTargets; } else { return new List<ChessboardPiece>() { MainScript.nullBoardPiece }; }
                if (thereIsASpotToTossAMinion)//there is a valid 
                {
                    
                    
                }
                else
                {
                    return new List<ChessboardPiece>() { MainScript.nullBoardPiece };
                }
                break;
            case "Setup Roadblock":
                ChessboardPiece pos = selectedUnit.currentBoardPiece;
                List<ChessboardPiece> roadBlockList = MainScript.currentBoard.GetAllPiecesWithinDistance(1, false, pos);
                MainScript.possibleAttackLocations = new List<ChessboardPiece>();
                for(int i =0; i < roadBlockList.Count;i++)
                {
                    ChessboardPiece ch = roadBlockList[i];
                    if (ch.hasChessPiece) { roadBlockList.Remove(ch);i--; MainScript.possibleAttackLocations.Add(ch); } else
                    {

                    }
                }
                return roadBlockList;

        }

        return tempList;
    }
    public List<GameEvent> SetupRoadblock(Card c, ChessboardPiece target,Chesspiece unit)
    {
        Vector2 direction = new Vector2(target.xPos - unit.currentBoardPiece.xPos, target.yPos - unit.currentBoardPiece.yPos);
        Vector2 perpendicularDirection = new Vector2(direction.y, direction.x);
        List<ChessboardPiece> tempList = new List<ChessboardPiece>() { target };
        if (MainScript.currentBoard.IsThisAValidPiecePosition(target.xPos + (int)perpendicularDirection.x, target.yPos + (int)perpendicularDirection.y))
        {
            if (!MainScript.currentBoard.chessboardPieces[target.xPos + (int)perpendicularDirection.x, target.yPos + (int)perpendicularDirection.y].hasChessPiece)
            {
                tempList.Add(MainScript.currentBoard.chessboardPieces[target.xPos + (int)perpendicularDirection.x, target.yPos + (int)perpendicularDirection.y]);
            }
        }
        if (MainScript.currentBoard.IsThisAValidPiecePosition(target.xPos - (int)perpendicularDirection.x, target.yPos - (int)perpendicularDirection.y))
        {
            if (!MainScript.currentBoard.chessboardPieces[target.xPos - (int)perpendicularDirection.x, target.yPos - (int)perpendicularDirection.y].hasChessPiece)
            {
                tempList.Add(MainScript.currentBoard.chessboardPieces[target.xPos - (int)perpendicularDirection.x, target.yPos - (int)perpendicularDirection.y]);
            }
        }
        return new List<GameEvent>() { GameEvent.GetSummonEventMultiple(barracksRef.cardlessBuildingLibrary[0],tempList,c.owner)};
    }
    public List<GameEvent> NobodyTossesADwarf(Card c, ChessboardPiece target, Chesspiece unit)
    {
        //Debug.Log(unit.unitRef.unitName);
        unit.SetupAnimation("toss", unit.GetAnimationLength("toss"));
        ChessboardPiece unitPosition = unit.currentBoardPiece;
        int yPosDifference = 3;
        if(unit.owner.theType != PlayerType.localHuman) { yPosDifference = -3; }
        int fourSpacesForward = unitPosition.yPos + yPosDifference;
        List<ChessboardPiece> possibleTargets = new List<ChessboardPiece>();
        List<ChessboardPiece> possibleOnPlayTargets = new List<ChessboardPiece>();
        bool thereIsASpotToTossAMinion = false;
        if (MainScript.currentBoard.IsThisAValidPiecePosition(0, fourSpacesForward))
        {
            int twoLeftOfTarget = unitPosition.xPos - 2;
            if (twoLeftOfTarget < 0) { twoLeftOfTarget = 0; }
            for (int i = twoLeftOfTarget; i < unitPosition.xPos + 3; i++)
            {
                if (MainScript.currentBoard.IsThisAValidPiecePosition(i, fourSpacesForward))
                {
                    ChessboardPiece currentPieceToCheck = MainScript.currentBoard.chessboardPieces[i, fourSpacesForward];
                    if (!currentPieceToCheck.hasChessPiece)
                    {
                        thereIsASpotToTossAMinion = true;
                        possibleTargets.Add(currentPieceToCheck);
                    }
                }
            }
        }
        if (thereIsASpotToTossAMinion)//there is a valid 
        {
            int randomInt = (int)Random.Range(0, possibleTargets.Count);
            //MainScript.unitsToReorient.Add(target.currentChessPiece);
            return new List<GameEvent>() { GameEvent.GetTossUnitEvent(target.currentChessPiece, possibleTargets[randomInt]) };
        }
        else
        {
            return new List<GameEvent>();
        }
    }
    public List<GameEvent> ApplyOnPlayEffect(Card c, ChessboardPiece target, ChessboardPiece secondaryTarget)
    {
        switch (c.cardName)
        {
            case "Doge":
                if (c.owner.theType == PlayerType.localHuman) { MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect, Color.green); } else { MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect, Color.red); }
                return new List<GameEvent>() { new GameEvent(c.owner, 1, GameEventType.drawCard) };
            case "Uncle Murphy":
                List<ChessboardPiece> temp = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, target);
                if (!temp.Contains(secondaryTarget) || !secondaryTarget.hasChessPiece) { return new List<GameEvent>(); } //if the target of uncle murphys battlecry isn't a neighbour or the selection has no chesspiece return an empty list
                else//otherwise return the two buffs for that unit
                {
                    secondaryTarget.currentChessPiece.BuffUnit(new UnitBuff(2, 2, false, false));
                    secondaryTarget.CreateHealthSymbol();
                    //secondaryTarget.currentChessPiece.attack += 3;
                    //secondaryTarget.currentChessPiece.maxHealth += 3;
                    //secondaryTarget.currentChessPiece.currentHealth += 3;
                    if (c.owner.theType == PlayerType.localHuman)
                    {
                        MainScript.leftEmptyCard.AppearAsThisUnit(secondaryTarget.currentChessPiece);
                        MainScript.leftEmptyCard.SetupAttackBuff(3);
                        MainScript.leftEmptyCard.SetupHealthBuff(3);
                        MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect, Color.green);
                    }
                    else
                    {
                        MainScript.rightEmptyCard.AppearAsThisUnit(secondaryTarget.currentChessPiece);
                        MainScript.rightEmptyCard.SetupAttackBuff(3);
                        MainScript.rightEmptyCard.SetupHealthBuff(3);
                        MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect, Color.green);
                    }
                    if (c.owner.theType == PlayerType.localHuman) { MainScript.rightEmptyCard.notifier.SetupColor(Color.green); } else { MainScript.rightEmptyCard.notifier.SetupColor(Color.red); }
                    MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
                    return new List<GameEvent>() { GameEvent.GetEmptyGameEvent(GameEventType.onPlayEffect) };
                }
                break;
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> ApplyDeathrattle(Chesspiece deadUnit)
    {
        List<GameEvent> tempList = new List<GameEvent>();
        switch (deadUnit.unitRef.cardReference.cardName)
        {
            case "Cheems":
                return new List<GameEvent>() { new GameEvent(deadUnit.owner, 1, GameEventType.drawCard) };
            case "Barrel":
                List<ChessboardPiece> barrelNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, deadUnit.currentBoardPiece);
                List<Chesspiece> unitsToDamage = new List<Chesspiece>();
                foreach (ChessboardPiece c in barrelNeighbours)
                {
                    if (c.hasChessPiece)
                    {
                        unitsToDamage.Add(c.currentChessPiece);
                    }
                }
                if (unitsToDamage.Count > 0) { return new List<GameEvent>() { new GameEvent(unitsToDamage, 10) }; } else { return new List<GameEvent>(); };
        }
        return tempList;
    }
    public List<GameEvent> ResearchCard(Card c, Player owner)
    {
        switch (c.cardName)
        {
            case "Stimpak":
                //Debug.Log("we get here right" + barracksRef.unitLibrary[0].cardReference.cardName);
                barracksRef.unitLibrary[0].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.addToAbilities, new List<CardReference>() { c.cardReference },owner));
                LibraryScript.buildingLibrary[0].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.removeFromResearchableCards, new List<CardReference>() { c.cardReference },owner));
                //LibraryScript.unitLibrary[1].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.addToAbilities, new List<CardReference>() { c.cardReference }));
                //
                return new List<GameEvent>();
            case "Upgrade Barracks Defence":
                UnitReferenceModifier m = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), c.owner);
                m.SetHealth(1);
                LibraryScript.ApplyModToCards(CardReference.GetCardReferencesFromUnitReferences(barracksRef.unitLibrary), new List<CardModifier>() {  }, new List<UnitReferenceModifier>() { m});
                break;
            case "Upgrade Barracks Attack":
                UnitReferenceModifier m2 = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), c.owner);
                m2.SetAttack(1);
                LibraryScript.ApplyModToCards(CardReference.GetCardReferencesFromUnitReferences(barracksRef.unitLibrary), new List<CardModifier>() { }, new List<UnitReferenceModifier>() { m2 });
                break;
        }
        return new List<GameEvent>();
    }
    public bool DoesThisListenerActivate(EventListener l, List<GameEvent> theQueue)
    { //checks if a listener is activated. a listener returns true if the queue has been rearranged, otherwise it just returns false and adds the listener effects to the queue if the circumstances apply
        GameEvent triggerEvent = theQueue[0];
        switch (l.unitOwner.unitRef.unitName)//within the switch case statement the function must decide whether the listener activates or changes the queue at all. you can have a when allies are attacked listener, and it would take the move attack event type etc, but would have to conclude whether the person being attacked was an ally
        {

        }
        return false;//defaults to false
    }
    public List<GameEvent> GetListenerEvents(EventListener l)
    {
        switch (l.listenerName)
        {
            
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> PowerOfFriendship(Card c)
    {
        EmptyReadableCardPrefabScript readCard = c.owner.GetPlayerEmptyReadableCard();
        readCard.AppearAsThisCard(c.cardReference,c.owner);
        List<Chesspiece> playerUnits = new List<Chesspiece>();
        List<Chesspiece> buffedUnits = new List<Chesspiece>();
        List<GameEvent> tempList = new List<GameEvent>();
        foreach (Chesspiece u in c.owner.currentUnits)
        {
            if (u.unitRef.cardReference.cardType != CardType.building)
            {
                playerUnits.Add(u);
            }
        }
        int numberOfBuffs = playerUnits.Count;
        for (int i = 0; i < numberOfBuffs; i++)
        {
            int randomInt = (int)Random.Range(0, playerUnits.Count);
            Chesspiece randomUnit = playerUnits[randomInt];
            if (!buffedUnits.Contains(randomUnit)) { buffedUnits.Add(randomUnit); }
            //randomUnit.BuffUnit(1,0,1,true);
            GameEvent buffEvent = GameEvent.GetBuffEvent(new UnitBuff(1,0,false,false),new List<Chesspiece>() { randomUnit });
            tempList.Add(buffEvent);
        }
        if (buffedUnits.Count > 0)
        {
            /*int randomInt = (int)Random.Range(0, playerUnits.Count);
            Chesspiece randomUnit = buffedUnits[randomInt];
            readCard.otherCard.AppearAsThisUnit(randomUnit);
            readCard.otherCard.SetupHealthBuff(1);
            readCard.otherCard.SetupAttackBuff(1);*/
        }
        //readCard.notifier.SetupNotification(AbilityNotificationState.playCard,)
        return tempList;
    }
    public List<GameEvent> Fireball(Card c, ChimpChessBoard b, Chesspiece unit)
    {
        List<GameEvent> events = new List<GameEvent>();
        Color playerColor = Color.green;
        if (c.owner.theType != PlayerType.localHuman) { playerColor = Color.red; }
        MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
        MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.playCard, playerColor);
        unit.DealDamage(6);
        MainScript.leftEmptyCard.AppearAsThisUnit(unit);
        MainScript.leftEmptyCard.SetupHealthBuff(-6);
        MainScript.leftEmptyCard.health.SetColor(Color.red);
        MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.dealDamage, Color.red);
        return (events);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class BuildingReference
{

    public string buildingRefName = "none";
    public List<CardReference> storefront;
    public List<CardReference> researchStorefront;
    public List<CardReference> cardLibrary;//basically every card
    public List<CardReference> spellLibrary;//every proper spell that can be bought or put into your starting deck normally
    public List<CardReference> cardlessSpellLibrary;// spells that are indirectly produced. like playing "Behold" shuffles the card "My stuff" into your discard. "My stuff" isn't produceable otherwise so its reference is here
    public List<UnitReference> cardlessBuildingLibrary;
    public List<UnitReference> unitLibrary;//library of all proper units that can be bought or put into your starting deck normally
    public List<UnitReference> buildingLibrary;//library of all proper buildings you can buy or put in your start deck
    public List<CardReference> unitAbilityLibrary;//list of abilities available to units like Doge Deals Damage, a spell card available when you have doge selected
    public List<UnitReference> cardlessUnitLibrary; // for minions that are produced through spells or other means than playing a minion card, like the barrels produced from "its raining barrels"s
    public List<CardReference> emptyCardLibrary; // for cards that dont really do anything but are necessary for a choose card event. you dont need each of 3 chosen cards to be 3 different cards if they aren't used anywhere else
    public BuildingReference()
    {
        cardlessBuildingLibrary = new List<UnitReference>();
        storefront = new List<CardReference>();
        researchStorefront = new List<CardReference>();
        cardLibrary = new List<CardReference>();
        spellLibrary = new List<CardReference>();
        cardlessSpellLibrary = new List<CardReference>();
        unitLibrary = new List<UnitReference>();
        buildingLibrary = new List<UnitReference>();
        unitAbilityLibrary = new List<CardReference>();
        cardlessUnitLibrary = new List<UnitReference>();
        emptyCardLibrary = new List<CardReference>();
    }
    
    
    
    
}