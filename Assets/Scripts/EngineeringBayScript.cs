using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineeringBayScript : MonoBehaviour
{
    public GameObject artilleryTargetPrefab;
    public GameObject mortarSlugPrefab;
    public GameObject laserBeamPrefab;
    // Start is called before the first frame update
    public static BuildingReference engineeringBayRef;
    void Start()
    {
        
    }
    public void SetupEngineeringBayLibrary()
    {
        engineeringBayRef = new BuildingReference();
        LibraryScript.buildingLibrary[2].buildingRefParent = engineeringBayRef;
        LibraryScript.buildingLibrary[2].cardReference.buildingRefParent = engineeringBayRef;
        LibraryScript.buildingRefLibrary.Add(engineeringBayRef);
        engineeringBayRef.buildingRefName = "engineering";
        engineeringBayRef.cardLibrary = new List<CardReference>();
        engineeringBayRef.unitLibrary = new List<UnitReference>()
        {
            new UnitReference("Cheems", 2, 9, 2, 2, "engineerPrefab", 4, ChessPieceMovementAbilityType.bishop, 1, "cheemsCard",true,false,CardTargetType.requiresNoTarget,AttackType.melee),
            new UnitReference("Electrician", 3, 7, 2, 2, "electricianPrefab", 4, ChessPieceMovementAbilityType.bishop, 1, "electricianCard",true,false,CardTargetType.requiresNoTarget,AttackType.melee)
        };
        foreach (UnitReference u in engineeringBayRef.unitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = engineeringBayRef;
            engineeringBayRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = engineeringBayRef;
            currentReference.ai = new List<CardAIComponent>() { new CardAIComponent(CardAIComponentType.playRandomly), new CardAIComponent(CardAIComponentType.playBeforeSummoning) };
        }

        engineeringBayRef.unitAbilityLibrary = new List<CardReference>()
        {
            //new CardReference(CardType.spell,1,")
            new CardReference(CardType.spell,CardTargetType.emptyBoardPiece,2,"Deploy Walking Mine", "walkingMineDeployCard"),
            CardReference.DrawRange(new CardReference(CardType.spell, CardTargetType.emptyBoardPiece, 2, "Deploy Static Field Disruptor", false, "deployStaticFieldDisruptorCard"), 4),
            new CardReference(CardType.spell, CardTargetType.emptyBoardPiece, 2, "Deploy Icarus Mortar", false, "icarusMortarCard"),
            CardReference.GiveCardReferenceDrawLineToSecondTarget(new CardReference(CardType.spell,CardTargetType.emptyBoardPiece,2,"Deploy Electric Fence",true,"deployElectricFenceCard")),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,0,"Stoney Dark built this IN A CAVE","stoneyDarkBuiltThisCard"),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,0,"Box of Scraps","boxOfScrapsCard"),
            
        };
        foreach (CardReference c in engineeringBayRef.unitAbilityLibrary) { c.buildingRefParent = engineeringBayRef;c.isUnitAbility = true; }
        engineeringBayRef.emptyCardLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,0,"With Great Power Comes Great Responsibility","greatPowerGreatResponsibilityCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,0,"Dont Waste Your Life","dontWasteYourLifeCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,0,"I Want You To Give All That Up","IWantYouToGiveThatAllUpCard")
        };
        engineeringBayRef.cardlessUnitLibrary = new List<UnitReference>()
        {
            new UnitReference("Barrel", 0, 5, 0, 0, "barrelPrefab", 0, ChessPieceMovementAbilityType.none, 0, "barrelCard", true),
            new UnitReference("Walking Mine",1,2,2,1,"walkingMinePrefab",2,ChessPieceMovementAbilityType.rook,1,"walkingMineUnitCard",AttackType.shoot),
            new UnitReference("Manos Feros",5,10,3,4,"manosFerosPrefab",3,ChessPieceMovementAbilityType.queen,1,"manosFerosUnitCard",AttackType.melee),
            new UnitReference("Dying Mentor", 2, 9, 2, 2, "basicUnitPrefab", 4, ChessPieceMovementAbilityType.bishop, 1, "dyingMentorCard",true,false,CardTargetType.requiresNoTarget,AttackType.melee)
        };
        foreach (UnitReference u in engineeringBayRef.cardlessUnitLibrary)
        {
            CardReference currentReference = new CardReference(u);
            currentReference.buildingRefParent = engineeringBayRef;
            //engineeringBayRef.cardLibrary.Add(currentReference);
            u.cardReference = currentReference;
            u.buildingRefParent = engineeringBayRef;
        }
        //foreach (UnitReference c in engineeringBayRef.cardlessUnitLibrary) { c.buildingRefParent = engineeringBayRef; }
        engineeringBayRef.spellLibrary = new List<CardReference>()
        {
            (new CardReference(CardType.spell, CardTargetType.requiresNoTarget, engineeringBayRef.cardlessUnitLibrary[0],"Its Raining Barrels", "itsRainingBarrels")),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Behold","beholdCard")

        };
        foreach(CardReference c in engineeringBayRef.spellLibrary) { c.buildingRefParent = engineeringBayRef; }
        foreach (UnitReference u in engineeringBayRef.cardlessUnitLibrary)
        {
            //CardReference currentReference = new CardReference(u);
            //engineeringBayRef.cardLibrary.Add(currentReference);
            //u.cardReference = currentReference;
        }
        engineeringBayRef.researchStorefront = new List<CardReference>()
        {
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Upgrade Engineering Bay Attack","UpgradeBarracksAttackCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,1,"Upgrade Engineering Bay Defence","UpgradeBarracksDefenceCard")
        };
        foreach(CardReference r in engineeringBayRef.researchStorefront) { r.buildingRefParent = engineeringBayRef; }
        engineeringBayRef.cardlessSpellLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,3,"My Stuff","myStuffCard"),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,2,"Stimpak","stimpakCard"),
            new CardReference(CardType.spell,CardTargetType.requiresNoTarget,3,"If you throw another moon at me","throwAnotherMoonCard"),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Push Units Outward","pushUnitsOutwardCard"),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,0,"Dont Whiz on the Electric Fence", "dontWhizOnTheElectricFenceCard")
        };
        foreach(CardReference c in engineeringBayRef.cardlessSpellLibrary)
        {
            c.isUnitAbility = true;
            c.buildingRefParent = engineeringBayRef;
        }
        engineeringBayRef.cardlessBuildingLibrary = new List<UnitReference>()
        {
            new UnitReference("Icarus Mortar Installation", 2 ,12,0,2,"icarusMortarPrefab",0,ChessPieceMovementAbilityType.none,0,"icarusMortarCard",AttackType.shoot),
            new UnitReference("Static Field Disruptor", 2 ,12,0,2,"staticFieldDisruptorPrefab",0,ChessPieceMovementAbilityType.none,0,"staticFieldDisruptorUnitCard",AttackType.shoot),
            new UnitReference("Electric Fence", 2 ,22,0,2,"electricFencePrefab",0,ChessPieceMovementAbilityType.none,0,"electricFenceUnitCard",AttackType.shoot),
            new UnitReference("Cave",0,3,0,4,"cavePrefab",0,ChessPieceMovementAbilityType.none,0,"caveUnitCard",AttackType.shoot)
        };
        engineeringBayRef.cardlessBuildingLibrary[1].hasEventListeners = true;
        engineeringBayRef.cardlessBuildingLibrary[3].hasEventListeners = true;
        foreach (UnitReference r in engineeringBayRef.cardlessBuildingLibrary)
        {
            CardReference temp = CardReference.GetBuilding(r);
            //engineeringBayRef.cardLibrary.Add(temp);
            //cardLibraryProper.Add(temp);
            r.cardReference = temp;
            temp.buildingRefParent = engineeringBayRef;
            r.buildingRefParent = engineeringBayRef;
            r.storeFront = new List<CardReference>();// dont think any non carded buildings are gonna have their own storefronts
            r.researchStoreFront = new List<CardReference>();
        }
        /*engineeringBayRef.unitAbilityLibrary = new List<CardReference>()
        {
            new CardReference(CardType.spell, CardTargetType.targetsUnit, 3, "Doge Deals Damage", "dogeDealsDamage", true),
            new CardReference(CardType.spell, CardTargetType.requiresNoTarget,1,"Stimpak","stimpakCard",true),
            new CardReference(CardType.spell,CardTargetType.emptyBoardPiece,2,"Deploy Walking Mine", "walkingMineDeployCard"),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,1,"Toxic Flask","toxicFlaskCard"),
            new CardReference(CardType.spell, CardTargetType.targetsBoardPiece,1,"Wall of Toxin","wallOfToxinCard")

        };*/
        foreach (CardReference c in engineeringBayRef.unitAbilityLibrary)
        {
            
            //engineeringBayRef.cardLibrary.Add(c);
        }
    }
    public List<GameEvent> GetChooseCardEvent(Card cardChosen, CardReference originalCard, ChessboardPiece target, ChessboardPiece secondaryTarget, Chesspiece selectedUnit, Player targetPlayer)
    {
        switch (originalCard.cardName)
        {
            case "Dying Mentor":
                //Debug.Log("runnign dying mentor from choose card event chosen card is " + cardChosen.cardName + " and the original card was " + originalCard.cardName);
                //if(target != MainScript.nullBoardPiece) { target.LogCoordinates("target "); }
                //if(secondaryTarget != MainScript.nullBoardPiece) { secondaryTarget.LogCoordinates("target "); }
                //if(selectedUnit != MainScript.nullUnit) { Debug.Log(selectedUnit.unitRef.unitName); }
                //Debug.Log()
                switch (cardChosen.cardName)
                {
                    case "I Want You To Give All That Up":
                        if(selectedUnit.unitRef.unitName == "Manos Feros")
                        {
                            return new List<GameEvent>() { GameEvent.GetBuffEvent(new UnitBuff(3, 3, false, false), new List<Chesspiece>() { selectedUnit }) };
                        }
                        //if()
                        break;
                }
                //cardChosen.owner = targetPlayer;
                //targetPlayer.hand.cardsInHand.Add(cardChosen);
                //cardChosen);
                break;

        }
        return new List<GameEvent>();
    }
    public List<GameEvent> BoxOfScraps(Chesspiece manosFerros, Card c)
    {
        List<GameEvent> tempList = new List<GameEvent>();
        List<ChessboardPiece> surroundingArea = MainScript.currentBoard.GetBoardPieceNeighbours(true,true,true,manosFerros.currentBoardPiece);
        for(int i = 0; i< surroundingArea.Count; i++)
        {
            ChessboardPiece currentPiece = surroundingArea[i];
            if (currentPiece.hasChessPiece)
            {
                surroundingArea.RemoveAt(i);i--;
            }
        }
        if(surroundingArea.Count > 0)
        {
            int randoInt = (int)Random.Range(0, surroundingArea.Count);
            ChessboardPiece randomPiece = surroundingArea[randoInt];
            tempList.Add(GameEvent.GetSummonEvent(engineeringBayRef.cardlessUnitLibrary[0], randomPiece, MainScript.neutralPlayer));//barrels belong to the neutral player as they can be attacked by either player
            surroundingArea.Remove(randomPiece);
        }
        if (surroundingArea.Count > 0)
        {
            int randoInt = (int)Random.Range(0, surroundingArea.Count);
            ChessboardPiece randomPiece = surroundingArea[randoInt];
            tempList.Add(GameEvent.GetSummonEvent(engineeringBayRef.cardlessBuildingLibrary[2], randomPiece, manosFerros.owner));
            surroundingArea.Remove(randomPiece);
        }
        if (surroundingArea.Count > 0)
        {
            int randoInt = (int)Random.Range(0, surroundingArea.Count);
            ChessboardPiece randomPiece = surroundingArea[randoInt];
            GameEvent summonDyingMentor = GameEvent.GetSummonDyingUnitEvent(engineeringBayRef.cardlessUnitLibrary[3], randomPiece, manosFerros.owner);
            summonDyingMentor.relevantBool = true;//relevant bool set to true indicates it dies immediately
            summonDyingMentor.relevantUnits = new List<Chesspiece>() { manosFerros };
            tempList.Add(summonDyingMentor);
            surroundingArea.Remove(randomPiece);
        }
        return tempList;
    }
    public List<GameEvent> PlaySpell(Card c, List<ChessboardPiece> targets, List<Chesspiece> units, GameEvent e)//if a target is not required simply use nullboardpiece
    {
        //Debug.Log("playing spell " + c.cardName);
        switch (c.cardName)
        {
            case "Box of Scraps":
                return BoxOfScraps(units[0],c);
            case "Deploy Walking Mine":
                targets[0].CreateHammerWrenchSymbol();
                return DeployWalkingMine(targets[0], c.owner);
            case "Toxic Flask":
                //return ToxicFlask(units[0], targets[0], e);
            case "Wall of Toxin":
                //return WallOfToxin(units[0].currentBoardPiece, targets[0], e);
                break;
            case "If you throw another moon at me":
                return ThrowAMoon(units[0]);
            case "Push Units Outward":
                return PushUnitsOutward(units[0]);
            case "Its Raining Barrels":
                return ItsRainingBarrels(c);
            case "Deploy Static Field Disruptor":
                targets[0].CreateHammerWrenchSymbol();
                return new List<GameEvent>() { GameEvent.GetSummonEvent(engineeringBayRef.cardlessBuildingLibrary[1], targets[0], units[0].owner)};
            case "Deploy Icarus Mortar":
                targets[0].CreateHammerWrenchSymbol();
                return new List<GameEvent>() { GameEvent.GetSummonEvent(engineeringBayRef.cardlessBuildingLibrary[0], targets[0], units[0].owner)};
            case "Deploy Electric Fence":
                targets[0].CreateHammerWrenchSymbol();
                targets[1].CreateHammerWrenchSymbol();
                return DeployElectricFence(targets[0],targets[1],units[0]);
            case "Stoney Dark built this IN A CAVE":
                List<GameEvent> tempList = new List<GameEvent>();
                tempList.Add(GameEvent.GetDeathEvent(new List<Chesspiece>() { units[0] }));
                //tempList[0].requiresAnimation = true;
                tempList.Add(GameEvent.GetSummonEvent(engineeringBayRef.cardlessBuildingLibrary[3], units[0].currentBoardPiece, units[0].owner));
                return tempList;
            case "Dont Whiz on the Electric Fence":
                return DontWhizz(e);
        }
        return new List<GameEvent>();
    }
    public List<GameEvent> DontWhizz(GameEvent e)
    {
        //Debug.Log("running dontwhizz");
        List<GameEvent> tempList = new List<GameEvent>();
        EventListener l = e.relevantListener;
        Chesspiece unitGettingShocked = MainScript.nullUnit;
        if (e.theTarget[0].hasChessPiece) { unitGettingShocked = e.theTarget[0].currentChessPiece;unitGettingShocked.SetupAnimation("takeDamage", unitGettingShocked.GetAnimationLength("takeDamage")); }
        MainScript.projectileTransforms = new List<Transform>();
        for(int i = 0; i < l.unitOwners.Count; i++)
        {
            ChessboardPiece c = l.unitOwners[i].currentBoardPiece;
            Vector3 directionToTarget = c.transform.position - e.theTarget[0].transform.position;//get the direction from the attacker to the defender, use it to orient the projectile
            Vector3 positionToInstantiate = c.transform.position + (Vector3.up * MainScript.distanceBetweenUnitAndBoardPiece);
            Transform t = Instantiate(laserBeamPrefab, positionToInstantiate, Quaternion.identity).transform;
            t.SendMessage("SetupProjectile", directionToTarget * -1f);

            MainScript.projectileTransforms.Add(t);
            //projectileTransforms = new List<Transform>() { Instantiate(projectilePrefab, positionOfProjectile, Quaternion.LookRotation(Vector3.up, directionToTarget.normalized)).transform };
            //projectileTransforms[0].SendMessage("SetupProjectile", directionToTarget * -1f);
        }
        return tempList;
    }
    public List<GameEvent> DeployElectricFence(ChessboardPiece target,ChessboardPiece secondTarget, Chesspiece electricianUnit)
    {
        
        bool thereIsAnotherFencePost = false;//assum this fence has no other fencePost to attach to
        ChessboardPiece otherFence = MainScript.nullBoardPiece;
        return new List<GameEvent>() { GameEvent.GetSummonEventMultiple(engineeringBayRef.cardlessBuildingLibrary[2], new List<ChessboardPiece>() { target, secondTarget }, electricianUnit.owner) ,GameEvent.GetSetupFenceEvent(target,secondTarget)};
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
    public List<GameEvent> ApplyDeathrattle(Chesspiece deadUnit)
    {
        List<GameEvent> tempList = new List<GameEvent>();
        switch (deadUnit.unitRef.cardReference.cardName)
        {
            case "Dying Mentor":
                List<GameEvent> mentor = new List<GameEvent>();
                if (deadUnit.deathTarget != MainScript.nullBoardPiece)
                {
                    if (deadUnit.deathTarget.hasChessPiece)
                    {
                        Chesspiece manosTarget = deadUnit.deathTarget.currentChessPiece;
                        if(manosTarget.unitRef.cardReference.cardName == "Manos Feros" && manosTarget.owner == deadUnit.owner)
                        {
                            GameEvent dyingMentorChoose = GameEvent.GetChooseFromCardsEvent(new List<CardReference>() { engineeringBayRef.emptyCardLibrary[0], engineeringBayRef.emptyCardLibrary[1], engineeringBayRef.emptyCardLibrary[2]}, Card.GetCardWithNoTransform(deadUnit.unitRef.cardReference, deadUnit.owner), deadUnit.owner, false);
                            dyingMentorChoose.relevantUnits = new List<Chesspiece>() { manosTarget };
                            dyingMentorChoose.theTarget = new List<ChessboardPiece>() { manosTarget.currentBoardPiece };
                            mentor.Add(dyingMentorChoose);
                        }
                    }
                }
                return mentor;
            case "Cheems":
                return new List<GameEvent>() { new GameEvent(deadUnit.owner, 1, GameEventType.drawCard) };
            case "Barrel":
                List<ChessboardPiece> barrelNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, deadUnit.currentBoardPiece);
                List<Chesspiece> unitsToDamage = new List<Chesspiece>();
                deadUnit.SetupAnimation("death", 0.85f);
                foreach (ChessboardPiece c in barrelNeighbours)
                {
                    if (c.hasChessPiece)
                    {
                        unitsToDamage.Add(c.currentChessPiece);
                    }
                }
                if (unitsToDamage.Count > 0) 
                {
                    foreach(Chesspiece c in unitsToDamage)
                    {
                        c.DealDamage(10);
                        c.SetupAnimation("takeDamage", 0.85f);
                    }
                    
                    return new List<GameEvent>() {  }; 
                } else { return new List<GameEvent>(); };
        }
        return tempList;
    }
    public bool DoesExecutingThisEventActivateListenever(List<EventListener> ls, List<GameEvent> currentEvents)//for when an event has been confirmed for executing, rather than checking for an interuption. if you interupt attacking and a unit has a listener that responds to attacking the listener will activate twice, so this function is necessary for when an action is literally about to happen
    {
        GameEvent e = currentEvents[0];
        EventListener l = ls[0];
        //Debug.Log("engineering bay script checking for " + l.listenerName);
        switch (l.listenerName)
        {
            case "Walter":
                //Debug.Log("checking walters listener for activation");
                l.unitOwner.currentHealth += 2;
                l.unitOwner.maxHealth += 2;
                currentEvents.Insert(1, new GameEvent(l));
                return false;//pretty sure just the game event of attack unit, moveunit attack and deal damage should have walter covered
            case "Icarus Mortar Installation":
                currentEvents.Insert(0, new GameEvent(l));
                l.disabledUntilEndOfQueue = true;
                return true;
            case "fence":
                //Debug.Log("checking against the fence");
                
                bool hasBeenBlockedOnce = true;
                //Debug.Log("we are in this thing");
                ChessboardPiece start = e.theActor[0];
                ChessboardPiece end = e.theTarget[0];
                for (int i = 0; i < e.theActor.Count; i++)
                {
                    //Debug.Log("we get this far");
                    List<ChessboardPiece> areaTraversed = new List<ChessboardPiece>();
                    if (e.theType != GameEventType.throwUnit)//must be move unit if not throwunit
                    {
                        //Debug.Log("the spaces we are checking are from " + e.theActor[0].xPos + " , " + e.theActor[0].yPos + " to the point of " + e.theTarget[0].xPos + " , " + e.theTarget[0].yPos);
                        areaTraversed = MainScript.GetSpacesAlongLineBetweenTwoSpaces(e.theActor[0], e.theTarget[0], true,true);
                        //foreach(ChessboardPiece p in areaTraversed){ p.ChangeColor(Color.blue);p.ChangeTransparentRenderColor(Color.blue); }
                        //Debug.Log("we have a traveresed area of " + areaTraversed.Count + "spaces");
                    }
                    else
                    {
                        areaTraversed.Add(e.theTarget[i]);//if we're throwing a unit then we still have to check if it lands in the fence, otherwise he flies OVER the fence and isn't affected by it
                    }
                    bool hasBeenBlocked = false;
                    ChessboardPiece blockedPoint = MainScript.nullBoardPiece;
                    for(int ii = 0; ii < areaTraversed.Count ; ii++)
                    {
                        List<EventListener> listenersActivatingOnThisPiece = new List<EventListener>();
                        //ChessboardPiece currentPiece = areaTraversed[areaTraversed.Count - ii - 1];
                        ChessboardPiece currentPiece = areaTraversed[ii];
                        //Debug.Log("we are checking our path, on piece " + ii);
                        Color currentPieceColor = Color.white;
                        switch (ii)
                        {
                            case 0:
                                currentPieceColor = Color.yellow;
                                break;
                            case 1:
                                currentPieceColor = Color.blue;
                                break;
                            case 2:
                                currentPieceColor = Color.magenta;
                                break;
                            case 3:
                                currentPieceColor = Color.black;
                                break;
                            case 4:
                                currentPieceColor = Color.grey;
                                break;
                        }
                        //currentPiece.LogCoordinates("current piece is ");
                        //Debug.Log("coloring it : " + currentPieceColor.ToString());
                        //currentPiece.ChangeColor(currentPieceColor);
                        int listenersChecked = 0;
                        foreach (EventListener el in ls) 
                        {
                            listenersChecked++;
                            bool listenerHasActivated = false;
                            //Debug.Log("checking with EventListener " + listenersChecked);
                            List<ChessboardPiece> interceptArea = MainScript.GetSpacesAlongLineBetweenTwoSpaces(el.unitOwners[0].currentBoardPiece, el.unitOwners[1].currentBoardPiece, true, true);
                            //foreach(ChessboardPiece p in interceptArea) { p.ChangeColor(Color.cyan); }
                            if (interceptArea.Contains(currentPiece))
                            {
                                //Debug.Log("the interecept area contains te piece we're looking for at point " + ii + " out of " + areaTraversed.Count);
                                //currentPiece.LogCoordinates("current piece is ");
                                //Debug.Log("we are adding because current piece is within intercept area");
                                listenersActivatingOnThisPiece.Add(el);
                                hasBeenBlocked = true; 
                                blockedPoint = currentPiece; 
                                hasBeenBlockedOnce = true;
                                listenerHasActivated = true;
                            }
                            else { //Debug.Log("not in intercept area"); 
                            }

                            if (e.theType != GameEventType.throwUnit && !listenerHasActivated)//if the unit isn't moving literally over one of our spaces and isn't a throw unit which goes over the fence anyway check if it has crossed through diagonally
                            {//added listener has activated because a movement can both step on an intercept spot and cross the border and I think it was casting this twice for that reason
                                //Debug.Log("checking if we have croshed the threshold");
                                
                                Vector3 originPosition = e.theActor[0].transform.position;
                                //e.theActor[0].LogCoordinates("origin is ");
                                
                                
                                //Vector2 originPoint = new Vector2(originPosition.x, originPosition.z);
                                //e.theActor[0].ChangeColor(Color.cyan);
                                Vector3 fenceOnePos = el.unitOwners[0].currentBoardPiece.transform.position;
                                Vector3 fenceTwoPos = el.unitOwners[1].currentBoardPiece.transform.position;
                                Vector2 originPoint = new Vector2(originPosition.x, originPosition.z);
                                Vector2 fenceOnePoint = new Vector2(fenceOnePos.x, fenceOnePos.z);
                                Vector2 fenceTwoPoint = new Vector2(fenceTwoPos.x, fenceTwoPos.z);
                                Vector2 currentPiecePoint = new Vector2(currentPiece.transform.position.x, currentPiece.transform.position.z);
                                Vector2 directFromFenceOneToOrigin = originPoint - fenceOnePoint;
                                Vector2 directFromFenceTwoToOrigin = originPoint - fenceTwoPoint;
                                Vector2 perpindicularDirection = MainScript.RotateVector((fenceOnePoint - fenceTwoPoint).normalized, 90f);
                                //Debug.Log("directfrom fence one " + directFromFenceOneToOrigin + " direct from fence two " + directFromFenceTwoToOrigin);
                                Vector2 directFromFenceOneToCurrentPiece = currentPiecePoint - fenceOnePoint;
                                Vector2 directFromFenceTwoToCurrentPiece = currentPiecePoint - fenceTwoPoint;
                                Vector2 derpPoint = (directFromFenceOneToOrigin + directFromFenceTwoToOrigin).normalized;
                                float dotOne = Vector2.Dot(directFromFenceOneToCurrentPiece, fenceTwoPoint - fenceOnePoint);
                                float dotTwo = Vector2.Dot(directFromFenceTwoToCurrentPiece, fenceOnePoint - fenceTwoPoint);
                                if(Vector2.Dot(perpindicularDirection,derpPoint) < 0f) { perpindicularDirection *= -1f; }
                                Vector2 currentPoint = (directFromFenceOneToCurrentPiece + directFromFenceTwoToCurrentPiece).normalized;
                                //Debug.Log("derppoint normalized is " + derpPoint + " and current point normalized is " + currentPoint + " dot one " + dotOne + " and dot two " + dotTwo);
                                float pointDot = Vector2.Dot(perpindicularDirection.normalized, currentPoint.normalized);
                                //Debug.Log("point dot is " + pointDot);
                                //if(pointDot < 0f) { currentPiece.ChangeColor(Color.black); }else if(pointDot == 0f){ currentPiece.ChangeColor(Color.magenta); };
                                Vector3 middle = originPosition;
                                Vector3 directCurrent = currentPiece.transform.position - middle;
                                Vector3 derp = ((originPosition - fenceOnePos) + (originPosition - fenceTwoPos)) / 2f;
                                Vector3 directFromFenceOne = currentPiece.transform.position - fenceOnePos;
                                Vector3 directFromFenceTwo = currentPiece.transform.position - fenceTwoPos;
                                Vector3 sumCurrentPiece = (directFromFenceOne + directFromFenceTwo) / 2f;
                                //directFromFenceOne = prevPiecePos - fenceOnePos;
                                //directFromFenceTwo = prevPiecePos - fenceTwoPos;
                                Vector3 sumPrevPiece = (directFromFenceOne + directFromFenceTwo) / 2f;
                                float dotProduct = Vector3.Dot(derp.normalized, sumCurrentPiece.normalized);
                                //Debug.Log("we have a dot product of " + dotProduct + " from directTo origin " + derp.normalized + " and directTo currentPiece " + sumCurrentPiece.normalized);
                                if (pointDot <= 0f && dotOne > 0f && dotTwo > 0f)
                                {
                                    //currentPiece.ChangeColor(Color.black);
                                    hasBeenBlocked = true;
                                    blockedPoint = currentPiece;
                                    listenersActivatingOnThisPiece.Add(el);

                                }
                                else
                                {
                                    //currentPiece.ChangeColor(Color.magenta);
                                }
                                if (ii > 0)
                                {
                                    /*int previousRef = ii - 1;
                                    ChessboardPiece previousPiece = areaTraversed[previousRef];
                                    Vector3 prevPiecePos = previousPiece.transform.position;
                                    Vector3 directPrev = prevPiecePos - middle;*/
                                    
                                    //if(dotProduct > 0f) { currentPiece.ChangeColor(Color.blue); } else if( dotProduct < 0f){ currentPiece.ChangeColor(Color.black); } else { currentPiece.ChangeColor(Color.magenta); }
                                }
                                /*Vector3 rightAngle = ((originPosition - fenceOnePos) + (originPosition - fenceTwoPos)) / 2f;
                                float originDot = Mathf.Sign(Vector2.Dot(rightAngle.normalized, (originPosition - middle).normalized));
                                Vector3 directFromFenceOne = currentPiece.transform.position - fenceOnePos;
                                Vector3 directFromFenceTwo = currentPiece.transform.position - fenceTwoPos;
                                Vector3 directFromMiddle = currentPiece.transform.position - middle;
                                float currentPieceDot = Vector2.Dot(directFromMiddle.normalized, rightAngle.normalized);
                                if (Mathf.Sign(originDot) != Mathf.Sign(currentPieceDot))
                                {
                                    /*Debug.Log("now activating the fence because it is on the other side");
                                    hasBeenBlocked = true;
                                    blockedPoint = currentPiece;
                                    listenersActivatingOnThisPiece.Add(el);
                                }*/
                            }
                        }
                        if (listenersActivatingOnThisPiece.Count > 0) 
                        { 

                        }
                        if (hasBeenBlocked)
                        {
                            e.theTarget[i] = currentPiece;
                            List<GameEvent> fenceShockEvents = new List<GameEvent>();
                            foreach (EventListener el in listenersActivatingOnThisPiece)
                            {
                                Card c = new Card(engineeringBayRef.cardlessSpellLibrary[4], l.playerOwner);
                                c.hasNoTransform = true;
                                c.exileFromGameWhenPlayed = true;
                                GameEvent fenceShockEvent = GameEvent.GetPlayCardEvent(c, l.playerOwner, blockedPoint);
                                fenceShockEvent.relevantListener = el;
                                fenceShockEvents.Add(fenceShockEvent);
                            }
                            //Debug.Log("queue count is " + currentEvents.Count + " and event types are ");
                            /*for (int ii = 0; ii < currentEvents.Count; ii++)
                            {
                                GameEvent v = currentEvents[ii];
                                //Debug.Log(v.theType);
                            }*/
                            ii = areaTraversed.Count;
                            currentEvents.InsertRange(1, fenceShockEvents);
                            //currentEvents.Add(GameEvent.GetAttackEvent(randomUnitOwner, e.theActor[i].currentChessPiece));
                            //Debug.Log("one has been blocked!");
                        }

                    }
                    
                    //Debug.Log("we get further");
                    
                    //Debug.Log("was not blocked");
                }
                return false;
            
        }
        return false;
    }
    public List<CardReference> GetStorefront()
    {
        return new List<CardReference>() { engineeringBayRef.unitLibrary[1].cardReference,engineeringBayRef.unitLibrary[0].cardReference, engineeringBayRef.spellLibrary[1] };
    }
    public List<GameEvent> ResearchCard(Card c, Player owner)
    {
        switch (c.cardName)
        {
            case "Stimpak":
                LibraryScript.unitLibrary[1].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.addToAbilities, new List<CardReference>() { c.cardReference },owner));
                LibraryScript.buildingLibrary[2].modifiers.Add(new UnitReferenceModifier(UnitReferenceModifierType.removeFromResearchableCards, new List<CardReference>() { LibraryScript.cardlessSpellLibrary[1] },owner));
                return new List<GameEvent>();
            case "Upgrade Engineering Bay Attack":
                UnitReferenceModifier m = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), c.owner);
                m.SetAttack(1);
                LibraryScript.ApplyModToCards(CardReference.GetCardReferencesFromUnitReferences(engineeringBayRef.unitLibrary), new List<CardModifier>() { }, new List<UnitReferenceModifier>() { m });
                break;
            case "Upgrade Engineering Bay Defence":
                UnitReferenceModifier m2 = new UnitReferenceModifier(UnitReferenceModifierType.addStats, new List<CardReference>(), c.owner);
                m2.SetHealth(1);
                LibraryScript.ApplyModToCards(CardReference.GetCardReferencesFromUnitReferences(engineeringBayRef.unitLibrary), new List<CardModifier>() { }, new List<UnitReferenceModifier>() { m2 });
                break;
        }
        return new List<GameEvent>();
    }
    public bool DoesThisListenerActivate(EventListener l, List<GameEvent> theQueue)
    { //checks if a listener is activated. a listener returns true if the queue has been rearranged, otherwise it just returns false and adds the listener effects to the queue if the circumstances apply
        GameEvent triggerEvent = theQueue[0];
        //Debug.Log("gets here it do");
        switch (l.unitOwner.unitRef.unitName)//within the switch case statement the function must decide whether the listener activates or changes the queue at all. you can have a when allies are attacked listener, and it would take the move attack event type etc, but would have to conclude whether the person being attacked was an ally
        {
            case "Static Field Disruptor":
                Debug.Log(triggerEvent.theType);
                Chesspiece unit = triggerEvent.relevantUnits[0];
                ChessboardPiece destinationTarget = triggerEvent.theTarget[0];
                List<ChessboardPiece> disruptorRange = MainScript.currentBoard.GetAllPiecesWithinDistance(3, true, l.unitOwner.currentBoardPiece);
                List<ChessboardPiece> grenadeTrajectory = MainScript.GetSpacesAlongLineBetweenTwoSpaces(unit.currentBoardPiece, destinationTarget,true,true);
                foreach(ChessboardPiece c  in grenadeTrajectory)
                {
                    if (disruptorRange.Contains(c))
                    {
                        theQueue.Remove(triggerEvent);
                        //Debug.Log(triggerEvent.relevantCards[0].cardName);
                        l.disabledUntilEndOfQueue = true;
                        triggerEvent.theTarget[0].ChangeTransparentRenderColor(new Color(0f, 0f, 0f, 0f));
                        c.ChangeTransparentRenderColor(Color.magenta);
                        theQueue.Insert(0, GameEvent.GetInterceptEvent(triggerEvent.relevantCardReferences[0], unit, l.unitOwner, c));
                        return true;
                    }
                }
                return false;
            case "Cave":
                if(MainScript.currentGame.currentPlayer.playerNumber == l.unitOwner.owner.playerNumber) 
                {
                    l.disabledUntilEndOfQueue = true;
                    theQueue.Add( GameEvent.GetDeathEvent(new List<Chesspiece>() { l.unitOwner })); 
                    theQueue.Add( GameEvent.GetSummonEvent(engineeringBayRef.cardlessUnitLibrary[2], l.unitOwner.currentBoardPiece, l.unitOwner.owner)); 
                    return false;
                }
                return false;
        }
        return false;//defaults to false
    }
    public List< GameEvent>GetListenerEvents(EventListener l)
    {
        //Debug.Log("Getting listenere evensts from eng bay");
        switch (l.listenerName)
        {
            case "Icarus Mortar Installation":
                return IcarusMortarEndTurnActivation(l);
                break;
        }
        return new List<GameEvent>();
    }
    public List<CardReference> GetUnitAbilities(UnitReference unit)
    {
        switch (unit.unitName)
        {
            case "Manos Feros":
                return new List<CardReference>() { engineeringBayRef.unitAbilityLibrary[5] };
            case "Cheems":
                return new List<CardReference>() { engineeringBayRef.spellLibrary[0], engineeringBayRef.unitAbilityLibrary[0] ,engineeringBayRef.unitAbilityLibrary[2],engineeringBayRef.unitAbilityLibrary[4]};
            case "Icarus Mortar Installation":
                return new List<CardReference>() { engineeringBayRef.cardlessSpellLibrary[2]} ;
            case "Walking Mine":
                return new List<CardReference>() { engineeringBayRef.cardlessSpellLibrary[3] };
            case "Electrician":
                return new List<CardReference>() { engineeringBayRef.unitAbilityLibrary[1], engineeringBayRef.unitAbilityLibrary[3] };
            //case ""
        }
        return new List<CardReference>();
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
    public List<CardReference> GetBuildingResearchableCards()
    {
        return new List<CardReference>() {engineeringBayRef.researchStorefront[0],engineeringBayRef.researchStorefront[1] };
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
            case "Deploy Icarus Mortar":
                List<ChessboardPiece> mortarPlacement = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, selectedUnit.currentBoardPiece);
                for(int i = 0; i < mortarPlacement.Count; i++) { ChessboardPiece currentPiece = mortarPlacement[i]; if (currentPiece.hasChessPiece) { mortarPlacement.RemoveAt(i);i--; } }
                return mortarPlacement;
            case "Deploy Static Field Disruptor":
                List<ChessboardPiece> staticFieldRange = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, selectedUnit.currentBoardPiece);
                for(int i = 0; i < staticFieldRange.Count; i++)
                {
                    if (staticFieldRange[i].hasChessPiece) { staticFieldRange.Remove(staticFieldRange[i]);i--; }
                }
                return staticFieldRange;
            case "Deploy Electric Fence":
                //return MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, selectedUnit.currentBoardPiece);
                List<ChessboardPiece> electricFenceDeployRange = MainScript.currentBoard.GetAllPiecesWithinDistance(8, false, selectedUnit.currentBoardPiece);
                for(int i = 0; i < electricFenceDeployRange.Count; i++)
                {
                    if (electricFenceDeployRange[i].hasChessPiece) { electricFenceDeployRange.RemoveAt(i);i--; }
                }
                return electricFenceDeployRange;
        }

        return tempList;
    }
    public List<EventListener> GetEventListenerForUnit(UnitReference u)
    {
        switch (u.unitName)
        {
            case "Static Field Disruptor":
                return new List<EventListener>() { new EventListener(u, new List<GameEventType>(){ GameEventType.throwGrenade }) };
            case "Cave":
                return new List<EventListener>() { new EventListener(u, new List<GameEventType>() { GameEventType.beginTurn }) };
        }
        return new List<EventListener>();
    }
    public List<GameEvent> ThrowAMoon(Chesspiece unit)
    {
        int unitRow = unit.currentBoardPiece.yPos;
        List<ChessboardPiece> allPossibleTargets = new List<ChessboardPiece>();
        for (int x = 0; x < MainScript.currentBoard.width; x++)
        {
            if ((unitRow + 3) < MainScript.currentBoard.height) { allPossibleTargets.Add(MainScript.currentBoard.chessboardPieces[x, unitRow + 3]); }
            if ((unitRow + 4) < MainScript.currentBoard.height) { allPossibleTargets.Add(MainScript.currentBoard.chessboardPieces[x, unitRow + 4]); }
            if ((unitRow + 5) < MainScript.currentBoard.height) { allPossibleTargets.Add(MainScript.currentBoard.chessboardPieces[x, unitRow + 5]); }

        }
        List<ChessboardPiece> randomlyTargetedSpaces = new List<ChessboardPiece>();
        EventListener tempListen = new EventListener(unit, new List<GameEventType>() { GameEventType.endTurn });
        tempListen.destroyOnActivation = true;
        unit.listeners.Add(tempListen);
        MainScript.allEventListeners.Add(tempListen);
        int piecesAdded = 0;
        while (piecesAdded < 10 && allPossibleTargets.Count != 0)
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
    public List<GameEvent> DeployWalkingMine(ChessboardPiece target, Player owns)
    {
        return new List<GameEvent>() { GameEvent.GetSummonEvent(engineeringBayRef.cardlessUnitLibrary[1], target, owns) };
    }
    public List<GameEvent> PushUnitsOutward(Chesspiece walkingMine)
    {
        return new List<GameEvent>(){MainScript.currentBoard.GetPushAwayEventFromOrigin(walkingMine.currentBoardPiece, 4, true)};
    }
    public List<GameEvent> ItsRainingBarrels(Card c)
    {
        List<GameEvent> events = new List<GameEvent>();
        List<ChessboardPiece> tempList = MainScript.currentBoard.GetRandomBoardPiece(false, true, 3);
        events.Add(new GameEvent(c.reference, tempList, MainScript.neutralPlayer));
        return (events);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
