using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static BuildingReference hospitalRef;
    public GameObject toxicFlask;
    void Start()
    {
        
    }
    public void SetupHospitalLibrary()
    {
        hospitalRef = new BuildingReference();
        LibraryScript.buildingLibrary[3].buildingRefParent = hospitalRef;
        LibraryScript.buildingLibrary[3].cardReference.buildingRefParent = hospitalRef;
        LibraryScript.buildingRefLibrary.Add(hospitalRef);
        hospitalRef.buildingRefName = "hospital";
        hospitalRef.cardLibrary = new List<CardReference>();
        hospitalRef.unitLibrary = new List<UnitReference>()
        {
            new UnitReference("Plague Virologist", 4, 9, 2, 4, "plagueDoctorPrefab", 4, ChessPieceMovementAbilityType.rook, 2, "plagueDoctorCard",false,false,CardTargetType.requiresNoTarget,AttackType.shoot) ,
            new UnitReference("Candy Stripe Nurse", 3, 5, 2, 4, "candyStripeNursePrefab", 4, ChessPieceMovementAbilityType.rook, 1, "candyStripeNurseCard",false,false,CardTargetType.requiresNoTarget,AttackType.melee) 
        };
        foreach (UnitReference u in hospitalRef.unitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = hospitalRef;
            hospitalRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = hospitalRef;
        }
        hospitalRef.cardlessUnitLibrary = new List<UnitReference>()
        {
            
        };
        foreach (UnitReference u in hospitalRef.cardlessUnitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = hospitalRef;
            hospitalRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = hospitalRef;
        }
        hospitalRef.cardlessBuildingLibrary = new List<UnitReference>()
        {
            new UnitReference("Care Package",0,10,0,5,"carePackagePrefab",0,ChessPieceMovementAbilityType.none,0,"carePackageUnitCard",AttackType.melee)
        };
        foreach (UnitReference r in hospitalRef.cardlessBuildingLibrary)
        {
            CardReference temp = CardReference.GetBuilding(r);
            r.cardReference = temp;
            temp.buildingRefParent = hospitalRef;
            r.buildingRefParent = hospitalRef;
            r.storeFront = new List<CardReference>();// dont think any non carded buildings are gonna have their own storefronts
            r.researchStoreFront = new List<CardReference>();
        }
        hospitalRef.cardlessBuildingLibrary[0].hasOnPlayAnimation = true;
        hospitalRef.spellLibrary = new List<CardReference>()
        {
            //(new CardReference(CardType.spell, CardTargetType.targetsUnit, 6, "Fireball", "fireball"))
            new CardReference(CardType.spell,CardTargetType.targetsFriendlyUnit,2,"Love Potion No 4.20","lovePotionCard"),
            new CardReference(CardType.spell,CardTargetType.targetsFriendlyUnit,2,"Noodle Healing","noodleHealingCard")

        };
        foreach (CardReference c in hospitalRef.spellLibrary) { hospitalRef.cardLibrary.Add(c); c.buildingRefParent = hospitalRef; }
        hospitalRef.unitAbilityLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard",true),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,1,"Toxic Flask", "toxicFlaskCard"),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,1,"Wall of Toxin", "wallOfToxinCard"),
            new CardReference(CardType.spell, CardTargetType.emptyBoardPiece,1,"it must be my birthday","itMustBeMyBirthdayCard"),
            new CardReference(CardType.spell, CardTargetType.targetsUnit,4,"I Heart You", "IHeartYouCard")

        };
        foreach(CardReference c in hospitalRef.unitAbilityLibrary) { c.buildingRefParent = hospitalRef; c.isUnitAbility = true; }
        hospitalRef.cardlessSpellLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard"),
        };
        foreach (CardReference c in hospitalRef.cardlessSpellLibrary) { c.buildingRefParent = hospitalRef; }
        hospitalRef.storefront = new List<CardReference>()
        {
            hospitalRef.unitLibrary[0].cardReference,
            //hospitalRef.spellLibrary[1],
            //hospitalRef.spellLibrary[2]
        };
        hospitalRef.researchStorefront = new List<CardReference>()
        {
            //hospitalRef.unitAbilityLibrary[0]
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Upgrade Hospital Attack","UpgradeBarracksAttackCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Upgrade Hospital Defence","UpgradeBarracksDefenceCard")
        };
        foreach (CardReference r in hospitalRef.researchStorefront) { r.buildingRefParent = hospitalRef; }
        
    }
    public List<CardReference> GetBuildingStorefront()
    {
        return new List<CardReference>() { hospitalRef.unitLibrary[0].cardReference,hospitalRef.unitLibrary[1].cardReference };
            
    }
    public List<GameEvent> GetListenerEvents(EventListener l)
    {
        switch (l.listenerName)
        {

        }
        return new List<GameEvent>();
    }
    public List<GameEvent> PlaySpell(Card c, List<ChessboardPiece> targets, List<Chesspiece> units,GameEvent e)//if a target is not required simply use nullboardpiece
    {
        //Debug.Log("playing spell " + c.cardName);
        switch (c.cardName)
        {
            case "Toxic Flask":
                return ToxicFlask(units[0],targets[0],e);
            case "Wall of Toxin":
                return WallOfToxin(units[0].currentBoardPiece, targets[0],e);
            case "it must be my birthday":
                return ItMustBeMyBirthday(c, targets[0], units[0]);
            case "I Heart You":
                if (targets[0].hasChessPiece)
                {
                    Chesspiece iHeartYouTarget = targets[0].currentChessPiece;
                    //Chesspiece iHeartYouTarget = units[1];
                    iHeartYouTarget.currentBoardPiece.CreateBigHeartSymbol();
                    if (iHeartYouTarget.alive)
                    {
                        
                        iHeartYouTarget.currentHealth += 3; if (iHeartYouTarget.currentHealth > iHeartYouTarget.maxHealth) { iHeartYouTarget.currentHealth = iHeartYouTarget.maxHealth; }
                        //units[0].currentHealth += 3; if (units[0].currentHealth > units[0].maxHealth) { units[0].currentHealth = units[0].maxHealth; }
                        EmptyReadableCardPrefabScript temp = c.owner.GetPlayerEmptyReadableCard();
                        temp.otherCard.AppearAsThisUnit(iHeartYouTarget);
                        temp.otherCard.SetupHealthBuff(3);
                    }
                }
                //e.requiresAnimation = false;
                return new List<GameEvent>();
        }
        return new List<GameEvent>();
    }
    public GameObject GetProjectilePrefab(CardReference c)
    {
        switch (c.cardName)
        {
            case "Toxic Flask":
                return toxicFlask;
        }
        return toxicFlask;
    }
    public List<GameEvent> GetGrenadeEvents(CardReference c, ChessboardPiece target)
    {
        switch (c.cardName)
        {
            case "Toxic Flask":
                return ToxicFlaskGrenadeEvents(target);
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> WallOfToxin(ChessboardPiece doctorPosition, ChessboardPiece target, GameEvent e)
    {

        int xDiff = doctorPosition.xPos - target.xPos;
        int yDiff = doctorPosition.yPos - target.yPos;
        doctorPosition.currentChessPiece.SetupAnimation("shoot", e.animationTime);
        doctorPosition.currentChessPiece.transform.rotation = Quaternion.LookRotation(new Vector3(xDiff, 0f, yDiff) * -1f, Vector3.up);
        MainScript.unitsToReorient.Add(doctorPosition.currentChessPiece);
        List<ChessboardPiece> targets = MainScript.currentBoard.GetAllPiecesFromOriginInDirection(doctorPosition, new Vector2(xDiff * -1, yDiff * -1), true, false, 3, MainScript.neutralPlayer);
        return new List<GameEvent>() { GameEvent.CreatePoisonAddingEvent(targets, 3) };
    }
    public List<GameEvent>ToxicFlaskGrenadeEvents(ChessboardPiece target)
    {
        List<ChessboardPiece> usedPieces = new List<ChessboardPiece>() { target };
        List<ChessboardPiece> possibleNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, false, target);
        List<ChessboardPiece> partTwoTargets = new List<ChessboardPiece>();
        List<GameEvent> eventsToAdd = new List<GameEvent>() { GameEvent.CreatePoisonAddingEvent(new List<ChessboardPiece>() { target }, 3) };
        //List<ChessboardPiece> possibleNeighboursNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, false, target);
        //target.SetupGasCloud(3);
        for (int i = 0; i < 2 && possibleNeighbours.Count > 0; i++)
        {
            int randomInt = (int)Random.Range(0, possibleNeighbours.Count);
            ChessboardPiece randomPiece = possibleNeighbours[randomInt];
            if (usedPieces.Contains(randomPiece)) { }
            else
            {
                //randomPiece.SetupGasCloud(2);
                usedPieces.Add(randomPiece);

                partTwoTargets.Add(randomPiece);
            }
            possibleNeighbours.Remove(randomPiece);
        }
        eventsToAdd.Add(GameEvent.CreatePoisonAddingEvent(partTwoTargets, 2));
        List<ChessboardPiece> partThreeTargets = new List<ChessboardPiece>();
        foreach (ChessboardPiece c in partTwoTargets)
        {
            possibleNeighbours = new List<ChessboardPiece>();
            possibleNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, c);
            List<ChessboardPiece> toRemove = new List<ChessboardPiece>();
            foreach (ChessboardPiece c2 in possibleNeighbours) { if (usedPieces.Contains(c2)) { toRemove.Add(c2); } } while (toRemove.Count > 0) { possibleNeighbours.Remove(toRemove[0]); toRemove.RemoveAt(0); }
            bool hasFoundATarget = false;
            while (hasFoundATarget == false && possibleNeighbours.Count > 0)
            {
                int randomInt = (int)Random.Range(0, possibleNeighbours.Count);
                ChessboardPiece randomPiece = possibleNeighbours[randomInt];
                if (usedPieces.Contains(randomPiece))
                {
                    possibleNeighbours.Remove(randomPiece);
                    //    Debug.Log("removing random piece because it is in used pieces"); 
                }
                else
                {
                    //randomPiece.SetupGasCloud(1);
                    partThreeTargets.Add(randomPiece);
                    usedPieces.Add(randomPiece);
                }
                if (partThreeTargets.Count == 3 || partThreeTargets.Count == 6) { hasFoundATarget = true; }
            }
        }
        eventsToAdd.Add(GameEvent.CreatePoisonAddingEvent(partThreeTargets, 1));
        return eventsToAdd;
    }
    public List<GameEvent> ToxicFlask( Chesspiece theDoctor,ChessboardPiece target,GameEvent e)
    {
        GameEvent tempor = GameEvent.GetGrenadeEvent(e.relevantCards[0].cardReference, theDoctor, target);
        return new List<GameEvent>() { tempor };
    }
        
    public List<CardReference> GetStorefront()
    {
        return new List<CardReference>() { hospitalRef.unitLibrary[0].cardReference, hospitalRef.unitLibrary[1].cardReference};
    }

    public List<CardReference> GetUnitAbilities(UnitReference unit, Player owner)
    {
        switch (unit.unitName)
        {
            case "Doge":
                return new List<CardReference>() { hospitalRef.unitAbilityLibrary[0] };
            case "Plague Virologist":
                return new List<CardReference>() { hospitalRef.unitAbilityLibrary[1], hospitalRef.unitAbilityLibrary[2] };
            case "Candy Stripe Nurse":
                return new List<CardReference>() { hospitalRef.unitAbilityLibrary[3], hospitalRef.unitAbilityLibrary[4] };
            case "Care Package":
                return new List<CardReference>() { hospitalRef.unitAbilityLibrary[4], hospitalRef.unitAbilityLibrary[4] };
        }
        return new List<CardReference>();
    }
    public List<CardReference> GetBuildingResearchableCards()
    {
        //Debug.Log("getting buildings researchable cards at hospital");
        return new List<CardReference>() { hospitalRef.cardlessSpellLibrary[0],hospitalRef.researchStorefront[0],hospitalRef.researchStorefront[1] };
    }
    public List<ChessboardPiece> GetOnPlayTargets(Card c, ChessboardPiece target, Chesspiece selectedUnit)
    {
        
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        switch (c.cardName)
        {
            case "Uncle Murphy":
                return MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, target);//uncle murphy uses tareget because its an on play effect
            case "Wall of Toxin":
                return MainScript.currentBoard.GetBoardPieceNeighbours(true, true, false, selectedUnit.currentBoardPiece);//wall of toxin uses the selected unit's position becuase it is a unit ability
            case "Toxic Flask":
                int toxicFlaskRange = 3;
                return MainScript.currentBoard.GetAllPiecesWithinDistance(toxicFlaskRange, true, selectedUnit.currentBoardPiece);
            case "I Heart You":
                List<ChessboardPiece> possibleTargets = new List<ChessboardPiece>() { MainScript.nullBoardPiece};
                foreach (ChessboardPiece p in MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, selectedUnit.currentBoardPiece)) { if (p.hasChessPiece) {if (p.currentChessPiece.owner.playerNumber == c.owner.playerNumber) { possibleTargets.Add(p); } else { MainScript.possibleAttackLocations.Add(p); }}else{   MainScript.possibleAttackLocations.Add(p);}}
                    return possibleTargets;
        }
        return tempList;
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
                    secondaryTarget.currentChessPiece.attack += 3;
                    secondaryTarget.currentChessPiece.maxHealth += 3;
                    secondaryTarget.currentChessPiece.currentHealth += 3;
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
    public List<GameEvent> ItMustBeMyBirthday(Card c, ChessboardPiece target,Chesspiece nurse)
    {
        GameEvent temp = GameEvent.GetSummonDyingUnitEvent(hospitalRef.cardlessBuildingLibrary[0], target, nurse.owner);
        temp.relevantBool = false;
        return new List<GameEvent>() { temp};
    }
    public List<GameEvent> ResearchCard(Card c, Player owner)
    {
        switch (c.cardName)
        {
            case "Stimpak":
                LibraryScript.unitLibrary[1].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.addToAbilities, new List<CardReference>() { c.cardReference },owner));
                LibraryScript.buildingLibrary[2].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.removeFromResearchableCards, new List<CardReference>() { LibraryScript.cardlessSpellLibrary[1] },owner));
                return new List<GameEvent>();
            case "Upgrade Hospital Attack":
                UnitReferenceModifier m = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), c.owner);
                m.SetAttack(1);
                LibraryScript.ApplyModToCards(CardReference.GetCardReferencesFromUnitReferences(hospitalRef.unitLibrary), new List<CardModifier>() { }, new List<UnitReferenceModifier>() { m });
                break;
            case "Upgrade Hospital Defence":
                UnitReferenceModifier m2 = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), c.owner);
                m2.SetHealth(1);
                LibraryScript.ApplyModToCards(CardReference.GetCardReferencesFromUnitReferences(hospitalRef.unitLibrary), new List<CardModifier>() { }, new List<UnitReferenceModifier>() { m2 });
                break;
        }
        return new List<GameEvent>();
    }
    public bool DoesThisListenerActivate(EventListener l, List<GameEvent> theQueue)
    { //checks if a listener is activated. a listener returns true if the queue has been rearranged, otherwise it just returns false and adds the listener effects to the queue if the circumstances apply
        GameEvent triggerEvent = theQueue[0];
        //Debug.Log("hospitalscript checking listenere " + l.listenerName);
        switch (l.listenerName)//within the switch case statement the function must decide whether the listener activates or changes the queue at all. you can have a when allies are attacked listener, and it would take the move attack event type etc, but would have to conclude whether the person being attacked was an ally
        {
            case "dieAtTheEndOfTurn":
                theQueue.Insert(0, GameEvent.GetDeathEvent(new List<Chesspiece>() { l.unitOwner }));
                MainScript.allEventListeners.Remove(l);
                return true;
        }
        return false;//defaults to false
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
