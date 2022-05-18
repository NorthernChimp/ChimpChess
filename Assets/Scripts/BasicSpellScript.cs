using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject artilleryTargetPrefab;
    public GameObject mortarSlugPrefab;
    public GameObject basicGrenade;
    public BuildingReference barracksRef;
    public BuildingReference engineeringBayRef;
    public BuildingReference hospitalRef;
    public BarracksScript barracksScript;
    public HospitalScript hospitalScript;
    public EngineeringBayScript engineeringBayScript;
    public EnemyAI enemy;
    void Start()
    {
        
    }
    public List<GameEvent> PlaySpell(Card c, List<ChessboardPiece> targets, List<Chesspiece> units,GameEvent e)
    {
        //Debug.Log("playing spell from basic spell script " + c.cardName);
        switch(c.cardReference.buildingRefParent.buildingRefName)
        {
            case "barracks":
            return barracksScript.PlaySpell(c, targets, units);
            case "hospital":
            return hospitalScript.PlaySpell(c, targets, units,e);
            case "engineering":
                return engineeringBayScript.PlaySpell(c, targets, units, e);
            case "enemy":
                return (c.owner.aiComponent.PlaySpell(c, targets, units, e));
        }
        return new List<GameEvent>();
    }
    public List<CardReference> GetUnitAbilities(Chesspiece u)//for base unit abilities, modifiers may remove after the fact or add to them
    {
        //Debug.Log("getting unit abilities for " + u.unitRef.unitName);
        switch (u.unitRef.buildingRefParent.buildingRefName)
        {
            case "barracks":
                return barracksScript.GetUnitAbilities(u.unitRef);
            case "hospital":
                return hospitalScript.GetUnitAbilities(u.unitRef,u.owner);
            case "engineering":
                return engineeringBayScript.GetUnitAbilities(u.unitRef);
            case "enemy":
                return u.owner.aiComponent.GetUnitAbilities(u.unitRef);
                
        }
        return new List<CardReference>();
    }
    public List<EventListener> GetEventListenerForUnit(UnitReference u)
    {
        switch (u.buildingRefParent.buildingRefName)
        {
            case "engineering":
                return engineeringBayScript.GetEventListenerForUnit(u);
        }
        return new List<EventListener>();
    }
    public List<CardReference> GetBuildingResearchableCards(Chesspiece u)
    {

        switch (u.unitRef.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.GetBuildingResearchableCards();
            case "barracks":
                return barracksScript.GetBuildingResearchableCards();
            case "engineering":
                return engineeringBayScript.GetBuildingResearchableCards();
            //case "enemy":
                //return u.owner.aiComponent.get
        }
        return new List<CardReference>();
    }
    public List<CardReference> GetBuildingStorefront(UnitReference u, Player ownerPlayer)
    {
        switch (u.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.GetStorefront();
            case "barracks":
                return barracksScript.GetStorefront();
            case "engineering":
                return engineeringBayScript.GetStorefront();
        }
        return new List<CardReference>();
    }
    public List<GameEvent> ResearchCard(Card c, Player owner,Chesspiece unitResearching)
    {
        if(unitResearching != MainScript.nullUnit && unitResearching.alive){
            unitResearching.currentBoardPiece.CreateScienceSymbol();
        }
        switch (c.cardReference.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.ResearchCard(c,owner);
            case "barracks":
                return barracksScript.ResearchCard(c,owner);
            case "engineering":
                return engineeringBayScript.ResearchCard(c, owner);
        }
        /*switch (c.cardName)
        {
            case "Stimpak":
                LibraryScript.unitLibrary[1].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.addToAbilities, new List<CardReference>() { c.cardReference }));
                LibraryScript.buildingLibrary[2].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.removeFromResearchableCards, new List<CardReference>() { LibraryScript.cardlessSpellLibrary[1] }));
                return new List<GameEvent>();
        }*/
        return new List<GameEvent>();
    }
    public GameObject GetProjectilePrefab(CardReference c)
    {
        switch (c.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.GetProjectilePrefab(c);
        }
        return basicGrenade;
    }
    public List<GameEvent> GetGrenadeResult(CardReference c, ChessboardPiece target)
    {
        switch (c.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.GetGrenadeEvents(c, target);
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> DeployWalkingMine(ChessboardPiece target,Player owns)
    {
        return new List<GameEvent>() { GameEvent.GetSummonEvent(LibraryScript.cardlessUnitLibrary[2],target,owns)};
    }
    public List<GameEvent> ResearchCard2(Card c, Player owner)
    {
        switch (c.cardName)
        {
            case "Stimpak":
                LibraryScript.unitLibrary[1].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.addToAbilities, new List<CardReference>() { c.cardReference },owner));
                LibraryScript.buildingLibrary[2].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.removeFromResearchableCards, new List<CardReference>() { LibraryScript.cardlessSpellLibrary[1] },owner));
                return new List<GameEvent>();
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> ThrowAMoon(Chesspiece unit)
    {
        int unitRow = unit.currentBoardPiece.yPos;
        List<ChessboardPiece> allPossibleTargets = new List<ChessboardPiece>();
        for(int x = 0; x < MainScript.currentBoard.width;x++)
        {
            if((unitRow + 3) < MainScript.currentBoard.height) { allPossibleTargets.Add(MainScript.currentBoard.chessboardPieces[x, unitRow + 3]); }
            if((unitRow + 4) < MainScript.currentBoard.height) { allPossibleTargets.Add(MainScript.currentBoard.chessboardPieces[x, unitRow + 4]); }
            if((unitRow + 5) < MainScript.currentBoard.height) { allPossibleTargets.Add(MainScript.currentBoard.chessboardPieces[x, unitRow + 5]); }
            
        }
        List<ChessboardPiece> randomlyTargetedSpaces = new List<ChessboardPiece>();
        EventListener tempListen = new EventListener(unit, new List<GameEventType>(){ GameEventType.endTurn });
        tempListen.destroyOnActivation = true;
        unit.listeners.Add(tempListen);
        MainScript.allEventListeners.Add(tempListen);
        int piecesAdded = 0;
        while(piecesAdded < 10 && allPossibleTargets.Count != 0)
        {
            int randomInt = (int)Random.Range(0, allPossibleTargets.Count);
            randomlyTargetedSpaces.Add(allPossibleTargets[randomInt]);
            allPossibleTargets.RemoveAt(randomInt);
            piecesAdded++;
        }
        tempListen.targets = randomlyTargetedSpaces;
        foreach (ChessboardPiece c in randomlyTargetedSpaces)
        { 
            c.ChangeTransparentRenderColor(Color.red);
            if (!c.hasArtillery) { c.SetupArtillery(); }
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> Stimpak(Chesspiece unitStimming)
    {
        unitStimming.canAttack = true;
        return new List<GameEvent>();
    }
    public List<GameEvent> Behold(Player p,Card c)
    {
        Color color = Color.white;
        if(c.owner.theType == PlayerType.localHuman) 
        {
            color = Color.green;
            MainScript.leftEmptyCard.AppearAsThisCard(LibraryScript.cardlessSpellLibrary[0],p);
            MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.discarding,color);
            MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,p);
            MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.playCard,color);
        }
        else 
        {
            color = Color.red;
            MainScript.rightEmptyCard.AppearAsThisCard(LibraryScript.cardlessSpellLibrary[0],p);
            MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.discarding, color);
            MainScript.leftEmptyCard.AppearAsThisCard(c.cardReference,p);
            MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.playCard, color);
        }
        p.discardPile.cardsInDiscard.Add(new Card(LibraryScript.cardlessSpellLibrary[0], p));
        return new List<GameEvent>() { new GameEvent(p,1,GameEventType.drawCard)};
    }
    public List<GameEvent> MyStuff(Player p)
    {
        if(p.currentUnits.Count > 0)
        {
            int randomInt = (int)Random.Range(0, p.currentUnits.Count);
            Chesspiece unit = p.currentUnits[randomInt];
            unit.attack += 3;
            unit.maxHealth += 3;
            unit.currentHealth += 3;
            if(p.theType == PlayerType.localHuman) { MainScript.leftEmptyCard.AppearAsThisUnit(unit); } else { MainScript.rightEmptyCard.AppearAsThisUnit(unit);}
            
        }
        return new List<GameEvent>() { new GameEvent(p,2,GameEventType.drawCard)};
    }
    public List<GameEvent> YounglingSlayer(Chesspiece target, Card c)
    {
        target.attack += c.attack;
        target.maxHealth += c.attack;
        target.currentHealth += c.attack;
        if(c.owner.theType == PlayerType.localHuman)
        {
            MainScript.leftEmptyCard.AppearAsThisUnit(target);
            MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
        }
        else
        {
            MainScript.rightEmptyCard.AppearAsThisUnit(target);
            MainScript.leftEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
        }
        
        return new List<GameEvent>();
    }
    public List<GameEvent> DogeDealsDamage(Chesspiece target, Card c)
    {
        //target.currentHealth -= 3;
        MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
        //MainScript.leftEmptyCard.AppearAsThisUnit(target);
        //MainScript.leftEmptyCard.SetupHealthBuff(-3);
        return new List<GameEvent>() {new GameEvent(new List<Chesspiece>() { target },c.attack)};
    }
    public List<GameEvent> ItsRainingBarrels(Card c,ChimpChessBoard b)
    {
        List<GameEvent> events = new List<GameEvent>();
        List<ChessboardPiece> tempList = b.GetRandomBoardPiece(false, true, 3);
        events.Add(new GameEvent(c.reference, tempList,MainScript.neutralPlayer));
        return (events);
    }
    public List<GameEvent> Fireball(Card c, ChimpChessBoard b, Chesspiece unit)
    {
        List<GameEvent> events = new List<GameEvent>();
        MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
        MainScript.leftEmptyCard.AppearAsThisUnit(unit);
        //Transform t = Instantiate((GameObject)Resources.Load("spellPrefabs/FireballPrefab"), target.transform.position, Quaternion.identity).transform;
        //t.SendMessage("SetupFireball");
        //MainScript.spellTransforms.Add(t);
        events.Add(new GameEvent(new List<Chesspiece>() { unit }, c.attack));
        return (events);
    }
    public List<GameEvent> ApplyDeathrattle2(Chesspiece deadUnit)
    {
        switch (deadUnit.unitRef.cardReference.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.ApplyDeathrattle(deadUnit);
            case "barracks":
                return barracksScript.ApplyDeathrattle(deadUnit);
            case "engineering":
                return engineeringBayScript.ApplyDeathrattle(deadUnit);
        }
        return new List<GameEvent>();
    }
        public List<GameEvent> ApplyDeathrattle(Chesspiece deadUnit)
    {
        switch (deadUnit.unitRef.buildingRefParent.buildingRefName)
        {
            case "engineering":
                return engineeringBayScript.ApplyDeathrattle(deadUnit);
            case "hospital":
                return hospitalScript.ApplyDeathrattle(deadUnit);
            case "barracks":
                return barracksScript.ApplyDeathrattle(deadUnit);
        }
        List<GameEvent> tempList = new List<GameEvent>();
        switch (deadUnit.unitRef.cardReference.cardName)
        {
            case "Cheems":
                return new List<GameEvent>() { new GameEvent(deadUnit.owner, 1, GameEventType.drawCard) };
            case "Barrel":
                return engineeringBayScript.ApplyDeathrattle(deadUnit);
                List<ChessboardPiece> barrelNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true,true,true,deadUnit.currentBoardPiece);
                List<Chesspiece> unitsToDamage = new List<Chesspiece>();
                foreach(ChessboardPiece c in barrelNeighbours)
                {
                    if (c.hasChessPiece)
                    {
                        unitsToDamage.Add(c.currentChessPiece);
                    }
                }
                if(unitsToDamage.Count > 0) { return new List<GameEvent>() { new GameEvent(unitsToDamage, 10) }; } else { return new List<GameEvent>(); };
                
        }
        return tempList;
    }
    public List<GameEvent> GetChooseCardEvent(Card c,CardReference refer, ChessboardPiece target, ChessboardPiece secondaryTarget,Chesspiece selectedUnit,Player p)
    {
        switch (refer.buildingRefParent.buildingRefName)
        {
            case "barracks":
                return barracksScript.GetChooseCardEvent(c, refer,target, secondaryTarget, selectedUnit,p);
            case "engineering":
                return engineeringBayScript.GetChooseCardEvent(c, refer, target, secondaryTarget, selectedUnit, p);
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> ApplyOnPlayEffect(Card c, ChessboardPiece target, ChessboardPiece secondaryTarget)
    {
        //if(c.cardReference.buildingRefParent == LibraryScript.buildingRefLibrary[2]) { Debug.Log("its the engineering bay"); };
        switch (c.cardReference.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.ApplyOnPlayEffect(c, target, secondaryTarget);
            case "barracks":
                return barracksScript.ApplyOnPlayEffect(c, target, secondaryTarget);
            case "engineering":
                return engineeringBayScript.ApplyOnPlayEffect(c, target, secondaryTarget);
        }
        return new List<GameEvent>();
    }
        public List<GameEvent> ApplyOnPlayEffect2(Card c, ChessboardPiece target,ChessboardPiece secondaryTarget)
    {
        switch (c.cardName)
        {
            case "Doge":
                if (c.owner.theType == PlayerType.localHuman) { MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect,Color.green); } else { MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect, Color.red); }
                //if (c.owner.theType == PlayerType.localHuman) { MainScript.rightEmptyCard.notifier.SetupColor(Color.green); } else { MainScript.rightEmptyCard.notifier.SetupColor(Color.red); }
                return new List<GameEvent>() { new GameEvent(c.owner, 1, GameEventType.drawCard) };
            case "Uncle Murphy":
                List<ChessboardPiece> temp = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, target);
                if(!temp.Contains(secondaryTarget) || !secondaryTarget.hasChessPiece) { return new List<GameEvent>(); } //if the target of uncle murphys battlecry isn't a neighbour or the selection has no chesspiece return an empty list
                else//otherwise return the two buffs for that unit
                {
                    secondaryTarget.currentChessPiece.attack += 3;
                    secondaryTarget.currentChessPiece.maxHealth += 3;
                    secondaryTarget.currentChessPiece.currentHealth += 3;
                    if(c.owner.theType == PlayerType.localHuman)
                    {
                        MainScript.leftEmptyCard.AppearAsThisUnit(secondaryTarget.currentChessPiece);
                        MainScript.leftEmptyCard.SetupAttackBuff(3);
                        MainScript.leftEmptyCard.SetupHealthBuff(3);
                        MainScript.rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect,Color.green);
                    }
                    else
                    {
                        MainScript.rightEmptyCard.AppearAsThisUnit(secondaryTarget.currentChessPiece);
                        MainScript.rightEmptyCard.SetupAttackBuff(3);
                        MainScript.rightEmptyCard.SetupHealthBuff(3);
                        MainScript.leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.onPlayEffect,Color.green);
                    }
                    if(c.owner.theType == PlayerType.localHuman) { MainScript.rightEmptyCard.notifier.SetupColor(Color.green); } else { MainScript.rightEmptyCard.notifier.SetupColor(Color.red); }
                    MainScript.rightEmptyCard.AppearAsThisCard(c.cardReference,c.owner);
                    return new List<GameEvent>() {GameEvent.GetEmptyGameEvent(GameEventType.onPlayEffect) };
                }
                break;
        }
        return new List<GameEvent>();
    }
    public static List<ChessboardPiece> NoActualTargetsCheckForUnits(Card c)//this is for when the card returns an empty list, thus the target type is all inclusive. if a card was exclusive, it would have supplied a list of targets
    {
        //Debug.Log("no actual targets for card " + c.cardName);
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        if(c.targetType == CardTargetType.targetsUnit || c.targetType == CardTargetType.targetsFriendlyUnit || c.targetType == CardTargetType.targetsEnemyUnit)
        {
            tempList = MainScript.currentBoard.GetAllBoardPieces(true, false);
            MainScript.possibleAttackLocations = new List<ChessboardPiece>();
            if(c.targetType == CardTargetType.targetsEnemyUnit)
            {
                for(int i = 0; i < tempList.Count; i++)
                {
                    ChessboardPiece p = tempList[i];
                    if(p.currentChessPiece.owner == c.owner)
                    {
                        tempList.Remove(p);
                        MainScript.possibleAttackLocations.Add(p);
                        i--;
                    }
                }
            }else if (c.targetType == CardTargetType.targetsFriendlyUnit)
            {
                //Debug.Log("no actual target friendly unit check");
                for (int i = 0; i < tempList.Count; i++)
                {
                    ChessboardPiece p = tempList[i];
                    if (p.currentChessPiece.owner != c.owner)
                    {
                        tempList.Remove(p);
                        MainScript.possibleAttackLocations.Add(p);
                        i--;
                    }
                }
            }
            //MainScript.poss
        }else if(c.targetType == CardTargetType.targetsBoardPiece || c.targetType == CardTargetType.emptyBoardPiece || c.targetType == CardTargetType.playUnit)
        {
            bool getEmpty = false;
            bool getFull = true;
            if(c.targetType == CardTargetType.emptyBoardPiece || c.targetType == CardTargetType.playUnit) { getEmpty = true; getFull = false; }
            tempList = MainScript.currentBoard.GetAllBoardPieces(getFull, getEmpty);
        }
        return tempList;
    }
    public List<ChessboardPiece> GetOnPlayTargets(Card c, ChessboardPiece target,Chesspiece selectedUnit)
    {
        if(c == MainScript.attackUnitCard)
        {
            MainScript.possibleAttackLocations = new List<ChessboardPiece>();
            MainScript.possibleOnPlayTargets = new List<ChessboardPiece>() { MainScript.nullBoardPiece};
            foreach(ChessboardPiece p in MainScript.currentBoard.GetAllPiecesWithinDistance(selectedUnit.attackRange, false, selectedUnit.currentBoardPiece))
            {
                if (p.hasChessPiece)
                {
                    if(p.currentChessPiece.owner != selectedUnit.owner)
                    {
                        MainScript.possibleOnPlayTargets.Add(p);
                    }
                    else
                    {
                        MainScript.possibleAttackLocations.Add(p);
                    }
                }
                else
                {
                    MainScript.possibleAttackLocations.Add(p);
                }
            }
            return MainScript.possibleOnPlayTargets;
        }else if (c == MainScript.moveUnitCard)
        {
            return MainScript.GetMoveableSpacesForUnit(selectedUnit, false, true);//this defaults to not being able to move over units that'll have to change when you make units taht do
        }
        //Debug.Log("Getting on play targets for " + c.cardName + " from " + c.cardReference.buildingRefParent.buildingRefName);//this will run an error with cards that dont have a building ref parent
        if(((c.cardType == CardType.minion ) && target == MainScript.nullBoardPiece)|| c.cardType == CardType.building)//minions that require you to select a target are told appart from whether there is a target
        {                                                                                                              //with no target you are selecting where to place the minion, with a target, you've selected it
            List<ChessboardPiece> minionPlayableLocations = new List<ChessboardPiece>() { MainScript.nullBoardPiece};  //then it wont trigger this if clause for getting places to play your units
            foreach(ChessboardPiece p in c.owner.ownedBoardPieces)
            {
                if (!p.hasChessPiece)
                {
                    minionPlayableLocations.Add(p);
                }
            }
            return minionPlayableLocations;
        }
        switch (c.cardReference.buildingRefParent.buildingRefName)
        {
            case "barracks":
                return barracksScript.GetOnPlayTargets(c, target, selectedUnit);
            case "hospital":
                return hospitalScript.GetOnPlayTargets(c, target, selectedUnit);
            case "engineering":
                return engineeringBayScript.GetOnPlayTargets(c, target, selectedUnit);
            case "enemy": 
                return enemy.GetOnPlayTargets(c, target, selectedUnit);
        }
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
     
        return tempList;
    }
    public bool DoesExecutingThisEventActivateListenever(List<EventListener> l, List<GameEvent> currentEvents)//for when an event has been confirmed for executing, rather than checking for an interuption. if you interupt attacking and a unit has a listener that responds to attacking the listener will activate twice, so this function is necessary for when an action is literally about to happen
    {
        //Debug.Log("checking does exuting this event activate listener " + l.listenerName);
        switch (l[0].unitOwner.unitRef.cardReference.buildingRefParent.buildingRefName)
        {
            case "hospital":
                return hospitalScript.DoesExecutingThisEventActivateListenever( l, currentEvents);
            case "barracks":
                return barracksScript.DoesExecutingThisEventActivateListenever(l, currentEvents);
            case "engineering":
                return engineeringBayScript.DoesExecutingThisEventActivateListenever(l, currentEvents);
                
        }
        return CheckNonBuildingEventListenerForExecution(l[0], currentEvents);//pretty sure non building event listeners wont be put together into a list, examples are die at the end of turn, basic things that aren't building specific
    }
    public bool CheckNonBuildingEventListenerForExecution(EventListener l, List<GameEvent> theQueue)
    {
        //Debug.Log("checking from a non building created listenere named " + l.listenerName);
        switch (l.listenerName)
        {
            case "dieAtTheEndOfTurn":
                Debug.Log("checking for dies at end of turn");
                theQueue.Insert(0, GameEvent.GetDeathEvent(new List<Chesspiece>() { l.unitOwner }));
                return true;
        }
        return false;
    }
        
    public bool DoesThisListenerActivate(EventListener l,List<GameEvent> theQueue)
    {
        //GameEvent triggerEvent = theQueue[0];
        //Debug.Log(l.unitOwner.unitRef.unitName);
        switch (l.unitOwner.unitRef.buildingRefParent.buildingRefName)
        {
            case "barracks":
                return barracksScript.DoesThisListenerActivate(l, theQueue);
            case "hospital":
                return hospitalScript.DoesThisListenerActivate(l, theQueue);
            case "engineering":
                return engineeringBayScript.DoesThisListenerActivate(l, theQueue);
            //case "none"://for non building specific ones like die at the end of turn
                //return 
            
                
        }
        return DoesThisNonBuildingCreatedListenerActivate(l,theQueue);
    }
    bool DoesThisNonBuildingCreatedListenerActivate(EventListener l,List<GameEvent> theQueue)//this is for neutral event listeners like draw cards at the beginning oft he turn
    {
        //Debug.Log("checking from a non building created listenere named " + l.listenerName);
        switch (l.listenerName)
        {
            case "DrawCardsTurnBegin":
                if (l.playerOwner == MainScript.currentGame.currentPlayer)
                {
                    theQueue.Add(new GameEvent(l.playerOwner, 5, GameEventType.drawCard));
                }
                return false;
            case "DiscardCardsTurnEnd":
                if (l.playerOwner == MainScript.currentGame.currentPlayer)
                {
                    //theQueue.Insert(0, new GameEvent(l.playerOwner, l.playerOwner.hand.cardsInHand.Count, GameEventType.discardCard));
                    l.disabledUntilEndOfQueue = true;
                    return true;
                }
                break;
            case "dieAtTheEndOfTurn":
                theQueue.Insert(0, GameEvent.GetDeathEvent(new List<Chesspiece>() { l.unitOwner }));
                MainScript.allEventListeners.Remove(l);
                return true;
                
                //theQueue.Add(GameEvent.get)
        }
        return false;
    }
    public List<GameEvent> GetListenerEvents(EventListener l)
    {
        Debug.Log("gettings listenerEVents for " + l.listenerName);
        switch (l.unitOwner.unitRef.buildingRefParent.buildingRefName)
        {
            case "barracks":
                return barracksScript.GetListenerEvents(l);
            case "hospital":
                return hospitalScript.GetListenerEvents(l);
            case "engineering":
                return engineeringBayScript.GetListenerEvents(l);
        }
        switch (l.listenerName)
        {
            case "Walter":
                if(l.unitOwner.owner.theType == PlayerType.localHuman) { MainScript.rightEmptyCard.SetupHealthBuff(2); } else { MainScript.leftEmptyCard.SetupHealthBuff(2); }
                break;
            case "Perro"://perro changes the queue and thus does not add an effect to the queue
                break;
            case "Caesar":
                return new List<GameEvent>() { new GameEvent(l.unitOwner.owner, 1, GameEventType.drawCard) };
            case "Icarus Mortar Installation":
                return IcarusMortarEndTurnActivation(l);
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> IcarusMortarEndTurnActivation(EventListener l)
    {
        l.disabledUntilEndOfQueue = true;
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        List<Chesspiece> unitsTargeted = new List<Chesspiece>();
        foreach (ChessboardPiece c in l.targets)
        {
            Transform t = Instantiate(mortarSlugPrefab, c.transform.position + (Vector3.up * MainScript.distanceBetweenUnitAndBoardPiece * 15f), Quaternion.identity).transform;
            t.SendMessage("SetupProjectile", (Vector3.up * MainScript.distanceBetweenUnitAndBoardPiece * -15f));
            MainScript.projectileTransforms.Add(t);
            c.ChangeTransparentRenderColor(Color.red);
            if (c.hasChessPiece) { tempList.Add(c); unitsTargeted.Add(c.currentChessPiece); }
        }
        return new List<GameEvent>() { new GameEvent(unitsTargeted, 3) };
    }
    public void ActivateEnemyAI2(Chesspiece unit, List<GameEvent> queue)
    {
        ChessboardPiece pos = unit.currentBoardPiece;
        List<ChessboardPiece> possibleAttackLocations = MainScript.currentBoard.GetAllPiecesWithinDistance(unit.attackRange, false, unit.currentBoardPiece);
        List<Chesspiece> enemiesWihtinAttackRange = new List<Chesspiece>();
        List<ChessboardPiece> possibleMoveLocations = MainScript.GetMoveableSpacesForUnit(unit, false, true);
        List<ChessboardPiece> possibleAttackMoveLocations = new List<ChessboardPiece>();
        List<ChessboardPiece> possibleMoveBeforeAttackLocations = new List<ChessboardPiece>();
        Chesspiece randomUnitWithinRange = MainScript.nullUnit;
        bool hasAttacked = false;
        foreach (ChessboardPiece c in possibleAttackLocations)//for all possible attack locations
        {
            if (c.hasChessPiece)//if it has a unit
            {
                if(c.currentChessPiece.owner != unit.owner)// andit has a different owner
                {
                    enemiesWihtinAttackRange.Add(c.currentChessPiece);//ad it to enemies within range
                }
            }
        }
        foreach(ChessboardPiece c in possibleMoveLocations)//for all possible move locations
        {
            if (c.hasChessPiece)//if it has a unit
            {
                if (c.currentChessPiece.owner != unit.owner)//and the unit has not the same owner
                {
                    List<ChessboardPiece> currentPieceNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, c);//get all the neighbour pieces to that enemy unit
                    ChessboardPiece currentClosestMoveAttackLocation = MainScript.nullBoardPiece;//make a variable for the closest move attack location set it to a null unit
                    if (!currentPieceNeighbours.Contains(unit.currentBoardPiece))//if the unit IS NOT already a neighbour, i.e. doesn't have to move and can immediately moveattack find out if we can move towards it
                    {
                        float currentLowestDistance = Mathf.Infinity;//set the lowest distance found to something so high any distance becomes teh new lowest
                        foreach (ChessboardPiece temp in currentPieceNeighbours)//go through all of this enemies neighbours
                        {
                            if (possibleMoveLocations.Contains(temp) && !temp.hasChessPiece )//if the neighbour is a space we can move to that doest not have a unit on it and can be moved to
                            {
                                float currentDistance = Vector3.Distance(unit.currentBoardPiece.transform.position, temp.transform.position);//get the distance between the units current space and this possible move space
                                if (currentDistance < currentLowestDistance)//if this is the closest space to your current location, if no space came before it has to be the lowest
                                {
                                    currentLowestDistance = currentDistance;//set the new lowest distance and
                                    currentClosestMoveAttackLocation = temp;// make the closest move attack location to that location
                                }
                            }
                        }
                        if(currentClosestMoveAttackLocation != MainScript.nullBoardPiece)//if the closest move attack location has been set it means we found a moveable space to initiate a move attack
                        {
                            possibleAttackMoveLocations.Add(c);//add it to possible move attack locations
                            possibleMoveBeforeAttackLocations.Add(currentClosestMoveAttackLocation);
                        }
                    }
                    else//we are already next to this unit and can moveAttack immediately
                    {
                        currentClosestMoveAttackLocation = unit.currentBoardPiece;
                        possibleAttackMoveLocations.Add(c);//add it to possible move attack locations
                        possibleMoveBeforeAttackLocations.Add(currentClosestMoveAttackLocation);//
                    }
                }
            }
            else//if it does not have a unit
            {
                //possibleMoveLocations.Add(c);//ad it to move locations
            }
        }
        List<ChessboardPiece> idealAttackPieces = new List<ChessboardPiece>();
        List<ChessboardPiece> idealMoveBeforeAttackPieces = new List<ChessboardPiece>();

        for(int i = 0; i < possibleAttackMoveLocations.Count;i++)
        {
            ChessboardPiece c = possibleAttackMoveLocations[i];
            if (enemiesWihtinAttackRange.Contains(c.currentChessPiece)) { idealAttackPieces.Add(c); idealMoveBeforeAttackPieces.Add(possibleMoveBeforeAttackLocations[i]); }//if we can attack a unit and move attack in the same turn add it to templist
        }
        if(idealAttackPieces.Count > 0)//if there are any we found that we can attack and move attack...
        {
            Debug.Log("we have ideal");
            int randomInt = (int)Random.Range(0, idealAttackPieces.Count);
            ChessboardPiece randomPieceToAttack = idealAttackPieces[randomInt];//get a random unit to attack
            ChessboardPiece moveBeforeAttackPiece = possibleMoveBeforeAttackLocations[randomInt];
            queue.Add(GameEvent.GetAttackEvent(unit, randomPieceToAttack.currentChessPiece));
            if (possibleMoveBeforeAttackLocations[randomInt] != unit.currentBoardPiece) { queue.Add(GameEvent.GetMoveEvent(unit, moveBeforeAttackPiece)); }
            queue.Add(GameEvent.GetMoveAttackFromBoardPieces(moveBeforeAttackPiece, randomPieceToAttack,unit.owner));
        }
        else
        {
            //Debug.Log("we dont have ideal");
            if (enemiesWihtinAttackRange.Count > 0)
            {
                Debug.Log("adding attack");
                int randomInt = (int)Random.Range(0, enemiesWihtinAttackRange.Count);
                randomUnitWithinRange = enemiesWihtinAttackRange[randomInt];
                queue.Add(GameEvent.GetAttackEvent(unit, randomUnitWithinRange));
                hasAttacked = true;
            }
            if(possibleAttackMoveLocations.Count > 0)
            {
                Debug.Log("adding move");
                int randomInt = (int)Random.Range(0, possibleAttackMoveLocations.Count);
                ChessboardPiece randomAttackFromPiece = possibleMoveBeforeAttackLocations[randomInt];
                queue.Add(GameEvent.GetMoveEvent(unit, randomAttackFromPiece));
                Debug.Log("adding move attack");
                queue.Add(GameEvent.GetMoveAttackEvent(unit, possibleAttackMoveLocations[randomInt].currentChessPiece));
            }
            else
            {
                if(possibleMoveLocations.Count > 0)
                {
                    ChessboardPiece furthestDownPiece = MainScript.nullBoardPiece;
                    int lowestYValue = 420;
                    foreach(ChessboardPiece c in possibleMoveLocations)
                    {
                        if(c.yPos < lowestYValue)
                        {
                            furthestDownPiece = c;
                            lowestYValue = c.yPos;
                        }
                    }
                    if(furthestDownPiece != MainScript.nullBoardPiece)
                    {
                        queue.Add(GameEvent.GetMoveEvent(unit, furthestDownPiece));
                    }
                }
            }
        }
        

    }
    public void ActivateEnemyAI(Chesspiece unit, List<GameEvent> queue)
    {
        //Debug.Log("activating AI");
        //pretend this is on an enemy Unit which has specific AI Instructions, it would be activated from ExecuteAITurn
        ChessboardPiece pos = unit.currentBoardPiece;
        List<ChessboardPiece> spacesForward = MainScript.currentBoard.GetAllPiecesFromOriginInDirection(pos, new Vector2(0f, -1f), false, true, 4,unit.owner);//get the spaces in front of this unit
        bool willMoveForward = false;
        bool hasAttacked = false;
        if (spacesForward.Count > 0)//if it can move forward at all
        {
            //Debug.Log("we can move forward");
            ChessboardPiece furthestPiece = spacesForward[spacesForward.Count - 1];//get the furthest forward piece
            if (furthestPiece.hasChessPiece)//if the furthest piece has a chespiece
            {
                if (furthestPiece.currentChessPiece.owner != unit.owner)//and that piece is an enemy
                {
                    hasAttacked = true;//we'll just attack the guy right in front of us
                    willMoveForward = true;//we will be moving forward and dont need to move sideways
                    //Debug.Log("adding the attack Event before moving forward furthest piece");
                    queue.Add(GameEvent.GetAttackEvent(unit, furthestPiece.currentChessPiece));//add an attack event for that minion
                    if (spacesForward.Count > 1)//if that minion is not right next to us...
                    {
                        //Debug.Log("spacesforward count bigger than 1");
                        queue.Add(GameEvent.GetMoveEvent(unit, spacesForward[spacesForward.Count - 2]));//move this unit towards him
                        queue.Add(new GameEvent(new List<ChessboardPiece>() { spacesForward[spacesForward.Count - 2] }, new List<ChessboardPiece>() { furthestPiece }, unit.owner));
                        
                        //queue.Add(GameEvent.GetMoveAttackEvent(unit, furthestPiece.currentChessPiece));//and add a moveattack afterwards
                    }
                    else
                    {
                        //Debug.Log("spacesforward count bigger just one 1");
                        queue.Add(GameEvent.GetMoveAttackEvent(unit, furthestPiece.currentChessPiece));//and add a moveattack afterwards
                    }
                    
                }
            }
            else//if there is no unit on the furthest space
            {
                if (spacesForward.Count > 2) //if we can move ahead at least 3 spaces
                {
                    willMoveForward = true;
                    queue.Add(GameEvent.GetMoveEvent(unit, furthestPiece));//move this unit to the furthest space
                }
            }
        }
        if (!willMoveForward)//if we didn't move forward then consider how we might move left or right
        {
            Debug.Log("not moving forward");
            List<ChessboardPiece> rightSideSpace = MainScript.currentBoard.GetAllPiecesFromOriginInDirection(pos, Vector2.right, false, true, 2,unit.owner);
            List<ChessboardPiece> leftSideSpace = MainScript.currentBoard.GetAllPiecesFromOriginInDirection(pos, Vector2.left, false, true, 2,unit.owner);
            List<ChessboardPiece> allSpaces = new List<ChessboardPiece>() { };
            allSpaces.AddRange(leftSideSpace); allSpaces.AddRange(rightSideSpace);
            List<ChessboardPiece> spacesWithLongestDistanceDownward = new List<ChessboardPiece>();
            List<ChessboardPiece> spacesWithEnemies = new List<ChessboardPiece>();
            int currentMaxDistance = 0;
            foreach (ChessboardPiece c in allSpaces)
            {
                if (c.hasChessPiece)
                {
                    if (c.currentChessPiece.owner != unit.owner)
                    {
                        spacesWithEnemies.Add(c);
                    }
                }
                List<ChessboardPiece> temp = MainScript.currentBoard.GetAllPiecesFromOriginInDirection(c, Vector2.down, false, true, 4,unit.owner);
                if (temp.Count > currentMaxDistance)
                {
                    spacesWithLongestDistanceDownward = new List<ChessboardPiece>() { c };
                    currentMaxDistance = temp.Count;
                }
                else if (temp.Count == currentMaxDistance) { spacesWithLongestDistanceDownward.Add(c); }
            }
            if (spacesWithLongestDistanceDownward.Count > 0)
            {
                int randomInt = (int)Random.Range(0, spacesWithLongestDistanceDownward.Count);
                ChessboardPiece randomSpace = spacesWithLongestDistanceDownward[randomInt];
                List<ChessboardPiece> primeSpaces = new List<ChessboardPiece>();
                if (spacesWithEnemies.Count > 0)//if we can move attack someone we'll do that
                {
                    foreach (ChessboardPiece s in spacesWithEnemies)
                    {
                        if (spacesWithLongestDistanceDownward.Contains(s)) { primeSpaces.Add(s); }
                    }
                    if (primeSpaces.Count > 0) //if there is a prime space (one where you can attack a minion and sets you up to move far downward)
                    {
                        randomInt = (int)Random.Range(0, primeSpaces.Count);
                        randomSpace = primeSpaces[randomInt];
                    }
                    else
                    {
                        randomInt = (int)Random.Range(0, spacesWithEnemies.Count);
                        randomSpace = spacesWithEnemies[randomInt];
                    }
                    hasAttacked = true;
                    ChessboardPiece furthestPiece = pos;
                    Debug.Log("adding the spaces sides furthest piece unit belongs to " + unit.owner.theType + "and the defender is " + randomSpace.currentChessPiece.owner.theType);
                    queue.Add(GameEvent.GetAttackEvent(unit, randomSpace.currentChessPiece));//add an attack event for that minion
                    ChessboardPiece preAttackMoveSpot = pos;
                    if (rightSideSpace.Contains(randomSpace))
                    {
                        if (rightSideSpace.Count > 1)
                        {
                            preAttackMoveSpot = rightSideSpace[rightSideSpace.Count - 2];
                        }
                    }
                    else if (leftSideSpace.Contains(randomSpace))
                    {
                        if (leftSideSpace.Count > 1)
                        {
                            preAttackMoveSpot = leftSideSpace[leftSideSpace.Count - 2];
                        }
                    }
                    if (preAttackMoveSpot != pos)//the furthest piece isn't the piece were already standing on, move to it
                    {
                        Debug.Log("moving there");
                        queue.Add(GameEvent.GetMoveEvent(unit, preAttackMoveSpot));//move this unit towards him
                        queue.Add(GameEvent.GetMoveAttackFromBoardPieces(preAttackMoveSpot, randomSpace,unit.owner));
                    }
                    else
                    {
                        Debug.Log("move attacking there");
                        queue.Add(GameEvent.GetMoveAttackEvent(unit, furthestPiece.currentChessPiece));//and add a moveattack afterwards
                    }
                }
                else
                {
                    Debug.Log("moving to a random space");
                    queue.Add(GameEvent.GetMoveEvent(unit, randomSpace));// just move to a random space
                }
            } 
        }
        if (!hasAttacked)
        {
            List<ChessboardPiece> spacesWithinAttackRange = MainScript.currentBoard.GetAllPiecesWithinDistance(unit.attackRange, false, pos);
            List<Chesspiece> possibleAttackTargets = new List<Chesspiece>();
            foreach(ChessboardPiece c in spacesWithinAttackRange)
            {
                if (c.hasChessPiece)
                {
                    if(c.currentChessPiece.owner != unit.owner && c != unit.currentBoardPiece)
                    {
                        possibleAttackTargets.Add(c.currentChessPiece);
                    }
                }
            }
            if(possibleAttackTargets.Count > 0)
            {
                int randomInt = (int)Random.Range(0, possibleAttackTargets.Count);
                Chesspiece randomAttackTarget = possibleAttackTargets[randomInt];
                Debug.Log("adding random attack target");
                //queue.Add();
                queue.Insert(0, GameEvent.GetAttackEvent(unit, randomAttackTarget));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
