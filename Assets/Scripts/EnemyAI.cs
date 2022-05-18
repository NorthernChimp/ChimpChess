using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI :MonoBehaviour
{
    // Start is called before the first frame update
    public Player thisPlayer;
    public Chesspiece currentUnit = MainScript.nullUnit;
    ChessboardPiece currentTarget = MainScript.nullBoardPiece;
    public BasicSpellScript spellScript;
    public AIBuildOrder build;
    public List<UnitReference> minions;
    public static BuildingReference enemyReference;
    public bool hasSummonedMinions = false;
    public bool hasBuiltDeckThisTurn = false;
    public bool hasResearchedThisTurn = false;
    public bool currentUnitHasAttacked = false;
    public bool currentUnitCanAttack = false;
    public bool currentUnitHasAttackTarget = false;
    public bool hasCheckedForTargets = false;
    public bool currentUnitHasMoved = false;
    public bool currentUnitCanMove = false;
    public bool hasCheckeforMovement = false;
    bool hasCheckedForAbilities = false;
    public Chesspiece unitTargetForMoveAttack = MainScript.nullUnit;
    public List<Chesspiece> piecesToMove;
    public List<Chesspiece> possibleAttackTargets;
    List<ChessboardPiece> possibleMoveLocation = new List<ChessboardPiece>();
    List<ChessboardPiece> possibleMoveAttackLocation = new List<ChessboardPiece>();
    List<CardReference> cardsPlayed = new List<CardReference>();// a track of every card the enemy played and the turn it was played on
    int currentTurn = 0;
    void Start()
    {
        
    }
    public void SetupEnemyAi()
    {
        build = new AIBuildOrder(BuildOrderType.buildDeckRandomly);
        cardsPlayed = new List<CardReference>() { };
        hasCheckedForAbilities = false;
        enemyReference = new BuildingReference();
        enemyReference.buildingRefName = "enemy";
        enemyReference.cardLibrary = new List<CardReference>();
        enemyReference.unitLibrary = new List<UnitReference>()
        {
            new UnitReference("Enemy Doge",2,8,2,1,"enemyMarinePrefab",3,ChessPieceMovementAbilityType.rook,3,"enemyDogeCard",AttackType.shoot)
        };
        foreach(UnitReference u in enemyReference.unitLibrary)
        {
            u.buildingRefParent = enemyReference;
            u.cardReference = new CardReference(u);
            enemyReference.cardLibrary.Add(u.cardReference);
        }
        enemyReference.unitAbilityLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.targetsUnit, 0, "Doge Deals Damage", "dogeDealsDamage", true)
        };
        foreach(CardReference s in enemyReference.unitAbilityLibrary)
        {
            s.buildingRefParent = enemyReference;
            s.isUnitAbility = true;
        }
        enemyReference.cardlessBuildingLibrary = new List<UnitReference>()
        {

        };
        foreach (UnitReference u in enemyReference.cardlessBuildingLibrary)
        {
            u.buildingRefParent = enemyReference;
            CardReference temp = CardReference.GetBuilding(u);
            u.cardReference = temp;
            temp.buildingRefParent = enemyReference;
            u.buildingRefParent = enemyReference;
            u.storeFront = new List<CardReference>();// dont think any non carded buildings are gonna have their own storefronts
            u.researchStoreFront = new List<CardReference>();
            enemyReference.cardLibrary.Add(u.cardReference);
        }
        foreach (UnitReference r in enemyReference.cardlessBuildingLibrary)
        {
            
        }
        enemyReference.cardlessUnitLibrary = new List<UnitReference>()
        {
            new UnitReference("Barrel", 0, 5, 0, 0, "barrelPrefab", 0, ChessPieceMovementAbilityType.none, 0, "barrelCard", true)

        };
        foreach (UnitReference u in enemyReference.cardlessUnitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = enemyReference;
            //engineeringBayRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = enemyReference;
        }
        //foreach (UnitReference c in engineeringBayRef.cardlessUnitLibrary) { c.buildingRefParent = engineeringBayRef; }
        enemyReference.spellLibrary = new List<CardReference>()
        {
            //(new CardReference(CardType.spell, CardTargetType.requiresNoTarget, enemyReference.cardlessUnitLibrary[0],"Its Raining Barrels", "itsRainingBarrels")),
            //(new CardReference(CardType.spell, CardTargetType.requiresNoTarget, 0, "The Power Of Friendship", "powerOfFriendshipCard"))
            
        };
        foreach (CardReference c in enemyReference.spellLibrary) { c.buildingRefParent = enemyReference; }
        

        //enemyReference.spellLibrary[0].ai[0].aiChildren.Add(AIChildComponent.GetDontPlayBeforeAfter(new List<CardReference>() { enemyReference.spellLibrary[1] }, true, 1, -10));
        //enemyReference.spellLibrary[1].ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playBeforeAnything, 10, 10, 10,5) };
        //enemyReference.spellLibrary[1].ai[0].aiChildren.Add(AIChildComponent.GetDontPlayBeforeAfter(new List<CardReference>() { enemyReference.spellLibrary[0] }, true, 1, -10));
        foreach (CardReference c in enemyReference.spellLibrary) 
        {
            foreach(CardAIComponent ai in c.ai)
            {
                foreach(AIChildComponent child in ai.aiChildren)
                {
                    child.SetupParent(ai);
                }
            }
        }
        Debug.Log("adding first spell");
        enemyReference.spellLibrary.Add(EngineeringBayScript.engineeringBayRef.spellLibrary[0]);
        enemyReference.spellLibrary[0].ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playBeforeAnything, 10, 10, 10, 5) };
        Debug.Log("adding second spell");
        //Debug.Log(BarracksScript.barracksRef.spellLibrary.Count);
        enemyReference.spellLibrary.Add(BarracksScript.barracksRef.spellLibrary[2]);
        Debug.Log(enemyReference.spellLibrary[1].targetType);
        enemyReference.spellLibrary[1].ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playBeforeAttackingWithUnit, 0, 0, 0, 0) };
        enemyReference.spellLibrary[1].ai[0].aiChildren.Add(new AIChildComponent(AIChildComponentType.targetUnitCanAttackAMinion, new List<CardReference>(), true, 3, 1, 0, 0));
        enemyReference.spellLibrary[1].ai[0].aiChildren.Add(new AIChildComponent(AIChildComponentType.ifBestTargetIsCurrentUnit, new List<CardReference>(), true, 6, 1, 0, 0));
        Debug.Log("adding third spell");
        enemyReference.spellLibrary.Add(HospitalScript.hospitalRef.spellLibrary[0]);
        enemyReference.spellLibrary[2].ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playAfterMoveAttacking, 3, 3, 3, 4) };
        enemyReference.spellLibrary[2].ai[0].aiChildren.Add(new AIChildComponent(AIChildComponentType.ifHealIsAboveThreshold, new List<CardReference>(), true, 5, 4, 4, 0));
        enemyReference.spellLibrary[2].ai[0].aiChildren.Add(new AIChildComponent(AIChildComponentType.ifBestTargetIsCurrentUnit, new List<CardReference>(), true, 6, 1, 0, 0));
        foreach(AIChildComponent a in enemyReference.spellLibrary[1].ai[0].aiChildren)
        {
            a.SetupParent(enemyReference.spellLibrary[1].ai[0]);
        }
        foreach (AIChildComponent a in enemyReference.spellLibrary[2].ai[0].aiChildren)
        {
            a.SetupParent(enemyReference.spellLibrary[2].ai[0]);
        }
    }
    public List<CardReference> GetUnitAbilities(UnitReference u)
    {
        //Debug.Log("getting unit abilities of " + u.unitName);
        switch (u.unitName)
        {
            case "Enemy Doge":
                return new List<CardReference>() { enemyReference.unitAbilityLibrary[0] };
        }
        return new List<CardReference>();
    }
    public List<GameEvent> ItsRainingBarrels(Card c)
    {
        List<GameEvent> events = new List<GameEvent>();
        List<ChessboardPiece> tempList = MainScript.currentBoard.GetRandomBoardPiece(false, true, 3);
        events.Add(new GameEvent(c.reference, tempList, MainScript.neutralPlayer));
        return (events);
    }
    void ResetCurrentUnitBools()
    {
        currentUnitCanAttack = true;
        unitTargetForMoveAttack = MainScript.nullUnit;
        currentUnitHasAttacked = false;
        currentUnitHasAttackTarget = false;
        hasCheckedForTargets = false;
        currentUnitHasMoved = false;
        currentUnitCanMove = true;
        hasCheckeforMovement = false;
    }
    void SetupMoveLocations()
    {
        possibleMoveLocation = MainScript.GetMoveableSpacesForUnit(currentUnit, false, true);
        possibleMoveAttackLocation = new List<ChessboardPiece>();
        foreach (ChessboardPiece c in possibleMoveLocation)
        {
            if (c.hasChessPiece)
            {
                //Debug.Log("haschesspiece at " + c.xPos + " and " + c.yPos);
                if (c.currentChessPiece.owner != currentUnit.owner)
                {
                    if(c.currentChessPiece.owner.playerNumber != MainScript.neutralPlayer.playerNumber)
                    {
                        possibleMoveAttackLocation.Add(c);
                    }
                    //Debug.Log("found a target");
                    
                }
            }
        }
        hasCheckeforMovement = true;
    }
    public List<GameEvent> PlaySpell(Card c , List<ChessboardPiece> targets, List<Chesspiece> units, GameEvent e)
    {
        switch (c.cardName)
        {
            case "Its Raining Barrels":
                return ItsRainingBarrels(c);
            case "Doge Deals Damage":
                targets[0].ChangeTransparentRenderColor(Color.red);
                targets[0].currentChessPiece.DealDamage(3);
                break;
        }
        return new List<GameEvent>();
    }
    
public List<ChessboardPiece> GetOnPlayTargets(Card c, ChessboardPiece target, Chesspiece selectedUnit)
    {
        switch (c.cardName)
        {
            case "Doge Deals Damage":
                ChimpChessBoard b = MainScript.currentBoard;
                List<ChessboardPiece> allUnitPieces = b.GetAllBoardPieces(true, false);
                List<Chesspiece> allEnemyUnits = b.BoardPieceToUnit(allUnitPieces, thisPlayer, false, true, false);
                List<ChessboardPiece> enemyTargets = b.UnitToBoardPiece(allEnemyUnits);
                foreach(ChessboardPiece p in enemyTargets) { p.ChangeTransparentRenderColor(Color.cyan); }
                return enemyTargets;
        }
        return new List<ChessboardPiece>();
    }
        public void SetupAITurn()
    {
        //Debug.Log("setting up turn cards in hand : " + thisPlayer.hand.cardsInHand.Count);
        hasSummonedMinions = false;
        hasBuiltDeckThisTurn = false;
        hasResearchedThisTurn = false;
        currentTurn++;
        //cardsPlayed.Add(new List<CardReference>());
        piecesToMove = new List<Chesspiece>();
        piecesToMove.AddRange(thisPlayer.currentUnits);
        foreach(Chesspiece c in piecesToMove) { c.canAttack = true;c.canMove = true; }
        GetNewCurrentUnit();
        //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeUsingUnits);
    }
    public List<GameEvent> GetNextAction()
    {

        if (!hasSummonedMinions)
        {
            Debug.Log("summoning");
            hasSummonedMinions = true;
            return GetSummonEvents();
        }
        else
        {
            //Debug.Log("we have already summoned");

            if (currentUnit == MainScript.nullUnit) { Debug.Log("ending turn, current unit is null unit"); return new List<GameEvent>() { GameEvent.GetEndTurnEvent() }; }
            else
            {
                //Debug.Log("Getting next action for unit at " + currentUnit.currentBoardPiece.xPos + " Xpos and " + currentUnit.currentBoardPiece.yPos + " Ypos");
                List<GameEvent> templist = new List<GameEvent>();
                if ((!currentUnitHasMoved || !currentUnitHasAttacked))
                {

                    return GetUnitAction();
                }
                else
                {
                    Debug.Log("changing currentUnit" + currentUnitHasMoved + currentUnitHasAttacked);
                    piecesToMove.Remove(currentUnit);
                    if (piecesToMove.Count > 0)
                    {
                        int randomInt = (int)Random.Range(0, piecesToMove.Count);
                        currentUnit = piecesToMove[randomInt];
                        ResetCurrentUnitBools();
                        //possibleMoveLocation = MainScript.GetMoveableSpacesForUnit(currentUnit, false, true);
                        //possibleMoveAttackLocation = new List<ChessboardPiece>(); ;
                    }
                    else { currentUnit = MainScript.nullUnit; }
                    return templist;
                }


            }

        }
    }
    List<Chesspiece> GetPossibleAttackTargets()
    {
        List<Chesspiece> temp = new List<Chesspiece>();
        List<ChessboardPiece> rangePieces = MainScript.currentBoard.GetAllPiecesWithinDistance(currentUnit.attackRange, false, currentUnit.currentBoardPiece);
        //Debug.Log("there is " + rangePieces.Count + "pieces within range");
        foreach (ChessboardPiece c in rangePieces)
        {
            if (c.hasChessPiece) 
            { 
                if(c.currentChessPiece.owner.playerNumber != currentUnit.owner.playerNumber)
                {
                    if(c.currentChessPiece.owner.playerNumber != MainScript.neutralPlayer.playerNumber)
                    {
                        //Debug.Log("adding a attack target");
                        temp.Add(c.currentChessPiece);
                    }
                    
                }
            }
        }
        return temp;
    }
    bool CanCurrentUnitUseAbility()
    {
        if (currentUnit.stunned) { return false; }
        //hasCheckedForAbilities = true;
        //Debug.Log("checking can current unit use ability");
        bool canPlayAbility = false;
        List<CardReference> refs = spellScript.GetUnitAbilities(currentUnit);
        //Debug.Log(refs.Count + " and " + currentUnit.cardsPlayedThisTurn.Count);
        foreach(CardReference c in currentUnit.cardsPlayedThisTurn)
        {
            if (refs.Contains(c)) { refs.Remove(c); }
        }
        if(refs.Count > 0)
        {
            canPlayAbility = true;
        }
        //Debug.Log("can play ability = " + canPlayAbility);
        return canPlayAbility;
    }
    bool CheckIfChildComponentBlocksParent(AIChildComponent ai)
    {
        //Debug.Log("testing ai child component " + ai.type);
        bool blocksParent = false;
        switch (ai.type)
        {
            case AIChildComponentType.dontPlayAfterThisCard://if the card was just played, amount can stand for how long ago it was played (second third fourth card ago) 
                break;
            case AIChildComponentType.dontPlayIfThisCardWasPlayed://dont play if the card was ever played, might just use the other one for this
                int cardsToCheck = ai.amount;
                for (int i = 0; i < cardsPlayed.Count && i < cardsToCheck; i++)
                {
                    CardReference car = cardsPlayed[cardsPlayed.Count - (1 + i)];
                    if (ai.relevantCards.Contains(car)) { return true; }
                    //if (ai.relevantCards.Contains(car)) { Debug.Log("return true"); return true; }
                }
                break;
            case AIChildComponentType.ifBestTargetIsCurrentUnit:
                /*if(currentUnit != MainScript.nullUnit)
                {
                    //List<ChessboardPiece> targetsPlural = GetBestTargetsForCard(ai.parentComponent.parentCard, ai.parentComponent.initiative, ai.parentComponent);
                    //Debug.Log("best targets for card returns list of " + targetsPlural.Count);
                    ChessboardPiece target = GetBestTargetForCard(ai.parentComponent.parentCard, ai.parentComponent.initiative, ai.parentComponent);
                    if (target != MainScript.nullBoardPiece)
                    {
                        if (currentUnit.currentBoardPiece == target)
                        {
                            return true;
                        }
                        else
                        {
                            int currentUnitInitiative = ai.parentComponent.initiative + GetInitiativeBonusForTarget(ai.parentComponent.parentCard, ai.parentComponent, currentUnit.currentBoardPiece);
                            if (currentUnitInitiative == ai.parentComponent.initiative + GetInitiativeBonusForTarget(ai.parentComponent.parentCard, ai.parentComponent, target))
                            {
                                return true;
                            }
                            else { Debug.Log("not the best target"); return false; }
                        }
                    }
                    else
                    {
                        Debug.Log("get bewst target for card returns nullboasrdpiece");
                    }
                }
                else { Debug.Log("current unit is null"); return false; }*/
                break;
            case AIChildComponentType.dontPlayIfYouHaveThisUnit:
                bool youHaveOneOfTheUnits = false;
                foreach(Chesspiece u in thisPlayer.currentUnits)
                {
                    if (u.alive)
                    {
                        foreach(CardReference r in ai.relevantCards)
                        {
                            if (u.unitRef.cardReference.cardName == r.cardName)
                            {
                                youHaveOneOfTheUnits = true;
                            }
                        }
                        
                    }
                }
                return youHaveOneOfTheUnits;
                break;
            case AIChildComponentType.dontPlayIfYourHealthIsAboveBelow://for units, if the unit's health is above or below amount
                break;
            case AIChildComponentType.ifDamageIsAboveThreshold://for damage based units, its asking if we are ACTUALLY dealing damage, like is the target's health high enough to justify a pyroblast 10 damage or 
                                                                //should we just wait for a frostbolt 3 dmg
                break;
            case AIChildComponentType.ifHealIsAboveThreshold://same as damage but for heals. are we ACTUALLY healing a damaged unit for enough heals? is it one health from max or 10
                break;
            case AIChildComponentType.ifYouHaveThisCardPlayItInstead://checks if another card is available and to play it instead. if you dont want to play this card at all add a dont play after this card 
                                                                        //with the card youre playing instead. otherwise it will play that card first then when its not available it will no longer block the parent
                break;
            case AIChildComponentType.ifOddsOfDrawingRelevantCardAreAboveThreshold://if the discard pile is small enough and the percentage of drawing the relevant card(s) is above amount (out of 100). 
                                                                                   //I.e. 3 cards in discard one is relevant 33%
                break;
            case AIChildComponentType.ifItIsPossibleToShuffleDiscard://if you can shuffle Your DiscardPile through a card or unit ability
                break;
        }
        return blocksParent;
    }
    List<CardAIComponent> GetAIComponentsThatReactToEvent(CardAIComponentType t,bool thatWeCanAfford,bool includeRandom)//get components that react to (have) this cardaicomponenttype. they aren't necessarily activating this just gets them all together to check for that
    {
        
        List<CardAIComponent> ai = new List<CardAIComponent>();
        //Debug.Log("cards in hand are " + thisPlayer.hand.cardsInHand.Count);
        foreach (Card c in thisPlayer.hand.cardsInHand)//probably need to check if we can afford the card too
        {
            if(c.cost <= thisPlayer.resources)
            {
                //bool canAfford = (c.cardReference.cardType != CardType.building);
                bool canAfford = true;
                if (canAfford || !thatWeCanAfford)//summoning units is handled elsewhere this is mainly for spells and possibly later unit abilities
                {
                    foreach (CardAIComponent a in c.aiComponent)
                    {
                        if (a.type == t || (a.type == CardAIComponentType.playRandomly && includeRandom))//this ai component does respond to this type
                        {
                            ai.Add(a);//add it to the list of ai components to check for
                        }
                    }
                }
            }
        }
        //Debug.Log("getting all possible card ai components that are of type " + t + " can we afford set to " + thatWeCanAfford + " and there were " + ai.Count);
        return ai;
    }
    int GetInitiativeBonusForTarget(Card c,ChessboardPiece target,List<AIChildComponent> childrenToCheckTargetFor)
    {
        int bonusDifference = 0;
        for(int i = 0; i < childrenToCheckTargetFor.Count; i++)
        {
            AIChildComponent child = childrenToCheckTargetFor[i];
            switch (child.type)
            {
                case AIChildComponentType.ifDamageIsAboveThreshold:// check if this target has enough health to deal damage damage (i.e. not to deal 6 damage to a 1 health minion)
                    int minimumDAmage = child.power;
                    if (target.hasChessPiece)//if there is no chesspiece there is no damage
                    {
                        if(target.currentChessPiece.currentHealth > minimumDAmage)//there has to be enough remaining health above the aiChildComponents power variable
                        {
                            bonusDifference += child.initiative;//add the initiative to the bonus as it does deal max damage
                        }
                        else
                        {
                            bonusDifference += -5 + target.currentChessPiece.currentHealth;//it is less than the minimum damage reduce initiative by 5 and add whatever health the unit has so it scales depending on how much dmg
                        }
                    }
                    else
                    {
                        bonusDifference -= 10;
                    }
                    break;
                case AIChildComponentType.ifHealIsAboveThreshold:
                    int minHeal = child.power;
                    if (target.hasChessPiece)//if there is no chesspiece there is no damage
                    {
                        int missingHealth = target.currentChessPiece.maxHealth - target.currentChessPiece.currentHealth;
                        if (missingHealth > minHeal)//there has to be enough remaining health above the aiChildComponents power variable
                        {
                            bonusDifference += child.initiative;//add the initiative to the bonus as it does deal max damage
                        }
                        else
                        {
                            bonusDifference += -5 + minHeal;//it is less than the minimum damage reduce initiative by 5 and add whatever health the unit has so it scales depending on how much dmg
                        }
                    }
                    else
                    {
                        bonusDifference -= 10;
                    }
                    break;
                case AIChildComponentType.playClosestToenemyZone:
                    int yPos = target.yPos;
                    int differenceFromTop = 7 - yPos;
                    if(differenceFromTop > child.amount)//the amount in this case indicates how far you want it to go from the top. it wont give you more value than the amount variable
                    {
                        differenceFromTop = child.amount;
                    }
                    bonusDifference += child.initiative * differenceFromTop;
                    break;
                case AIChildComponentType.targetIsNextToUnit:
                    Debug.Log("checking if target is next to a unit");
                    List<ChessboardPiece> targetNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, target);
                    List<ChessboardPiece> occupiedNeighbours = new List<ChessboardPiece>();
                    bool friendlyUnitsOnly = child.relevantBool; //relevant bool in the case of the target is next to unit decides if the neighbour must be friendly
                    foreach(ChessboardPiece piece in targetNeighbours)
                    {
                        if (piece.hasChessPiece)
                        {
                            Chesspiece u = piece.currentChessPiece;
                            if(((c.owner == u.owner )||!friendlyUnitsOnly) && u.unitRef.cardReference.cardType != CardType.building)
                            {
                                occupiedNeighbours.Add(piece);
                            }
                        }
                    }
                    Debug.Log("we have found " + occupiedNeighbours.Count + " neighbours with friendly units only set to " + friendlyUnitsOnly);
                    if (occupiedNeighbours.Count >= child.amount)//the amount of the target is next to unit minimum
                    {
                        
                        List<int> initiativeForSecondaryTargets = new List<int>();
                        if(child.grandChildComponents.Count > 0)//if the child has grandchild components for the secondary target. for primary target would be children of the grandfather
                        {
                            foreach (ChessboardPiece piece in occupiedNeighbours)
                            {
                                int initBonus = GetInitiativeBonusForTarget(c, piece, child.grandChildComponents);
                                initiativeForSecondaryTargets.Add(initBonus);
                            }
                        }
                        int initsAdded = 0;
                        
                        while((initsAdded < child.amount) && initiativeForSecondaryTargets.Count > 0)
                        {
                            int highestInitiative = -420;
                            List<ChessboardPiece> bestSecondaryTargets = new List<ChessboardPiece>();
                            for (int ii = 0; ii < initiativeForSecondaryTargets.Count; ii++)
                            {
                                int currentInitiative = initiativeForSecondaryTargets[ii];
                                if(currentInitiative > highestInitiative)
                                {
                                    highestInitiative = currentInitiative;
                                    bestSecondaryTargets = new List<ChessboardPiece>() { occupiedNeighbours[ii] };
                                }else if(currentInitiative == highestInitiative)
                                {
                                    bestSecondaryTargets.Add(occupiedNeighbours[ii]);
                                }
                            }
                            //if(bestSecondaryTargets.Count > )
                        }
                    }
                    break;
            }
        }
        return bonusDifference;
    }
    List<ChessboardPiece> GetTargetsForCard(Card c)
    {
        List<ChessboardPiece> allTargets = spellScript.GetOnPlayTargets(c, MainScript.nullBoardPiece, currentUnit);
        if(allTargets.Count == 0) { allTargets = BasicSpellScript.NoActualTargetsCheckForUnits(c); }
        return allTargets;
    }
    ChessboardPiece GetBestTargetForCard(Card c, int currentInitiative, CardAIComponent currentAI)
    {
        List<ChessboardPiece> targets = GetTargetsForCard(c);//get all possible targets for it
        //Debug.Log("getting best targets for card " + c.cardName + " there are " + targets.Count + "possible targets");
        if (targets.Contains(MainScript.nullBoardPiece))//if the list contains nullboardpiece it is exclusive
        {
            targets.Remove(MainScript.nullBoardPiece);//remove the nullboard piece
            if (targets.Count > 0)//if there is still a target left then the card can be played, later there needs to be functionality for cards that require 2 targets
            {
                int bestInitiativeSoFar = -420;
                ChessboardPiece bestTargetSoFar = MainScript.nullBoardPiece;
                for (int ii = 0; ii < targets.Count; ii++)
                {
                    ChessboardPiece currentTarget = targets[ii];
                    int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c, currentTarget, currentAI.aiChildren);
                    if (currentTargetInitiative > bestInitiativeSoFar)
                    {
                        bestTargetSoFar = currentTarget;
                        bestInitiativeSoFar = currentTargetInitiative;
                    }
                }
                return bestTargetSoFar;
                //if (bestTargetSoFar != MainScript.nullBoardPiece) { aiComponentInitiative.Add(bestInitiativeSoFar); }

                //finalTarget = targets[(int)Random.Range(0, targets.Count)];
            }
            else
            {
                return MainScript.nullBoardPiece;
            }
        }
        else//it is non exclusive so if there is a taget you can use it otherwise no targets
        {
            if(targets.Count > 0)
            {
                int bestInitiativeSoFar = -420;
                //ChessboardPiece bestTargetSoFar = MainScript.nullBoardPiece;
                List<ChessboardPiece> bestTargetsSoFar = new List<ChessboardPiece>();
                //Debug.Log("target count above zero");
                for(int i = 0; i < targets.Count; i++)
                {
                    ChessboardPiece currentTarget = targets[i];
                    int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c, currentTarget,currentAI.aiChildren);
                    if(currentTargetInitiative > bestInitiativeSoFar)
                    {
                        //bestTargetSoFar = currentTarget;
                        bestInitiativeSoFar = currentInitiative;
                        bestTargetsSoFar = new List<ChessboardPiece>() { currentTarget };
                    }else if (currentTargetInitiative == bestInitiativeSoFar)
                    {
                        bestTargetsSoFar.Add(currentTarget);
                    }
                }
                if(bestTargetsSoFar.Count > 0)
                {
                    int randomInt = (int)Random.RandomRange(0, bestTargetsSoFar.Count);
                    return bestTargetsSoFar[randomInt];
                }
                else
                {
                    return MainScript.nullBoardPiece;
                }
                /*int bestInitiativeSoFar = -420;
                ChessboardPiece bestTargetSoFar = MainScript.nullBoardPiece;
                for (int ii = 0; ii < targets.Count; ii++)
                {
                    ChessboardPiece currentTarget = targets[ii];
                    int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c, currentAI, currentTarget);
                    if (currentTargetInitiative > bestInitiativeSoFar)
                    {
                        bestTargetSoFar = currentTarget;
                        bestInitiativeSoFar = currentTargetInitiative;
                    }
                }
                return bestTargetSoFar;*/
            }
        }
        return MainScript.nullBoardPiece;
    }
    List<ChessboardPiece> GetBestTargetsForCard(Card c, int currentInitiative, CardAIComponent currentAI)
    {

        List<ChessboardPiece> targets = GetTargetsForCard(c);//get all possible targets for it
        List<ChessboardPiece> bestTargets = new List<ChessboardPiece>();
        //Debug.Log("getting best targets for card " + c.cardName + " there are " + targets.Count + "possible targets");
        if (targets.Contains(MainScript.nullBoardPiece))//if the list contains nullboardpiece it is exclusive
        {
            targets.Remove(MainScript.nullBoardPiece);//remove the nullboard piece
            if (targets.Count > 0)//if there is still a target left then the card can be played, later there needs to be functionality for cards that require 2 targets
            {
                int bestInitiativeSoFar = -420;
                for (int ii = 0; ii < targets.Count; ii++)
                {
                    ChessboardPiece currentTarget = targets[ii];
                    int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c,  currentTarget, currentAI.aiChildren);
                    if (currentTargetInitiative > bestInitiativeSoFar)
                    {
                        bestInitiativeSoFar = currentTargetInitiative;
                        bestTargets = new List<ChessboardPiece>() { currentTarget };
                    }else if (currentTargetInitiative == bestInitiativeSoFar)
                    {
                        bestTargets.Add(currentTarget);
                    }
                }
                return bestTargets;
                //if (bestTargetSoFar != MainScript.nullBoardPiece) { aiComponentInitiative.Add(bestInitiativeSoFar); }

                //finalTarget = targets[(int)Random.Range(0, targets.Count)];
            }
            else
            {
                return new List<ChessboardPiece>();
            }
        }
        else//it is non exclusive so if there is a taget you can use it otherwise no targets
        {
            if (targets.Count > 0)
            {
                //Debug.Log("target count above zero");
                int bestInitiativeSoFar = -420;
                //ChessboardPiece bestTargetSoFar = MainScript.nullBoardPiece;
                List<ChessboardPiece> bestTargetsSoFar = new List<ChessboardPiece>();
                foreach (AIChildComponent child in currentAI.aiChildren)//go through that ai's children components
                {
                    if (CheckIfChildComponentBlocksParent(child))//if that cild component activates (i.e. child component is does this spell kill a unit, if it does then this child component activates add its initiative
                    {
                        //Debug.Log("child blocks parent " + child.type);
                        currentInitiative += child.initiative;//child initiative can be positive or negative depending on whether its a good or bad thing (if instead no it doesn't kill the minion, that would be negative, dealing too much excess damage also negaitive)
                    }
                }
                //Debug.Log("target count above zero");
                for (int i = 0; i < targets.Count; i++)
                {
                    ChessboardPiece currentTarget = targets[i];
                    int currentTargetInitiativeBonus = GetInitiativeBonusForTarget(c, currentTarget, currentAI.aiChildren);
                    //int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c, currentAI, currentTarget);
                    if (currentTargetInitiativeBonus > bestInitiativeSoFar)
                    {
                        //bestTargetSoFar = currentTarget;
                        bestInitiativeSoFar = currentTargetInitiativeBonus;
                        bestTargetsSoFar = new List<ChessboardPiece>() { currentTarget };
                    }
                    else if (currentTargetInitiativeBonus == bestInitiativeSoFar)
                    {
                        bestTargetsSoFar.Add(currentTarget);
                    }
                }
                return bestTargetsSoFar;
                /*if (bestTargetsSoFar.Count > 0)
                {
                    int randomInt = (int)Random.RandomRange(0, bestTargetsSoFar.Count);
                    return bestTargetsSoFar[randomInt];
                }
                else
                {
                    return MainScript.nullBoardPiece;
                }*/
                /*int bestInitiativeSoFar = -420;
                ChessboardPiece bestTargetSoFar = MainScript.nullBoardPiece;
                for (int ii = 0; ii < targets.Count; ii++)
                {
                    ChessboardPiece currentTarget = targets[ii];
                    int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c, currentAI, currentTarget);
                    if (currentTargetInitiative > bestInitiativeSoFar)
                    {
                        bestTargetSoFar = currentTarget;
                        bestInitiativeSoFar = currentTargetInitiative;
                    }
                }
                return bestTargetSoFar;*/
            }
        }
        return new List<ChessboardPiece>();
    }
    int GetBestInitiativeForTargetForCard(Card c, int currentInitiative, CardAIComponent currentAI)
    {
        List<ChessboardPiece> targets = GetTargetsForCard(c);//get all possible targets for it
        if (targets.Contains(MainScript.nullBoardPiece))//if the list contains nullboardpiece it is exclusive
        {
            targets.Remove(MainScript.nullBoardPiece);//remove the nullboard piece
            if (targets.Count > 0)//if there is still a target left then the card can be played, later there needs to be functionality for cards that require 2 targets
            {
                int bestInitiativeSoFar = -420;
                ChessboardPiece bestTargetSoFar = MainScript.nullBoardPiece;
                for (int ii = 0; ii < targets.Count; ii++)
                {
                    ChessboardPiece currentTarget = targets[ii];
                    int currentTargetInitiative = currentInitiative + GetInitiativeBonusForTarget(c,  currentTarget,currentAI.aiChildren);
                    if (currentTargetInitiative > bestInitiativeSoFar)
                    {
                        bestTargetSoFar = currentTarget;
                        bestInitiativeSoFar = currentTargetInitiative;
                    }
                }
                return bestInitiativeSoFar;
                //if (bestTargetSoFar != MainScript.nullBoardPiece) { aiComponentInitiative.Add(bestInitiativeSoFar); }

                //finalTarget = targets[(int)Random.Range(0, targets.Count)];
            }
            else
            {
                return -10;
            }
        }
        return -10;
    }

    List<GameEvent> CheckCardsWithAIComponentType(CardAIComponentType t,bool checkRandom)// the type is the one we'd be activating, if it is after we summoned it would be searching for cards with the after summon event type
    {
        
        //List<CardAIComponent> ai = new List<CardAIComponent>();
        List<CardAIComponent> ai = GetAIComponentsThatReactToEvent(t,true,checkRandom);//get every aicomponent that has this cardaicomponent type (time to play the card)
        
        //Debug.Log("checking card aicomponent type " + t + " there are " + ai.Count);
        CardAIComponent eventualComponent = new CardAIComponent(CardAIComponentType.playRandomly);
        List<int> aiComponentInitiative = new List<int>();
        for(int i = 0; i < ai.Count; i++)
        {
            CardAIComponent currentAI = ai[i];
            int currentInitiative = ai[i].initiative;
            Card c = ai[i].parentCard;
            //Debug.Log("going through a card ai children there are " + currentAI.aiChildren.Count + " of them. Card name is " + c.cardName);
            foreach(AIChildComponent child in ai[i].aiChildren)//go through that ai's children components
            {
                //Debug.Log("Checking ai child component type " + child.type);
                if (CheckIfChildComponentBlocksParent(child))//if that cild component activates (i.e. child component is does this spell kill a unit, if it does then this child component activates add its initiative
                {
                    //Debug.Log("child blocks parent " + child.type);
                    currentInitiative += child.initiative;//child initiative can be positive or negative depending on whether its a good or bad thing (if instead no it doesn't kill the minion, that would be negative, dealing too much excess damage also negaitive)
                }
            }
            //Debug.Log("checking for target type of card");
            if(c.targetType != CardTargetType.requiresNoTarget)//if the card does require a target
            {
                //Debug.Log("requires target");
                List<ChessboardPiece> temp = GetBestTargetsForCard(c, currentInitiative, currentAI);
                //Debug.Log("best targets for card retusn " + temp.Count);
                if (temp.Count > 0)
                {
                    int randoInt = (int)Random.Range(0, temp.Count);
                    currentInitiative += GetInitiativeBonusForTarget(c, temp[randoInt], currentAI.aiChildren);
                    aiComponentInitiative.Add(currentInitiative);
                }
                else
                {
                    ai.Remove(currentAI);
                    i--;
                }
                //Debug.Log("best targets count is " + temp.Count + " and does it contain null?" + temp.Contains(MainScript.nullBoardPiece));
                /*int tempInt = GetInitiativeBonusForTarget(c, temp[0],currentAI.aiChildren );
                currentInitiative += tempInt;
                aiComponentInitiative.Add(currentInitiative);*/
            }
            else
            {
                //Debug.Log("adding the card" + c.cardName + " with an initiative of " + currentInitiative);
                aiComponentInitiative.Add(currentInitiative);
                
                
            }
            
        }
        
        int highestInitiative = -420;
        Card highestInitiativeCard = MainScript.nullCard;//go through all ai components and get the highest of them all
        for (int i =0; i < ai.Count;i++)
        {
            CardAIComponent comp = ai[i];
            //Debug.Log("checking initiative for comp number " + i + " of the type " + comp.type + " to play the card " + comp.parentCard.cardName);
            int initiative = aiComponentInitiative[i];
            //Debug.Log("its initiative is " + initiative);
            if(initiative > highestInitiative)
            {
                highestInitiative = initiative;
                highestInitiativeCard = comp.parentCard;
                eventualComponent = comp;
            }
        }
        //Debug.Log("just before high");
        if(highestInitiativeCard != MainScript.nullCard && highestInitiative > 0) 
        {
            //Debug.Log("highest initiative is " + highestInitiative + " on " + highestInitiativeCard.cardName + " with ai component type " + eventualComponent.type);
            //Debug.Log("cards in hand before " + thisPlayer.hand.cardsInHand.Count + " comp parent card is " + highestInitiativeCard.cardName);
            //thisPlayer.hand.cardsInHand.Remove(highestInitiativeCard);
            bool hasFoundCard = false;
            for(int i = 0; i < thisPlayer.hand.cardsInHand.Count && !hasFoundCard; i++)
            {
                Card c = thisPlayer.hand.cardsInHand[i];
                if(c.cardName == highestInitiativeCard.cardName)
                {
                    //thisPlayer.hand.cardsInHand.Remove(c);
                    //Debug.Log("removing from here");
                    hasFoundCard = true;
                }
            }
            //Debug.Log("cards in hand after " + thisPlayer.hand.cardsInHand.Count);
            List<ChessboardPiece> bestTargets = GetBestTargetsForCard(highestInitiativeCard, eventualComponent.initiative, eventualComponent);
            int randoInt = (int)Random.Range(0, bestTargets.Count);
            ChessboardPiece eventualTarget = bestTargets[randoInt];
            List<GameEvent> resultFromPlayingCard = GetPlayCardResult(highestInitiativeCard,eventualTarget,MainScript.nullUnit);
            return resultFromPlayingCard;
            //CheckCardsWithAIComponentType(CardAIComponentType.playBeforeAfterThisCard)
        }
        //List<GameEvent> resultingEvents = new List<GameEvent>();
        
        return new List<GameEvent>();
    }
    List<GameEvent> GetPlayCardResult(Card c,ChessboardPiece target,Chesspiece currentlySelectedUnit)//the name is kind of confusing, we've basically decided to play this card from hand but we're going to check if any other cards are have an aicomponent for when the card is played
    {                                        //most of the time it'll probably just return the "play this card" game event, but if there is a card that we specifically want to play before or after this we add it here in the right order
        List<GameEvent> tempList = new List<GameEvent>();
        List<CardAIComponent> playBeforeAfterCardEvents = GetAIComponentsThatReactToEvent(CardAIComponentType.playBeforeAfterThisCard,true,false);
        //GameEvent playCardEvent  = GameEvent.g
        foreach(CardAIComponent comp in playBeforeAfterCardEvents)
        {
            if (comp.relevantCards.Contains(c.cardReference))//yes this card should be played before or after this event
            {
                if (comp.componentBool)// play after the card
                {
                    Debug.Log("we are in here");
                    //GameEvent.GetPlayCardEvent()
                }
            }
        }
        if (c.targetType != CardTargetType.requiresNoTarget)
        {
            //List<ChessboardPiece>
            return new List<GameEvent>() { GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(c, thisPlayer, target, MainScript.nullBoardPiece, currentlySelectedUnit) };
            //GetBestTargetForCard(c)
        }
        else
        {
            cardsPlayed.Add(c.cardReference);
            return new List<GameEvent>() { GameEvent.GetPlayCardEvent(c, thisPlayer, MainScript.nullBoardPiece) };
        }
        //
        return tempList;
    }
    bool CanCurrentUnitAttack() { return (currentUnitCanAttack && !currentUnitHasAttacked && currentUnit.canAttack && !currentUnit.stunned) ; }
    bool CanCurrentUnitMove() { return (currentUnitCanMove&& !currentUnitHasMoved && currentUnit.canMove && !currentUnit.stunned); }
    List<GameEvent> GetResearchOrders()
    {
        List<GameEvent> researchEvents = new List<GameEvent>();
        List<CardReference> allResearchableCards = new List<CardReference>();
        List<Chesspiece> researchingUnits = new List<Chesspiece>();
        foreach (Chesspiece u in thisPlayer.currentUnits)
        {
            if (u.unitRef.cardReference.cardType == CardType.building)
            {
                //Debug.Log("found a building");
                researchingUnits.Add(u);
                List<CardReference> tump = spellScript.GetBuildingResearchableCards(u);
                //Debug.Log("we found a building it is " + u.unitRef.unitName + " and it has a storefront of " + tump.Count);
                foreach (CardReference r in tump)
                {
                    if (r.cost <= thisPlayer.resources)
                    {
                        allResearchableCards.Add(r);
                    }
                    //Debug.Log(r.cardName);
                }
            }
        }
        if(allResearchableCards.Count > 0)
        {
            //Debug.Log("we have researchable cards");
            
            
            int randomInt = (int)Random.Range(0, allResearchableCards.Count);
            CardReference cRef = allResearchableCards[randomInt];
            for(int i = 0; i <researchingUnits.Count;i++)
            {
                Chesspiece u = researchingUnits[i];
                List<CardReference> tump = spellScript.GetBuildingResearchableCards(u);
                if (!tump.Contains(cRef)) { researchingUnits.Remove(u); i--; }
            }
            Card tempCard = Card.GetCardWithNoTransform(cRef, thisPlayer);
            tempCard.isResearchableCard = true;
            GameEvent res = new GameEvent(new List<Card>() { tempCard }, researchingUnits[0]);
            //researchEvents.Add(GameEvent.GetPlayCardEvent(tempCard, thisPlayer, researchingUnits));
            researchEvents.Add(res);
        }
        return researchEvents;
    }
    List<GameEvent> GetBuildOrdersThisTurn()
    {
        //Debug.Log("getting build orders. build type is " + build.type);
        hasBuiltDeckThisTurn = true;
        List<GameEvent> temp = new List<GameEvent>();
        List<CardReference> allBuyableCards = new List<CardReference>();
        List<Chesspiece> allUnitsWithBuyableCards = new List<Chesspiece>();
        foreach(Chesspiece u in thisPlayer.currentUnits)
        {
            if(u.unitRef.cardReference.cardType == CardType.building)
            {
                //Debug.Log("found a building");

                List<CardReference> tump = spellScript.GetBuildingStorefront(u.unitRef,thisPlayer);
                //Debug.Log("we found a building it is " + u.unitRef.unitName + " and it has a storefront of " + tump.Count);
                foreach(CardReference r in tump)
                {
                    if (r.cost <= thisPlayer.resources)
                    {
                        allBuyableCards.Add(r);
                        allUnitsWithBuyableCards.Add(u);
                    }
                    //Debug.Log(r.cardName);
                }
            }
        }
        //Debug.Log("there are " + allBuyableCards.Count + " cards available to put into discard");
        if(allBuyableCards.Count > 0)
        {
            switch (build.type)
            {
                case BuildOrderType.buildDeckRandomly:
                    //Debug.Log("doing build deck randomly");
                    int unitsAdded = 0;
                    int spellsAdded = 0;
                    while(unitsAdded < build.unitsPerTurn)
                    {
                        unitsAdded++;
                        if (!build.poolSpellsWithUnits)//not pooling spells and minions i.e. just getting units
                        {
                            List<CardReference> unitList = new List<CardReference>();
                            foreach(CardReference r in allBuyableCards) { if (r.cardType == CardType.minion) { unitList.Add(r); } }//make a list of just the units
                            if(unitList.Count > 0)
                            {
                                int randomInt = (int)Random.Range(0, unitList.Count);
                                Card tempCard = Card.GetCardWithNoTransform(unitList[randomInt], thisPlayer);//make a card of it
                                //Debug.Log("build order chose card " + tempCard.cardName);
                                for(int i = 0; i < allUnitsWithBuyableCards.Count;i++)
                                {
                                    Chesspiece u = allUnitsWithBuyableCards[i];
                                    if (spellScript.GetBuildingStorefront(u.unitRef,thisPlayer).Contains(tempCard.cardReference))
                                    {
                                        //this card has the card we are purchasing, if it didn't we'd remove it
                                    }
                                    else { allUnitsWithBuyableCards.Remove(u);i--; }
                                    
                                }
                                tempCard.isPurchaseCard = true;
                                //GameEvent.ge
                                GameEvent otherPurchas = GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(tempCard, thisPlayer, MainScript.nullBoardPiece, MainScript.nullBoardPiece, allUnitsWithBuyableCards[0]);
                                //GameEvent buyCardEvent = GameEvent.GetPlayCardEvent(tempCard, thisPlayer, MainScript.nullBoardPiece);//add it to the list
                                //buyCardEvent.relevantUnits = allUnitsWithBuyableCards;
                                temp.Add(otherPurchas);
                            }
                            else { unitsAdded = build.unitsPerTurn; }//there are no units to add so just get out of the while loop 
                            
                        }
                    }
                    /*int randomInt = (int)Random.Range(0, allBuyableCards.Count);
                    Card tempCard = Card.GetCardWithNoTransform(allBuyableCards[randomInt], thisPlayer);
                    tempCard.isPurchaseCard = true;
                    GameEvent buyCardEvent = GameEvent.GetPlayCardEvent(tempCard, thisPlayer, MainScript.nullBoardPiece);
                    temp.Add(buyCardEvent);*/
                    break;
            }
        }
        return temp;
    }
    public List<GameEvent> GetNextAction2()
     {
        //Debug.Log("getting next action");
        
        if (!hasSummonedMinions)
        {
            /*for (int i = 0; i < thisPlayer.hand.cardsInHand.Count; i++)
            {
                Debug.Log(" card in enemy hadn number " + i + " is named " + thisPlayer.hand.cardsInHand[i].cardName);
            }*/
            List<GameEvent> playCardsEvent = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeAnything,true);
            if(playCardsEvent.Count > 0)
            {
                return playCardsEvent;
            }
            else if (!hasBuiltDeckThisTurn)
            {
                hasBuiltDeckThisTurn = true;//just for now
                return GetBuildOrdersThisTurn();
            }
            else if(!hasResearchedThisTurn){
                hasResearchedThisTurn = true;
                return GetResearchOrders();
            }
            else if(!hasSummonedMinions)
            {
                hasSummonedMinions = true;
                return GetSummonEvents();
            }
            else { return GetNextAction2(); }
            
        }
        else
        {
            if (currentUnit == MainScript.nullUnit)
            {
                //there is no unit use left
                return new List<GameEvent>() { GameEvent.GetEndTurnEvent() };
            }
            else
            {
                if ((CanCurrentUnitMove() || CanCurrentUnitAttack() || CanCurrentUnitUseAbility() )&& currentUnit.alive)
                {
                    if (CanCurrentUnitAttack())
                    {
                        //Debug.Log("can current unit attack");
                        if (!hasCheckedForTargets)
                        {
                            possibleAttackTargets = GetPossibleAttackTargets();
                            //Debug.Log("checking for targets found " + possibleAttackTargets.Count);
                        }
                        if (possibleAttackTargets.Count > 0)// if there is a target pick a random one from the list, has attacked = true;
                        {
                            //Debug.Log("we are checking for cards to play before attacking");
                            List<GameEvent> beforeAttackEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeAttackingWithUnit,false);
                            //Debug.Log("we get " + beforeAttackEvents.Count + " events");
                            int randomInt = (int)Random.Range(0, possibleAttackTargets.Count);
                            Chesspiece randomUnit = possibleAttackTargets[randomInt];
                            currentUnitHasAttacked = true;
                            beforeAttackEvents.Add(GameEvent.GetAttackEvent(currentUnit, randomUnit));
                            return beforeAttackEvents;
                        }
                        else//if there are no targets can atack = false;
                        {
                            //Debug.Log("no targets found");
                            currentUnitCanAttack = false;
                            return GetNextAction2();
                        }
                    }
                    else if (CanCurrentUnitMove())
                    {
                        //Debug.Log("can current unit move");
                        if (!hasCheckeforMovement)
                        {
                            SetupMoveLocations();
                            //Debug.Log("there is " + possibleMoveLocation.Count + " spaces to move to and " + possibleMoveAttackLocation.Count + " spaces to moveattack to");
                        }
                        
                        if (possibleMoveAttackLocation.Count > 0)
                        {
                            //Debug.Log("unit has more than 0 possible move attack locations");
                            int randomInt = (int)Random.Range(0, possibleMoveAttackLocation.Count);
                            ChessboardPiece randomPiece = possibleMoveAttackLocation[randomInt];
                            List<ChessboardPiece> randomPieceNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, randomPiece);
                            if (randomPieceNeighbours.Contains(currentUnit.currentBoardPiece))
                            {
                                currentUnitHasAttacked = true;
                                currentUnitHasMoved = true;

                                //***** List<GameEvent> moveAttackEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeMoveAttacking);
                                List<GameEvent> allEvents = new List<GameEvent>() {  };
                                GameEvent temp = GameEvent.GetMoveAttackEvent(currentUnit, randomPiece.currentChessPiece);
                                temp.relevantUnits = new List<Chesspiece>() { currentUnit, randomPiece.currentChessPiece };
                                allEvents.Add(temp);
                                List<GameEvent> afterMoveAttackEvents = CheckCardsWithAIComponentType(CardAIComponentType.playAfterMoveAttacking,false);
                                //allEvents.AddRange(afterMoveAttackEvents);
                                return allEvents;
                            }
                            else
                            {
                                List<ChessboardPiece> possibleMoveAttackPositions = new List<ChessboardPiece>();
                                foreach (ChessboardPiece c in possibleMoveLocation)
                                {
                                    if (randomPieceNeighbours.Contains(c))
                                    {
                                        possibleMoveAttackPositions.Add(c);
                                    }
                                }
                                if (possibleMoveAttackPositions.Count > 0)
                                {
                                    float lowestDistance = 420f;
                                    ChessboardPiece closestPiece = MainScript.nullBoardPiece;
                                    foreach (ChessboardPiece c in possibleMoveAttackPositions)
                                    {
                                        float currentDistance = Vector3.Distance(c.transform.position, currentUnit.currentBoardPiece.transform.position);

                                        if (currentDistance < lowestDistance)
                                        {
                                            lowestDistance = currentDistance;
                                            closestPiece = c;
                                        }
                                    }
                                    //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeMoving);
                                    List<GameEvent> allEvents = new List<GameEvent>();
                                    allEvents.Add(GameEvent.GetMoveEvent(currentUnit, closestPiece) );
                                    //allEvents.Add(GameEvent.GetMoveAttackEvent(currentUnit, randomPiece.currentChessPiece));
                                    GameEvent temp = GameEvent.GetMoveAttackFromBoardPieces(closestPiece, randomPiece, thisPlayer);
                                    temp.relevantUnits = new List<Chesspiece>() { currentUnit, randomPiece.currentChessPiece };
                                    allEvents.Add(temp);
                                    List<GameEvent> afterMoveAttackEvents = CheckCardsWithAIComponentType(CardAIComponentType.playAfterMoveAttacking,false);
                                    //allEvents.AddRange(afterMoveAttackEvents);
                                    return allEvents;
                                    
                                }
                                else
                                {
                                    possibleMoveAttackLocation.Remove(randomPiece);
                                    if (possibleMoveLocation.Contains(randomPiece)) { possibleMoveLocation.Remove(randomPiece); }
                                    return GetNextAction2();
                                }
                            }
                        }
                        else
                        {
                            if (possibleMoveLocation.Count > 0)
                            {
                                currentUnitHasMoved = true;
                                int randomInt = (int)Random.Range(0, possibleMoveLocation.Count);
                                if (!currentUnitHasAttacked) { currentUnitCanAttack = true; hasCheckedForTargets = false; }
                                int lowestDistance = 420;
                                ChessboardPiece lowestPiece = MainScript.nullBoardPiece;
                                foreach (ChessboardPiece c in possibleMoveLocation) 
                                {
                                    if (c.yPos < lowestDistance)
                                    {
                                        lowestPiece = c;
                                        lowestDistance = c.yPos;
                                    }
                                }
                                if (lowestPiece != MainScript.nullBoardPiece)
                                {
                                    //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeMoving);
                                    return new List<GameEvent>() { GameEvent.GetMoveEvent(currentUnit, lowestPiece) };
                                }
                                else
                                {
                                    currentUnitCanMove = false;
                                    return GetNextAction2();
                                }
                                //return new List<GameEvent>() { GameEvent.GetMoveEvent(currentUnit, possibleMoveLocation[randomInt]) };
                            }
                            else
                            {
                                currentUnitCanMove = false;
                                return GetNextAction2();
                            }
                        }
                    }
                    else if (CanCurrentUnitUseAbility() && !hasCheckedForAbilities)
                    {
                        hasCheckedForAbilities = true;
                        //Debug.Log(" we acn use current unit ability");
                        //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeUsingUnits);
                        List<CardReference> playableCards = spellScript.GetUnitAbilities(currentUnit);
                        foreach (CardReference r in currentUnit.cardsPlayedThisTurn) { if (playableCards.Contains(r)) { playableCards.Remove(r); } }
                        CardReference randomReference = playableCards[(int)Random.Range(0f, playableCards.Count)];
                        currentUnit.cardsPlayedThisTurn.Add(randomReference);
                        Card c = Card.GetCardWithNoTransform(randomReference, currentUnit.owner);
                        //Debug.Log("checking to use ability " + c.cardName + " of type " + c.cardType + " with target type " + c.targetType);
                        //c.hasNoTransform = true;
                        ChessboardPiece randomTarget = MainScript.nullBoardPiece;
                        ChessboardPiece secondRandomTarget = MainScript.nullBoardPiece;
                        switch (c.targetType)
                        {
                            case CardTargetType.requiresNoTarget:
                                return new List<GameEvent>() { GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(c, thisPlayer, MainScript.nullBoardPiece, MainScript.nullBoardPiece, currentUnit) };
                            case CardTargetType.emptyBoardPiece:
                            case CardTargetType.targetsBoardPiece:
                            case CardTargetType.targetsUnit:
                                List<ChessboardPiece> targets =  spellScript.GetOnPlayTargets(c, currentUnit.currentBoardPiece, currentUnit);    
                                if (targets.Contains(MainScript.nullBoardPiece)){ targets.Remove(MainScript.nullBoardPiece); }
                                if(targets.Count > 0)
                                {
                                    //Debug.Log("tasrgets count is " + targets.Count + " and first one x pos is " + targets[0].xPos);
                                    randomTarget = targets[(int)Random.Range(0f, targets.Count)];
                                    if (c.requiresSecondTarget)
                                    {
                                        targets.Remove(randomTarget);
                                        secondRandomTarget = targets[(int)Random.Range(0f, targets.Count)];
                                    }
                                    //targets = new List<ChessboardPiece>();
                                }
                                break;
                        }
                        if(randomTarget!= MainScript.nullBoardPiece)
                        {
                            //Debug.Log(randomTarget.xPos);
                            //GameEvent temp = GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(c, thisPlayer, randomTarget, MainScript.nullBoardPiece, currentUnit);
                            //Debug.Log(currentUnit.unitRef.unitName);
                            if (c.requiresSecondTarget)
                            {
                                if(secondRandomTarget != MainScript.nullBoardPiece)
                                {
                                    GameEvent temp = GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(c, thisPlayer, randomTarget, secondRandomTarget, currentUnit);
                                    //Debug.Log(temp.theTarget.Count);
                                    return new List<GameEvent>() { temp };
                                }
                                else
                                {
                                    return GetNextAction2();
                                }
                                
                            }
                            else
                            {
                                GameEvent temp = GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(c, thisPlayer, randomTarget, currentUnit.currentBoardPiece, currentUnit);
                                //Debug.Log(temp.theTarget.Count);
                                return new List<GameEvent>() { temp };
                            }
                            
                        }
                        else
                        {
                            GetNewCurrentUnit();
                            //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeUsingUnits);
                            hasCheckedForAbilities = true;
                            return GetNextAction2();
                        }
                        
                        //targets[((int)Random.Range(0f, targets.Count))];
                        //new GameEvent(c,currentUnit.owner,)
                        //GameEvent event = new GameEvent(char,currentUnit.owner,)
                        //queue.Add(new GameEvent(currentlySelectedCard, currentlySelectedCard.owner, currentlySelectedTarget, secondarySelectedTarget));//add teh play card event to the queue
                        //GameEvent.GetPlayCardEvent()
                        //this will cause a recursive loop, the red line is just messing with me
                    }
                    else 
                    {   
                        GetNewCurrentUnit();
                        //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeUsingUnits);
                        //hasCheckedForAbilities = false;
                        return GetNextAction2();
                    }
                }
                else
                {
                    GetNewCurrentUnit();
                    //List<GameEvent> moveBeforeAttackingEvents = CheckCardsWithAIComponentType(CardAIComponentType.playBeforeUsingUnits);
                    return GetNextAction2();
                }
            }
        }
    }
    public List<GameEvent> GetUnitAction()
    {
        Debug.Log("getting unit action");
        switch (currentUnit.unitRef.unitName)
        {

        }
        if (!currentUnitHasAttacked && !hasCheckedForTargets)
        {
            Debug.Log("can attack");
            List<Chesspiece> possibleAttackTargets = new List<Chesspiece>();
            List<ChessboardPiece> spotsWithinRange = MainScript.currentBoard.GetAllPiecesWithinDistance(currentUnit.attackRange, false, currentUnit.currentBoardPiece);
            foreach(ChessboardPiece c in spotsWithinRange)
            {
                if (c.hasChessPiece)
                {
                    if( c.currentChessPiece.owner != currentUnit.owner && c.currentChessPiece.owner != MainScript.neutralPlayer)
                    {
                        currentUnitHasAttackTarget = true;
                        possibleAttackTargets.Add(c.currentChessPiece);
                    }
                }
            }
            hasCheckedForTargets = true;
            if(possibleAttackTargets.Count > 0)
            {
                int randomInt = (int)Random.Range(0, possibleAttackTargets.Count);
                Chesspiece randomUnit = possibleAttackTargets[randomInt];
                GameEvent attackEvent = GameEvent.GetAttackEvent(currentUnit, randomUnit);
                //currentUnit.canAttack = false;
                currentUnitHasAttacked = true;
                return new List<GameEvent>() { attackEvent };
            }
            else {  return GetUnitAction();  }
        }
        else if(!currentUnitHasMoved && !hasCheckeforMovement)
        {
            SetupMoveLocations();
            hasCheckeforMovement = true;
            if(possibleMoveAttackLocation.Count > 0)
            {
                int randomInt = (int)Random.Range(0, possibleMoveAttackLocation.Count);
                ChessboardPiece randomPiece = possibleMoveAttackLocation[randomInt];
                List<ChessboardPiece> randomPieceNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, randomPiece);
                if (randomPieceNeighbours.Contains(currentUnit.currentBoardPiece)) 
                {
                    currentUnitHasMoved = true;
                    currentUnitHasAttacked = true;
                    return new List<GameEvent>() { GameEvent.GetMoveAttackEvent(currentUnit, randomPiece.currentChessPiece) };
                } else
                {
                    float lowestDistance = Mathf.Infinity;
                    ChessboardPiece closestPiece = MainScript.nullBoardPiece;
                    foreach (ChessboardPiece c in randomPieceNeighbours) 
                    {
                        if (possibleMoveLocation.Contains(c))
                        {
                            float currentDistance = (Vector3.Distance(currentUnit.currentBoardPiece.transform.position, c.transform.position));
                            if (currentDistance < lowestDistance)
                            {
                                lowestDistance = currentDistance;
                                closestPiece = c;
                            }
                        }
                    }
                    if(closestPiece != MainScript.nullBoardPiece) 
                    {
                        currentUnitHasAttacked = true;
                        hasCheckeforMovement = false;
                        unitTargetForMoveAttack = randomPiece.currentChessPiece;
                        return new List<GameEvent>() { GameEvent.GetMoveEvent(currentUnit, closestPiece) };
                    } else
                    {
                        //Debug.Log("possible move attack > 0 and not a neighbour piece but closest piece is nullboardpiece");
                        possibleMoveAttackLocation.Remove(randomPiece);
                        return GetUnitAction();
                        //MainScript.secondarySelectedTarget = currentUnit.currentBoardPiece;
                    }
                }
                //MainScript.secondarySelectedTarget = 
                //return new List<GameEvent>() { GameEvent.GetPlayCardEvent(MainScript.currentlySelectedCard, thisPlayer, randomPiece) };
                //return new List<GameEvent>() { GameEvent.GetMoveAttackEvent(currentUnit, possibleMoveAttackLocation[randomInt].currentChessPiece) };
            }
            else
            {
                //Debug.Log("no possible move attack locations finding lowest possible spot");
                
                int lowestYValue = 420;
                List<ChessboardPiece> lowestPossiblePieces = new List<ChessboardPiece>();
                foreach(ChessboardPiece c in possibleMoveLocation)
                {
                    if(c.yPos < lowestYValue)
                    {
                        lowestYValue = c.yPos;
                        lowestPossiblePieces = new List<ChessboardPiece>() { c };
                    }else if(c.yPos == lowestYValue)
                    {
                        lowestPossiblePieces.Add(c);
                    }
                }
                if(lowestPossiblePieces.Count > 0)
                {
                    int randomInt = (int)Random.Range(0, lowestPossiblePieces.Count);
                    Debug.Log("getting a lowest possible piece randomly");
                    currentUnit.canMove = false;
                    currentUnitHasMoved = true;
                    //if (!currentUnitHasAttacked) { hasCheckedForTargets = false; currentUnitCanAttack = true; }
                    return new List<GameEvent>() { GameEvent.GetMoveEvent(currentUnit, lowestPossiblePieces[randomInt]) };
                }
                else
                {
                    Debug.Log("no lowest possible, I think that might be weird");
                    //currentUnitCanMove = false;
                    currentUnitHasMoved = true;
                    //currentUnit.canMove = false;
                    int randomInt = (int)Random.Range(0, possibleMoveLocation.Count);
                    return new List<GameEvent>() { GameEvent.GetMoveEvent(currentUnit, possibleMoveLocation[randomInt]) };
                }
            }

        }
        else if(unitTargetForMoveAttack != MainScript.nullUnit)
        {
            GameEvent temp = GameEvent.GetMoveAttackEvent(currentUnit, unitTargetForMoveAttack);
            unitTargetForMoveAttack = MainScript.nullUnit;
            return new List<GameEvent>() { temp };
        }
        else
        {
            Debug.Log("are we lost in here");
            piecesToMove.Remove(currentUnit);
            if (piecesToMove.Count > 0)
            {
                int randomInt = (int)Random.Range(0, piecesToMove.Count);
                
                currentUnit = piecesToMove[randomInt];
                
                ResetCurrentUnitBools();
            }
            else { currentUnit = MainScript.nullUnit; }
            return new List<GameEvent>(); }
        
    }
    
     void GetNewCurrentUnit() 
    {
        hasCheckedForAbilities = false;
        piecesToMove.Remove(currentUnit);
        if (piecesToMove.Count > 0)
        {
            int randomInt = (int)Random.Range(0, piecesToMove.Count);

            currentUnit = piecesToMove[randomInt];

            ResetCurrentUnitBools();
        }
        else { currentUnit = MainScript.nullUnit; }
    }
    public List<GameEvent> GetSummonEvents2()
    {
        List<GameEvent> summonEvents = new List<GameEvent>();
        List<Card> possibleCardsToPlay = new List<Card>();
        List<ChessboardPiece> summonTargets = new List<ChessboardPiece>();
        possibleCardsToPlay.AddRange(thisPlayer.hand.cardsInHand);
        List<ChessboardPiece> randomBoardPieceList = new List<ChessboardPiece>();

        return summonEvents;
    }
    public List<GameEvent> GetSummonEvents()
    {
        //Debug.Log("running get summon events");
        List<GameEvent> summonEvents = new List<GameEvent>();
        List<Card> possibleCardsToPlay = new List<Card>();
        List<ChessboardPiece> summonTargets = new List<ChessboardPiece>();
        possibleCardsToPlay.AddRange(thisPlayer.hand.cardsInHand);
        List<ChessboardPiece> randomBoardPieceList = new List<ChessboardPiece>();
        randomBoardPieceList.AddRange(thisPlayer.ownedBoardPieces);
        for(int i = 0;i < randomBoardPieceList.Count;i++)
        {
            ChessboardPiece c = randomBoardPieceList[i];
            if (c.hasChessPiece) { randomBoardPieceList.RemoveAt(i);i--; }
        }
        int cardsPlayed = 0;
        while (cardsPlayed < 1 && possibleCardsToPlay.Count > 0 && randomBoardPieceList.Count > 0)
        {
            int randomInt = (int)Random.Range(0, possibleCardsToPlay.Count);
            Card randomCard = possibleCardsToPlay[randomInt];
            if(randomCard.cardReference.cost <= thisPlayer.resources)
            {
                switch (randomCard.cardType)
                {
                    case CardType.minion:
                        //Debug.Log("in minion");
                        if (randomBoardPieceList.Count > 0)
                        {
                            //Debug.Log("randomboardpiece list greater than 0");
                            int ranInt = (int)Random.Range(0, randomBoardPieceList.Count);
                            ChessboardPiece randomPiece = randomBoardPieceList[ranInt];
                            if (!summonTargets.Contains(randomPiece))
                            {
                                //Debug.Log("summon targets does not contain randompiece");
                                if (!randomCard.cardReference.reference.applyOnPlayEffect)
                                {
                                    //Debug.Log("we are adding to summone events minion named " + randomCard.cardName + " and we have " + thisPlayer.resources);
                                    thisPlayer.resources -= randomCard.cardReference.cost;
                                    //Debug.Log("now we have " + thisPlayer.resources);
                                    thisPlayer.bank.SetInt(thisPlayer.resources);
                                    summonEvents.Add(new GameEvent(randomCard, thisPlayer, randomPiece));
                                    summonTargets.Add(randomBoardPieceList[ranInt]);
                                    cardsPlayed++;
                                }//else { Debug.Log("card has an apply on play effect"); }
                                else
                                {
                                    //if (randomCard.requiresSecondTarget) { Debug.Log(" card from summon events requires second target. name is " + randomCard.cardName); }

                                    if (randomCard.requiresSecondTarget)
                                    {
                                        List<ChessboardPiece> temp = randomBoardPieceList;
                                        //List<ChessboardPiece> temp = spellScript.GetOnPlayTargets(randomCard, MainScript.nullBoardPiece, MainScript.nullUnit);
                                        if (temp.Contains(MainScript.nullBoardPiece)) { temp.Remove(MainScript.nullBoardPiece); }
                                        //Debug.Log("possible original targets : " + temp.Count);
                                        List<ChessboardPiece> possibleTargets = new List<ChessboardPiece>();
                                        foreach (ChessboardPiece p in temp)
                                        {
                                            //p.ChangeColor(Color.cyan);
                                            List<ChessboardPiece> possibleSecondaryTargets = spellScript.GetOnPlayTargets(randomCard, p, MainScript.nullUnit);
                                            if (possibleSecondaryTargets.Contains(MainScript.nullBoardPiece)) { possibleSecondaryTargets.Remove(MainScript.nullBoardPiece); }
                                            if (possibleSecondaryTargets.Count > 0)
                                            {
                                                possibleTargets.Add(p);
                                                /*Debug.Log("possible secondary targets = " + possibleSecondaryTargets.Count);
                                                if (possibleSecondaryTargets.Contains(MainScript.nullBoardPiece)) { possibleSecondaryTargets.Remove(MainScript.nullBoardPiece); }
                                                foreach(ChessboardPiece c in possibleSecondaryTargets)
                                                {
                                                    c.ChangeColor(Color.red);
                                                }*/
                                            }
                                        }
                                        if (possibleTargets.Count > 0)
                                        {
                                            int randomTargetInt = (int)Random.Range(0f, possibleTargets.Count);
                                            ChessboardPiece finalTarget = possibleTargets[randomTargetInt];
                                            List<ChessboardPiece> possibleSecondaryTargets = spellScript.GetOnPlayTargets(randomCard, finalTarget, MainScript.nullUnit);
                                            if (possibleSecondaryTargets.Contains(MainScript.nullBoardPiece)) { possibleSecondaryTargets.Remove(MainScript.nullBoardPiece); }
                                            int randomSecondTargetInt = (int)Random.Range(0f, possibleSecondaryTargets.Count);
                                            if (possibleSecondaryTargets.Count > 0)
                                            {
                                                //Debug.Log("finally reaches here");
                                                if (randomBoardPieceList.Contains(finalTarget)) { randomBoardPieceList.Remove(finalTarget); }
                                                GameEvent tempa = new GameEvent(randomCard, thisPlayer, finalTarget, possibleSecondaryTargets[randomSecondTargetInt]);
                                                //Debug.Log(tempa.theActor.Count);
                                                thisPlayer.resources -= randomCard.cardReference.cost;
                                                thisPlayer.bank.SetInt(thisPlayer.resources);
                                                summonEvents.Add(tempa);
                                            }
                                        }
                                        //List<ChessboardPiece> bestTargets = GetBestTargetsForCard(randomCard, 0, randomCard.aiComponent[0]);
                                        //Debug.Log("best targets contains " + bestTargets.Count);
                                    }
                                    //if (temp.Contains(MainScript.nullBoardPiece)) { temp.Remove(MainScript.nullBoardPiece); }
                                    //Debug.Log("ha this many targets " + temp.Count);
                                }
                            }//else { Debug.Log("summon targets contains randompiece"); }
                        }//else { Debug.Log("there are no more in randomboardpiecelist"); }
                        break;
                    case CardType.attackUnit://for now just remove the random cards from played units
                    case CardType.moveUnit:
                        break;
                }
                
            }
            possibleCardsToPlay.Remove(randomCard);
        }
            //Debug.Log("checking random card " + randomCard.cardName + " it is of card type " + randomCard.cardType);
            
        return summonEvents;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum CardAIComponentType { 
    none,playRandomly,playBeforeAnything,playBeforeSummoning,playAfterSummoning,playBeforeUsingUnits,playAfterUsingUnits,playAfterEverything, 
    playBeforeAfterThisCard,playBeforeAttackingWithUnit,playBeforeMoveAttacking,playBeforeMoving,playAfterMoveAttacking
}//specifically where in the ai turn the card should be played
public class CardAIComponent  //an AI component to dictate when and where an ai should roughly play a card. 
{
    public Card parentCard = MainScript.nullCard;
    public ChessboardPiece primaryTarget = MainScript.nullBoardPiece;
    public ChessboardPiece secondrayTarget = MainScript.nullBoardPiece;
    public List<CardReference> relevantCards;
    public CardAIComponentType type;//dictates when the card might hypothetically be played
    public bool componentBool = false;
    public int initiative = 0; //the "initiative" deciding which goes first. the higher it is the more it'll be prioritized over other cards you might play at the same time as it, i.e. if you have two one cost spells and one draws a card, and one draws two, the card which draws two cards would probably be higher in initiative
    public int amount = 0; //relevant amount, ie if you have 3 or more units would use the amount value, play after using units would use it to dictate how many units have to have been used, did you use one or two or three?
    public int power = 0; //relevant power of hypothetical card (healing or damage) i.e. do we heal 5 points to a unit or only 2 before its health maxes out
    public int utility = 0;//general all purpose variable for any utility purpose
    public List<AIChildComponent> aiChildren; //secondary checks for when we're playing these cards, like do we have more than ten health or do we outnumber enemy units
    //public List<>
    public CardAIComponent(CardAIComponentType theType)
    {
        type = theType;
        aiChildren = new List<AIChildComponent>();
    }
    public static CardAIComponent GetPlayBeforeAfterCardCompo(bool playBeforeNotAfter, List<CardReference> cardsToPlayBeforeAfter)
    {
        CardAIComponent compo = new CardAIComponent(CardAIComponentType.playBeforeAfterThisCard);
        compo.componentBool = playBeforeNotAfter;
        compo.relevantCards = cardsToPlayBeforeAfter;
        compo.aiChildren = new List<AIChildComponent>();
        return compo;
    }
    public CardAIComponent(CardAIComponentType theType,int amt,int pow,int util,int initiativ)
    {
        aiChildren = new List<AIChildComponent>();
        type = theType;
        amount = amt;
        power = pow;
        utility = util;
        initiative = initiativ;
    }
}
public enum AIChildComponentType { 
    dontPlayIfThisCardWasPlayed,dontPlayAfterThisCard,dontPlayIfYouHaveThisUnit,dontPlayIfYourHealthIsAboveBelow,ifYouHaveThisCardPlayItInstead,ifDamageIsAboveThreshold,ifHealIsAboveThreshold,
    ifOddsOfDrawingRelevantCardAreAboveThreshold,ifItIsPossibleToShuffleDiscard,killsUnit,isFriendlyUnit,isEnemyUnit,targetUnitCanAttackAMinion,ifBestTargetIsCurrentUnit,playNextToUnit,targetIsNextToUnit,
    targetDoesNotBlockMoveAttack,playClosestToenemyZone
}
public class AIChildComponent
{
    public List<CardReference> relevantCards;
    public CardAIComponent parentComponent;
    public List<AIChildComponent> grandChildComponents;
    public AIChildComponentType type;
    public bool relevantBool = false;
    public int initiative = 0;// the amount the child component affects the initiative
    public int amount = 0;
    public int power = 0;
    public int utility = 0;
    public AIChildComponent(AIChildComponentType t, List<CardReference> relCards,bool relBool,int init,int amou,int pow, int util)
    {
        relevantCards = relCards;
        relevantBool = relBool;
        initiative = init;
        power = pow;
        amount = amou;
        utility = util;
        type = t;
        grandChildComponents = new List<AIChildComponent>();
    }
    public void SetupParent(CardAIComponent p)
    {
        parentComponent = p;
    }
    public static AIChildComponent GetDontPlayBeforeAfter(List<CardReference> relCards, bool afterNotBefore,int numberOfCardsToLookBackOn, int initBonus)
    {
        return new AIChildComponent(AIChildComponentType.dontPlayAfterThisCard,  relCards, afterNotBefore, initBonus, numberOfCardsToLookBackOn, 0, 0);
    }
}
public class AIBuildOrder
{
    public BuildOrderType type;
    public int amount = 0;
    public int utility = 0;
    public int degree = 0;
    public int spellsPerTurn = 1;
    public int unitsPerTurn = 1;
    public bool randomizeUnitsBought = true;//simply pick a unit at random rather than in an order
    public bool randomizeSpellsBought = true;//simply pick a spell at random rather than in a specific order
    public bool poolSpellsWithUnits = false;//if you want to randomize whether there is a spell added or a unit added every time
    public List<CardReference> unitsToBuy = new List<CardReference>();
    public List<CardReference> spellsToBuy = new List<CardReference>();
    public AIBuildOrder(BuildOrderType t)
    {
        type = t;
        unitsToBuy = new List<CardReference>();
        spellsToBuy = new List<CardReference>();
    }
    public void SetAmt(int a) { amount = a; }
    public void SetUti(int a) { utility = a; }
    public void SetDeg(int a) { degree = a; }
    public void SpellsPerTurn(int s) { spellsPerTurn = s; }
    public void UnitsPerTurn(int u) { unitsPerTurn = u; }
}
public enum BuildOrderType { buildDeckRandomly}
public class AIResearchOrder
{

}
public enum researchOrderType { researchCardsRandomly}