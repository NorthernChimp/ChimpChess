using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public static ChimpChessGame currentGame;
    public static ChimpChessBoard currentBoard;
    public GameObject fenceBeamPrefab;
    public GameObject boardPiecePrefab;
    public GameObject cardPrefab;
    public GameObject boardPrefab;
    public GameObject playerAvatarPrefab;
    public GameObject emptyReadableCardPrefab;
    public GameObject endTurnButtonPrefab;
    public GameObject projectilePrefab;
    public GameObject artilerryTarget;
    public GameObject cardNumberPrefab;
    public GameObject deckHolderPrefab;
    public GameObject hideCardsButtonPrefab;
    public List<Sprite> numberSprites;
    public List<Color> playerColors;
    public List<Card> cardsPlayerIsChoosingFrom;
    public List<Vector3> cardPositions;

    public Transform backGround;
    public Transform foreGround;

    public Color canPlaySpaceColor;//for spaces you can play a card on
    public Color withinRangeSpaceColor;//for spaces you're using to illustrate attack range, or any ranged effect
    public Color couldPlayButDoesNotMeetCriteria;//for spaces that hypothetically a card could be played, but some alterior criteria is not met, i.e. you want to throw a unit, but there are no targets. you could throw the unit, but it could not be played. this color indicates that

    bool hideCardsPlayerIsChoosingFrom = false;
    public DeckHolderScript playerDeck;
    public DeckHolderScript playerDiscard;
    public GameObject basicUnitPrefab;
    Vector3 centerOfCameraFocus = Vector3.zero;
    public float currentCameraZoomDistance = 10f;
    public Transform canvas;
    public static Transform theCanvas;
    public BasicSpellScript spellScript;
    public static BuildingReference nullBuildingReference;
    public static float distanceBetweenBoardPieces = 1f;
    public static float distanceBetweenUnitAndBoardPiece = 0.15f;
    public static float distanceBetweenCardsInHand = 0.1f;
    public static float cardHeight;
    public static float defaultCardSpeed = 0f;
    bool waitingForPlayerInput = false;
    float cardChoosingOffSet = 0f;
    bool keepChosenCard = false;
    bool justViewingCards = false;
    float mouseXWhenSelecting = 0f;
    float totalCardDistance = 0f;
    public bool hasLiftedSinceLeftClick = false;
    public bool selectingOnPlayEffectTarget = false;
    public bool enemyHasPlayed = false;
    public bool enemyHasSummonedUnits = false;
    public bool enemiesHaveAttacked = false;
    public List<Chesspiece> enemyUnitsWaitingToAttack;
    public static EndTurnButtonScript endTurnButton;
    public static Color invisibleColor;
    public Color boardColor;
    public static MeshRenderer boardRender;
    public bool currentCardHasTarget = false;
    public static Vector3 halfABoard = new Vector3(distanceBetweenBoardPieces * 0.5f, 0f, distanceBetweenBoardPieces * 0.5f);
    public static Vector3 cardScale;
    public static Vector3 cardNumberScale = new Vector3(0.25f, 0.25f, 1f);
    public Vector3 cameraDirectionFromFocus = new Vector3(0f, 0.5f, -0.5f);
    public float verticalAngleInRadians = 0f;
    public float horizontalAngleInRadians = 0f;
    public Vector2 cameraHorizontalAngle = new Vector2(0f, -1f);//the y is the z value essentially
    public Vector2 cameraVerticalAngle = new Vector2(0f, 0.5f);
    public static Vector3 unitPositionRelativeToBoardPiece = new Vector3(0f, distanceBetweenUnitAndBoardPiece, 0f);
    public static Player neutralPlayer = new Player(2);
    public static UnitReference nullUnitReference = new UnitReference("nullUnit", 0, 0, 0, 0, "none",0, ChessPieceMovementAbilityType.none,0,"nullCard",AttackType.shoot);
    public static CardReference nullCardReference = new CardReference(0, 0, 0, 0, CardTargetType.requiresNoTarget, CardType.nullCard, "null card","none");
    public static ChessboardPiece nullBoardPiece;
    public static Chesspiece nullUnit;
    public static Chesspiece currentAIUnit;
    public static EventListener nullEventListener = new EventListener();
    public static Card nullCard; 
    public static Card currentlySelectedCard = nullCard;
    public static Card moveUnitCard;
    public static Card attackUnitCard;
    public static List<Card> currentUnitCards;
    public static List<Chesspiece> allUnits;
    public static List<Vector2> currentUnitCardPositions;
    public static List<ChessboardPiece> possibleUnitMoveLocations;
    public static List<ChessboardPiece> possibleAttackLocations;
    public static List<ChessboardPiece> possibleOnPlayTargets;
    public static List<ChessboardPiece> boardPiecesWithTransparentRenderActive;
    public static List<EventListener> allEventListeners;
    public static List<Transform> projectileTransforms;
    public static List<Transform> nonAnimatedProjectileTransforms;
    public static List<FenceBeam> fenceBeams;
    public static List<CardNumber> transformShiftingCardNumbers;
    public static List<Transform> artilleryTargetTransforms;
    public static List<Transform> spellTransforms;
    public static List<Chesspiece> unitsToReorient;
    public static Vector3 positionOfPlayedCard = Vector3.zero;
    public static ChessboardPiece currentlySelectedTarget;
    public static ChessboardPiece secondarySelectedTarget;
    public static Chesspiece currentlySelectedUnit;
    public static EmptyReadableCardPrefabScript leftEmptyCard;
    public static EmptyReadableCardPrefabScript rightEmptyCard;
    public static bool mainMenuIsOpen = false;
    public static bool animating = false;
    HideCardsButtonScript hidebutton;
    public Counter animationCounter;
    public List<GameEvent> queue = new List<GameEvent>();
    public List<GameEvent> executedEvents = new List<GameEvent>();
    // Start is called before the first frame update
    void Start()
    {
        transformShiftingCardNumbers = new List<CardNumber>();
        cardsPlayerIsChoosingFrom = new List<Card>();
        fenceBeams = new List<FenceBeam>();
        //nullCard =  new Card(nullCardReference, neutralPlayer);
        
        nullBuildingReference = new BuildingReference();
        nullUnit = new Chesspiece();
        nullUnit.unitRef = nullUnitReference;
        nullUnit.unitRef.buildingRefParent = nullBuildingReference;
        nullUnitReference.cardReference = nullCardReference;
        nullCardReference.buildingRefParent = nullBuildingReference;
        unitsToReorient = new List<Chesspiece>();
        nonAnimatedProjectileTransforms = new List<Transform>();
        artilleryTargetTransforms = new List<Transform>();
        projectileTransforms = new List<Transform>();
        enemyUnitsWaitingToAttack = new List<Chesspiece>();
        horizontalAngleInRadians *= Mathf.PI;
        verticalAngleInRadians *= Mathf.PI;
        boardPiecesWithTransparentRenderActive = new List<ChessboardPiece>();
        allEventListeners = new List<EventListener>();
        possibleOnPlayTargets = new List<ChessboardPiece>();
        spellTransforms = new List<Transform>();
        //Instantiate(cardPrefab,  Vector3.zero,Quaternion.identity).transform.SetParent(canvas);
        possibleAttackLocations = new List<ChessboardPiece>();
        possibleUnitMoveLocations = new List<ChessboardPiece>();
        theCanvas = canvas;
        hidebutton = Instantiate(hideCardsButtonPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0f), Quaternion.identity, theCanvas).transform.GetComponent<HideCardsButtonScript>();
        
        spellScript = GetComponent<BasicSpellScript>();
        leftEmptyCard = Instantiate(emptyReadableCardPrefab, new Vector3(Screen.width * 0.125f, Screen.height * 0.665f, 0f), Quaternion.identity,backGround).transform.GetComponent<EmptyReadableCardPrefabScript>() ;
        rightEmptyCard = Instantiate(emptyReadableCardPrefab, new Vector3(Screen.width * 0.875f, Screen.height * 0.665f, 0f), Quaternion.identity,backGround).transform.GetComponent<EmptyReadableCardPrefabScript>() ;
        leftEmptyCard.otherCard = rightEmptyCard;
        rightEmptyCard.otherCard = leftEmptyCard;
        //endTurnButton = Instantiate()
        invisibleColor = new Color(0f, 0f, 0f, 0f);
        nullBoardPiece = new ChessboardPiece(transform);
        allUnits = new List<Chesspiece>();
        currentlySelectedTarget = nullBoardPiece;
        currentlySelectedCard = nullCard;
        currentlySelectedUnit = nullUnit;
        secondarySelectedTarget = nullBoardPiece;
        cardHeight = ((Screen.height / 4f) * 0.01f);
        CardNumber.distanceBetweenNumbers = ((100f * cardHeight)/Screen.width) * 2.75f;
        positionOfPlayedCard = new Vector3(Screen.width * 0.45f, 0f, 1f);
        defaultCardSpeed = Screen.height * 0.35f;
        distanceBetweenCardsInHand = cardHeight * 0.65f;
        cardScale = new Vector3(cardHeight, cardHeight, 1f);
        rightEmptyCard.SetupReadableCard();
        leftEmptyCard.SetupReadableCard();
        SetupLibrary();
        hidebutton.render.enabled = false;
        SetupGame();
        hidebutton.transform.localScale = new Vector3(cardScale.x , cardScale.y * 0.5f, cardScale.z);
        currentUnitCards = new List<Card>();
        ChangeUnitCardsEnabled(false);
        endTurnButton = Instantiate(endTurnButtonPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.925f, 0f), Quaternion.identity, foreGround).transform.GetComponent<EndTurnButtonScript>();
        endTurnButton.SetupEndturnButton();
        //currentBoard.chessboardPieces[0, 0].CreateScienceSymbol();
    }
    void SetupLibrary()
    {
        GetComponent<LibraryScript>().CreateLibrary();
    }
    public static Vector2 RotateVector(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    void SetupGame()
    {
        
        currentGame = new ChimpChessGame(2);
        
        foreach (Player p in currentGame.allPlayers)
        {
            Vector3 topRightCorner = new Vector3(Screen.width * 0.605f, Screen.height * 0.925f, 0f);//, Quaternion.identity,canvas).transform.GetComponent<EmptyReadableCardPrefabScript>();
            if (p.theType != PlayerType.localHuman)
            {
                
                p.aiComponent = new EnemyAI();
                p.aiComponent.SetupEnemyAi();
                p.aiComponent.spellScript = spellScript;
                p.aiComponent.thisPlayer = p;
                spellScript.enemy = p.aiComponent;
                CreateEnemyDeck(p);
                //Debug.Log(LibraryScript.enemyBuildingRef.unitAbilityLibrary.Count);
                //Debug.Log(LibraryScript.enemyBuildingRef.unitAbilityLibrary[0].reference.unitName);
                p.playerAvatar = Instantiate(playerAvatarPrefab, Vector3.zero + (Vector3.forward * 5.5f), Quaternion.LookRotation(Vector3.forward * -1f, Vector3.up)).transform;
                p.playerAvatar.GetComponent<OtherPlayerAvatarScript>().thisPlayer = p;
                topRightCorner = new Vector3(0.395f * Screen.width, topRightCorner.y, topRightCorner.z);
            }
            else
            {
                
            }
            Transform tr = Instantiate(cardNumberPrefab, topRightCorner, Quaternion.identity, canvas).transform;
            CardNumberScript s = tr.GetComponent<CardNumberScript>();
            p.bank = s.SetupNumber(0);
            if (p.theType != PlayerType.localHuman)
            {
                p.bank.SetColor(Color.red);
            }
            else
            {
                p.bank.SetColor(Color.green);
            }
                s.ChangeSprite(3);
            tr.localScale = MainScript.cardNumberScale * 3.75f;
            p.bank.SetOriginalScale();
        }

        Transform tempTran = Instantiate(deckHolderPrefab, new Vector3(Screen.width * 0.925f, Screen.height * 0.125f, 3f), Quaternion.identity, foreGround).transform;
        playerDeck = tempTran.GetComponent<DeckHolderScript>();
        tempTran.localScale = cardScale;
        tempTran = Instantiate(deckHolderPrefab, new Vector3(Screen.width * 0.075f, Screen.height * 0.125f, 3f), Quaternion.identity, foreGround).transform;
        playerDiscard = tempTran.GetComponent<DeckHolderScript>();
        tempTran.localScale = cardScale;
        int width = 8;
        int height = 8;
        Transform t = Instantiate(boardPrefab, new Vector3(0f, -0.2f, 0f), Quaternion.identity).transform;
        t.localScale = new Vector3(width + 0.5f, 0.2f, height + 0.5f);
        boardRender = t.GetComponent<MeshRenderer>();
        boardRender.material.color = boardColor;
        Vector3 boardOrigin = new Vector3((float)width * -0.5f * distanceBetweenBoardPieces, 0f, ((float)height * -0.5f * distanceBetweenBoardPieces));
        currentBoard = new ChimpChessBoard(width, height, boardOrigin, boardPiecePrefab);
        queue = new List<GameEvent>();
        //queue = new List<GameEvent>() { new GameEvent(currentGame.allPlayers[1], 7, GameEventType.drawCard) , new GameEvent(currentGame.allPlayers[0], 7, GameEventType.drawCard) };
        
        for (int i = 0; i < 2; i++)
        {
            CardReference tempRef = LibraryScript.attackUnit ;
            if (i == 0) { tempRef = LibraryScript.moveUnit; }
            Transform temp = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity).transform;
            temp.SetParent( canvas);
            CardScript tempScript = temp.GetComponent<CardScript>();
            tempScript.SetupCard(new Card(tempRef, MainScript.neutralPlayer), MainScript.neutralPlayer,true);
            tempScript.thisCard.cardSpriteName = tempRef.cardSpriteName;
            if (i == 0) { MainScript.moveUnitCard = tempScript.thisCard; } else { MainScript.attackUnitCard = tempScript.thisCard; }
        }

        for (int i = 0; i < currentBoard.width; i++)
        {
            ChessboardPiece bottomPiece = currentBoard.chessboardPieces[i, 0];
            bottomPiece.ChangeOwner(currentGame.allPlayers[0], playerColors[currentGame.allPlayers[0].playerNumber]);
            ChessboardPiece topPiece = currentBoard.chessboardPieces[i, currentBoard.height - 1];
            topPiece.ChangeOwner(currentGame.allPlayers[1], playerColors[currentGame.allPlayers[1].playerNumber]);
        }
        //playerDeck.SayHello();
        
        //RectTransform rt = tempTran.GetComponent<RectTransform>().
        //tempTran.localPosition = new Vector3(tempTran.localPosition.x, tempTran.localPosition.y, 3f);
        

        //playerDeck.SetupDeck(DeckHolderType.discardPile, new List<Card>());
        playerDeck.SetupDeck(DeckHolderType.deck, currentGame.allPlayers[0].deck.cardsInDeck);
        playerDiscard.SetupDeck(DeckHolderType.discardPile, currentGame.allPlayers[0].discardPile.cardsInDiscard);
        
        //queue = new List<GameEvent>() { new GameEvent(new UnitReference(1, 1, 1, 1, ), new List<ChessboardPiece> { currentBoard.chessboardPieces[0, 0], currentBoard.chessboardPieces[0, 1], currentBoard.chessboardPieces[0, 2] }, currentGame.allPlayers[0]), new GameEvent(GameEventType.moveUnit, new List<ChessboardPiece>() { currentBoard.chessboardPieces[0, 1] }, new List<ChessboardPiece>() { currentBoard.chessboardPieces[1, 1] }, currentGame.allPlayers[0]) };
        StartGame();
        queue.Add(new GameEvent(GameEventType.beginGame));
        queue.Add(new GameEvent(GameEventType.beginTurn));
        
    }
    void CreateEnemyDeck(Player p)
    {
        /*LibraryScript.enemyBuildingRef = EnemyAI.enemyReference;
        for(int i = 0; i < EnemyAI.enemyReference.unitLibrary.Count; i++)
        {
            UnitReference u = EnemyAI.enemyReference.unitLibrary[i];
            p.deck.cardsInDeck.Add(new Card(u.cardReference, p));
        }
        for (int i = 0; i < EnemyAI.enemyReference.spellLibrary.Count; i++)
        {
            CardReference c = EnemyAI.enemyReference.spellLibrary[i];
            p.deck.cardsInDeck.Add(new Card(c, p));
        }
        for(int i = 0; i < EnemyAI.enemyReference.buildingLibrary.Count; i++)
        {
            CardReference c = EnemyAI.enemyReference.buildingLibrary[i].cardReference;
            p.deck.cardsInDeck.Add(new Card(c,p));
        }*/
        p.deck.cardsInDeck = new List<Card>()
        {
            new Card(BarracksScript.barracksRef.unitLibrary[0].cardReference,p),
            new Card(BarracksScript.barracksRef.unitLibrary[0].cardReference,p),
            //new Card(BarracksScript.barracksRef.unitLibrary[1].cardReference,p),
            new Card(BarracksScript.barracksRef.unitLibrary[2].cardReference,p),
            new Card(BarracksScript.barracksRef.unitLibrary[2].cardReference,p),
            //new Card(BarracksScript.barracksRef.unitLibrary[2].cardReference,p),
            //new Card(LibraryScript.enemyBuildingRef.unitLibrary[0].cardReference,p),
            //new Card(LibraryScript.enemyBuildingRef.unitLibrary[0].cardReference,p),
            //new Card(LibraryScript.enemyBuildingRef.unitLibrary[1].cardReference,p),
            //new Card(LibraryScript.enemyBuildingRef.unitLibrary[1].cardReference,p),
            new Card(HospitalScript.hospitalRef.unitLibrary[0].cardReference,p),
            //new Card(HospitalScript.hospitalRef.unitLibrary[0].cardReference,p),
            //new Card(HospitalScript.hospitalRef.unitLibrary[0].cardReference,p),
            //new Card(BarracksScript.barracksRef.spellLibrary[3],p),
            //new Card(BarracksScript.barracksRef.spellLibrary[3],p),
            //new Card(HospitalScript.hospitalRef.spellLibrary[0],p),
            //new Card(HospitalScript.hospitalRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[1],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[1],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[1],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[1],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[1],p),
            new Card(EngineeringBayScript.engineeringBayRef.unitLibrary[1],p),
            /*new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),*/
            //new Card(EngineeringBayScript.engineeringBayRef.spellLibrary[0],p),
            new Card(LibraryScript.buildingLibrary[0].cardReference,p),
            new Card(LibraryScript.buildingLibrary[0].cardReference,p),
            //new Card(LibraryScript.buildingLibrary[1].cardReference,p),
            new Card(LibraryScript.buildingLibrary[2].cardReference,p),
            new Card(LibraryScript.buildingLibrary[2].cardReference,p),
            new Card(LibraryScript.buildingLibrary[3].cardReference,p)
        };
    }
    void StartGame()
    {
        currentGame.isActive = true;
        currentGame.isPaused = false;
        currentGame.currentPlayer = currentGame.allPlayers[0];
    }
    void UpdateFenceBeams()
    {
        List<FenceBeam> beamsToDestroy = new List<FenceBeam>();
        foreach(FenceBeam f in MainScript.fenceBeams)
        {
            f.UpdateFenceBeam();
        }
    }
    public List<ChessboardPiece> GetSpacesTraversedFromMoveEvent(GameEvent e)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        int xDiff = e.theTarget[0].xPos - e.theActor[0].xPos;
        int yDiff = e.theTarget[0].yPos - e.theActor[0].yPos;
        int distanceTravelled = 0;
        if(xDiff == 0) { distanceTravelled = yDiff; } else if(yDiff == 0) { distanceTravelled = xDiff; } else
        {
            distanceTravelled = Mathf.Abs(xDiff);
        }
        xDiff = (int)Mathf.Sign(xDiff);
        yDiff = (int)Mathf.Sign(yDiff);
        //Debug.Log("geting spaces Traversed " + xDiff + "  is xdiff and " + yDiff + " is ydiff");
        tempList = currentBoard.GetAllPiecesFromOriginInDirection(e.theActor[0], new Vector2(xDiff, yDiff), false, true, distanceTravelled, e.theActor[0].currentChessPiece.owner);
        return tempList;
    }
    public List<ChessboardPiece> GetPossibleBuildingLocationsForPlayer(Player p)
    {
        List<ChessboardPiece> checkedPieces = new List<ChessboardPiece>();
        List<ChessboardPiece> immediateNeighbours = new List<ChessboardPiece>();
        foreach(ChessboardPiece c in p.ownedBoardPieces)
        {
            List<ChessboardPiece> currentNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, c);
            foreach(ChessboardPiece b in currentNeighbours)
            {
                if (!immediateNeighbours.Contains(b) && !p.ownedBoardPieces.Contains(b))
                {
                    immediateNeighbours.Add(b);
                }
            }
        }
        /*foreach(ChessboardPiece c in immediateNeighbours)//creates a larger range of 2 spaces
        {
            List<ChessboardPiece> currentNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, c);
            foreach (ChessboardPiece b in currentNeighbours)
            {
                if (!checkedPieces.Contains(b) && !p.ownedBoardPieces.Contains(b)&& !immediateNeighbours.Contains(b))
                {
                    checkedPieces.Add(b);
                }
            }
        }*/
        checkedPieces.AddRange(immediateNeighbours);
        return checkedPieces;
    }
    void UpdatePoisonBeginningOfTurn()
    {
        List<ChessboardPiece> piecesWithPoison = new List<ChessboardPiece>();
        foreach (ChessboardPiece c in currentBoard.chessboardPieces) { if (c.hasToxicCloud) { piecesWithPoison.Add(c); } }
        foreach (ChessboardPiece c in piecesWithPoison)
        {
            //c.SetupGasCloud(-1);
        }
    }
    void ExecuteTheQueue()
    {
        if (!waitingForPlayerInput) 
        {
            if (queue.Count > 0)
            {
                GameEvent currentEvent = queue[0];
                bool canEventExecute = CanWeExecuteThisEvent(currentEvent);//check if the event is possible
                if (canEventExecute)//it is possible and should be executed
                {
                    List<EventListener> listenersWithMatchingEventTypes = new List<EventListener>();//make a list for any listeners that may be activated
                    foreach (EventListener l in allEventListeners)//for each of all the listeners
                    {
                        //Debug.Log("checking listener " + l.listenerName);
                        foreach (GameEventType t in l.typeOfEventToListenFor)//for all the eventtypes they listen for
                        {
                            if (t == currentEvent.theType)//if that eventtype is the current events type
                            {
                                if (!listenersWithMatchingEventTypes.Contains(l)) { listenersWithMatchingEventTypes.Add(l); };//add it to the list if it isn't already there to check if the listener conditions are met
                            }
                        }
                    }
                    bool hasQueueBeenInterupted = false;//nominally the queue is not interupted
                    foreach (EventListener l in listenersWithMatchingEventTypes)//for any of the listeners with this events gameeventtype
                    {
                        //Debug.Log("matching event listener game event type " + l.listenerName);
                        if (!l.disabledUntilEndOfQueue)//if its been disabled until the queue finished dont use the listener
                        {
                            if (spellScript.DoesThisListenerActivate(l, queue)) { hasQueueBeenInterupted = true; Debug.Log("the queue has been interupted by " + l.listenerName); }//if it returns true from the spell script it has reorganized the queue thus interupting it
                        }
                    }
                    if (!hasQueueBeenInterupted)//if the queue was not interupted proceed normally
                    {
                        bool hasBeenPreponed = false;
                        for(int i = 0; i < listenersWithMatchingEventTypes.Count;i++)
                        {
                            EventListener l = listenersWithMatchingEventTypes[i];
                            if (!l.disabledUntilEndOfQueue)//if its been disabled until the queue finished dont use the listener
                            {
                                List<EventListener> listeners = new List<EventListener>() { l };
                                if (l.poolAllListenersTogether)
                                {
                                    for(int ii = i; ii < listenersWithMatchingEventTypes.Count; ii++)//presumably we pool them on the first instance of the listeners we get so we dont need to check previous listeners
                                    {
                                        EventListener currentListen = listenersWithMatchingEventTypes[ii];//if this listener has the same name as the original one we are pooling it with then it must be added to the pool
                                        if(currentListen.listenerName == l.listenerName) { listeners.Add(currentListen);listenersWithMatchingEventTypes.Remove(currentListen);ii--; }
                                    }
                                }
                                if (spellScript.DoesExecutingThisEventActivateListenever(listeners, queue)) { hasBeenPreponed = true; }//pretty sure I wont ever need to change this so another event can cancel the queue but the possibility is there
                            }
                        }
                        if (!hasBeenPreponed) { ExecuteEvent(currentEvent); } else { ExecuteTheQueue(); }
                    }
                    else //if it has re execute the queue;
                    {

                        ExecuteTheQueue();
                    }

                }
                else
                {
                    Debug.Log("removing event cannot execute" + currentEvent.theType);
                    queue.Remove(currentEvent);
                    ExecuteTheQueue();
                }
            }
            else //there is nothing left in the queue
            {
                foreach (EventListener l in allEventListeners) { l.disabledUntilEndOfQueue = false; }
                if (currentGame.currentPlayer.theType == PlayerType.localHuman)// if its a local human simply unpause and wait input, they will end turn when ready
                {
                    //Debug.Log("should be local ending turn");
                    DeselectEverything();
                    currentGame.isPaused = false;
                }
                else//it is not the local human so it must be an AI, later it may be a proxy human and wait for input, but for now its an AI, and if all their Cards have been played and the queue has finished simply end their turn
                {
                    queue.AddRange(currentGame.currentPlayer.aiComponent.GetNextAction2());
                    ExecuteTheQueue();
                }
                //currentGame.currentPlayer.aiComponent.


                /*
                //Debug.Log("nothing left in queue and no human is current player");
                if (!enemyHasSummonedUnits)
                {
                    //Debug.Log("have not summoned units, setting that up player type is " + currentGame.currentPlayer.theType);
                    ExecuteAITurn(currentGame.currentPlayer);
                    enemyHasSummonedUnits = true;
                } 
                if(queue.Count == 0)
                {
                    if (!enemiesHaveAttacked)
                    {
                        //Debug.Log("have not set up units to attack, setting that up");
                        enemyUnitsWaitingToAttack = new List<Chesspiece>();
                        enemyUnitsWaitingToAttack.AddRange(currentGame.currentPlayer.currentUnits);
                        //Debug.Log(enemyUnitsWaitingToAttack.Count);
                        enemiesHaveAttacked = true;
                    }
                    else
                    {
                        //Debug.Log("we are stuck here " + enemyUnitsWaitingToAttack.Count);
                        if (enemyUnitsWaitingToAttack.Count > 0)
                        {
                            //Debug.Log("getting a random unit waiting to attack");
                            int randomInt = (int)Random.Range(0, enemyUnitsWaitingToAttack.Count);
                            Chesspiece randomUnit = enemyUnitsWaitingToAttack[randomInt];
                            spellScript.ActivateEnemyAI2(randomUnit, queue);
                            enemyUnitsWaitingToAttack.Remove(randomUnit);
                        }
                        else
                        {
                            //Debug.Log("there are no units left");
                            enemiesHaveAttacked = true;
                            enemyHasPlayed = true;
                            queue.Add(new GameEvent(GameEventType.endTurn));
                        }
                    }
                    
                }

                if (enemyUnitsWaitingToAttack.Count > 0)
                {
                    //Debug.Log("getting a random unit waiting to attack");
                    int randomInt = (int)Random.Range(0, enemyUnitsWaitingToAttack.Count);
                    Chesspiece randomUnit = enemyUnitsWaitingToAttack[randomInt];
                    //Debug.Log(randomUnit.transform.name + "but down here " + randomUnit.movementType);
                    spellScript.ActivateEnemyAI2(randomUnit, queue);
                    enemyUnitsWaitingToAttack.Remove(randomUnit);
                }

                //EndTurn();
//                Debug.Log("queue count before setting to false " + queue.Count);
                if (queue.Count == 0) { currentGame.isPaused = false; } else { ExecuteTheQueue(); }
            }
            */
            }
        }
        
    }
    public static List<ChessboardPiece> GetSpacesAlongLineBetweenTwoSpaces(ChessboardPiece origin, ChessboardPiece endPoint,bool includeEndPoint,bool moveOverUnits)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        Vector2 direction = (new Vector2(endPoint.xPos, endPoint.yPos) - new Vector2(origin.xPos, origin.yPos) ).normalized;
        Vector2 absoluteDirection = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        int currentX = origin.xPos;
        int currentY = origin.yPos;
        Vector2 position = Vector2.zero;
        float xDistance = 0.5f;
        float yDistance = 0.5f;
        //bool firstRound = true;
        int count = 0;
        int endX = endPoint.xPos;
        int endY = endPoint.yPos;
        //Debug.Log("we are setting up a spot with endpoint " + endX +" and " + endY);
        //if (includeEndPoint) { endX++;endY++; }
        bool hasReachedEnd = false;
        while((currentX != endX || currentY != endY ) && ((currentX >= 0 && currentX < currentBoard.chessboardPieces.GetLength(0) && currentY >= 0 && currentY < currentBoard.chessboardPieces.GetLength(1))) && !hasReachedEnd)
        {
            //Debug.Log("running through them count " + count);
            //if(currentX == endX) { Debug.Log("current x is end x"); }
            //if(currentY == endY) { Debug.Log("current y is end y"); }
            if (count > 0) 
            {
                if (!moveOverUnits)
                {
                    if(!currentBoard.chessboardPieces[currentX, currentY].hasChessPiece)//if the currentboardpiece doesn't have a minion and we aren't moving over units you can add it
                    {
                        //currentBoard.chessboardPieces[currentX, currentY].ChangeTransparentRenderColor(Color.magenta);
                        tempList.Add(currentBoard.chessboardPieces[currentX, currentY]);
                    }
                    else//if there is a unit there and we aren't moving over units we have reached the end
                    {
                        hasReachedEnd = true;
                    }

                }
                else { tempList.Add(currentBoard.chessboardPieces[currentX, currentY]); }

            }
            
            count++;
            float xDifference = 1f - xDistance;
            float yDifference = 1f - yDistance;
            float timeToX = xDifference/ absoluteDirection.x ;
            float timeToY = yDifference/ absoluteDirection.y ;
            //Debug.Log("legnth is " + currentBoard.chessboardPieces.GetLength(0));
            if (timeToX < timeToY)
            {
                currentX += (int)Mathf.Sign(direction.x);
                xDistance = 0f;
                yDistance += timeToX * absoluteDirection.y;
                
                //tempList.Add(currentBoard.chessboardPieces[currentX, currentY]);
            }
            else if (timeToY < timeToX)
            {
                yDistance = 0f;
                xDistance += timeToY * absoluteDirection.x;
                currentY += (int)Mathf.Sign(direction.y);
            }
            else if (timeToX == timeToY)
            {
                xDistance = 0f;
                yDistance = 0f;
                currentX += (int)Mathf.Sign(direction.x);
                currentY += (int)Mathf.Sign(direction.y);
            }
            //Debug.Log("current x is " + currentX + " and current y is " + currentY);
            if ((currentX >= 0 && currentX < currentBoard.chessboardPieces.GetLength(0) && currentY >= 0 && currentY < currentBoard.chessboardPieces.GetLength(1)))
            {
                //Debug.Log("adding");
                
            }
            else
            {
                //Debug.Log("done");
                
                //Debug.Log("nothing happens");
            }
            
        }
        //Debug.Log(direction);
        if (includeEndPoint) { tempList.Add(endPoint); }
        return tempList;
    }
    void InterceptGrenade(GameEvent e)
    {
        Chesspiece thrower = e.relevantUnits[0];
        Chesspiece interceptor = e.relevantUnits[1];
        ChessboardPiece target = e.theTarget[0];
        Vector3 direct = thrower.currentBoardPiece.transform.position - target.transform.position;
        Vector3 interceptDirect = (target.transform.position - interceptor.currentBoardPiece.transform.position);
        if (thrower.unitRef.cardReference.cardType != CardType.building) 
        { 
            thrower.SetupAnimation("throwing", e.animationTime);
            unitsToReorient.Add(thrower);
            thrower.transform.rotation = Quaternion.LookRotation(direct * -1, Vector3.up);
        }
        if(interceptor.unitRef.cardReference.cardType != CardType.building) 
        { 
            interceptor.SetupAnimation("shoot", e.animationTime);
            
            interceptor.transform.rotation = Quaternion.LookRotation(interceptDirect, Vector3.up);
            unitsToReorient.Add(interceptor);
            
        }
        Transform laserTransform = Instantiate(projectilePrefab, interceptor.currentBoardPiece.transform.position + (Vector3.up * distanceBetweenUnitAndBoardPiece), Quaternion.LookRotation(interceptDirect, Vector3.up)).transform;
        laserTransform.SendMessage("SetupProjectile", interceptDirect);
        projectileTransforms.Add(laserTransform);
        GameObject grenadePrefab = spellScript.GetProjectilePrefab(e.relevantCardReferences[0]);
        Transform tr = Instantiate(grenadePrefab, thrower.transform.position + (Vector3.up * MainScript.distanceBetweenBoardPieces), Quaternion.identity).transform;
        tr.SendMessage("SetupProjectile", direct * -1f);
    }
    void ThrowGrenade(GameEvent e)
    {
        Chesspiece thrower = e.relevantUnits[0];
        ChessboardPiece target = e.theTarget[0];
        thrower.SetupAnimation("throwing", e.animationTime);
        MainScript.unitsToReorient.Add(thrower);
        Debug.Log("target is " + target.xPos + " and " + target.yPos);
        Vector3 direct = thrower.currentBoardPiece.transform.position - target.transform.position;
        thrower.transform.rotation = Quaternion.LookRotation(direct * -1, Vector3.up);
        GameObject grenadePrefab = spellScript.GetProjectilePrefab(e.relevantCardReferences[0]);
        Transform tr = Instantiate(grenadePrefab, thrower.transform.position + (Vector3.up * MainScript.distanceBetweenBoardPieces), Quaternion.identity).transform;
        tr.SendMessage("SetupProjectile", direct * -1f);
        queue.AddRange(spellScript.GetGrenadeResult(e.relevantCardReferences[0], target));
    }
    void ChooseFromCardsEvent(GameEvent e)
    {
        
        if(e.targetPlayer.theType == PlayerType.localHuman)
        {
            cardsPlayerIsChoosingFrom = new List<Card>();
            cardPositions = new List<Vector3>();
            int numberOfCards = e.relevantCardReferences.Count;
            float distance = distanceBetweenCardsInHand * (numberOfCards - 1) * 50f;
            totalCardDistance = distance * 0.525f;
            Vector3 originPosition = new Vector3((Screen.width * 0.5f) - distance, Screen.height * 0.5f, 0f);
            for(int i = 0; i < numberOfCards;i++)
            {
                
                CardReference c = e.relevantCardReferences[i];
                Vector3 positionToInstantiate = originPosition + (Vector3.right * i * distanceBetweenCardsInHand * 100f);
                Transform t = Instantiate(cardPrefab, positionToInstantiate, Quaternion.identity,canvas).transform;
                CardScript tempScript = t.GetComponent<CardScript>();
                tempScript.SetupCard(new Card(c, e.targetPlayer),neutralPlayer, false);
                cardsPlayerIsChoosingFrom.Add(tempScript.thisCard);
                cardPositions.Add( positionToInstantiate);
                
            }
            if(numberOfCards > 0)
            {
                currentGame.isPaused = true;
                waitingForPlayerInput = true;
                cardChoosingOffSet = 0f;
                hidebutton.render.enabled = true;
            }
            if (e.theType != GameEventType.playerViewsCards)
            {
                GameEvent post = GameEvent.GetPlayerChoosesEvent(e.targetPlayer, e.relevantCards[0].cardReference);//for executing the result of choosing that card (put it into your hand, play it, shuffle it into your opponents deck etc)
                post.relevantUnits = e.relevantUnits;
                post.theTarget = e.theTarget;
                queue.Insert(1, post);
                justViewingCards = false;
            }
            else
            {
                justViewingCards = true;
            }
            
        }
        keepChosenCard = e.relevantBool;
    }
    void PlayerChoosesACard(GameEvent e)
    {
        hidebutton.render.enabled = false;
        ChessboardPiece primeTarget = MainScript.nullBoardPiece;
        ChessboardPiece secondaryTarget = MainScript.nullBoardPiece;
        Chesspiece selectedOrTargetedUnit = MainScript.nullUnit;
        if(e.theTarget.Count > 0)
        {
            primeTarget = e.theTarget[0];
            if(e.theTarget.Count > 1)
            {
                secondaryTarget = e.theTarget[1];
            }
        }
        if(e.relevantUnits.Count > 0) { selectedOrTargetedUnit = e.relevantUnits[0]; }
        queue.AddRange(spellScript.GetChooseCardEvent(e.relevantCards[0], e.relevantCardReferences[0], primeTarget, secondaryTarget,selectedOrTargetedUnit, e.targetPlayer));
    }
    void DrawCard(GameEvent e)
    {
        if (e.targetPlayer.theType == PlayerType.localHuman) { rightEmptyCard.AppearAsPlayer(e.targetPlayer); rightEmptyCard.ShowNotifierOnly(AbilityNotificationState.drawCard, Color.green); } else { leftEmptyCard.AppearAsPlayer(e.targetPlayer); leftEmptyCard.ShowNotifierOnly(AbilityNotificationState.drawCard, Color.red); }
        if (e.targetPlayer.theType == PlayerType.localHuman)
        {
            if (e.utility > e.targetPlayer.deck.cardsInDeck.Count)
            {
                int remainder = e.utility - e.targetPlayer.deck.cardsInDeck.Count;
                e.utility = e.targetPlayer.deck.cardsInDeck.Count;
                //Debug.Log("trying to draw too many addingReshuffle");
                if (e.targetPlayer.discardPile.cardsInDiscard.Count > 0)
                {
                    //Debug.Log("we have cards in discard to reshuffle");
                    queue.Insert(1, new GameEvent(e.targetPlayer, 1, GameEventType.reshuffleDeck));//inserting reshuffle
                    queue.Insert(2, new GameEvent(e.targetPlayer, remainder, GameEventType.drawCard));//drawing the remainder of the cards
                }
                else
                {
                    Debug.Log("we DO NOT have cards in discard to reshuffle");
                }
            }//if you are trying to draw more cards than are in the deck set the utility to match the number of cards it can draw
            for (int i = 0; i < e.utility; i++)
            {
                Transform t = Instantiate(cardPrefab, playerDeck.transform.position, Quaternion.identity).transform;
                t.SetParent(canvas);
                CardScript tempScript = t.GetComponent<CardScript>();
                //Debug.Log(e.targetPlayer.playerNumber);
                tempScript.SetupCard(e.targetPlayer.deck.GetRandomCard(), e.targetPlayer, false);
            }
            playerDeck.SetupCardBacks();
        }
        else
        {
            e.targetPlayer.playerAvatar.SendMessage("DrawCard", e.utility);
        }
    }
    void PlayCardFromEvent(GameEvent e)
    {
        Card relevantCard = e.relevantCards[0];

        //Debug.Log("the owner is " + relevantCard.owner.playerNumber + "and type " + relevantCard.owner.theType);
        
        //Debug.Log("playing card " + relevantCard.cardName);// + " it has target type " + relevantCard.targetType + " and apply on play effect is " + relevantCard.reference.applyOnPlayEffect + " purchase card : " + relevantCard.isPurchaseCard + " and research card? : " + relevantCard.isResearchableCard + " and the owner is type " + relevantCard.owner);
        //Debug.Log("playing card " + relevantCard.cardName + " it has target type " + relevantCard.targetType);
        if(relevantCard.cost > 0 && relevantCard != moveUnitCard && relevantCard != attackUnitCard)
        {
            //Debug.Log(relevantCard.cardName + " is making a transform shift start");
            e.targetPlayer.resources -= relevantCard.cost;
            e.targetPlayer.bank.ReturnToOriginalScale();
            e.targetPlayer.bank.SetInt(e.targetPlayer.resources);
            e.targetPlayer.bank.SetupCardNumberTransformShift(CardNumberTransformShiftType.pulsate, 3f, Mathf.PI * 1.85f);
        }
        
        //if(e.targetPlayer.theType == PlayerType.localHuman) { playerin}
        if(e.targetPlayer.resources < 0) { e.targetPlayer.resources = 0; }
        //if (!relevantCard.isPurchaseCard) { Debug.Log("playing card " + relevantCard.cardName); } else { Debug.Log("shuffling card " + relevantCard.cardName + " into discard"); }
        if (relevantCard.targetType != CardTargetType.requiresNoTarget && currentlySelectedTarget != nullBoardPiece) { currentlySelectedTarget.ChangeTransparentRenderColor(Color.blue); }
        if (relevantCard.cardType != CardType.moveUnit && relevantCard.cardType != CardType.attackUnit)
        {
            if (e.relevantCards[0].owner.theType == PlayerType.localHuman)
            {
                rightEmptyCard.AppearAsThisCard(relevantCard.cardReference,relevantCard.owner);
                if (!relevantCard.isPurchaseCard)
                {
                    rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.playCard, Color.green);
                }
                else
                {
                    rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.discarding, Color.green);
                }

            }
            else
            {
                leftEmptyCard.AppearAsThisCard(relevantCard.cardReference,relevantCard.owner);
                if (!relevantCard.isPurchaseCard) { leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.playCard, Color.red); }
                else { leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.upgrading, Color.red); }
            }
            if (e.targetPlayer.theType == PlayerType.localHuman)
            {
                //Debug.Log("we get in here adding to player discard "+ e.targetPlayer.theType);
                //e.relevantCards[0].transform.SendMessage("MakeInvisible");//cards also have an actual "makeinvisble" function now
                //if(relevantCard.)
                if (!relevantCard.hasNoTransform && !relevantCard.isPurchaseCard && !relevantCard.isResearchableCard ) 
                { 
                    playerDiscard.cardsIncoming.Add(relevantCard); 
                }
                
                e.targetPlayer.hand.cardsInHand.Remove(relevantCard);
                rightEmptyCard.notifier.SetupColor(Color.green);
            }
            else
            {
                //Debug.Log("we are here right playing " + relevantCard.cardName + " and type player " + e.targetPlayer.theType + " cards in hand is " + e.targetPlayer.hand.cardsInHand.Count + " it is a purchase card " + relevantCard.isPurchaseCard);
                //Debug.Log(e.targetPlayer.hand.cardsInHand.Count + " count first " + relevantCard.cardName + " purchase " + relevantCard.isPurchaseCard);
                for(int i = 0; i < e.targetPlayer.hand.cardsInHand.Count; i++)
                {
                    Card t = e.targetPlayer.hand.cardsInHand[i];
                    if(t.cardName == relevantCard.cardName && relevantCard.isPurchaseCard != true && !relevantCard.isResearchableCard) 
                    { 
                        e.targetPlayer.hand.cardsInHand.Remove(t);i = e.targetPlayer.hand.cardsInHand.Count;
                        //Debug.Log("removing");
                    }
                }
                rightEmptyCard.notifier.SetupColor(Color.red);
                //Debug.Log(" first " + e.targetPlayer.hand.cardsInHand.Contains(relevantCard));
                e.targetPlayer.hand.cardsInHand.Remove(relevantCard);
                //Debug.Log(e.targetPlayer.hand.cardsInHand.Count + " count end ");
                //Debug.Log(" second " + e.targetPlayer.hand.cardsInHand.Contains(relevantCard));
                //Debug.Log("now it is at " + e.targetPlayer.hand.cardsInHand.Count);
                
                if (!relevantCard.isPurchaseCard && !relevantCard.isResearchableCard && !relevantCard.cardReference.isUnitAbility && !relevantCard.exileFromGameWhenPlayed) { e.targetPlayer.discardPile.cardsInDiscard.Add(relevantCard); }
                
                //leftEmptyCard.AppearAsThisCard(e.relevantCards[0].cardReference); 
            }
        }
        if (e.relevantCards[0] == MainScript.attackUnitCard || e.relevantCards[0] == MainScript.moveUnitCard) { ChangeUnitCardsEnabled(false); }
        if (!relevantCard.isPurchaseCard)
        {
            if (relevantCard.isResearchableCard)
            {
                queue.Add(new GameEvent(e.relevantCards,e.relevantUnits[0]));
            }
            else
            {
                //Debug.Log("playing card here " + e.relevantCards[0].cardName);
                PlayCard(e.relevantCards[0], e);
            }
        }
        else
        {
            //foreach(Card c in e.relevantCards) { c.isPurchaseCard = false; }
            //if(e.targetPlayer.theType == PlayerType.localHuman && )
            foreach (Chesspiece u in e.relevantUnits) { if (u.alive && u != MainScript.nullUnit) { u.currentBoardPiece.CreateCardGenerationSymbol(); } }
            e.targetPlayer.discardPile.AddToDiscardFromPurchase(e.relevantCards);
        }
    }
    void SetupFenceFromEvent(GameEvent e)
    {
        ChessboardPiece postA = e.theTarget[0];
        ChessboardPiece postB = e.theTarget[1];
        EventListener fenceListener = EventListener.GetMultipleUnitListener(new List<Chesspiece>() { postA.currentChessPiece, postB.currentChessPiece }, "fence", new List<GameEventType>() { GameEventType.moveUnit,GameEventType.throwUnit});
        postA.currentChessPiece.listeners.Add(fenceListener);
        postB.currentChessPiece.listeners.Add(fenceListener);
        MainScript.allEventListeners.Add(fenceListener);
        Vector3 origin = postA.transform.position + (Vector3.up * distanceBetweenUnitAndBoardPiece * 0.25f);
        Vector3 difference = postB.transform.position - postA.transform.position;
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = origin + ((difference * 0.5f) * i) + (Vector3.up * (0.35f + (i * 0.25f)));
            Transform t = Instantiate(fenceBeamPrefab, pos, Quaternion.LookRotation(difference.normalized, Vector3.up)).transform;
            FenceBeam b = new FenceBeam(postA, postB, t);
            fenceBeams.Add(b);
            postA.currentChessPiece.owner.fenceBeams.Add(b);
        }
    }
    void AttackMoveAttackFromEvent(GameEvent e)
    {
        ChessboardPiece attacker = e.theActor[0];//the actor is the one attacking
        ChessboardPiece defender = e.theTarget[0];//the target is the one defending
        attacker.SetupAttackSymbol();
        defender.SetupDefendSymbol();
        Vector3 directToDefender = defender.transform.position - attacker.transform.position;
        float lengthOfAnim = 0f;
        attacker.ChangeTransparentRenderColor(Color.blue);
        defender.ChangeTransparentRenderColor(Color.red);
        string attackType = "shoot";
        if (attacker.currentChessPiece.attackType == AttackType.melee) { attackType = "meleeAttack"; };
        if (attacker.currentChessPiece.unitRef.cardReference.cardType != CardType.building)
        {
            attacker.currentChessPiece.transform.rotation = Quaternion.LookRotation(directToDefender.normalized, Vector3.up); attacker.currentChessPiece.thisAnim.Play(attackType); lengthOfAnim = attacker.currentChessPiece.GetAnimationLength(attackType); attacker.currentChessPiece.thisAnim.speed = lengthOfAnim / e.animationTime;
        }
        if (defender.currentChessPiece.unitRef.cardReference.cardType != CardType.building)
        {
            defender.currentChessPiece.transform.rotation = Quaternion.LookRotation(directToDefender.normalized * -1f, Vector3.up); lengthOfAnim = defender.currentChessPiece.GetAnimationLength("takeDamage"); defender.currentChessPiece.thisAnim.speed = lengthOfAnim / e.animationTime; defender.currentChessPiece.thisAnim.Play("takeDamage");
        }
        defender.currentChessPiece.DealDamage(attacker.currentChessPiece.attack);
        if (attacker.currentChessPiece.owner.theType == PlayerType.localHuman)
        {
            rightEmptyCard.AppearAsThisUnit(attacker.currentChessPiece);
            rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.attacksUnit, Color.green);
            leftEmptyCard.AppearAsThisUnit(defender.currentChessPiece);
            leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.defending, Color.red);
            leftEmptyCard.SetupHealthBuff(attacker.currentChessPiece.attack * -1);
        }
        else
        {
            leftEmptyCard.AppearAsThisUnit(attacker.currentChessPiece);
            leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.attacksUnit, Color.red);
            rightEmptyCard.AppearAsThisUnit(defender.currentChessPiece);
            rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.defending, Color.green);
            rightEmptyCard.SetupHealthBuff(attacker.currentChessPiece.attack * -1);
        }

        //defender.currentChessPiece.ChangeHealth(-1 * attacker.currentChessPiece.attack);

        if (e.theType == GameEventType.moveAttack)
        {
            if (!defender.currentChessPiece.alive) //if this is a move attack then it has to continue until someone dies. if the defender is still alive add a move attack back
            {
                //Debug.Log("defender dies in move attack" + e.targetPlayer.playerNumber + " and player number is " + attacker.currentChessPiece.owner.playerNumber + " and of course " + attacker.currentChessPiece.alive);
                defender.currentChessPiece.queuedForDeletion = true;//queue defender for deletion, rather than waiting for other update as we already know its dead
                if (defender.currentChessPiece.hasOnDeathEffect)
                {
                    queue.Add(new GameEvent(defender.currentChessPiece));
                }
                else
                {
                    queue.Add(new GameEvent(new List<ChessboardPiece>() { defender }));//add the deletion to queue like updatetheboardwould
                }
                if (attacker.currentChessPiece.owner.playerNumber == e.targetPlayer.playerNumber)
                {
                    if (attacker.currentChessPiece.alive)//if the current attacker is the one who initiated the attack
                    {
                        queue.Add(new GameEvent(GameEventType.moveUnit, new List<ChessboardPiece>() { attacker }, new List<ChessboardPiece>() { defender }, e.targetPlayer)); //then move the attacker to defender spot if it is the one that initiated attack
                    }
                }
            }
            else
            {
                GameEvent temp = GameEvent.GetMoveAttackEvent(defender.currentChessPiece, attacker.currentChessPiece);
                temp.targetPlayer = e.targetPlayer;
                queue.Insert(1, temp);
                //queue.Insert(1,new GameEvent(new List<ChessboardPiece>() { defender }, new List<ChessboardPiece>() { attacker }, e.targetPlayer));//add another move attack reversing the attacker and targetsu
            }
        }
        if (attacker.currentChessPiece.attackType == AttackType.shoot)
        {
            Vector3 positionOfProjectile = attacker.transform.position + (Vector3.up * distanceBetweenUnitAndBoardPiece);//create the projectile above the attacker's boardpiece
            Vector3 directionToTarget = attacker.transform.position - defender.transform.position;//get the direction from the attacker to the defender, use it to orient the projectile
            projectileTransforms = new List<Transform>() { Instantiate(projectilePrefab, positionOfProjectile, Quaternion.LookRotation(Vector3.up, directionToTarget.normalized)).transform };
            projectileTransforms[0].SendMessage("SetupProjectile", directionToTarget * -1f);
        }
    }
    void ExecuteEvent(GameEvent e)
    {
        //Execute the Event, at this point We know it can be executed. Any gameObject Instantiation or game changes that need to happen
        //when the Event is executed from the Queue, i.e. Instantiating the gameObject(s) for a summonUnit gameEventType or
        //instantiating a projectile for an attackUnit GameEventType
        executedEvents.Add(e);
        Debug.Log("Executing event of type " + e.theType);
        switch (e.theType)
        {
            case GameEventType.playerChoosesACard:
                PlayerChoosesACard(e);
                break;
            case GameEventType.playerViewsCards:
            case GameEventType.playerChoosesFromCards:
                ChooseFromCardsEvent(e);
                break;
            case GameEventType.playerIncome:
                foreach(ChessboardPiece income in e.targetPlayer.ownedBoardPieces) { income.CreateMoneyGeneration(e.animationTime); }
                e.targetPlayer.resources += e.targetPlayer.ownedBoardPieces.Count;
                e.targetPlayer.bank.SetInt(e.targetPlayer.resources);
                e.targetPlayer.bank.SetupCardNumberTransformShift(CardNumberTransformShiftType.inflateBriefly, 1.5f, 1f);
                break;
            case GameEventType.interceptGrenade:
                InterceptGrenade(e);
                break;
            case GameEventType.throwGrenade:
                ThrowGrenade(e);
                break;
            case GameEventType.setupFence:
                SetupFenceFromEvent(e);
                break;
            case GameEventType.AddPoisonToPieces:
                foreach(ChessboardPiece c in e.theTarget)
                {
                    c.SetupGasCloud(e.utility);
                    c.CreateGasBubbles();
                }
                break;
            case GameEventType.buffUnit:
                foreach (Chesspiece u in e.relevantUnits)
                {
                    //u.BuffUnit(e.attack, e.utility, e.defence, e.relevantBool);
                    u.BuffUnit(e.relevantBuffs[0]);
                    u.currentBoardPiece.ChangeTransparentRenderColor(u.owner.GetPlayerColor());
                    u.CalculateBuffs();
                }
                if (e.relevantUnits.Count > 0)
                {
                    EmptyReadableCardPrefabScript readCard = e.relevantUnits[0].owner.GetPlayerEmptyReadableCard();
                    readCard.AppearAsThisUnit(e.relevantUnits[0]);
                    if (e.attack != 0) { readCard.SetupAttackBuff(e.attack); };
                    if (e.defence != 0) { readCard.SetupHealthBuff(e.defence); };
                    //if(e.utility != 0) {readCard.SetupUtilityBuff(e.utility);};
                    if (e.relevantUnits.Count > 1)
                    {
                        EmptyReadableCardPrefabScript otherReadCard = readCard.otherCard;
                        otherReadCard.AppearAsThisUnit(e.relevantUnits[1]);
                        if (e.attack != 0) { otherReadCard.SetupAttackBuff(e.attack); };
                        if (e.defence != 0) { otherReadCard.SetupHealthBuff(e.defence); };
                        //if(e.utility != 0) {readCard.SetupUtilityBuff(e.utility);};

                    }
                }
                break;
            case GameEventType.endTurn:
                EndTurn(e);
                
                EndTurn();
                break;
            case GameEventType.activateEventListener:
                //Debug.Log("activating listener is named " + e.relevantListener.listenerName);
                e.relevantListener.unitOwner.currentBoardPiece.ChangeTransparentRenderColor(Color.blue);
                
                if (e.relevantListener.unitOwner.owner.theType == PlayerType.localHuman)
                {
                    rightEmptyCard.AppearAsThisUnit(e.relevantListener.unitOwner);
                    rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.eventActivatedAbility,Color.green);
                    //rightEmptyCard.AppearAsThisUnit(e.relevantListener.unitOwner);
                }
                else
                {
                    leftEmptyCard.AppearAsThisUnit(e.relevantListener.unitOwner);
                    leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.eventActivatedAbility,Color.red);
                    //leftEmptyCard.AppearAsThisUnit(e.relevantListener.unitOwner);
                }
                List<GameEvent> listenerEvents = spellScript.GetListenerEvents(e.relevantListener);
                //Debug.Log("inserting " + listenerEvents.Count + " events");
                queue.InsertRange(1,listenerEvents);
                break;
            case GameEventType.moveAttack:// does the same thing as attack Unit but changes targets and repeats itself until one dies, then puts a move minion event onto the moveattack location
            case GameEventType.attackUnit:// functional for one unit attacking one thing at a time for now, possible for more later
                AttackMoveAttackFromEvent(e);
                break;
            case GameEventType.applyPoison:
                List<Chesspiece> unitsToDealDamageTo = new List<Chesspiece>();
                
                foreach(ChessboardPiece c in e.theTarget)
                {
                    //Debug.Log("poison toxic is at this point " + c.toxicCloudAmount);
                    c.CreateGasBubbles();
                    if (c.hasChessPiece)
                    {
                        c.currentChessPiece.currentHealth -= c.toxicCloudAmount;
                        unitsToDealDamageTo.Add(c.currentChessPiece);
                    }
                    //c.SetupGasCloud(-1);
                    //Debug.Log("poison toxic is after that point " + c.toxicCloudAmount);
                }
                if(unitsToDealDamageTo.Count > 0)
                {
                    rightEmptyCard.AppearAsThisUnit(unitsToDealDamageTo[0]);
                    rightEmptyCard.SetupHealthBuff(-1 * unitsToDealDamageTo[0].currentBoardPiece.toxicCloudAmount);
                    rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.applyingPoison, Color.white);
                }
                if(unitsToDealDamageTo.Count > 1)
                {
                    leftEmptyCard.AppearAsThisUnit(unitsToDealDamageTo[1]);
                    leftEmptyCard.SetupHealthBuff(-1 * unitsToDealDamageTo[1].currentBoardPiece.toxicCloudAmount);
                    leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.applyingPoison, Color.white);
                }
                foreach (ChessboardPiece c in e.theTarget)
                {
                    c.SetupGasCloud(-1);
                }
                foreach (Chesspiece c in unitsToDealDamageTo) { c.DealDamage(e.utility); c.currentBoardPiece.ChangeTransparentRenderColor(Color.red); if (c.unitRef.cardReference.cardType != CardType.building) { c.SetupAnimation("takeDamage", e.animationTime); unitsToReorient.Add(c); } }
                //queue.Insert(1, new GameEvent(unitsToDealDamageTo, 1));
                //queue.Insert(1, GameEvent.GetEmptyGameEvent(GameEventType.dealDamage));
                break;
            case GameEventType.dealDamage:
                //Debug.Log("dealing damage of " + e.utility);
                foreach (Chesspiece c in e.relevantUnits) { c.DealDamage(e.utility); c.currentBoardPiece.ChangeTransparentRenderColor(Color.red); if (c.unitRef.cardReference.cardType != CardType.building) { c.SetupAnimation("takeDamage", e.animationTime); } }
                if(e.relevantUnits.Count > 0)
                {
                    rightEmptyCard.AppearAsThisUnit(e.relevantUnits[0]);
                    rightEmptyCard.SetupHealthBuff(e.utility * -1);
                    rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.dealDamage, Color.red);
                    rightEmptyCard.notifier.SetupColor(Color.red);
                    if (e.relevantUnits.Count > 1)
                    {
                        leftEmptyCard.AppearAsThisUnit(e.relevantUnits[1]);
                        leftEmptyCard.SetupHealthBuff(e.utility * -1);
                        leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.dealDamage, Color.red);
                        leftEmptyCard.notifier.SetupColor(Color.red);
                    }
                }
                else
                {
                    e.requiresAnimation = false;
                }
                
                //if (rightEmptyCard.fadeCounter.hasFinished && e.relevantUnits.Count > 1) { rightEmptyCard.AppearAsThisUnit(e.relevantUnits[1]); }
                break;
            case GameEventType.destroyMinion:
                DestroyMinion(e);
                break;
            case GameEventType.throwUnit:
            case GameEventType.pushUnits:               //MOVE PUSH AND THROW UNIT
            case GameEventType.moveUnit:
                //Debug.Log("Get spaces traversed returns this many : " + GetSpacesTraversedFromMoveEvent(e).Count);
                MoveUnits(e);
                //rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.mom)
                break;
            case GameEventType.summonDyingUnit:
            case GameEventType.summonUnit:              //SUMMON UNIT
                //Debug.Log("summoning for player number " + e.targetPlayer.playerNumber);
                Debug.Log("summoning unit " + e.unitToSummon.unitName);
                //GameObject prefab = (GameObject)Resources.Load("UnitPrefabs/" + e.unitToSummon.unitPrefabName); for now we're using the same prefab
                GameObject prefab;
                if(e.unitToSummon.cardReference.cardType == CardType.building )
                {
                    //Debug.Log(e.unitToSummon.unitPrefabName);
                    prefab = (GameObject)Resources.Load("UnitPrefabs/" + e.unitToSummon.unitPrefabName);
                }
                else { prefab = (GameObject)basicUnitPrefab; }
                prefab = (GameObject)Resources.Load("UnitPrefabs/" + e.unitToSummon.unitPrefabName);
                Vector3 playerForward = Vector3.forward;
                if (e.targetPlayer.theType == PlayerType.localHuman) 
                {
                    rightEmptyCard.AppearAsThisCard(e.unitToSummon.cardReference, e.targetPlayer);
                    rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.summonMinion, Color.green);
                }
                else
                {
                    playerForward *= -1f;
                    leftEmptyCard.AppearAsThisCard(e.unitToSummon.cardReference,e.targetPlayer);
                    leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.summonMinion, Color.red);
                }
                foreach(ChessboardPiece c in e.theTarget)
                {
                    
                    int randoInt = (int)Random.Range(0, 3);
                    //c.SetupAttackSymbol();
                    if(c != nullBoardPiece)
                    {
                        c.SetupArrowSymbol();
                        c.ChangeTransparentRenderColor(Color.green);
                    }
                    else
                    {
                        Debug.Log("summoning on nullboardpieces : " + e.unitToSummon.cardReference.cardName);
                    }
                    //c.SetupDefendSymbol();
                    switch (randoInt)
                    {
                        case 0:
                            //c.CreateScienceSymbol();
                            break;
                        case 1:
                            
                            break;
                        case 2:
                            //c.SetupDefendSymbol();
                            break;
                    }
                    
                    //if(c == nullBoardPiece) { Debug.Log("the target is still null"); }
                    Vector3 positionToInstantiate = c.transform.position + (Vector3.up * distanceBetweenUnitAndBoardPiece);
                    Transform t = Instantiate(prefab, positionToInstantiate, prefab.transform.rotation).transform;
                    if (e.targetPlayer.theType != PlayerType.localHuman) { t.Rotate(new Vector3(0f, 180f, 0f)); }
                    ChesspieceScript tempScript = t.GetComponentInChildren<ChesspieceScript>();
                    tempScript.thisChessPiece = new Chesspiece(c, t, e.targetPlayer,e.unitToSummon);
                    tempScript.SetupChesspiece();
                    tempScript.thisChessPiece.ChangeColor(playerColors[e.targetPlayer.playerNumber]);
                    e.targetPlayer.currentUnits.Add(tempScript.thisChessPiece);
                    if (tempScript.thisChessPiece.hasOnDeathEffect)
                    {
                        if(e.relevantUnits.Count > 0)
                        {
                            int randomRelevantUnit = (int)Random.Range(0, e.relevantUnits.Count);
                            tempScript.thisChessPiece.deathTarget = e.relevantUnits[randomRelevantUnit].currentBoardPiece;
                        }
                    }
                    allUnits.Add(tempScript.thisChessPiece);
                    if(e.unitToSummon.cardReference.cardType == CardType.building && LibraryScript.buildingLibrary.Contains(e.unitToSummon)) //if its a building and isn't a cardless building (like an artillery placement or mine
                    {
                        List<ChessboardPiece> neighbours = currentBoard.GetBoardPieceNeighbours(true, true, true, c);
                        foreach(ChessboardPiece b in neighbours)
                        {
                            if(b.owner == MainScript.neutralPlayer)
                            {
                                b.ChangeOwner(e.targetPlayer,playerColors[e.targetPlayer.playerNumber]);
                            }
                        }
                        c.ChangeOwner(e.targetPlayer, playerColors[e.targetPlayer.playerNumber]);
                    }
                    if (e.unitToSummon.hasEventListeners)
                    {
                        
                        tempScript.thisChessPiece.listeners = spellScript.GetEventListenerForUnit(e.unitToSummon);
                        foreach(EventListener l in tempScript.thisChessPiece.listeners)
                        {
                            l.unitOwner = tempScript.thisChessPiece;
                        }
                        allEventListeners.AddRange(tempScript.thisChessPiece.listeners);
                    }
                    if (e.unitToSummon.hasOnPlayAnimation)
                    {
                        //Debug.Log("executing on play animation");
                        tempScript.thisChessPiece.SetupAnimation("onPlay",e.animationTime);
                        unitsToReorient.Add(tempScript.thisChessPiece);
                    }
                    if(e.theType == GameEventType.summonDyingUnit)
                    {
                        //tempScript.thisChessPiece.SetupDeathAtEndOfTurn();\
                        //relevant bool is used to indicate weather it dies immediately(true) or if it should die at the end of the turn (false")
                        if(e.relevantBool == true) { tempScript.thisChessPiece.alive = false; } else { tempScript.thisChessPiece.SetupDeathAtEndOfTurn(); }
                        
                    }
                }
                break;
            case GameEventType.reshuffleDeck:
                if(e.targetPlayer.theType == PlayerType.localHuman) { rightEmptyCard.AppearAsPlayer(currentGame.currentPlayer); rightEmptyCard.ShowNotifierOnly(AbilityNotificationState.reshuffling,Color.green); } else { leftEmptyCard.AppearAsPlayer(currentGame.currentPlayer); leftEmptyCard.ShowNotifierOnly(AbilityNotificationState.reshuffling,Color.red); }
                e.targetPlayer.deck.cardsInDeck.AddRange(e.targetPlayer.discardPile.cardsInDiscard);
                while(e.targetPlayer.discardPile.cardsInDiscard.Count > 0)
                {
                    e.targetPlayer.discardPile.cardsInDiscard.RemoveAt(0);
                }
                break;
            case GameEventType.drawCard:
                
                DrawCard(e);
                break;
            case GameEventType.researchCard:
                //Debug.Log("research card event");
                spellScript.ResearchCard(e.relevantCards[0], e.relevantCards[0].owner,e.relevantUnits[0]);//using relevant card's owner pretty sure it'll always be your own cards
                if (e.relevantCards[0].owner.theType == PlayerType.localHuman)
                {
                    //Debug.Log("human researches");
                    rightEmptyCard.AppearAsThisCard(e.relevantCards[0].cardReference,e.relevantCards[0].owner);
                    if (!e.relevantCards[0].isPurchaseCard) { rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.upgrading, Color.green); }
                    else { rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.upgrading, Color.green); }
                }
                else
                {
                    //Debug.Log("non human researches");
                    leftEmptyCard.AppearAsThisCard(e.relevantCards[0].cardReference,e.relevantCards[0].owner);
                    if (!e.relevantCards[0].isPurchaseCard) { leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.upgrading, Color.red); }
                    else { leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.upgrading, Color.red); }
                }
                break;
            case GameEventType.playCard:
                PlayCardFromEvent(e);
                break;
            case GameEventType.applyOnDeathEffect:
                List<GameEvent> deathEffectList = spellScript.ApplyDeathrattle(e.relevantUnits[0]);
                if(e.relevantUnits[0].owner.theType == PlayerType.localHuman) { rightEmptyCard.AppearAsThisUnit(e.relevantUnits[0]); rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.onDeathEffect,Color.green); } else { leftEmptyCard.AppearAsThisUnit(e.relevantUnits[0]); leftEmptyCard.notifier.SetupNotification(AbilityNotificationState.onDeathEffect,Color.red); }
                e.relevantUnits[0].currentBoardPiece.ChangeTransparentRenderColor(Color.magenta);
                deathEffectList.Add(new GameEvent(new List<ChessboardPiece>() { e.relevantUnits[0].currentBoardPiece }));
                queue.InsertRange(1, deathEffectList);
                break;
            case GameEventType.changeHealth:
                foreach(Chesspiece c in e.relevantUnits)
                {
                    if (c.alive) { c.maxHealth += e.utility; c.currentHealth += e.utility; }
                }
                break;
            case GameEventType.changeAttack:
                foreach (Chesspiece c in e.relevantUnits)
                {
                    if (c.alive) { c.attack += e.utility; }
                }
                break;
            case GameEventType.discardCard:
                Color temp = Color.white;
                if (e.targetPlayer.theType == PlayerType.localHuman) { rightEmptyCard.AppearAsPlayer(e.targetPlayer); rightEmptyCard.ShowNotifierOnly(AbilityNotificationState.discarding, Color.green); } else { leftEmptyCard.AppearAsPlayer(e.targetPlayer); leftEmptyCard.ShowNotifierOnly(AbilityNotificationState.discarding, Color.red); }
                
                if(e.utility > e.targetPlayer.hand.cardsInHand.Count) { e.utility = e.targetPlayer.hand.cardsInHand.Count; }
                for(int i = 0; i < e.utility; i++)
                {
                    Card currentCard = e.targetPlayer.hand.cardsInHand[0];
                    e.targetPlayer.discardPile.cardsInDiscard.Add(currentCard);
                    e.targetPlayer.hand.cardsInHand.RemoveAt(0);
                    if (e.targetPlayer.theType == PlayerType.localHuman)// I dont think you can discard cards that are from unit abilities may cause a problem later...
                    {
                        playerDiscard.cardsIncoming.Add(currentCard);
                        //Destroy(currentCard.transform.gameObject);
                        
                    }
                }
                break;
            case GameEventType.beginTurn:
                queue.Add(GameEvent.GetIncomeEventForPlayer(currentGame.currentPlayer));
                UpdatePoisonBeginningOfTurn();
                if(currentGame.currentPlayer.theType != PlayerType.localHuman)
                {
                    //queue.AddRange(currentGame.currentPlayer.aiComponent.SetupAITurn());
                    currentGame.currentPlayer.aiComponent.SetupAITurn();
                }
                enemyHasPlayed = false;
                if (MainScript.currentGame.currentPlayer.theType == PlayerType.localHuman) 
                { 
                    temp = (Color.green);
                    rightEmptyCard.AppearAsPlayer(currentGame.currentPlayer);
                    rightEmptyCard.ShowNotifierOnly(AbilityNotificationState.localHumanTurn, temp);
                } else 
                {
                    temp = (Color.red);
                    leftEmptyCard.AppearAsPlayer(currentGame.currentPlayer);
                    leftEmptyCard.ShowNotifierOnly(AbilityNotificationState.enemyTurn, temp);
                }
                break;
        }
        if (e.requiresAnimation)
        {
            //if the Event requires Animation it sets the animation counter to the appropriate time and sets the game to Animating
            //the function AnimateEvent() will execute the appropriate function, moving from 
            animationCounter = new Counter(e.animationTime);
            animating = true;
        }
        else
        {
//            Debug.Log("event does not require animation " + e.theType);
            //the Event does not require Animation and will immediately execute the final steps of what its doing
            EndAnimation(e);
        }
    }
    void EndTurn(GameEvent e)
    {
        foreach (ChessboardPiece p in currentBoard.chessboardPieces)
        {
            if (p.hasChessPiece)
            {
                bool requiresCalculation = false;
                if (p.currentChessPiece.stunned && p.currentChessPiece.owner == currentGame.currentPlayer) 
                {
                    for(int i = 0; i < p.currentChessPiece.buffs.Count; i++)
                    {
                        UnitBuff currentBuff = p.currentChessPiece.buffs[i];
                        if(currentBuff.remainingStunnedTurns > 0)//it has to be set higher than zero and then reduced. if it is set to zero its a normal buff that stays for ever. also this only works with stuns
                        {
                            currentBuff.remainingStunnedTurns--;
                            if (currentBuff.remainingStunnedTurns == 0)
                            {
                                currentBuff.removeAtEndOfturn = true;
                                requiresCalculation = true;
                            }
                        }
                    }
                    //p.currentChessPiece.stunned = false; 
                }
                
                for (int i = 0; i < p.currentChessPiece.buffs.Count; i++)
                {
                    UnitBuff b = p.currentChessPiece.buffs[i];
                    if (b.removeAtEndOfturn) { p.currentChessPiece.RemoveBuff(b); requiresCalculation = true; i--; }
                }
                //foreach(UnitBuff b in p.currentChessPiece.buffs) { if (b.removeAtEndOfturn) { p.currentChessPiece.RemoveBuff(b); requiresCalculation = true; } }
                if (requiresCalculation) {  p.currentChessPiece.CalculateBuffs(); }
                //if(p.hasStun)p.CreateStunnedSymbol()
            }
        }
        enemiesHaveAttacked = false;
        enemyHasPlayed = false;
        enemyHasSummonedUnits = false;
        CheckForPoison();
        
        if (currentGame.currentPlayer.theType == PlayerType.localHuman)
        {
            rightEmptyCard.AppearAsPlayer(currentGame.currentPlayer);
            rightEmptyCard.ShowNotifierOnly(AbilityNotificationState.endOfTurn, Color.green);
        }
        else
        {
            queue.Add(new GameEvent(currentGame.currentPlayer, currentGame.currentPlayer.hand.cardsInHand.Count, GameEventType.discardCard));//the discard event is added to the queue for the local player when he hits the button, since nobody hits a button to end the enemy turn I put this here
            leftEmptyCard.AppearAsPlayer(currentGame.currentPlayer);
            leftEmptyCard.ShowNotifierOnly(AbilityNotificationState.endOfTurn, Color.red);
        }
        while (artilleryTargetTransforms.Count > 0)
        {
            Transform t = artilleryTargetTransforms[0];
            artilleryTargetTransforms.RemoveAt(0);
            Destroy(t.gameObject);
        }
        foreach (ChessboardPiece c in currentBoard.GetAllBoardPieces(true, false))
        {
            if (c.hasChessPiece)
            {
                if (c.currentChessPiece.alive)
                {
                    c.currentChessPiece.EndOfTurnUpdate();
                }
            }
        }
        foreach (ChessboardPiece c in currentBoard.chessboardPieces)
        {
            c.hasArtillery = false;
        }
    }
    void DestroyMinion(GameEvent e)
    {
        bool allDeadAreDeathrattles = false;
        bool haveFoundAFalse = false;
        for (int i = 0; i < e.theTarget.Count; i++)
        {
            ChessboardPiece c = e.theTarget[i];
            c.ChangeTransparentRenderColor(Color.red);
            Chesspiece u = c.currentChessPiece;
            if (!u.hasOnDeathEffect) { haveFoundAFalse = true; }
            if (u.owner.theType == PlayerType.localHuman)
            {
                //rightEmptyCard.notifier.SetupNotification(AbilityNotificationState.)
                rightEmptyCard.AppearAsThisCard(u.unitRef.cardReference,u.owner);
                rightEmptyCard.ShowNotifierOnly(AbilityNotificationState.unitDies, Color.green);
            }
            else
            {
                leftEmptyCard.AppearAsThisCard(u.unitRef.cardReference,u.owner);
                leftEmptyCard.ShowNotifierOnly(AbilityNotificationState.unitDies, Color.red);
            }
            if (u.unitRef.cardReference.cardType != CardType.building) { u.SetupAnimation("death", e.animationTime); }
            u.owner.currentUnits.Remove(u);
            u.owner.deceasedUnits.Add(u);
            foreach (EventListener l in u.listeners)
            {
                if (l != nullEventListener)
                {
                    if (allEventListeners.Contains(l)) { allEventListeners.Remove(l); }
                    if (l.unitOwners.Count > 0)
                    {
                        foreach (Chesspiece unit in l.unitOwners)
                        {
                            if (unit.alive)
                            {
                                if (unit.listeners.Contains(l))
                                {
                                    unit.listeners.Remove(l);
                                    //Debug.Log("removing listener " + l.listenerName); 
                                }
                            }
                        }
                        if (l.listenerName == "fence")
                        {
                            Player p = l.unitOwners[0].owner;
                            List<FenceBeam> beamsToKill = new List<FenceBeam>();
                            foreach(FenceBeam f in p.fenceBeams)
                            {
                                if(f.postA == l.unitOwners[0].currentBoardPiece || f.postB == l.unitOwners[1].currentBoardPiece)
                                {
                                    beamsToKill.Add(f);
                                }
                            }
                            while(beamsToKill.Count > 0)
                            {
                                FenceBeam b = beamsToKill[0];
                                beamsToKill.Remove(b);
                                p.fenceBeams.Remove(b);
                                MainScript.fenceBeams.Remove(b);
                                Destroy(b.transform.gameObject);
                            }
                        };
                    }
                    
                }
            }
            c.hasChessPiece = false;
        }
        if (!haveFoundAFalse) { e.requiresAnimation = false; }
    }
    void CheckForPoison()
    {
        List<ChessboardPiece> piecesToApplyPoison = new List<ChessboardPiece>();
        foreach (ChessboardPiece c in currentBoard.chessboardPieces)
        {
            if (c.hasToxicCloud)
            {
                //Debug.Log("has toxic cloud at x " + c.xPos + " and " + c.yPos);
                piecesToApplyPoison.Add(c);
                if (c.hasChessPiece)
                {
                    
                }
                //piecesWithPoison.Add(c);
            }
        }
        if(piecesToApplyPoison.Count > 0)
        {
            queue.Add(GameEvent.GetApplyPoisonEvent(piecesToApplyPoison));
        }
    }
    void MoveUnits(GameEvent e)
    {
        for(int i = 0; i < e.theActor.Count; i++)
        {
            ChessboardPiece c = e.theActor[i];
            Chesspiece u = c.currentChessPiece;
            //Debug.Log("adding " + c.currentChessPiece.unitRef.unitName + " to units to reorient");
            unitsToReorient.Add(u);
            Vector3 directToFace = e.theTarget[i].transform.position - c.transform.position;
            string animationName = "walking";
            if(e.theType == GameEventType.moveUnit) {
                u.currentBoardPiece.ChangeColor(Color.white);
                u.numberOfMovesThisTurn++;
                if (u.numberOfMovesThisTurn >= u.numberOfMoves)
                {
                    u.canMove = false;
                }
            }
            if (e.theType == GameEventType.pushUnits) { animationName = "takeDamage"; directToFace *= -1f; }
            
            
            if(u.unitRef.cardReference.cardType != CardType.building) { u.transform.rotation = Quaternion.LookRotation(directToFace.normalized, Vector3.up); float animLength = u.GetAnimationLength(animationName);
                float animSpeed = animLength / e.animationTime;
                u.thisAnim.Play(animationName);
                u.thisAnim.speed = animSpeed;
            }
        }
    }
    void ChangeUnitCardsEnabled(bool newEnabled)
    {
        //Debug.Log("changing cards visibility to " + newEnabled);
        attackUnitCard.render.enabled = newEnabled;
        moveUnitCard.render.enabled = newEnabled;
    }
    bool CanWeExecuteThisEvent(GameEvent e)
    {
        //evaluates whether the basic requirements for a game Event will function and returns false if for some reason, they cannot
        //example: moveUnit doesn't have the unit or the space to move it to. This is a real possibility because something could interupt
        //an event and kill a unit that would have moved, or playing a card could target a unit that has died
        bool tempBool = true;
        //Debug.Log("checking event for execution of type " + e.theType);
        switch (e.theType)
        {
            case GameEventType.setupFence:
                if(e.theTarget[0].hasChessPiece && e.theTarget[1].hasChessPiece)
                {
                    if(e.theTarget[0].currentChessPiece.owner == e.theTarget[1].currentChessPiece.owner && e.theTarget[0].currentChessPiece.unitRef.unitName == e.theTarget[1].currentChessPiece.unitRef.unitName && e.theTarget[0].currentChessPiece.unitRef.unitName == "Electric Fence")
                    {
                        return true;
                    }
                    else { return false; }
                }else{return false;}
                break;
            case GameEventType.destroyMinion:
                for (int i = 0; i < e.theTarget.Count; i++)
                {
                    ChessboardPiece current = e.theTarget[i];
                    if (!current.hasChessPiece)
                    {
                        e.theTarget.Remove(current);
                        i--;
                    }
                }
                if (e.theTarget.Count > 0){return true;}else{return false;}
                break;
            case GameEventType.attackUnit:
            case GameEventType.moveAttack:
                //Debug.Log("evaluating move attack or attack unit " + e.theActor.Count + "  " + e.theTarget.Count);
                bool canAttack = true;
                if (e.theActor.Count != e.theTarget.Count || e.theActor.Count == 0 || e.theTarget.Count == 0) 
                { 
                    //Debug.Log("there is no actor or targets"); 
                    if(e.theType == GameEventType.moveAttack)
                    {
                        if (!currentBoard.GetBoardPieceNeighbours(true, true, true, e.theActor[0]).Contains(e.theTarget[0])) { Debug.Log("Cannotexecute move attack they are not neighbours"); return false; }
                    }
                    return false;  
                }//there must be a space for every actor and target
                for (int i = 0; i < e.theActor.Count; i++)
                {
                    ChessboardPiece currentBoardPiece = e.theActor[i];
                    bool hasUnit = false;
                    if (currentBoardPiece.hasChessPiece) 
                    { 
                        if(currentBoardPiece.currentChessPiece == e.relevantUnits[0])
                        {
                            //Debug.Log("actor is relevant unit 0");
                        }
                        //else { Debug.Log("actor is relevant unit 1 probs"); }
                        if (currentBoardPiece.currentChessPiece.alive) 
                        { hasUnit = true; } else { hasUnit = false;  } 
                    }
                    //each actor piece must have a unit
                    bool targetHasMinion = false;
                    if (e.theTarget[i].hasChessPiece) { targetHasMinion = true;  } else {  }//each target piece must not have a unit 
                    //Debug.Log("target has minion " + targetHasMinion + " and has unit " + hasUnit);
                    if (!targetHasMinion || !hasUnit) { canAttack = false; }
                }
                return canAttack;
                break;
            case GameEventType.moveUnit: //this is for basic movement in one direction
                bool canMove = false;
                if (e.theActor.Count != e.theTarget.Count || e.theActor.Count == 0) 
                { //Debug.Log("actors and target count dont match"); 
                    return false; 
                }//there must be a space for every actor and target

                for (int i = 0;i < e.theActor.Count && !canMove;i++)
                {
                    ChessboardPiece currentBoardPiece = e.theActor[i];
                    bool hasUnit = false;
                    if (currentBoardPiece.hasChessPiece) 
                    {
                        if (currentBoardPiece.currentChessPiece.alive && currentBoardPiece.currentChessPiece.currentHealth > 0) { hasUnit = true; }
                    }
                    else { return false; }

                    //each actor piece must have a unit
                    bool canMoveToSpace = false;
                    if (!e.theTarget[i].hasChessPiece) {  canMoveToSpace = true; } //each target piece must not have a unit
                    //Debug.Log("has unit " + hasUnit + " and can move to space " + canMoveToSpace);
                    if (canMoveToSpace && hasUnit) { canMove = true; }
                }
                tempBool = canMove;
                break;
            case GameEventType.summonDyingUnit:
            case GameEventType.summonUnit:
                if(e.unitToSummon.unitPrefabName == "none" || e.theTarget.Count == 0) { tempBool = false; }
                break;
            case GameEventType.discardCard:
            case GameEventType.drawCard:
                if(e.targetPlayer == MainScript.neutralPlayer || e.utility <= 0) { tempBool = false; }//if you ever want fatigue you'll prob have to change this
                break;
            case GameEventType.playCard:
                Card c = e.relevantCards[0];
                if (c.isPurchaseCard) { return true; }//if its a purchase card you dont need to check its targeting it can always be shuffled into a discard pile
                if(c.targetType == CardTargetType.requiresNoTarget) 
                { 
                    return true; 
                }
                else
                {
                    if(c.cardType == CardType.minion || c.cardType == CardType.moveUnit) 
                    {   
                        if (e.theTarget.Count == 0) 
                        {
                            return false;
                        }
                        else
                        {
                            if (e.theTarget[0] == MainScript.nullBoardPiece) { return false; }
                        }
                    }else if(c.cardType == CardType.spell)
                    {
                        switch (c.targetType)
                        {
                            case CardTargetType.targetsBoardPiece:
                            case CardTargetType.emptyBoardPiece:
                                if (e.theTarget.Count > 0)
                                {
                                    if(c.targetType == CardTargetType.emptyBoardPiece)
                                    {
                                        ChessboardPiece target = e.theTarget[0];
                                        //Debug.Log("target is " + target.xPos + " by " + target.yPos);
                                        if (e.theTarget[0].hasChessPiece)
                                        {
                                            return false;
                                            //tempBool = true;
                                        }
                                        else {  return true; }
                                    }
                                    else { tempBool = true; }
                                }
                                else
                                {
                                    //Debug.Log("this is the false you get");
                                    return false;
                                }
                                

                                break;
                            case CardTargetType.targetsUnit:
                            case CardTargetType.targetsEnemyUnit:
                            case CardTargetType.targetsFriendlyUnit:
                                //Debug.Log("card name is " + e.relevantCards[0].cardName);
                                if(e.relevantUnits.Count == 0)
                                {
                                    Debug.Log("targeting a unit without relevant units, card name is " + c.cardName + " targets are " + e.theTarget.Count + " and actors are " + e.theActor.Count);
                                }
                                if (e.theTarget.Count > 0)
                                {
                                    if (e.theTarget[0].hasChessPiece)
                                    {
                                        if (c.targetType == CardTargetType.targetsEnemyUnit)
                                        {
                                            if(e.theTarget[0].currentChessPiece.owner != c.owner)
                                            {
                                                tempBool = true;
                                            }
                                            else { return false; }
                                        }else if(c.targetType == CardTargetType.targetsFriendlyUnit)
                                        {
                                            if (e.theTarget[0].currentChessPiece.owner == c.owner)
                                            {
                                                tempBool = true;
                                            }
                                            else { return false; }
                                        }
                                        else { tempBool = true; ; }
                                    }
                                    else { return false; }
                                }
                                else return false;
                                break;
                        }
                    }
                        
                }
                break;
        }
        return tempBool;
    }
    void AnimateEvent(GameEvent e)
    {
        //this function is run every frame and executes the basic level of animations i.e. moving a unit towards a space in the moveUnit GameEventType
        //other more complicated Animations, if they are required can be adding here with a SendMessage function if you want a more specific kind of animation
        //basic things you're always going to want to do, like moving a Unit from one BoardPiece to the other or firing a projectile in a direction should be done here
        float currentPercent = animationCounter.GetPercentageDone();
        switch (e.theType)
        {
            case GameEventType.reshuffleDeck:
                if(e.targetPlayer.theType == PlayerType.localHuman)
                {
                    Vector3 directionToDeck = playerDeck.transform.position - playerDiscard.transform.position;
                    for(int i = 0; i < playerDiscard.cardBacks.Count; i++) 
                    {
                        Transform t = playerDiscard.cardBacks[i];
                        t.position = playerDiscard.transform.position + (directionToDeck * currentPercent) + (Vector3.right * (i * (directionToDeck.magnitude * 0.025f)));
                    }
                }
                break;
            case GameEventType.throwUnit:
            case GameEventType.pushUnits:
            case GameEventType.moveUnit:
                for (int i = 0; i < e.theActor.Count; i++)
                {
                    Chesspiece currentUnit = e.theActor[i].currentChessPiece;
                    ChessboardPiece targetDestination = e.theTarget[i];
                    ChessboardPiece originBoardPiece = currentUnit.currentBoardPiece;
                    Vector3 completeMovement = targetDestination.transform.position - originBoardPiece.transform.position;
                    float heightModifier = 0f;
                    if (e.theType == GameEventType.throwUnit)
                    {
                        heightModifier = (Mathf.Pow((((currentPercent * 2f) - 1f) * 3f), 2f) * -1f) + 9f;
                    }
                    Vector3 newPostion = originBoardPiece.transform.position + unitPositionRelativeToBoardPiece + (completeMovement * currentPercent) + (Vector3.up * heightModifier * 0.35f);
                    currentUnit.transform.position = newPostion;
                }
                break;
            case GameEventType.summonDyingUnit:
            case GameEventType.summonUnit:
                for (int i = 0; i < e.theTarget.Count; i++)
                {
                    Chesspiece currentUnit = e.theTarget[i].currentChessPiece;
                    Color currentColor = currentUnit.GetColor() ;
                    currentUnit.ChangeColor( new Color(currentColor.r, currentColor.g, currentColor.b, currentPercent));
                }
                break;
            case GameEventType.playCard:
                break;
            case GameEventType.moveAttack:
            case GameEventType.attackUnit:
                foreach(Transform t in projectileTransforms)
                {
                    //t.SendMessage("UpdateProjectile");
                }
                break;
            case GameEventType.destroyMinion:
                for (int i = 0; i < e.theTarget.Count; i++)
                {
                    Chesspiece currentUnit = e.theTarget[i].currentChessPiece;
                    Color currentColor =currentUnit.GetColor();
                    currentUnit.ChangeColor(new Color(currentColor.r, currentColor.g, currentColor.b, 1f - currentPercent));
                }
                break;
            case GameEventType.activateEventListener:
                foreach(Transform t in projectileTransforms)
                {
                    //t.SendMessage("UpdateProjectile");
                }
                break;
        }
        foreach (Transform t in projectileTransforms)
        {
            t.SendMessage("UpdateProjectile");
        }
    }
    void EndAnimation(GameEvent e)
    {
        // the final function run after every animation to finalize and effect. if you Move a unit to another space it updates the spaces to have a unit (true) or not, etc.
        // it would also move them directly above the boardpiece to tehir appropriate destination, anything that is done AT THE END OF THE ANIMATION is executed here
        while (playerDiscard.cardsIncoming.Count > 0)
        {
            Card discard = playerDiscard.cardsIncoming[0];
            playerDiscard.cardsIncoming.RemoveAt(0);
            if (!discard.hasNoTransform) { Destroy(discard.transform.gameObject); }
        }
        animating = false;
        switch (e.theType)
        {
            case GameEventType.discardCard:
                
                break;
            case GameEventType.AddPoisonToPieces:
            case GameEventType.applyPoison:
                while (projectileTransforms.Count > 0)
                {
                    Transform t = projectileTransforms[0];
                    projectileTransforms.Remove(t);
                    Destroy(t.gameObject);
                }
                projectileTransforms = new List<Transform>();
                break;
            case GameEventType.throwUnit:
            case GameEventType.pushUnits:
            case GameEventType.moveUnit:
                for (int i = 0; i < e.theActor.Count; i++)
                {
                    Chesspiece currentUnit = e.theActor[i].currentChessPiece;
                    if (!currentUnit.GetBuilding()) { currentUnit.SetupAnimation("idle", 4f); }
                    if (currentUnit.owner.theType == PlayerType.localHuman) { currentUnit.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); } else { currentUnit.transform.rotation = Quaternion.LookRotation(Vector3.forward * -11, Vector3.up); }
                    ChessboardPiece targetDestination = e.theTarget[i];
                    currentUnit.transform.position = targetDestination.transform.position + (Vector3.up * distanceBetweenUnitAndBoardPiece);
                    if (e.theTarget[i] != e.theActor[i])
                    {
                        e.theTarget[i].hasChessPiece = true;
                        e.theActor[i].hasChessPiece = false;
                        e.theTarget[i].currentChessPiece = e.theActor[i].currentChessPiece;
                        currentUnit.currentBoardPiece = targetDestination;
                    }
                }
                break;
            case GameEventType.drawCard:
                
                playerDiscard.UpdateDeckHolder();
                playerDeck.UpdateDeckHolder();
                playerDeck.SetupCardBacks();
                playerDiscard.SetupCardBacks();
                break;
            case GameEventType.playCard:
                Card c = e.relevantCards[0];
                //if (playerDiscard.cardsIncoming.Contains(c)) { playerDiscard.cardsIncoming.Remove(c); }
                if(e.targetPlayer.theType == PlayerType.localHuman)
                {
                    c.FadeAfterBeingPlayed();
                    if (currentlySelectedUnit == nullUnit && !c.hasNoTransform)//if we are playing a card from our hand with no unit selected remove it from the hadn and add to discard. has no transform 
                    {//*** its possible currently selected unit changes if a spell is interupted and it should probably added in that case something to indicate when a card is played from a unit and not from the player's hand
                        //e.targetPlayer.hand.cardsInHand.Remove(c);
                        
                    }
                    
                    if (!c.isResearchableCard && !c.isPurchaseCard && !c.cardReference.isUnitAbility && !c.exileFromGameWhenPlayed) { e.targetPlayer.discardPile.cardsInDiscard.Add(c); //Debug.Log("adding to discard here" + c.cardName + " AND is purchase card is " + c.isPurchaseCard); 
                    }
                        
                    //playerDiscard.
                    playerDeck.SetupCardBacks();
                    playerDiscard.SetupCardBacks();
                }
                currentlySelectedCard = nullCard;
                currentCardHasTarget = false;
                if(c != attackUnitCard && c != moveUnitCard && c.owner.theType == PlayerType.localHuman && !c.hasNoTransform) //if the card isn't the attack card or move card and it is real card the player owns not an ai player card
                { 
                    if(currentlySelectedUnit == nullUnit)//if its not a null unit create an empty card moving to discard to show it goes to discard
                    {

                    }
                    if(c == MainScript.nullCard) { Debug.Log("it is null card somehow"); }
                    //if (!c.hasNoTransform) { Debug.Log("destroying card with name " + c.cardName); Destroy(c.transform.gameObject); }
                }
                
                break;
            case GameEventType.summonDyingUnit:
            case GameEventType.summonUnit:
                break;
            case GameEventType.moveAttack:
            case GameEventType.attackUnit:
                Chesspiece currentGuy = e.theActor[0].currentChessPiece;
                if (currentGuy.unitRef.cardReference.cardType != CardType.building) 
                {
                    currentGuy.thisAnim.speed = 1f;
                    if (currentGuy.owner.theType == PlayerType.localHuman) { currentGuy.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); } else { currentGuy.transform.rotation = Quaternion.LookRotation(Vector3.forward * -1, Vector3.up); }
                    
                }
                currentGuy = e.theTarget[0].currentChessPiece;
                if (currentGuy.unitRef.cardReference.cardType != CardType.building) 
                { 
                    currentGuy.thisAnim.speed = 1f;
                    if (currentGuy.owner.theType == PlayerType.localHuman) { currentGuy.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); } else { currentGuy.transform.rotation = Quaternion.LookRotation(Vector3.forward * -1, Vector3.up); }
                }
                while(projectileTransforms.Count > 0)
                {
                    Transform t = projectileTransforms[0];
                    projectileTransforms.Remove(t);
                    Destroy(t.gameObject);
                }
                projectileTransforms = new List<Transform>();
                break;
            case GameEventType.destroyMinion:
                foreach(ChessboardPiece cp in e.theTarget)
                {
                    Destroy(cp.currentChessPiece.transform.gameObject);
                }
                break;
            case GameEventType.dealDamage:
                
                foreach(Chesspiece unit in e.relevantUnits)
                {
                    if (unit.alive)
                    {
                        if(!unit.GetBuilding()) { unit.SetupAnimation("idle", 4f); }
                    }
                }
                break;
            case GameEventType.activateEventListener:
                EventListener l = e.relevantListener;
                if (l.destroyOnActivation)
                {
                    l.unitOwner.listeners.Remove(l);
                    MainScript.allEventListeners.Remove(l);
                }
                while (projectileTransforms.Count > 0)
                {
                    Transform t = projectileTransforms[0];
                    projectileTransforms.Remove(t);
                    Destroy(t.gameObject);
                }
                break;
        }
        while (projectileTransforms.Count > 0)
        {
            Transform t = projectileTransforms[0];
            projectileTransforms.Remove(t);
            Destroy(t.gameObject);
        }
        while (nonAnimatedProjectileTransforms.Count > 0)
        {
            Transform t = nonAnimatedProjectileTransforms[0];
            nonAnimatedProjectileTransforms.Remove(t);
            Destroy(t.gameObject);
        }
        while(unitsToReorient.Count > 0)
        {
            Transform t = unitsToReorient[0].transform;
            Vector3 directToFace = Vector3.forward;
            if(unitsToReorient[0].owner.theType != PlayerType.localHuman) { directToFace *= -1f; }
            t.rotation = Quaternion.LookRotation(directToFace, Vector3.up);
            unitsToReorient[0].SetupAnimation("idle", 4f);
            unitsToReorient.RemoveAt(0);

        }
        playerDeck.SetupCardBacks();
        playerDiscard.SetupCardBacks();
        queue.Remove(e);
        UpdateTheBoard();
        DeselectEverything();
        ExecuteTheQueue();
        //queue.Remove[queue[0]];
    }
    void EndTurn()
    {
        //Debug.Log("ending turn");
        int currentPlayerNumber = currentGame.currentPlayer.playerNumber + 1;// add one to the currentplayer's number
        if(currentPlayerNumber >= currentGame.allPlayers.Count){currentPlayerNumber = 0;} //if there is no player with that number set it to zero (if its beyond the list then start over at the start)
        //Debug.Log("new player number is " + currentPlayerNumber);
        currentGame.currentPlayer = currentGame.allPlayers[currentPlayerNumber];
        //queue.Add(new GameEvent(currentGame.currentPlayer, currentGame.currentPlayer.hand.cardsInHand.Count,GameEventType.discardCard));
        if(currentGame.currentPlayer.theType != PlayerType.localHuman)//if the ai is not the local player, other players maybe later, but for now its just ai
        {
            //if WE know its an AI...
            
            //ExecuteAITurn(currentGame.currentPlayer);
        }
        queue.Add(new GameEvent(GameEventType.beginTurn));
    }
    void UpdateCameraControlsPC()
    {
        Vector2 moveDirect = Vector2.zero;
        if (Input.GetKey("w")) { moveDirect += Vector2.up; }
        if (Input.GetKey("a")) { moveDirect += Vector2.left; }
        if (Input.GetKey("d")) { moveDirect += Vector2.right; }
        if (Input.GetKey("s")) { moveDirect += Vector2.down; }
        float cameraPanSpeed = 10f;
        Vector3 transformForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 transformRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
        if (moveDirect != Vector2.zero) { centerOfCameraFocus += (((transformRight * moveDirect.normalized.x) + (transformForward * moveDirect.normalized.y)) * Time.deltaTime * cameraPanSpeed); }
        float cameraRotationSpeed = 15f;
        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (Input.GetKey(KeyCode.Space))
        {
            
            verticalAngleInRadians += mouse.y * cameraRotationSpeed * Time.deltaTime;
            verticalAngleInRadians = Mathf.Clamp(verticalAngleInRadians, 0.125f, 1f);
            horizontalAngleInRadians += mouse.x * cameraRotationSpeed * Time.deltaTime;
        }
        
        float cameraZoomSpeed = 150f;
        float cameraMinHeight = 0.45f;
        float cameraMaxHeight = 7.5f;
        Vector3 previousDirectionFromFocus = new Vector3(cameraDirectionFromFocus.x, cameraDirectionFromFocus.y, cameraDirectionFromFocus.z);
        cameraDirectionFromFocus = new Vector3(Mathf.Cos(horizontalAngleInRadians), Mathf.Sin(verticalAngleInRadians) * 2f, Mathf.Sin(horizontalAngleInRadians));
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll != 0f) 
        {
                
            currentCameraZoomDistance += (mouseScroll * cameraZoomSpeed * Time.deltaTime * -1f);
            currentCameraZoomDistance = Mathf.Clamp(currentCameraZoomDistance,cameraMinHeight, cameraMaxHeight);
        }
        transform.position = centerOfCameraFocus + (cameraDirectionFromFocus * currentCameraZoomDistance);
        transform.rotation = Quaternion.LookRotation(centerOfCameraFocus - transform.position, Vector3.up);
        
    }
    void PlayCard(Card c, GameEvent e)
    {
        switch (c.cardType)
        {
            case CardType.attackUnit:
                GameEvent atkUnit = new GameEvent(GameEventType.attackUnit, new List<ChessboardPiece> { currentlySelectedUnit.currentBoardPiece }, new List<ChessboardPiece>() { currentlySelectedTarget }, currentlySelectedUnit.owner);
                queue.Add(atkUnit);
                break;
            case CardType.moveUnit:
                if (currentlySelectedTarget.hasChessPiece)
                {
                    if(currentlySelectedUnit.owner != currentlySelectedTarget.currentChessPiece.owner)
                    {
                        if (secondarySelectedTarget != nullBoardPiece) //if there is a secondary selected target move to that point first
                        {
                            //Debug.Log(currentlySelectedUnit.owner.playerNumber);
                            //queue.Add(new GameEvent(new List<ChessboardPiece> { currentlySelectedUnit.currentBoardPiece }, new List<ChessboardPiece> { secondarySelectedTarget })); //move unit to the secondary target chosen for proximity to attacking target
                            queue.Add(new GameEvent(GameEventType.moveUnit, new List<ChessboardPiece>() { currentlySelectedUnit.currentBoardPiece }, new List<ChessboardPiece>() { secondarySelectedTarget }, currentlySelectedUnit.owner));//move the unit to the boardpiece
                            queue.Add(new GameEvent(new List<ChessboardPiece> { secondarySelectedTarget}, new List<ChessboardPiece> { currentlySelectedTarget }, currentlySelectedUnit.owner));//queue attack move after it has moved to said location
                            secondarySelectedTarget = nullBoardPiece;
                        }
                        else
                        {
                            queue.Add(new GameEvent(new List<ChessboardPiece> { currentlySelectedUnit.currentBoardPiece }, new List<ChessboardPiece> { currentlySelectedTarget }, currentlySelectedUnit.owner));//queue attack move after it has moved to said location
                            secondarySelectedTarget = nullBoardPiece;
                        }
                    }
                }
                else
                {
                    GameEvent temp = new GameEvent(GameEventType.moveUnit, new List<ChessboardPiece>() { currentlySelectedUnit.currentBoardPiece }, new List<ChessboardPiece>() { currentlySelectedTarget }, currentlySelectedUnit.owner);//move the unit to the boardpiece
                    queue.Add(temp);
                }
                
                break;
            case CardType.building:
            case CardType.minion:
                if(c.owner.theType == PlayerType.localHuman)
                {
                    queue.Add(new GameEvent(c.reference, new List<ChessboardPiece>() { currentlySelectedTarget }, c.owner));
                    //Debug.Log(c.reference.applyOnPlayEffect);
                    if (c.reference.applyOnPlayEffect)
                    {
                        if (!c.reference.requiresSecondTarget)
                        {
                            //queue.Add(new GameEvent(c.reference, new List<ChessboardPiece>() { currentlySelectedTarget }, c.owner));
                            queue.AddRange(spellScript.ApplyOnPlayEffect(c, nullBoardPiece, nullBoardPiece));//if there is an on play effect and it requires no target just apply it
                        }
                        else
                        {
                            //Debug.Log(e.theTarget.Count);
                            //Debug.Log(e.theActor.Count);

                            List<GameEvent> onPlayEvents = spellScript.ApplyOnPlayEffect(c, e.theTarget[0], e.theActor[0]);//for the sake of convenience theTarget is used for where a minion is summoned, however if you are "targeting" something with an on play effect it gets put into the actor list despite really being a target ** I changed this to be the second target may change back**
                            if (onPlayEvents.Count > 0)//if there are game events to be added (if the on play effect fails for some reason it can return an empty list and that will signal the targeting has failed and the on play effect is impossible
                            {
                                //Debug.Log("adding targeted on play events");
                                //queue.Add(new GameEvent(c.reference, new List<ChessboardPiece>() { currentlySelectedTarget }, c.owner));
                                queue.AddRange(onPlayEvents);
                            }
                            else //for some reason depending on the nature of the battlecry it cannot execute and the card cannot be played
                            {
                                //Debug.Log("not doing battlecry");
                                DeselectEverything();
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log(c.reference.unitName + " and " + c.reference.applyOnPlayEffect + " and " + c.reference.requiresSecondTarget);
                    queue.Insert(1,new GameEvent(c.reference, e.theTarget, c.owner));
                    if (c.reference.applyOnPlayEffect && c.isPurchaseCard == false && c.isResearchableCard == false)
                    {
                        if(c.reference.onPlayEffectTargetType == CardTargetType.requiresNoTarget)
                        {
                            queue.AddRange(spellScript.ApplyOnPlayEffect(c, nullBoardPiece, nullBoardPiece)); //if there is an on play effect and it requires no target just apply it. not sure how to handle ai selecting targeted battlecries yet or if they will
                        }
                        else
                        {
                            List<GameEvent> onPlayEvents = spellScript.ApplyOnPlayEffect(c, e.theTarget[0], e.theActor[0]);//for the sake of convenience theTarget is used for where a minion is summoned, however if you are "targeting" something with an on play effect it gets put into the actor list despite really being a target ** I changed this to be the second target may change back**
                            if (onPlayEvents.Count > 0)//if there are game events to be added (if the on play effect fails for some reason it can return an empty list and that will signal the targeting has failed and the on play effect is impossible
                            {
                                queue.AddRange(onPlayEvents);
                            }
                        }
                    }
                    if (c.reference.applyOnPlayEffect && c.reference.onPlayEffectTargetType == CardTargetType.requiresNoTarget) 
                    { 
                        
                    }
                    else
                    {
                        //Debug.Log(e.theTarget.Count);
                        //Debug.Log(e.theActor.Count);
                        
                    }
                }
                break;
            case CardType.spell:
                //Debug.Log(c.targetType);
                switch (c.targetType)
                {
                    case CardTargetType.requiresNoTarget:
                        if(currentlySelectedUnit != nullUnit)//if there is a unit selected then send a reference to it with playspell
                        {
                            queue.AddRange(spellScript.PlaySpell(c, new List<ChessboardPiece>(), new List<Chesspiece>() { currentlySelectedUnit},e));
                        }
                        else//otherwise theres no target or unit to send
                        {
                            queue.AddRange(spellScript.PlaySpell(c, new List<ChessboardPiece>(), e.relevantUnits,e));
                        }
                        
                        break;
                    case CardTargetType.targetsFriendlyUnit:
                    case CardTargetType.targetsEnemyUnit:
                    case CardTargetType.targetsUnit:
                        if (!c.cardReference.requiresSecondtarget) { queue.AddRange(spellScript.PlaySpell(c, new List<ChessboardPiece>() { e.theTarget[0] }, e.relevantUnits, e)); } else
                        {
                            queue.AddRange(spellScript.PlaySpell(c, new List<ChessboardPiece>() { }, new List<Chesspiece>() { e.theTarget[0].currentChessPiece, e.theActor[0].currentChessPiece, e.relevantUnits[0] }, e));
                        }
                        break;
                    case CardTargetType.emptyBoardPiece:
                        //Debug.Log(e.theTarget.Count + " target " + e.theActor.Count + " actor " );
                        queue.AddRange(spellScript.PlaySpell(c, new List<ChessboardPiece>() { e.theTarget[0], e.theActor[0] }, new List<Chesspiece>() { e.relevantUnits[0] },e));
                        break;
                    case CardTargetType.targetsBoardPiece:
                        queue.AddRange(spellScript.PlaySpell(c, new List<ChessboardPiece>() { e.theTarget[0], e.theActor[0] }, new List<Chesspiece>() { e.relevantUnits[0] },e));
                        break;
                }
                break;
        }
        currentlySelectedCard = nullCard;
    }
    void ResetAllMoveAttackUnitBoardPieces()
    {
        boardRender.material.color = Color.white;
        if(possibleUnitMoveLocations.Count > 0)
        {
            while(possibleUnitMoveLocations.Count > 0)
            {
                possibleUnitMoveLocations[0].ChangeColor(Color.white);
                possibleUnitMoveLocations.RemoveAt(0);
            }
        }
        if(possibleAttackLocations.Count > 0)
        {
            while (possibleAttackLocations.Count > 0)
            {
                possibleAttackLocations[0].ChangeColor(Color.white);
                possibleAttackLocations.RemoveAt(0);
            }
        }
        if(possibleOnPlayTargets.Count > 0)
        {
            while(possibleOnPlayTargets.Count > 0)
            {
                if (possibleOnPlayTargets[0] != nullBoardPiece) { possibleOnPlayTargets[0].ChangeColor(Color.white); }
                possibleOnPlayTargets.RemoveAt(0);
            }
        }
        
    }
    public static List<ChessboardPiece> GetMoveableSpacesForUnit(Chesspiece unit, bool moveOverUnits,bool includeAttackMove)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        //Debug.Log("checking for movablespaces for the unit");
        switch (unit.movementType)
        {
            case ChessPieceMovementAbilityType.rook:
                tempList = currentBoard.GetRookMovePositionsFromOrigin(unit.currentBoardPiece, moveOverUnits, includeAttackMove, unit.movementdistance,unit.owner);
                break;
            case ChessPieceMovementAbilityType.bishop:
                tempList = currentBoard.GetBishopMovePositionsFromOrigin(unit.currentBoardPiece, moveOverUnits, includeAttackMove, unit.movementdistance,unit.owner);
                break;
            case ChessPieceMovementAbilityType.queen:
                tempList = currentBoard.GetRookMovePositionsFromOrigin(unit.currentBoardPiece, moveOverUnits, includeAttackMove, unit.movementdistance,unit.owner);
                tempList.AddRange(currentBoard.GetBishopMovePositionsFromOrigin(unit.currentBoardPiece, moveOverUnits, includeAttackMove, unit.movementdistance,unit.owner));
                break;
        }
        return tempList;
    }
    Chesspiece CheckIfMouseIsOverUnit()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Chesspiece temp = MainScript.nullUnit;
        if (Physics.Raycast(r, out hit, LayerMask.GetMask("BoardPiece", "Unit")))//see if we are hovering over a unit or a boardpiece holding a unit
        {
            Transform t = hit.transform;
            if (t.tag == "BoardPiece")
            {
                ChessboardPiece c = t.GetComponent<ChessboardPieceScript>().thisChessboardPiece;
                if (c.hasChessPiece)
                {
                    currentlySelectedCard = MainScript.nullCard;
                    temp = c.currentChessPiece;
                }
            }
            else if (t.tag == "Unit")
            {
                currentlySelectedCard = MainScript.nullCard;
                temp = t.GetComponent<ChesspieceScript>().thisChessPiece;
            }
        }
        //if(temp != MainScript.nullUnit) { Debug.Log("check if mouse over unit has something"); } else { Debug.Log("check if mouse over unit does not have something"); }
        return temp;
    }
    void DeselectEverything()
    {
        if (currentlySelectedTarget != nullBoardPiece) { currentlySelectedTarget.ChangeColor(Color.white); currentlySelectedTarget = nullBoardPiece; }
        if (boardRender.material.color != boardColor) { boardRender.material.color = boardColor; }
        ResetAllMoveAttackUnitBoardPieces();
        currentlySelectedCard = nullCard;
        while (currentUnitCards.Count > 0)
        {
            Card c = currentUnitCards[0];
            currentUnitCards.Remove(c);
            if (c != moveUnitCard && c != attackUnitCard) { Destroy(c.transform.gameObject); }
        }
        if (currentlySelectedUnit != nullUnit) { currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.white); }
        currentlySelectedUnit = nullUnit;
        currentCardHasTarget = false;
    }
    void NothingSelectedInterface()
    {
        //Debug.Log("no card selected or unit selected");
        Player p = currentGame.currentPlayer;
        if (Input.GetMouseButtonDown(0))//if the player has just left clicked
        {
            //did we click on a card?
            Card tempCard = MainScript.nullCard;
            for (int i = 0; i < p.hand.cardsInHand.Count; i++)//check each card to see if its being selecteed
            {
                //List<Card> allCardsMouseOver eventually there should be a list of every card the mouse is over and select the top most one;
                Card currentCardInHand = p.hand.cardsInHand[i];
                if (currentCardInHand.mouseOver)
                {
                    tempCard = currentCardInHand;
                }
            }
            if (tempCard != nullCard)//if the mouse was over a card and assigned to tempcard
            {
                currentlySelectedCard = tempCard;
                //Debug.Log("selecting card " + tempCard.cardName);
                selectingOnPlayEffectTarget = false;
                hasLiftedSinceLeftClick = false;
                possibleOnPlayTargets = spellScript.GetOnPlayTargets(currentlySelectedCard, nullBoardPiece, nullUnit);
                if(possibleOnPlayTargets.Count == 0)
                {
                    possibleOnPlayTargets = BasicSpellScript.NoActualTargetsCheckForUnits(currentlySelectedCard);
                }
                //foreach(ChessboardPiece c in possibleOnPlayTargets)
                UpdatePossiblePlayTargets();
                //if (currentlySelectedCard.cardType == CardType.building) { possibleOnPlayTargets = GetPossibleBuildingLocationsForPlayer(currentlySelectedCard.owner); foreach (ChessboardPiece c in possibleOnPlayTargets) { c.ChangeColor(Color.yellow); } }
                rightEmptyCard.AppearAsThisCard(tempCard.cardReference,tempCard.owner);
            }
            else //we did not click on a card, check if we select a unit
            {
                Chesspiece temp = CheckIfMouseIsOverUnit(); //returns null unit if no chesspiece is under mouse
                if (temp != MainScript.nullUnit) // if we have clicked on a unit we select it
                {
                    SelectUnit(temp);
                }
                else
                {
                    if (playerDiscard.mouseIsOverImage)
                    {
                        
                        List<CardReference> tempList = new List<CardReference>();
                        foreach(Card c in currentGame.currentPlayer.discardPile.cardsInDiscard)
                        {
                            tempList.Add(c.cardReference);
                        }
                        GameEvent tempEvent = GameEvent.GetChooseFromCardsEvent(tempList, nullCard, currentGame.currentPlayer, false);
                        tempEvent.theType = GameEventType.playerViewsCards;
                        queue.Add(tempEvent);
                    }
                }
            }
        }
    }
    void CardSelectedInterface()
    {
        if (Input.GetMouseButton(0))//if you are still holding down right mouse
        {
            UpdateTargeting2();
            if (selectingOnPlayEffectTarget && hasLiftedSinceLeftClick)//if we are selecting the secondary target and have lifted our finger up since 
            {
                ResetAllMoveAttackUnitBoardPieces();
                if (currentCardHasTarget && CanPlayerAffordThisCard(currentlySelectedCard, currentGame.currentPlayer))//if we have a target
                {
                    
                    queue.Add(new GameEvent(currentlySelectedCard, currentlySelectedCard.owner, currentlySelectedTarget, secondarySelectedTarget));//add teh play card event to the queue
                    //reset all selections and boardpieces
                }
                else
                {
                    //Debug.Log("de selecting" + possibleAttackLocations.Count) ;
                    if(currentlySelectedTarget != nullBoardPiece) 
                    {
                        currentlySelectedTarget.ChangeColor(Color.white);
                    }
                    currentlySelectedCard = nullCard;
                    ResetAllMoveAttackUnitBoardPieces();
                }
                selectingOnPlayEffectTarget = false;
                hasLiftedSinceLeftClick = false;
            }
        }
        else//you have released teh left mouse button after draggin a card to your target
        {
            if (!selectingOnPlayEffectTarget) //if we are not selecting an on play effect or second target
            {
                if (currentCardHasTarget && CanPlayerAffordThisCard(currentlySelectedCard, currentGame.currentPlayer))// and we have a primary target
                {
                    //Debug.Log("we are there" + currentlySelectedCard.cardName);
                    if (!currentlySelectedCard.requiresSecondTarget)// and the card doesn't require a second card (most dont)
                    {
                        
                        queue.Add(new GameEvent(currentlySelectedCard, currentlySelectedCard.owner, currentlySelectedTarget, secondarySelectedTarget));//add teh play card event to the queue
                        ResetAllMoveAttackUnitBoardPieces();//reset all selections and boardpieces
                        currentCardHasTarget = false;
                    }
                    else
                    {
                        foreach(ChessboardPiece p in possibleOnPlayTargets) { if(p != MainScript.nullBoardPiece) { p.ChangeColor(Color.white); } }
                        possibleOnPlayTargets = spellScript.GetOnPlayTargets(currentlySelectedCard, currentlySelectedTarget, nullUnit);
                        //Debug.Log("selecting secondary play target with " + possibleOnPlayTargets.Count + "on play targets");
                        if (possibleOnPlayTargets.Contains(currentlySelectedTarget)) { possibleOnPlayTargets.Remove(currentlySelectedTarget); }
                        if(possibleOnPlayTargets.Count == 0)
                        {
                            possibleOnPlayTargets = BasicSpellScript.NoActualTargetsCheckForUnits(currentlySelectedCard);
                        }
                        //currentlySelectedTarget.ChangeColor(couldPlayButDoesNotMeetCriteria);
                        possibleUnitMoveLocations.Add(currentlySelectedTarget);
                        UpdatePossiblePlayTargets();
                        //foreach (ChessboardPiece c in possibleOnPlayTargets) { if (c != nullBoardPiece && c != currentlySelectedTarget) { c.ChangeColor(Color.yellow); } }
                        selectingOnPlayEffectTarget = true;
                        //currentlySelectedTarget.ChangeColor(Color.magenta);
                        hasLiftedSinceLeftClick = false;
                        currentCardHasTarget = false;
                    }
                }
                else
                {
                    
                    DeselectEverything();
                    ResetAllMoveAttackUnitBoardPieces();
                }
            }
            else//we are selecting a second target and we have just clicked so we should check if we have selected the secondaary target
            {
                UpdateTargeting2();
                hasLiftedSinceLeftClick = true;
                if (Input.GetMouseButton(0))
                {
                    
                }
                
            }

        }
    }
    void UpdatePossiblePlayTargets()
    {
        foreach (ChessboardPiece p in MainScript.possibleAttackLocations)
        {
            if(p!= nullBoardPiece) p.ChangeColor(withinRangeSpaceColor);
        }
        foreach (ChessboardPiece p in MainScript.possibleOnPlayTargets)
        {
            if (p != nullBoardPiece) p.ChangeColor(canPlaySpaceColor);
        }
        foreach (ChessboardPiece p in MainScript.possibleUnitMoveLocations)
        {
            if (p != nullBoardPiece) p.ChangeColor(couldPlayButDoesNotMeetCriteria);
        }
        if(currentlySelectedUnit != nullUnit)//if the current unit is not in any of these lists return it to green to show its selected
        {
            ChessboardPiece unit = currentlySelectedUnit.currentBoardPiece;
            if (!possibleAttackLocations.Contains(unit) && !possibleOnPlayTargets.Contains(unit) && !possibleUnitMoveLocations.Contains(unit)) { unit.ChangeColor(Color.green);  }
            
        }
    }
    void NoCardAndUnitSelectedInterface()
    {
        Card tempCard = MainScript.nullCard;
        if (Input.GetMouseButtonDown(0))//if we have just clicked
        {
            for (int i = 0; i < MainScript.currentUnitCards.Count; i++)//check each card to see if it was selected and if it isn't the current players unit dont even check
            {
                Card c = MainScript.currentUnitCards[i];
                if (c.mouseOver) { tempCard = c; }
            }
            if (tempCard != nullCard)//if a card was selected by this click select it and highlight moveable or attackable boardpieces if necessary
            {
                ResetSelectedTargets();
                currentlySelectedCard = tempCard;
                hasLiftedSinceLeftClick = false;
                if(!tempCard.isPurchaseCard && !tempCard.isResearchableCard)
                {
                    possibleOnPlayTargets = spellScript.GetOnPlayTargets(tempCard, currentlySelectedUnit.currentBoardPiece, currentlySelectedUnit);
                    if (possibleOnPlayTargets.Count == 0)
                    {
                        possibleOnPlayTargets = BasicSpellScript.NoActualTargetsCheckForUnits(currentlySelectedCard);
                    }
                }
                
                UpdatePossiblePlayTargets();
                if (currentGame.currentPlayer == currentlySelectedUnit.owner)
                {
                    foreach (ChessboardPiece c in possibleOnPlayTargets) { if (c != nullBoardPiece) { c.ChangeColor(Color.yellow); } }
                    if ((currentlySelectedCard.cardType == CardType.building || currentlySelectedCard.cardType == CardType.minion) && !currentlySelectedCard.isPurchaseCard && !currentlySelectedCard.isResearchableCard)
                    {
                        //spellScript.GetOnPlayTargets(currentlySelectedCard, nullBoardPiece, currentlySelectedUnit);
                        //UpdatePossiblePlayTargets();
                        /*possibleOnPlayTargets = currentBoard.GetBoardPieceNeighbours(true, true, true, currentlySelectedUnit.currentBoardPiece);
                        foreach (ChessboardPiece c in possibleOnPlayTargets)
                        {
                            if (c != nullBoardPiece) { c.ChangeColor(Color.yellow); }
                        }*/
                    }
                    if (currentlySelectedCard.cardType == CardType.spell)
                    {
                        // Debug.Log("setting on play target for spell " + currentlySelectedCard.cardName);
                        //possibleOnPlayTargets = spellScript.GetOnPlayTargets(currentlySelectedCard, currentlySelectedTarget, currentlySelectedUnit);
                        //UpdatePossiblePlayTargets();
                        //foreach (ChessboardPiece c in possibleOnPlayTargets) { if (c != nullBoardPiece) { c.ChangeColor(Color.yellow); } }
                    }
                    // possibleOnPlayTargets = GetPossibleBuildingLocationsForPlayer(currentlySelectedCard.owner); foreach (ChessboardPiece c in possibleOnPlayTargets) { c.ChangeColor(Color.yellow); } }
                    rightEmptyCard.AppearAsThisCard(currentlySelectedCard.cardReference,currentlySelectedCard.owner);
                }
                else
                {
                    rightEmptyCard.AppearAsThisCard(tempCard.cardReference,currentlySelectedCard.owner);
                }

                //UnitCardSelected();
            }
            else //we have clicked but not on a card, check if we clicked on a unit
            {
                Chesspiece tempPiece = CheckIfMouseIsOverUnit();//tempPiece becomes any unit we hovered over or null if we did not hover over one

                if (tempPiece != nullUnit)//if we clicked and didnot select a unit then deselect the unit we do have selected
                {
                    if (tempPiece != currentlySelectedUnit)//if we clicked and selected a unit that is not the currently selected unit
                    {
                        SelectUnit(tempPiece);
                    }
                }
                else
                {
                    DeselectEverything();
                }
            }
        }
    }
    bool CanPlayerAffordThisCard(Card c, Player p)
    {
        return (c.cost <= p.resources);
    }
    void CardAndUnitSelected()
    {
        if (Input.GetMouseButton(0))//if we are holding down the left mouse button
        {
            if (!selectingOnPlayEffectTarget)//if we aren't selecting a secondary target simply update targeting
            {
                UpdateTargeting2();
            }
            else//if we are selecting a secondary target then we just clicked so we must check if the mouse is over a target
            {
                if (hasLiftedSinceLeftClick)//make sure the left mouse button has lifted up because otherwise its just the next frame of you holding the button down
                {
                    UpdateTargeting2();
                    if (currentCardHasTarget && CanPlayerAffordThisCard(currentlySelectedCard, currentGame.currentPlayer))// we do have a target so play the card
                    {
                        //Debug.Log("gets in here");
                        currentlySelectedCard.MakeInvisible();
                        if(currentlySelectedCard.owner.theType == PlayerType.localHuman && currentlySelectedUnit.owner.theType == PlayerType.localHuman)
                        {
                            //Debug.Log("we are adding here");
                            currentlySelectedUnit.cardsPlayedThisTurn.Add(currentlySelectedCard.cardReference);
                            queue.Add(GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(currentlySelectedCard, currentlySelectedCard.owner, currentlySelectedTarget, secondarySelectedTarget, currentlySelectedUnit));//add teh play card event to the queue
                        }
                        
                        ResetAllMoveAttackUnitBoardPieces();//reset all selections and boardpieces
                    }
                    else 
                    {
                        currentlySelectedCard = nullCard;
                        ResetAllMoveAttackUnitBoardPieces();
                        ResetSelectedTargets();
                        
                    }
                }
                
            }
        }
        else //we are not pressing left mouse button
        {
            if (!hasLiftedSinceLeftClick) { hasLiftedSinceLeftClick = true; }
            if (!selectingOnPlayEffectTarget)//if we aren't selecting a secondary target we need to check if we have a primary target 
            {
                //Debug.Log("not selecting secondary target");
                if (currentCardHasTarget && CanPlayerAffordThisCard(currentlySelectedCard, currentGame.currentPlayer)&&(currentlySelectedUnit.owner.theType == PlayerType.localHuman)) //we do have a target
                {
                    //Debug.Log("has target and can afford");
                    if (!currentlySelectedCard.requiresSecondTarget || currentlySelectedCard.isResearchableCard || currentlySelectedCard.isPurchaseCard)//if we dont require a second target
                    {
                        //Debug.Log("we do not require a second target");
                        currentlySelectedCard.MakeInvisible();
                        if(currentlySelectedCard == moveUnitCard && currentlySelectedTarget.hasChessPiece)//
                        {
                            currentlySelectedUnit.numberOfMovesThisTurn++;
                            if (currentlySelectedUnit.numberOfMovesThisTurn >= currentlySelectedUnit.numberOfMoves)
                            {
                                currentlySelectedUnit.canMove = false;
                            }
                            //List<GameEvent> eventsToAdd = new List<GameEvent>();
                            if (secondarySelectedTarget != nullBoardPiece)
                            {
                                queue.Add(GameEvent.GetMoveEvent(currentlySelectedUnit, secondarySelectedTarget));
                                queue.Add(GameEvent.GetMoveAttackFromBoardPieces(secondarySelectedTarget, currentlySelectedTarget, currentlySelectedUnit.owner));
                            }
                            else
                            {
                                queue.Add(GameEvent.GetMoveAttackEvent(currentlySelectedUnit, currentlySelectedTarget.currentChessPiece));
                            }
                            currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.white);
                        }
                        else
                        {
                            if(currentlySelectedCard == moveUnitCard)
                            {
                                /*currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.white);
                                currentlySelectedUnit.numberOfMovesThisTurn++;
                                if (currentlySelectedUnit.numberOfMovesThisTurn >= currentlySelectedUnit.numberOfMoves)
                                {
                                    currentlySelectedUnit.canMove = false;
                                }*/
                            }else if(currentlySelectedCard == attackUnitCard)
                            {
                                currentlySelectedUnit.numberOfAttacksThisTurn++;
                                if(currentlySelectedUnit.numberOfAttacksThisTurn >= currentlySelectedUnit.numberOfAttacks)
                                {
                                    currentlySelectedUnit.canAttack = false;
                                }
                            }
                            else
                            {
                                currentlySelectedUnit.cardsPlayedThisTurn.Add(currentlySelectedCard.cardReference);
                            }
                            GameEvent temp = GameEvent.GetPlayCardEventWithSelectedUnitSecondaryTarget(currentlySelectedCard,currentlySelectedCard.owner, currentlySelectedTarget, secondarySelectedTarget, currentlySelectedUnit);
                            //Debug.Log("playing " + currentlySelectedCard.cardName);
                            queue.Add(temp);
                            //queue.Add(new GameEvent(currentlySelectedCard, currentlySelectedCard.owner, currentlySelectedTarget, secondarySelectedTarget));//add teh play card event to the queue
                        }
                        //currentlySelectedUnit = nullUnit;
                        currentlySelectedCard = nullCard;
                        ResetSelectedTargets();
                        ResetAllMoveAttackUnitBoardPieces();//reset all selections and boardpieces
                        currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.white);
                    }
                    else// we do require a second target set selecting onplay effect target to true and hasliftedsincelastClick to false
                    {
                        hasLiftedSinceLeftClick = false;
                        if (possibleOnPlayTargets.Contains(currentlySelectedTarget)) { possibleOnPlayTargets.Remove(currentlySelectedTarget); }
                        possibleUnitMoveLocations.Add(currentlySelectedTarget);
                        foreach (ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); }
                        possibleAttackLocations = new List<ChessboardPiece>();
                        //currentlySelectedTarget.ChangeColor(Color.magenta);
                        UpdatePossiblePlayTargets();
                        selectingOnPlayEffectTarget = true;
                        currentCardHasTarget = false;
                        //currentlySelectedCard = nullCard;
                        //ResetAllMoveAttackUnitBoardPieces();
                    }
                    
                }
                else//we dont deselect the card
                {
                    //Debug.Log("gets to here deselect" + possibleAttackLocations.Count);
                    //foreach(ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); }
                    //possibleAttackLocations = new List<ChessboardPiece>();
                    currentlySelectedCard = nullCard;
                    ResetAllMoveAttackUnitBoardPieces();
                    if(currentlySelectedUnit != nullUnit)
                    {
                        currentlySelectedUnit.currentBoardPiece.ChangeColor(currentlySelectedUnit.owner.GetPlayerColor());
                    }
                }
            }
            else
            {
                UpdateTargeting2();
            }
        }
    }
    void UpdateInterface()
    {
        Player p = currentGame.currentPlayer;
        if (currentlySelectedCard == nullCard && currentlySelectedUnit == nullUnit)// if a card is NOT already selected and a unit is NOT selected
        {
            NothingSelectedInterface();
        }else if(currentlySelectedCard != nullCard && currentlySelectedUnit == nullUnit)//if we have a selected card but not a selected unit
        {
            CardSelectedInterface();
        }else if(currentlySelectedCard == nullCard && currentlySelectedUnit != nullUnit)
        {
            NoCardAndUnitSelectedInterface();
        }else if (currentlySelectedCard != nullCard && currentlySelectedUnit != nullUnit)
        {
            CardAndUnitSelected();
        }
    }
    void UpdateTargeting2()
    {
        
        //if the current card targets teh board
        if(currentlySelectedCard.targetType == CardTargetType.requiresNoTarget || currentlySelectedCard.isResearchableCard || currentlySelectedCard.isPurchaseCard)//pretty sure purchase and research go under board
        {
            if (PCControlScript.GetTargetingBoard())
            {
                //we are over the board
                
                if (Input.GetMouseButton(0))//if we are holding down the left mouse button
                {
                    currentCardHasTarget = true;
                    if (boardRender.material.color != Color.green)
                    {
                        boardRender.material.color = Color.green;
                    }
                }
                else
                {
                    currentCardHasTarget = false;
                    if (boardRender.material.color != Color.white)
                    { boardRender.material.color = Color.white;}
                }
            }
            else
            {
                //we are not over the board
                currentCardHasTarget = false;
                if (boardRender.material.color != Color.white)
                { boardRender.material.color = Color.white; }
            }
        }
        else//if the current card targets a boardpiece or unit
        {
            ChessboardPiece tempPiece = PCControlScript.GetTargetUnderMouse(currentlySelectedCard.targetType);
            
            if (!selectingOnPlayEffectTarget)// if we aren't selecting an on play target, i.e. we have selected a card and are selecting the primary target (usually the only target)
            {
                if (tempPiece == MainScript.nullBoardPiece)//you are not targeting anythihng right now
                {
                    //Debug.Log("null target" + possibleAttackLocations.Count);
                    if (currentlySelectedCard.drawRange) { foreach (ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); possibleAttackLocations = new List<ChessboardPiece>(); } }
                    //possibleAttackLocations = new List<ChessboardPiece>();
                    //if (possibleAttackLocations.Contains(currentlySelectedTarget)) { objectiveColor = Color.cyan; } else if (possibleOnPlayTargets.Contains(currentlySelectedTarget)) { objectiveColor = Color.yellow; }
                    UpdatePossiblePlayTargets();
                    if (currentlySelectedTarget != nullBoardPiece) //if the currently selected target is not a null boardpiece
                    {
                        //Color objectiveColor = Color.white;
                        currentlySelectedTarget.ChangeColor(Color.white);
                        

                        //currentlySelectedTarget.ChangeColor(objectiveColor);
                        ResetSelectedTargets();//reset the currently selected targets
                    }
                    
                    currentlySelectedTarget = nullBoardPiece;
                    currentCardHasTarget = false;
                }
                else//you ARE TARGETING something right now
                {
                    
                    if (currentlySelectedTarget != MainScript.nullBoardPiece && currentlySelectedTarget != tempPiece)//if the currently selected piece is not null
                    {
                        //Color objectiveColor = Color.white;
                        currentlySelectedTarget.ChangeColor(Color.white);
                        if (currentlySelectedCard.drawRange)
                        {
                            foreach (ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); }
                            possibleAttackLocations = new List<ChessboardPiece>();
                        }
                        //if (possibleAttackLocations.Contains(currentlySelectedTarget)) { objectiveColor = Color.blue; } else if (possibleOnPlayTargets.Contains(currentlySelectedTarget)) { objectiveColor = Color.yellow; }
                        UpdatePossiblePlayTargets();
                        //currentlySelectedTarget.ChangeColor(objectiveColor);
                        //currentlySelectedTarget.ChangeColor(Color.white);
                        //ResetSelectedTargets();//reset the currently selected target
                    }
                    currentlySelectedTarget = tempPiece;
                    
                    bool canPlayCard = true;
                    if (currentlySelectedCard == moveUnitCard)//specifically for the move unit card
                    {
                        if (currentlySelectedTarget.hasChessPiece)//if there is a minion on the target
                        {
                            if(currentlySelectedTarget.currentChessPiece.owner != currentlySelectedCard.owner)//and it is not of ours
                            {
                                secondarySelectedTarget = nullBoardPiece;
                                List<ChessboardPiece> randomPieceNeighbours = MainScript.currentBoard.GetBoardPieceNeighbours(true, true, true, currentlySelectedTarget);
                                //foreach(ChessboardPiece p in randomPieceNeighbours) { p.ChangeTransparentRenderColor(Color.magenta); }
                                if (randomPieceNeighbours.Contains(currentlySelectedUnit.currentBoardPiece))
                                {
                                    currentlySelectedUnit.currentBoardPiece.ChangeTransparentRenderColor(Color.green);
                                    secondarySelectedTarget = nullBoardPiece;
                                    //you can play the target if you are already on top of it
                                     //queue.AddRange(new List<GameEvent>() { GameEvent.GetMoveAttackEvent(currentlySelectedUnit, currentlySelectedTarget.currentChessPiece) });
                                }
                                else
                                {
                                    bool canMoveToAttack = false;
                                    
                                    foreach(ChessboardPiece c in randomPieceNeighbours)
                                    {
                                        if (possibleOnPlayTargets.Contains(c)) { secondarySelectedTarget = c; canMoveToAttack = true; }
                                    }
                                    if (!canMoveToAttack) { canPlayCard = false; }
                                }
                            }
                        }
                    }
                    if (possibleOnPlayTargets.Count > 0)//if there is a list of possible on play targets for this card with more than one target (insert nullboardpiece if there are no possible targets and it will invalidate any hypothetical target that isn't in this list)
                    {
                        if (!possibleOnPlayTargets.Contains(currentlySelectedTarget)) 
                        { 
                            canPlayCard = false; 
                        } else
                        {
                        }

                    }
                    else {  }
                    if (canPlayCard)
                    {
                        //Debug.Log("can play card");
                        currentCardHasTarget = true;
                        if (currentlySelectedCard.drawRange)
                        {
                            foreach (ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); }
                            possibleAttackLocations = currentBoard.GetAllPiecesWithinDistance(currentlySelectedCard.drawRangeDistance, false, currentlySelectedTarget);
                            if(currentlySelectedUnit != nullUnit)
                            {
                                if (possibleAttackLocations.Contains(currentlySelectedUnit.currentBoardPiece)) { possibleAttackLocations.Remove(currentlySelectedUnit.currentBoardPiece); }
                            }
                        }
                        UpdatePossiblePlayTargets();
                        currentlySelectedTarget.ChangeColor(Color.green);
                    }
                    else
                    {
                        //Debug.Log("into here");
                        if (currentlySelectedTarget != MainScript.nullBoardPiece)//if the currently selected piece is not null
                        {

                            //Color objectiveColor = Color.white;
                            //UpdatePossiblePlayTargets();
                            //if (possibleAttackLocations.Contains(currentlySelectedTarget)) { objectiveColor = Color.blue; } else if (possibleOnPlayTargets.Contains(currentlySelectedTarget)) { objectiveColor = Color.yellow; }
                            //currentlySelectedTarget.ChangeColor(objectiveColor);
                            //ResetSelectedTargets();//reset the currently selected target
                        }
                        if (currentlySelectedCard.drawRange)
                        {
                            
                        }
                        if (currentlySelectedCard.drawRange) { foreach (ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); } possibleAttackLocations = new List<ChessboardPiece>(); }
                        //
                        UpdatePossiblePlayTargets();
                        currentlySelectedTarget = nullBoardPiece;
                        currentCardHasTarget = false;
                    }
                }
            }
            else//we are selecting the secondary selection, i.e. we havethe primary target stored in currently selected target, so this piece should be stored in secondary, and also checked to make sure it isn't the same piece as currently selected target
            {
                if (tempPiece == MainScript.nullBoardPiece || tempPiece == currentlySelectedTarget)//you are not targeting anythihng right now or you're targeting the primary target
                {
                    if(secondarySelectedTarget != MainScript.nullBoardPiece)
                    {
                        //Debug.Log("are we stuck in here");
                        possibleOnPlayTargets = spellScript.GetOnPlayTargets(currentlySelectedCard, currentlySelectedTarget, currentlySelectedUnit);
                        if (currentlySelectedCard.drawRange)
                        {
                            foreach (ChessboardPiece c in possibleAttackLocations) { c.ChangeColor(Color.white); }
                            possibleAttackLocations = new List<ChessboardPiece>();
                        }
                        
                        //Debug.Log("changing to white cause temppiece is null");
                        //Color objectiveColor = Color.white;
                        UpdatePossiblePlayTargets();
                        //if (possibleAttackLocations.Contains(secondarySelectedTarget)) { objectiveColor = Color.blue; } else if (possibleOnPlayTargets.Contains(secondarySelectedTarget)) { objectiveColor = Color.yellow; }
                        //secondarySelectedTarget.ChangeColor(objectiveColor);
                    }

                    secondarySelectedTarget = nullBoardPiece;
                    currentCardHasTarget = false;
                   
                }
                else if(tempPiece != secondarySelectedTarget)//you ARE TARGETING something right now and it cannot be the current target
                {
                    
                    
                    bool canPlayCard = true;
                    if (possibleOnPlayTargets.Count > 0)//if there is a list of possible on play targets for this card with more than one target (insert nullboardpiece if there are no possible targets and it will invalidate any hypothetical target that isn't in this list)
                    {
                        if (!possibleOnPlayTargets.Contains(tempPiece)) { canPlayCard = false; } else { canPlayCard = true; }
                        
                    }
                    if (canPlayCard)
                    {
                        currentCardHasTarget = true;
                    }
                    else
                    {
                        currentCardHasTarget = false;
                    }
                    if (currentCardHasTarget)
                    {
                        //Debug.Log("secondary select has a target");
                        if (secondarySelectedTarget != tempPiece && secondarySelectedTarget != nullBoardPiece) //if the currently selected target is not a null boardpiece
                        {
                            
                            //Debug.Log("changing to white cause secondary target is not temppiece and not nullpiece");
                            //Color objectiveColor = Color.white;
                            
                            //if (possibleAttackLocations.Contains(secondarySelectedTarget)) { objectiveColor = Color.blue; } else if (possibleOnPlayTargets.Contains(secondarySelectedTarget)) { objectiveColor = Color.yellow; }
                            //secondarySelectedTarget.ChangeColor(objectiveColor);
                            //ResetSelectedTargets();
                            //Debug.Log("reseting here");

                        }
                        
                        if (tempPiece != secondarySelectedTarget && tempPiece != nullBoardPiece)
                        {
                            if (currentlySelectedCard.drawLineToSecondTarget)
                            {
                                possibleOnPlayTargets = spellScript.GetOnPlayTargets(currentlySelectedCard, currentlySelectedTarget, currentlySelectedUnit);
                                possibleAttackLocations = GetSpacesAlongLineBetweenTwoSpaces(currentlySelectedTarget, tempPiece,false,false);
                                foreach (ChessboardPiece c in possibleAttackLocations)
                                {
                                    if (possibleOnPlayTargets.Contains(c))
                                    {
                                        possibleOnPlayTargets.Remove(c);
                                    }
                                }
                            }
                        }
                        //Debug.Log("in here right now");
                        currentCardHasTarget = true;
                        secondarySelectedTarget = tempPiece;
                        UpdatePossiblePlayTargets();
                        secondarySelectedTarget.ChangeColor(Color.green);
                    }
                    else
                    {
                        if (secondarySelectedTarget != nullBoardPiece) //if the currently selected target is not a null boardpiece
                        {
                            //Debug.Log("in here");
                            
                            //Color objectiveColor = Color.white;
                            UpdatePossiblePlayTargets();
                            //if (possibleAttackLocations.Contains(secondarySelectedTarget)) { objectiveColor = Color.blue; }else if (possibleOnPlayTargets.Contains(secondarySelectedTarget)) { objectiveColor = Color.yellow; }
                            //secondarySelectedTarget.ChangeColor(objectiveColor);
                            //ResetSelectedTargets();
                            //Debug.Log("reseting here");

                        }
                        if (currentlySelectedCard.drawLineToSecondTarget)
                        {
                            possibleOnPlayTargets = spellScript.GetOnPlayTargets(currentlySelectedCard, currentlySelectedTarget, currentlySelectedUnit);
                            possibleAttackLocations = GetSpacesAlongLineBetweenTwoSpaces(currentlySelectedTarget, secondarySelectedTarget,false,false);
                            foreach (ChessboardPiece c in possibleAttackLocations)
                            {
                                if (possibleOnPlayTargets.Contains(c))
                                {
                                    possibleOnPlayTargets.Remove(c);
                                }
                            }
                        }
                        secondarySelectedTarget = nullBoardPiece;
                    }
                }
            }
        }
    }
    void ResetSelectedTargets()
    {
        hasLiftedSinceLeftClick = false;
        selectingOnPlayEffectTarget = false;
        //Debug.Log("Reseting selected targets");
        Color objectiveColor = Color.white;
        if (currentlySelectedTarget != nullBoardPiece )
        {
            //if (possibleOnPlayTargets.Contains(currentlySelectedTarget)) { objectiveColor = Color.yellow; }
            //if (possibleAttackLocations.Contains(currentlySelectedTarget)) { objectiveColor = Color.blue; }
            if (!selectingOnPlayEffectTarget) {  currentlySelectedTarget.ChangeColor(objectiveColor); }
                
        }
        if(secondarySelectedTarget != nullBoardPiece)
        {
            objectiveColor = Color.white;
            //if (possibleOnPlayTargets.Contains(secondarySelectedTarget)) { objectiveColor = Color.yellow; };
            //if (possibleAttackLocations.Contains(secondarySelectedTarget)) { objectiveColor = Color.blue; }
            secondarySelectedTarget.ChangeColor(objectiveColor);
        }
        UpdatePossiblePlayTargets();
        //foreach (ChessboardPiece c in possibleOnPlayTargets) { if (c != nullBoardPiece) { c.ChangeColor(Color.yellow); } }
        //foreach(ChessboardPiece c in possibleAttackLocations) { if (c != nullBoardPiece) { c.ChangeColor(Color.blue); } }
    }
    void UpdateTheBoard()//checking for dead units and adding the Destroy Minion GameEvent to remove them
    {
        //Debug.Log("updating the board");
        List<ChessboardPiece> boardPiecesWithDeadUnits = new List<ChessboardPiece>();
        List<ChessboardPiece> boardPiecesWithOnDeathEffect = new List<ChessboardPiece>();
        foreach (ChessboardPiece c in currentBoard.chessboardPieces)//check ev ery boardpiece
        {
            if (c.hasChessPiece)//if it has a unit
            {
                if (!c.currentChessPiece.alive && !c.currentChessPiece.queuedForDeletion)//and it is not alive
                {
                    c.currentChessPiece.queuedForDeletion = true;
                    if (c.currentChessPiece.hasOnDeathEffect) //if the unit has a deathrattle affect
                    {
                        queue.Add(new GameEvent(c.currentChessPiece));//add the deathrattle to the queue
                        //Debug.Log("adding derprattel");
                        //***the deathrattle will apply before the dead units have been removed, it may be better to put deathrattles into a list and apply them AFTER the units that have died are removed
                        //it may be prudent to randomize the deathrattles anyway rather than the furthest left and highest up going first
                    }
                    else //if it does not have any on death effect
                    {
                        boardPiecesWithDeadUnits.Add(c);//put dead units into this list
                    }
                }
            }
        }
        if(boardPiecesWithDeadUnits.Count > 0) { queue.Add(new GameEvent(boardPiecesWithDeadUnits)); }
        while(boardPiecesWithOnDeathEffect.Count > 0) //if there is an on death effect to apply randomly choose one
        {
            /*int randomInt = (int)Random.Range(0f, boardPiecesWithOnDeathEffect.Count); //get a random number between 0 and the length of the list
            Chesspiece currentUnit = boardPiecesWithOnDeathEffect[randomInt].currentChessPiece;//get that array reference's unit
            //Add This Unit's on Death Effect to the Queue;
            boardPiecesWithOnDeathEffect.RemoveAt(randomInt);*/
        }
    }
    void ResetTargets()
    {

    }
    void ResetCurrentlySelectedTarget()
    {
        if (currentlySelectedTarget != nullBoardPiece)//if something is selected right now and not a null piece
        {
            if (!possibleUnitMoveLocations.Contains(currentlySelectedTarget) && !possibleAttackLocations.Contains(currentlySelectedTarget) && !possibleOnPlayTargets.Contains(currentlySelectedTarget))//if the current target is NOT yellow to indicate its within range and NOT red to say its attackable, 
            {
                currentlySelectedTarget.ChangeColor(Color.white);//it should be white like a normal neutral chessboardpiece
            }
            else
            {
                if (currentlySelectedTarget.hasChessPiece && currentlySelectedTarget.currentChessPiece.owner != currentlySelectedCard.owner)//pretty sure it has to be an enemy unit because you cannot move to or attack your own units and they'd never be a target
                {
                    currentlySelectedTarget.ChangeColor(Color.red);//has enemy so its red
                }
                else
                {
                    currentlySelectedTarget.ChangeColor(Color.yellow);//does not so its yellow to indicate you can move there (if current card is moveunit) or you could shoot something there (if current card is attack unit)
                }
            }
            //Debug.Log("reseting selected " + Time.time);
            //Debug.Log("deslect here");
            currentlySelectedTarget = nullBoardPiece;
        }
    }
    void SelectUnit(Chesspiece unit)
    {
        DeselectEverything();
        //Debug.Log("selecting unit");
        ChangeUnitCardsEnabled(true);
        if(currentlySelectedCard != nullCard) { currentlySelectedCard = nullCard; }
        if(currentlySelectedUnit != nullUnit) { currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.white); }
        currentlySelectedUnit = unit;
        if(unit.owner.theType == PlayerType.localHuman) { rightEmptyCard.AppearAsThisUnit(currentlySelectedUnit); } else { leftEmptyCard.AppearAsThisUnit(currentlySelectedUnit); }
        
        if(currentlySelectedUnit.owner == currentGame.currentPlayer){currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.green);}else{currentlySelectedUnit.currentBoardPiece.ChangeColor(Color.red);}
        currentUnitCards = GetUnitsCards(unit);
        currentUnitCardPositions = new List<Vector2>();
        int cards = currentUnitCards.Count;
        float distanceFromCenter = MainScript.distanceBetweenCardsInHand * ((float)(cards - 1) * 50f);
        Vector2 originPoint = new Vector2(0f - (distanceFromCenter), (Screen.height * -0.65f) + (MainScript.cardHeight * 55f));
        for (int i = 0; i < cards; i++)
        {
            currentUnitCardPositions.Add(originPoint + ((float)i * MainScript.distanceBetweenCardsInHand * Vector2.right * 100f));
        }
    }
    List<Card> GetUnitsCards(Chesspiece unit)
    {
        //Debug.Log("getting unit cards for " + unit.unitRef.unitName + " it belongs to a player of type " + unit.owner.theType);
        List<Card> tempList = new List<Card>();
        if (unit.canAttack) { tempList.Add(attackUnitCard); }
        if (unit.canMove) { tempList.Add(moveUnitCard); }
        List<CardReference> cardsUnitHas = spellScript.GetUnitAbilities(unit);//get the original base abilities the unit would have if it had no modifiers or upgrades
        //unit.unitRef.GetAbilities();
        cardsUnitHas = unit.unitRef.GetAbilities(cardsUnitHas);
        foreach (CardReference c in cardsUnitHas)//this is for ability cards, like normal cards you would play from your hand
        {
            if (!unit.cardsPlayedThisTurn.Contains(c))
            {
                Transform t = Instantiate(cardPrefab, new Vector3(Screen.width * 0.5f, Screen.height * -0.25f, 0f), Quaternion.identity).transform;
                t.SetParent(canvas);
                CardScript tempScript = t.GetComponent<CardScript>();
                Card tempCard = new Card(c, unit.owner);
                //
                //Debug.Log("first " + tempCard.owner.theType);
                tempScript.SetupCard(tempCard, unit.owner, true);
                //Debug.Log(tempCard.owner.theType);
                tempList.Add(tempScript.thisCard);
                
            }
        }
        if(unit.unitRef.cardReference.cardType == CardType.building && LibraryScript.buildingLibrary.Contains(unit.unitRef))
        {
            foreach (CardReference c in spellScript.GetBuildingStorefront(unit.unitRef,unit.owner))// this is for cards you can "buy" which puts them into your discard pile to eventually be drawn
            {
                Transform t = Instantiate(cardPrefab, new Vector3(Screen.width * 0.5f, Screen.height * -0.25f, 0f), Quaternion.identity).transform;
                t.SetParent(canvas);
                CardScript tempScript = t.GetComponent<CardScript>();
                //tempScript.SetupPurchasableCard(new Card(c, unit.owner), unit.owner, true);
                Card tempCard = new Card(c, unit.owner);
                //Debug.Log("first " + tempCard.owner.theType);
                tempScript.SetupPurchasableCard(tempCard, unit.owner, true);
                //Debug.Log(tempCard.owner.theType);
                tempList.Add(tempScript.thisCard);
                if (tempScript.thisCard.cardType == CardType.minion)
                {
                    
                    /*tempScript.thisCard.attack = tempScript.thisCard.reference.cardReference.GetAttack(unit.owner);
                    //Debug.Log("we get in here " + tempScript.thisCard.attack + " is the attack now");
                    tempScript.thisCard.reference.attack = tempScript.thisCard.reference.cardReference.GetAttack(unit.owner);
                    tempScript.ChangeAttack(tempScript.thisCard.reference.attack);
                    tempScript.thisCard.reference.defence = tempScript.thisCard.reference.cardReference.GetHealth(unit.owner);
                    tempScript.ChangeHealth(tempScript.thisCard.reference.defence);*/
                }
            }
        }
        if(unit.unitRef.cardReference.cardType == CardType.building && LibraryScript.buildingLibrary.Contains(unit.unitRef))//pretty sure only buildings will be for researching
        {
            List<CardReference> researchableCards = spellScript.GetBuildingResearchableCards(unit);
            //Debug.Log("we get to this place");
            foreach (CardReference c in researchableCards)
            {
                Transform t = Instantiate(cardPrefab, new Vector3(Screen.width * 0.5f, Screen.height * -0.25f, 0f), Quaternion.identity).transform;
                t.SetParent(canvas);
                CardScript tempScript = t.GetComponent<CardScript>();
                tempScript.SetupResearchableCard(new Card(c, unit.owner), unit.owner, true);
                tempList.Add(tempScript.thisCard);
                if (tempScript.thisCard.cardType == CardType.minion)
                {
                    //tempScript.thisCard.defence = tempScript.thisCard.reference.cardReference.GetHealth(unit.owner);
                    //tempScript.thisCard.reference.defence = tempScript.thisCard.defence;
                    //Debug.Log("we get in here " + tempScript.thisCard.attack + " is the attack now");
                }
            }
        }
        
        return tempList;
    }
    // Update is called once per frame
    void Update()
    {
        if (!mainMenuIsOpen)
        {
            //if main menu is not open and we are playing the game proper
            if (currentGame.isActive)
            {
                UpdateFenceBeams();
                UpdateCameraControlsPC();
                for(int i = 0; i < transformShiftingCardNumbers.Count; i++)
                {
                    CardNumber n = transformShiftingCardNumbers[i];
                    n.UpdateCardNumberTransformShift(Time.deltaTime);
                    if (n.transformShiftingCounter.hasFinished) { n.ReturnToOriginalScale() ; transformShiftingCardNumbers.Remove(n);i--; }
                }
                if (!currentGame.isPaused)
                {
                    //if the game is running right now and is not paused
                    if (queue.Count > 0)
                    {
                        //if there is any Events in the queue the game must be paused and those events must be executed
                        currentGame.isPaused = true;
                        ExecuteTheQueue();
                    }
                    else //no events are in the queue, waiting for input, moving a unit, playing a card
                    {
                        //Debug.Log("updating players hands");

                        if (currentGame.currentPlayer.theType == PlayerType.localHuman)
                        {
                            //if(App type == PC)
                            if (EndTurnButtonScript.endTurnHasBeenClicked)//if we've clicked end turn
                            {
                                if (currentGame.currentPlayer.hand.cardsInHand.Count > 0)//there are cards that must be discarded first
                                {
                                    queue.Add(new GameEvent(currentGame.currentPlayer, currentGame.currentPlayer.hand.cardsInHand.Count, GameEventType.discardCard));
                                }
                                else
                                {
                                    EndTurnButtonScript.endTurnHasBeenClicked = false;
                                    queue.Add(new GameEvent(GameEventType.endTurn));
                                }

                                //EndTurn();
                            }
                            else
                            {
                                UpdateInterface();
                                //UpdateLocalControlsPC();
                            }

                        }
                        else
                        {

                        }
                    }
                }
                else if (animating)
                {
                    //game is active but paused, and we are animating
                    animationCounter.AddTime(Time.deltaTime);

                    if (!animationCounter.hasFinished)
                    {
                        //if the animation Counter hasn't finished yet
                        AnimateEvent(queue[0]);
                    }
                    else
                    {
                        //if the animation Counter has finished
                        //Debug.Log("is it this?");
                        EndAnimation(queue[0]);
                        //Debug.Log("or not this?");
                    }
                }
                else if (waitingForPlayerInput)
                {
                    WaitingForPlayerInput();
                }
                else//the game is not paused and it is not animating, there is nothing happening
                {

                }
                foreach(ChessboardPiece c in currentBoard.chessboardPieces)
                {
                    c.UpdateUnitStats();
                    if (c.hasChessPiece)
                    {
                        
                        //c.healthIcon.Enable();
                    }
                }
            }
            for (int i = 0; i < currentGame.allPlayers.Count; i++)//update every player's hand
            {
                Player tempPlayer = currentGame.allPlayers[i];
                tempPlayer.UpdateHand();
            }
            for(int i = 0; i < boardPiecesWithTransparentRenderActive.Count; i++)
            {
                ChessboardPiece currentPiece = boardPiecesWithTransparentRenderActive[i];
                currentPiece.transparentRenderCounter.AddTime(Time.deltaTime);
                if (currentPiece.transparentRenderCounter.hasFinished)
                {
                    currentPiece.transparentRender.enabled = false;
                    boardPiecesWithTransparentRenderActive.Remove(currentPiece);
                    i--;
                }
            }
            leftEmptyCard.UpdateEmptyReadableCard(Time.deltaTime);
            rightEmptyCard.UpdateEmptyReadableCard(Time.deltaTime);
            foreach(Transform t in spellTransforms)
            {
                t.SendMessage("UpdateSpell", Time.deltaTime);
            }
            if (!animating)
            {
                
            }
            playerDeck.UpdateDeckHolder();
            playerDiscard.UpdateDeckHolder();
            //playerDiscard.UpdateDeckHolder();
        }
        
    }
    void WaitingForPlayerInput()
    {
        //Debug.Log("waiting for player input");
        for(int i = 0; i< cardsPlayerIsChoosingFrom.Count;i++)
        {
            Card c = cardsPlayerIsChoosingFrom[i];
            Vector3 position = cardPositions[i] + (Vector3.right * cardChoosingOffSet * defaultCardSpeed);
            Vector3 difference = position - c.transform.position;
            if(c.transform.position != position)
            {
                float speedModifier = difference.magnitude * 0.25f;
                float distanceToMove = defaultCardSpeed * Time.deltaTime * 2.50f * speedModifier;
                if(distanceToMove > difference.magnitude)
                {
                    c.transform.Translate(new Vector3(difference.normalized.x, difference.normalized.y, 0f) * distanceToMove);
                }
                else
                {
                    c.transform.position = position;
                }
                
            }
        }
        if(currentlySelectedCard != nullCard)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Vertical"));
            currentlySelectedCard.transform.position = Input.mousePosition;
            float xDelta = transform.position.x - mouseXWhenSelecting;
            //float xDelta = Input.mousePosition.x - mouseXWhenSelecting;
            cardChoosingOffSet += mouseDelta.x * Time.deltaTime * 25f;
            if (currentlySelectedCard.transform.position.y < 0.2f * Screen.height || currentlySelectedCard.transform.position.y > 0.8f * Screen.height)//we have chosen the card, adjust the queue and execute it
            {
                waitingForPlayerInput = false;
                currentGame.isPaused = false;
                if (!justViewingCards) 
                {
                    GameEvent nextEvent = queue[0];
                    //Debug.Log(nextEvent.theType);
                    nextEvent.relevantCards = new List<Card>() { currentlySelectedCard };
                }
                else
                {
                    justViewingCards = false;
                }
                
                while (cardsPlayerIsChoosingFrom.Count > 0)
                {
                    Card c = cardsPlayerIsChoosingFrom[0];
                    cardsPlayerIsChoosingFrom.Remove(c);
                    if (c == currentlySelectedCard)
                    {
                        if (!keepChosenCard) { Destroy(c.transform.gameObject); }
                        else
                        {
                            //Debug.Log("we dont destroy one");
                        }
                        //keep it alive
                    }
                    else
                    {
                        Destroy(c.transform.gameObject);
                    }
                }
                currentlySelectedCard = nullCard;
                
            }
            else
            {
                if (!Input.GetMouseButton(0)) { currentlySelectedCard = nullCard; }
            }
            
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (HideCardsButtonScript.mouseOver)
                {
                    if (justViewingCards)
                    {
                        waitingForPlayerInput = false;
                        currentGame.isPaused = false;
                        hidebutton.render.enabled = false;
                        while (cardsPlayerIsChoosingFrom.Count > 0)
                        {
                            Card c = cardsPlayerIsChoosingFrom[0];
                            cardsPlayerIsChoosingFrom.Remove(c);
                            if (c == currentlySelectedCard)
                            {
                                if (!keepChosenCard) { Destroy(c.transform.gameObject); }
                                else
                                {
                                    //Debug.Log("we dont destroy one");
                                }
                                //keep it alive
                            }
                            else
                            {
                                Destroy(c.transform.gameObject);
                            }
                        }
                        currentlySelectedCard = nullCard;
                    }
                    else
                    {
                        hideCardsPlayerIsChoosingFrom = !hideCardsPlayerIsChoosingFrom;
                        //Debug.Log("we are in here" + hideCardsPlayerIsChoosingFrom);
                        if (hideCardsPlayerIsChoosingFrom) { foreach (Card c in cardsPlayerIsChoosingFrom) { c.MakeInvisible(); } }
                        else
                        {
                            foreach (Card c in cardsPlayerIsChoosingFrom) { c.MakeVisible(); }
                        }
                    }
                    
                }
            }
            if (Input.GetMouseButton(0))//if we are holding 
            {
                
                //else { Debug.Log("we are NOT in here"); }
                if (!hideCardsPlayerIsChoosingFrom)
                {
                    foreach (Card c in cardsPlayerIsChoosingFrom)
                    {
                        if (c.mouseOver) { currentlySelectedCard = c; mouseXWhenSelecting = c.transform.position.x; }
                    }
                }
                else
                {

                }
                

            }
        }
        
        //if (Input.GetKey(KeyCode.M)) { cardChoosingOffSet += Time.deltaTime; }else if (Input.GetKey(KeyCode.N)) { cardChoosingOffSet += Time.deltaTime * -1f; }
        //Debug.Log(Mathf.Abs(cardChoosingOffSet) + " and " +  totalCardDistance * 0.001f);
        if (Mathf.Abs(cardChoosingOffSet) > totalCardDistance * 0.005f)
        {
            cardChoosingOffSet = (totalCardDistance * 0.005f) * Mathf.Sign(cardChoosingOffSet);
        }
    }
}
public class ChimpChessGame
{
    public bool isPaused = false;
    public bool hasStarted = false;
    public bool isActive = false;
    public bool hasEnded = false;
    public int currentTurn = 0;
    public Player currentPlayer = MainScript.neutralPlayer;
    public List<Player> allPlayers;
    public List<Chesspiece> allUnits;
    public ChimpChessGame(int numberOfPlayers)
    {
        allPlayers = new List<Player>();
        allUnits = new List<Chesspiece>();
        for(int i = 0;i < numberOfPlayers; i++) 
        {
            Player tempPlayer = new Player(i);
            allPlayers.Add(tempPlayer);
            if(i == 0) { tempPlayer.theType = PlayerType.localHuman; }
            tempPlayer.CreateDeck();
        }
    }
}
public class ChimpChessBoard
{
    public int width = 0;
    public int height = 0;
    public ChessboardPiece[,] chessboardPieces;
    public ChimpChessBoard(int theWid, int theHei, Vector3 origin, GameObject chessboardPiecePrefab)
    {
        width = theWid;
        height = theHei;
        float distanceBetweenBoardPieces = 1f;
        chessboardPieces = new ChessboardPiece[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 positionToInstantiate = origin + (Vector3.right * (float)x * distanceBetweenBoardPieces) + (Vector3.forward * (float)y * distanceBetweenBoardPieces) + MainScript.halfABoard;
                Transform t = GameObject.Instantiate(chessboardPiecePrefab, positionToInstantiate, Quaternion.identity).transform;
                ChessboardPieceScript script = t.GetComponent<ChessboardPieceScript>();
                script.SetupChessboardPiece(x, y);
                chessboardPieces[x, y] = script.thisChessboardPiece;
            }
        }
    }
    public bool CanThisPieceMoveToThisBoardPiece(ChessboardPiece piece, ChessboardPiece boardPiece)
    {

        return false;
    }
    public List<ChessboardPiece> UnitToBoardPiece(List<Chesspiece> units)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        foreach(Chesspiece u in units)
        {
            tempList.Add(u.currentBoardPiece);
        }
        return tempList;
    }
    public List<Chesspiece> BoardPieceToUnit(List<ChessboardPiece> pieces, Player alliedPlayer,bool getAllies,bool getEnemies,bool keepNeutral)
    {
        List<Chesspiece> tempList = new List<Chesspiece>();
        foreach (ChessboardPiece c in pieces)
        {
            if (c.hasChessPiece)
            {
                if(getAllies && getEnemies) { tempList.Add(c.currentChessPiece); }else if(getAllies && !getEnemies) { if(c.currentChessPiece.owner == alliedPlayer) { tempList.Add(c.currentChessPiece); } 
                }else if(!getAllies && getEnemies) { if(c.currentChessPiece.owner != alliedPlayer) { tempList.Add(c.currentChessPiece); } 
                }else if(!getAllies && !getEnemies)
                {
                    //pretty sure theres nothing you're gonna get for both of these
                }
                
            }
        }
        if (!keepNeutral)//if you dont want neutral comb through them and remove them if they are owned by teh neutral player (like barrels, either player could attack them)
        {
            for(int i = 0;i < tempList.Count;i++)
            {
                Chesspiece u = tempList[i];
                if(u.owner == MainScript.neutralPlayer)
                {
                    tempList.Remove(u);
                    i--;
                }
            }
        }
        return tempList;
    }
    public List<Chesspiece> GetAllEnemyUnits(Player alliedPlayer)
    {
        List<Chesspiece> tempList = new List<Chesspiece>();
        foreach(ChessboardPiece c in chessboardPieces)
        {
            if (c.hasChessPiece)
            {
                if(c.currentChessPiece.owner != alliedPlayer){ tempList.Add(c.currentChessPiece); }
            }
        }
        return tempList;
    }
    public List<ChessboardPiece> GetRookMovePositionsFromOrigin(ChessboardPiece originPiece, bool moveOverUnit, bool includeMoveAttack, int distance,Player forPlayer)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(1f, 0f), moveOverUnit, includeMoveAttack, distance,forPlayer));
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(0f, 1f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(-1f, 0f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(0f, -1f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        return (tempList);
    }
    public List<ChessboardPiece> GetRandomBoardPiece(bool withUnit, bool withoutUnit,int numberOfSpaces)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        List<ChessboardPiece> allPieces = GetAllBoardPieces(withUnit, withoutUnit);
        
        int piecesAdded = 0;
        while(allPieces.Count > 0 && piecesAdded < numberOfSpaces)
        {
            //piecesAdded++;
            int randomInt = (int)Random.Range(0, allPieces.Count);
            ChessboardPiece c = allPieces[randomInt];
            if (!tempList.Contains(c)) { piecesAdded++; tempList.Add(allPieces[randomInt]); allPieces.Remove(c); }
            
        }
        return tempList;
    }
    public List<ChessboardPiece> GetAllBoardPieces(bool withUnit, bool withoutUnit)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        foreach(ChessboardPiece c in chessboardPieces)
        {
            if(c.hasChessPiece && withUnit) { tempList.Add(c); }
            if(!c.hasChessPiece && withoutUnit){tempList.Add(c);}
        }
        return tempList;
    }
    public GameEvent GetPushAwayEventFromOrigin (ChessboardPiece origin, int spacesToMove,bool pushOtherunits)
    {
        List<ChessboardPiece> originNeighbours = GetBoardPieceNeighbours(true, true, true, origin);
        List<ChessboardPiece> unitsToPush = new List<ChessboardPiece>();
        List<ChessboardPiece> theirDestinations = new List<ChessboardPiece>();
        foreach(ChessboardPiece c in originNeighbours)
        {
            if (c.hasChessPiece)
            {
                int xDiff = c.xPos - origin.xPos;
                int yDiff = c.yPos - origin.yPos;
                int howFarUnitCanMove = 0;
                int maxDistance = spacesToMove;
                List<Chesspiece> piecesToMoveAhead = new List<Chesspiece>();
                List<Chesspiece> piecesWaitingToMoveAhead = new List<Chesspiece>();
                for (int i = 0; i < maxDistance && c.currentChessPiece.unitRef.canMove; i++)
                {
                    int currentX = c.xPos + (xDiff * (i + 1));
                    int currentY = c.yPos + (yDiff * (i + 1));
                    if (IsThisAValidPiecePosition(currentX, currentY))//is there a piece at i * push direction
                    {
                        ChessboardPiece p = MainScript.currentBoard.chessboardPieces[currentX,currentY];//get an instance of that piece
                        if (p.hasChessPiece && pushOtherunits)//does it have a unit? and are we pushing otherunits?
                        {
                            Chesspiece unit = p.currentChessPiece;
                            if (unit.unitRef.canMove)
                            {
                                piecesWaitingToMoveAhead.Add(unit);
                                maxDistance++;
                            }
                            else
                            {
                                i = maxDistance;
                            }
                        }
                        else if(!p.hasChessPiece)
                        {
                            howFarUnitCanMove++;
                        }
                        else
                        {
                            i = maxDistance;
                        }
                        //if(pushOtherUnits){} eventually I want to set it up so you can decide to push other units
                    }
                    else//
                    {
                        i = maxDistance;
                    }
                }
                ChessboardPiece positionToMoveTo = chessboardPieces[c.xPos + (xDiff * howFarUnitCanMove), c.yPos + (yDiff * howFarUnitCanMove)];
                unitsToPush.Add(c);
                theirDestinations.Add(positionToMoveTo);
                for (int i = 0; i < piecesWaitingToMoveAhead.Count; i++)
                {
                    unitsToPush.Add(piecesWaitingToMoveAhead[i].currentBoardPiece);
                    theirDestinations.Add(MainScript.currentBoard.chessboardPieces[positionToMoveTo.xPos + (xDiff * (i + 1)), positionToMoveTo.yPos + (yDiff * (i + 1))]);
                }

            }
        }
        GameEvent temp = new GameEvent(unitsToPush, theirDestinations);
        temp.theType = GameEventType.pushUnits;
        return temp;
    }
    public List<ChessboardPiece> GetBishopMovePositionsFromOrigin(ChessboardPiece originPiece, bool moveOverUnit, bool includeMoveAttack, int distance,Player forPlayer)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(1f, 1f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(1f, -1f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(-1f, 1f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        tempList.AddRange(GetAllPiecesFromOriginInDirection(originPiece, new Vector2(-1f, -1f), moveOverUnit, includeMoveAttack, distance, forPlayer));
        return (tempList);
    }
    public List<ChessboardPiece> GetAllPiecesWithinDistance(int distance, bool includeOrigin, ChessboardPiece origin)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();// make a bunch of lists for keeping track of this process
        List<ChessboardPiece> unTestedPieces = new List<ChessboardPiece>() { origin };//pieces that need to be tested
        List<ChessboardPiece> testedPieces = new List<ChessboardPiece>(); //pieces that have been tested
        List<ChessboardPiece> piecesToAdd = new List<ChessboardPiece>(); //pieces queued to be added to untestedPieces and to templist
        List<ChessboardPiece> piecesToRemove = new List<ChessboardPiece>();//pieces queued to be removed from untestedPieces and added to TestedPieces
        if (includeOrigin) { tempList.Add(origin); }  // if we include the origin add it now
        for (int i = 0; i < distance; i++)//for as many spaces as we have in the distance variable
        {
            foreach(ChessboardPiece c in unTestedPieces)// for every untested piece. origin is added at the declaration of untested Pieces
            {
                List<ChessboardPiece> currentNeighbours = GetBoardPieceNeighbours(true,true,false,c);// get all the immediate horizontal and vertical neighbours
                foreach(ChessboardPiece cp in currentNeighbours)//for every space that is a neighbour check...
                {
                    if(!unTestedPieces.Contains(cp) && !testedPieces.Contains(cp) && !piecesToAdd.Contains(cp))//check if it is already in untested pieces (it is queued to check for neighbours if distance is high enough
                    {                                                                                           //if it has already been tested and doesn't need to be tested, or if its already been queued to be added to list
                        piecesToAdd.Add(cp);
                    }
                }
                piecesToRemove.Add(c);
            }
            while(piecesToAdd.Count > 0)
            {
                tempList.Add(piecesToAdd[0]);
                unTestedPieces.Add(piecesToAdd[0]);
                piecesToAdd.Remove(piecesToAdd[0]);
            }
            while(piecesToRemove.Count > 0)
            {
                unTestedPieces.Remove(piecesToRemove[0]);
                testedPieces.Add(piecesToRemove[0]);
                piecesToRemove.RemoveAt(0);
            }
        }
        //Debug.Log("the spots in rage count is " + tempList.Count.ToString());
        return tempList;
    }
    public List<ChessboardPiece>GetBoardPieceNeighbours(bool includeHorizontal,bool includeVertical, bool includeDiagonal,ChessboardPiece origin)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        if (includeVertical)
        {
            if (IsThisAValidPiecePosition(origin.xPos , origin.yPos + 1)) { tempList.Add(chessboardPieces[origin.xPos , origin.yPos +1]); }
            if (IsThisAValidPiecePosition(origin.xPos , origin.yPos - 1)) { tempList.Add(chessboardPieces[origin.xPos , origin.yPos -1]); }
        }
        if (includeHorizontal)
        {
            if (IsThisAValidPiecePosition(origin.xPos + 1, origin.yPos)) { tempList.Add(chessboardPieces[origin.xPos + 1, origin.yPos]); }
            if (IsThisAValidPiecePosition(origin.xPos - 1, origin.yPos)) { tempList.Add(chessboardPieces[origin.xPos - 1, origin.yPos]); }
        }
        if (includeDiagonal)
        {
            if (IsThisAValidPiecePosition(origin.xPos + 1, origin.yPos + 1)) { tempList.Add(chessboardPieces[origin.xPos + 1, origin.yPos + 1]); }
            if (IsThisAValidPiecePosition(origin.xPos - 1, origin.yPos - 1)) { tempList.Add(chessboardPieces[origin.xPos - 1, origin.yPos -1]); }
            if (IsThisAValidPiecePosition(origin.xPos - 1, origin.yPos + 1)) { tempList.Add(chessboardPieces[origin.xPos - 1, origin.yPos + 1]); }
            if (IsThisAValidPiecePosition(origin.xPos + 1, origin.yPos - 1)) { tempList.Add(chessboardPieces[origin.xPos + 1, origin.yPos - 1]); }
        }
        return tempList;
    }
    public List<ChessboardPiece> GetAllPiecesFromOriginInDirection(ChessboardPiece originPiece, Vector2 direction, bool moveOverUnit, bool includeMoveAttack, int distance,Player forPlayer)
{
        //Debug.Log("Getting all pieces from origin in direction");// will get an error if you dont have a unit on the origin and you set include move attack to true
    List<ChessboardPiece> tempList = new List<ChessboardPiece>();
    int currentX = originPiece.xPos;
    int currentY = originPiece.yPos;
    int xDirect = (int)direction.x;
    int yDirect = (int)direction.y;
    bool canProceedFurther = true;
    int numberOfMoves = 0;
    while(canProceedFurther){
        currentX += xDirect;
        currentY += yDirect;
        numberOfMoves++;
        if(IsThisAValidPiecePosition(currentX, currentY))//does this position even exist on the board?
        {
            ChessboardPiece currentPiece = chessboardPieces[currentX, currentY];
            if(currentPiece.hasChessPiece)//is there a chessPiece on this board Position?
            {
                //Yes this piece has a chessPiece
                if(currentPiece.currentChessPiece.owner.playerNumber != forPlayer.playerNumber || forPlayer == MainScript.neutralPlayer||moveOverUnit )//is it an enemy player's chessPiece? or send neutral player if it doesn't matter
                {
                    if(includeMoveAttack || forPlayer == MainScript.neutralPlayer)//are we including attack Moves? If so include this piece
                    {
                        tempList.Add(currentPiece);
                            if (!moveOverUnit) { canProceedFurther = false; }
                    }else//no we are not including attackMoves in this list
                    {
                        
                        if(!moveOverUnit)//if we cant move over units (like the knight does) we have to stop here
                        {
                            canProceedFurther = false;
                        }
                    }
                }else//no this chessPiece must belong to the player moving
                {
                    if (!moveOverUnit)//if we cant move over units (like the knight does) we have to stop here
                    {
                        canProceedFurther = false;
                    }
                    //you cannot move on top of your own chess Piece therefore do not use tempList.Add(currentPiece); in this case
                }
                
            }else //no you can Move to this piece
            {
                tempList.Add(currentPiece);
            }
        }
        else{
                if (!moveOverUnit) { canProceedFurther = false; }
        
        }
        if(distance > 0 && numberOfMoves >= distance) { canProceedFurther = false; }// setting distance to zero is effectively saying infinite distance
    }
        //Debug.Log("number of spaces given : " + tempList.Count.ToString() + " and the distance is " + distance.ToString() + " and the direction is " + direction.ToString());
        return tempList;
}
public bool IsThisAValidPiecePosition(int xPosition,int yPosition)
{
if(xPosition >= 0 && xPosition <= width - 1)
{
    if(yPosition >=0 && yPosition <= height - 1)
    {
        return true;
    }
}
return false;
}
}
public class Player
{
    public EnemyAI aiComponent;
    public int playerNumber = 0;
    public int resources = 0;
    public DeckOfCards deck;
    public HandOfCards hand;
    public DiscardPile discardPile;
    public Transform playerAvatar;
    public CardNumber bank;
    public PlayerType theType = PlayerType.AI;
    public List<Chesspiece> currentUnits;
    public List<Chesspiece> deceasedUnits;
    public List<ChessboardPiece> ownedBoardPieces;
    public List<EventListener> playerListeners;
    public List<FenceBeam> fenceBeams;
    public List<Vector2> positionsForCards;//a list of where on screen every card should go, it must be the same size as the hand or it will not have a position for every card
    public Player(int playersNumber)
    {
        fenceBeams = new List<FenceBeam>();
        ownedBoardPieces = new List<ChessboardPiece>();
        playerListeners = new List<EventListener>() { new EventListener(this,new List<GameEventType>() { GameEventType.beginTurn},"DrawCardsTurnBegin",MainScript.nullUnit) };
        //new EventListener(this, new List<GameEventType>() { GameEventType.endTurn }, "DiscardCardsTurnEnd")
        if (playersNumber != 2) { foreach (EventListener l in playerListeners) { MainScript.allEventListeners.Add(l); } }
        playerNumber = playersNumber;
        currentUnits = new List<Chesspiece>();
        deceasedUnits = new List<Chesspiece>();
        deck = new DeckOfCards();
        hand = new HandOfCards();
        discardPile = new DiscardPile(this);
        positionsForCards = new List<Vector2>();
        //deck.cardsInDeck = new List<Card>() { new Card(LibraryScript.cardLibrary[0], this), new Card(LibraryScript.cardLibrary[0], this), new Card(LibraryScript.cardLibrary[0], this), new Card(LibraryScript.cardLibrary[0], this) };
        //deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[0], this));
    }
    public EmptyReadableCardPrefabScript GetPlayerEmptyReadableCard()
    {
        if(theType == PlayerType.localHuman) { return MainScript.rightEmptyCard; } else { return MainScript.leftEmptyCard; }
    }
    public Color GetPlayerColor()
    {
        if (theType == PlayerType.localHuman) { return Color.green; } else { return Color.red; }
    }
    public void CreateDeck()
    {
        deck.cardsInDeck = new List<Card>();
        for (int i = 0;i < 1; i++)
        {
            //deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[1], this));deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[1], this));
            
            //deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[0].reference,this));
            //deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[1].reference,this));
            //deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[3],this));
            //deck.cardsInDeck.Add(new Card(LibraryScript.cardLibrary[2],this));
        }
        foreach (UnitReference u in LibraryScript.buildingLibrary)
        {
            CardReference c = u.cardReference;
            //deck.cardsInDeck.Add(new Card(c, this));
        }
        for(int i = 0; i < LibraryScript.buildingLibrary.Count;i++)
        {
            CardReference c = LibraryScript.buildingLibrary[i].cardReference;
            if (i != 1) { deck.cardsInDeck.Add(new Card(c, this)); }
        }
        if (theType == PlayerType.localHuman)
        {
            foreach (CardReference c in LibraryScript.startingDeck)
            {
                //Debug.Log("adding card " + c.cardName);
                deck.cardsInDeck.Add(new Card(c, this));
            }
            //deck.cardsInDeck.Add(new Card(BarracksScript.barracksRef.spellLibrary[2],this));
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                //deck.cardsInDeck.Add(new Card(LibraryScript.cardlessUnitLibrary[1].cardReference, this));
            }
        }
        
        //Debug.Log("finished createing deck");
    }
    public void UpdateHand()
    {
        if(theType == PlayerType.localHuman)
        {
            if (hand.cardsInHand.Count != positionsForCards.Count) { UpdatePositionsForCards(); }//is there a position for each card in hand? otherwise UpdateCardPositions
            for (int i = 0; i < hand.cardsInHand.Count; i++)//for every card in hand
            {
                //get the card and the position it SHOULD be in now
                Card currentCard = hand.cardsInHand[i];
                Vector2 currentPosition = positionsForCards[i];
                if (currentCard == MainScript.currentlySelectedCard)// if this card is not the currently Selected Card move it into the appropriateposition
                {
                    //currentPosition = new Vector2(currentPosition.x, currentPosition.y + (MainScript.cardHeight * 75f));//if the card is the one currently selected its objective position is higher than the rest to make it stand out
                    currentPosition += new Vector2(0f, (MainScript.cardHeight * 45f));
                }
                else if (MainScript.currentlySelectedUnit != MainScript.nullUnit)//if we have a unit selected, move the cards further down to make room for the unit cards
                {
                    currentPosition += new Vector2(0f, (MainScript.cardHeight * -65f));
                }
                if (MainScript.currentlySelectedUnit != MainScript.nullUnit)// if we have a unit selected Lower the cards even further to make room for the unit ability cards
                {
                    currentPosition += new Vector2(0f, (MainScript.cardHeight * -45f));
                }
                if ((Vector2)currentCard.rectTransform.localPosition != currentPosition)//is the card's position not the same as its positionForCards[] counterpart? is it not where it SHOULD be? then it must move towards position
                {
                    MoveCardToPoint(currentCard, currentPosition);
                }
            }
            if (MainScript.currentlySelectedUnit != MainScript.nullUnit)//if we have a unit selected
            {
                if (!MainScript.currentlySelectedUnit.canAttack) { MoveCardBelowScreen(MainScript.attackUnitCard); }
                if (!MainScript.currentlySelectedUnit.canMove) { MoveCardBelowScreen(MainScript.moveUnitCard); }
                for (int i = 0; i < MainScript.currentUnitCards.Count; i++)//go through all the current Unit's cards and move them to their proper position
                {
                    
                    Card c = MainScript.currentUnitCards[i];
                    Vector2 currentPosition = MainScript.currentUnitCardPositions[i];
                    if (c == MainScript.currentlySelectedCard) { currentPosition += new Vector2(0f, MainScript.cardHeight * 45f); 
                    }else if(MainScript.currentlySelectedCard != MainScript.nullCard)
                    {
                        currentPosition += new Vector2(0f, MainScript.cardHeight * -45f);
                    }
                    MoveCardToPoint(c, currentPosition);
                }
            }
            else //we have no unit selected, if the moveUnit or attack Unit cards are above the bottom of the screen move them downward
            {
                MoveCardBelowScreen(MainScript.attackUnitCard);
                MoveCardBelowScreen(MainScript.moveUnitCard);
            }
        }
        
    }
    
    void MoveCardBelowScreen(Card currentCard)
    {
        if(currentCard.rectTransform.localPosition.y > (0f - (Screen.height)))
        {
            currentCard.rectTransform.localPosition = (Vector3.down * Time.deltaTime * MainScript.defaultCardSpeed) + currentCard.rectTransform.localPosition;
        }
    }
    void MoveCardToPoint(Card currentCard, Vector2 currentPosition)
    {
        Vector2 directionToPosition = currentPosition - (Vector2)currentCard.rectTransform.localPosition;// get a vector from card to position
        float cardSpeedModifier = directionToPosition.magnitude / (Screen.height * 0.05f); //divide the total distance by a ratio of distance to increase speed if its further away
        if (cardSpeedModifier < 1f) { cardSpeedModifier = 1f; }//make sure the speed modifier never gets lower than one
        float distanceToMove = MainScript.defaultCardSpeed * Time.deltaTime * cardSpeedModifier; //calculate how much distance the card will move this frame
        if (distanceToMove > directionToPosition.magnitude) //is the distance we are moving this frame greater then the total distance to the appropriate position?
        {
            currentCard.rectTransform.localPosition = new Vector3(currentPosition.x, currentPosition.y, currentCard.rectTransform.localPosition.z);//move the card to the position directly
        }
        else //we will not move enough distance this frame to reach the position
        {
            currentCard.rectTransform.localPosition = currentCard.transform.localPosition + ((Vector3)directionToPosition.normalized * distanceToMove);
        }
    }
    void UpdatePositionsForCards()
    {
        int cards = hand.cardsInHand.Count;
        positionsForCards = new List<Vector2>(cards);
        int remainder = cards % 2;
        bool isUneven = (remainder > 0);
        float distanceFromCenter = MainScript.distanceBetweenCardsInHand * ((float)cards * 50f);
        Vector2 originPoint = new Vector2(0f - (distanceFromCenter ) + (MainScript.distanceBetweenCardsInHand * 50f), (Screen.height * -0.5f) + (MainScript.cardHeight * 15f));
        for(int i = 0;i < cards; i++)
        {
            positionsForCards.Add(originPoint + ((float)i * MainScript.distanceBetweenCardsInHand * Vector2.right * 100f));
        }
    }
}
public enum PlayerType { localHuman,AI,proxyHuman}
public class GameEvent
{
    //anything that alters the events of the game, on the board or in the various decks of the player or their hand. any change in the game is done through events in a queue
    public GameEventType theType;
    public bool requiresAnimation = true;
    public bool relevantBool = false;
    public float animationTime = 1.5f;
    public int attack = 0;
    public int utility = 0;
    public int defence = 0;
    public List<ChessboardPiece> theActor;
    public List<ChessboardPiece> theTarget;
    public List<Chesspiece> relevantUnits;
    public List<Card> relevantCards;
    public List<UnitBuff> relevantBuffs;
    public List<CardReference> relevantCardReferences;
    public Player targetPlayer = MainScript.neutralPlayer;
    public UnitReference unitToSummon;
    public EventListener relevantListener = MainScript.nullEventListener;
    public GameEvent(GameEventType theTypeToApply, List<ChessboardPiece> actors, List<ChessboardPiece> targets, Player playerTargetted)
    {
        relevantBuffs = new List<UnitBuff>();
        theType = theTypeToApply;
        theActor = actors;
        theTarget = targets;
        targetPlayer = playerTargetted;
        unitToSummon = MainScript.nullUnitReference;
        relevantUnits = new List<Chesspiece>();
        foreach(ChessboardPiece c in theActor) { if (c.hasChessPiece) { relevantUnits.Add(c.currentChessPiece); } }
        foreach(ChessboardPiece c in theTarget) { if (c.hasChessPiece) { relevantUnits.Add(c.currentChessPiece); } }
    }
    public static GameEvent GetGrenadeEvent(CardReference grenadeReference, Chesspiece thrower, ChessboardPiece target)
    {
        return new GameEvent(grenadeReference, thrower, target);
        //GameEvent temp = new GameEvent()
    }
    public static GameEvent GetGrenadeEvent(Card grenadeReference, Chesspiece thrower, ChessboardPiece target)
    {
        return new GameEvent(grenadeReference, thrower, target);
        //GameEvent temp = new GameEvent()
    }
    //public static GameEvent GetPurchaseCardEvent(CardReference cardBeingBought,)
    public static GameEvent GetInterceptEvent(CardReference reference,Chesspiece thrower,Chesspiece interceptor, ChessboardPiece interceptPoint)
    {
        GameEvent temp = new GameEvent(reference, thrower, interceptPoint, interceptor);
        temp.theType = GameEventType.interceptGrenade;
        return temp;
    }
    public static GameEvent GetPlayerChoosesEvent(Player playerWhoChose,CardReference originalCard)
    {
        GameEvent temp = new GameEvent(GameEventType.playerChoosesACard);
        //temp.relevantCards = new List<Card>() { cardChosen };//the chosen card is added after it is chosen. the player chooses event is added after the choose from card event. once the card is chosen it will insert it into the event
        temp.targetPlayer = playerWhoChose;
        temp.relevantCardReferences = new List<CardReference>() { originalCard };
        return temp;
    }
    public static GameEvent GetChooseFromCardsEvent(List<CardReference> cardsToChooseFrom,Card originalCard, Player playerChoosing,bool keepSelectedCardAlive)
    {
        GameEvent temp = new GameEvent(GameEventType.playerChoosesFromCards);
        temp.relevantBool = keepSelectedCardAlive;
        temp.relevantCardReferences = cardsToChooseFrom;
        temp.relevantCards = new List<Card>() { originalCard };
        temp.targetPlayer = playerChoosing;
        return temp;
    }
    public static GameEvent GetSetupFenceEvent(ChessboardPiece a, ChessboardPiece b)
    {
        return new GameEvent(a, b);
    }
    public static GameEvent GetEndTurnEvent()
    {
        return new GameEvent(GameEventType.endTurn);
    }
    public static GameEvent GetSummonEventMultiple(UnitReference u, List<ChessboardPiece> targets, Player owner)
    {
        return new GameEvent(u, targets , owner);
    }
    public static GameEvent GetSummonEvent(UnitReference u, ChessboardPiece target,Player owner)
    {
        return new GameEvent(u, new List<ChessboardPiece>() { target }, owner);
    }
    public static GameEvent GetSummonDyingUnitEvent(UnitReference u, ChessboardPiece target, Player owner)
    {
        GameEvent temp = new GameEvent(u, new List<ChessboardPiece>() { target }, owner);
        temp.theType = GameEventType.summonDyingUnit;
        return temp;
    }
    public static GameEvent GetPlayCardEvent(Card c, Player owner,ChessboardPiece target)
    {
        return new GameEvent(c, owner,target);
    }
    public static GameEvent GetPlayCardEventWithSelectedUnitSecondaryTarget(Card c, Player owner, ChessboardPiece target, ChessboardPiece secondaryTarget, Chesspiece currentlySelectedUnit)
    {
        GameEvent temp = new GameEvent(c, owner, target,secondaryTarget);
        temp.relevantUnits = new List<Chesspiece>();
        if (currentlySelectedUnit != MainScript.nullUnit) { temp.relevantUnits.Add(currentlySelectedUnit); }
        if (target.hasChessPiece && c.targetType != CardTargetType.emptyBoardPiece) { temp.relevantUnits.Add(target.currentChessPiece); }
        
        foreach(Chesspiece unit in temp.relevantUnits)
        {
            //Debug.Log("game event relevant unit " + unit.unitRef.unitName);
        }
        return temp;
    }
    public static GameEvent GetEmptyGameEvent(GameEventType t)
    {
        GameEvent temp = new GameEvent(t);
        temp.requiresAnimation = false;
        temp.theTarget = new List<ChessboardPiece>();
        temp.theActor = new List<ChessboardPiece>();
        temp.relevantUnits = new List<Chesspiece>();
        return temp;
    }
    public static GameEvent GetApplyPoisonEvent(List<ChessboardPiece> piecesToApplyPoison)
    {
        return new GameEvent(piecesToApplyPoison, GameEventType.applyPoison);
    }
    public GameEvent(List<ChessboardPiece> c, GameEventType t)
    {
        theType = t;
        theTarget = c;
    }
    public static GameEvent GetTossUnitEvent(Chesspiece unitToToss,ChessboardPiece destination)
    {
        GameEvent temp = GameEvent.GetMoveEvent(unitToToss, destination);
        temp.theType = GameEventType.throwUnit;
            return temp;
        //return new GameEvent(new List<Chesspiece>() { unitToToss },new List<ChessboardPiece>() { destination })
    }
    public static GameEvent GetMoveEvent(Chesspiece unitToMove,ChessboardPiece destination)
    {
        return new GameEvent(new List<ChessboardPiece>() { unitToMove.currentBoardPiece }, new List<ChessboardPiece>() { destination });
    }
    public GameEvent(List<ChessboardPiece> unitsToMove,List<ChessboardPiece> theirDestinations)
    {
        theType = GameEventType.moveUnit;
        theActor = unitsToMove;
        theTarget = theirDestinations;
    }
    public GameEvent(EventListener listenerBeingActivated)
    {
        theType = GameEventType.activateEventListener;
        relevantListener = listenerBeingActivated;
    }
    public GameEvent(UnitReference summonUnit,List<ChessboardPiece> summonLocations,Player owner)
    {
        relevantUnits = new List<Chesspiece>();
        theActor = new List<ChessboardPiece>();
        theType = GameEventType.summonUnit;
        unitToSummon = summonUnit;
        theTarget = summonLocations;
        targetPlayer = owner;
    }
    public GameEvent(CardReference grenadeCardReference, Chesspiece thrower, ChessboardPiece target,Chesspiece interceptor)
    {
        theType = GameEventType.throwGrenade;
        relevantCardReferences = new List<CardReference>() { grenadeCardReference };
        theTarget = new List<ChessboardPiece>() { target };
        relevantUnits = new List<Chesspiece>() { thrower ,interceptor};
    }
    public GameEvent(Card grenadeCardReference,Chesspiece thrower, ChessboardPiece target)
    {
        theType = GameEventType.throwGrenade;
        relevantCards = new List<Card>() { grenadeCardReference };
        relevantCardReferences = new List<CardReference>() { grenadeCardReference.cardReference };
        theTarget = new List<ChessboardPiece>() { target };
        relevantUnits = new List<Chesspiece>() { thrower };
    }
    public GameEvent(CardReference grenadeCardReference, Chesspiece thrower, ChessboardPiece target)
    {
        theType = GameEventType.throwGrenade;
        //relevantCards = new List<Card>() { grenadeCardReference };
        relevantCardReferences = new List<CardReference>() { grenadeCardReference };
        theTarget = new List<ChessboardPiece>() { target };
        relevantUnits = new List<Chesspiece>() { thrower };
    }
    public GameEvent(ChessboardPiece a, ChessboardPiece b)
    {
        theTarget = new List<ChessboardPiece>() { a, b };
        theType = GameEventType.setupFence;
    }
    public GameEvent(Card unitCard, List<ChessboardPiece> summonLocations, Player owner)
    {
        requiresAnimation = false;
        theType = GameEventType.playCard;
        unitToSummon = unitCard.reference;
        relevantCards = new List<Card>() { unitCard };
        theTarget = summonLocations;
        targetPlayer = owner;
    }
    public GameEvent(Card onPlayEffectUnitCard, List<ChessboardPiece> summonLocations, Player owner, List<ChessboardPiece> targets)
    {
        requiresAnimation = false;
        theType = GameEventType.playCard;
        unitToSummon = onPlayEffectUnitCard.reference;
        relevantCards = new List<Card>() { onPlayEffectUnitCard };
        theTarget = summonLocations;
        theActor = targets;
        targetPlayer = owner;
    }
    public GameEvent(List<ChessboardPiece> unitsToDealDamageTo,int theAmount)
    {
        theType = GameEventType.dealDamage;
        theTarget = unitsToDealDamageTo;
        utility = theAmount;
        requiresAnimation = false;
    }
    public GameEvent(List<Chesspiece> unitsToDealDamageTo, int theAmount)
    {
        theType = GameEventType.dealDamage;
        relevantUnits = unitsToDealDamageTo;
        utility = theAmount;
        requiresAnimation = true;
    }
    public static GameEvent GetDeathEvent(List<Chesspiece> unitsToKill)
    {
        List<ChessboardPiece> list = new List<ChessboardPiece>();
        foreach(Chesspiece c in unitsToKill)
        {
            list.Add(c.currentBoardPiece);
        }
        return new GameEvent(list);
    }
    public GameEvent(List<ChessboardPiece> deathLocations)
    {
        relevantBuffs = new List<UnitBuff>();
        theType = GameEventType.destroyMinion;
        theTarget = deathLocations;
    }
    public static GameEvent GetBuffEvent(UnitBuff b, List<Chesspiece> unitsToBuff)
    {
        GameEvent temp = new GameEvent(GameEventType.buffUnit);
        temp.relevantBuffs = new List<UnitBuff>() { b };
        temp.relevantUnits = unitsToBuff;
        return temp;
    }
    public GameEvent(Player owner, int util, GameEventType typeToApply)
    {
        relevantBuffs = new List<UnitBuff>();
        targetPlayer = owner;
        utility = util;
        theType = typeToApply;
    }
    public GameEvent(Card playThisCard,Player owner, ChessboardPiece targetSpot)
    {
        relevantBuffs = new List<UnitBuff>();
        targetPlayer = owner;
        relevantCards = new List<Card>() { playThisCard };
        theTarget = new List<ChessboardPiece>() { targetSpot };
        relevantUnits = new List<Chesspiece>();
        if (targetSpot.hasChessPiece) relevantUnits.Add(targetSpot.currentChessPiece);
        theType = GameEventType.playCard;
        requiresAnimation = true;
        theActor = new List<ChessboardPiece>();
        relevantUnits = new List<Chesspiece>();
    }
    public GameEvent(Card playThisCard, Player owner, ChessboardPiece targetSpot,ChessboardPiece secondaryTarget)
    {
        relevantBuffs = new List<UnitBuff>();
        targetPlayer = owner;
        relevantCards = new List<Card>() { playThisCard };
        theTarget = new List<ChessboardPiece>() { targetSpot };
        relevantUnits = new List<Chesspiece>();
        if (targetSpot.hasChessPiece) { relevantUnits.Add(targetSpot.currentChessPiece); }
        theActor = new List<ChessboardPiece>() { secondaryTarget};
        if (secondaryTarget.hasChessPiece) { relevantUnits.Add(secondaryTarget.currentChessPiece); }
        theType = GameEventType.playCard;
        requiresAnimation = true;
    }
    public GameEvent(Card playThisCard, Player owner, Chesspiece targetUnit)
    {
        relevantBuffs = new List<UnitBuff>();
        targetPlayer = owner;
        relevantCards = new List<Card>() { playThisCard };
        relevantUnits = new List<Chesspiece>() { targetUnit };
        theType = GameEventType.playCard;
        //requiresAnimation = false;
        //animationTime = 0f;
    }
    public GameEvent(List<Card> cardsToResearch,Chesspiece unitResearching)
    {
        relevantBuffs = new List<UnitBuff>();
        if(unitResearching.alive && unitResearching != MainScript.nullUnit)
        {
            relevantUnits = new List<Chesspiece>() { unitResearching };
        }
        theType = GameEventType.researchCard;
        relevantCards = cardsToResearch;
    }
    public static GameEvent GetAttackEvent(Chesspiece attackingUnit, Chesspiece defendingUnit)
    {
        GameEvent temp =  new GameEvent(attackingUnit.owner, new List<ChessboardPiece>() { attackingUnit.currentBoardPiece }, new List<ChessboardPiece>() { defendingUnit.currentBoardPiece });
        temp.relevantUnits = new List<Chesspiece>() { attackingUnit, defendingUnit };
        return temp;
        
    }
    public static GameEvent GetMoveAttackFromBoardPieces(ChessboardPiece attacker, ChessboardPiece defender,Player attackingPlayer)
    {
        //Debug.Log(attackingPlayer.playerNumber);
        return new GameEvent(new List<ChessboardPiece>() { attacker }, new List<ChessboardPiece>() { defender }, attackingPlayer);
    }
    public static GameEvent GetMoveAttackEvent(Chesspiece attackingUnit, Chesspiece defendingUnit)
    {
        GameEvent temp = new GameEvent(new List<ChessboardPiece>() { attackingUnit.currentBoardPiece }, new List<ChessboardPiece>() { defendingUnit.currentBoardPiece },attackingUnit.owner);
        temp.relevantUnits = new List<Chesspiece>() { attackingUnit, defendingUnit };
        return temp;
    }
    public GameEvent(Player attackingPlayer,List<ChessboardPiece> actors, List<ChessboardPiece> targets)
    {
        relevantBuffs = new List<UnitBuff>();
        theType = GameEventType.attackUnit;
        targetPlayer = attackingPlayer;
        theActor = actors;
        theTarget = targets;
    }
    public GameEvent(List<ChessboardPiece> moveAttacker, List<ChessboardPiece> moveAttackTargets,Player moveAttackingPlayer)
    {
        relevantBuffs = new List<UnitBuff>();
        theType = GameEventType.moveAttack;
        targetPlayer = moveAttackingPlayer;
        theActor = moveAttacker;
        theTarget = moveAttackTargets;
        relevantUnits = new List<Chesspiece>() { moveAttacker[0].currentChessPiece, theTarget[0].currentChessPiece };
    }
    public GameEvent(Chesspiece unitWithDeathEffect)
    {
        relevantBuffs = new List<UnitBuff>();
        theType = GameEventType.applyOnDeathEffect;
        relevantUnits = new List<Chesspiece>() { unitWithDeathEffect };
    }
    public static GameEvent GetBuffEvent(Chesspiece unitToBuff, int attackBuff, int healthBuff, int utilityBuff, bool changeCurrentHealth)
    {
        return new GameEvent(unitToBuff,  attackBuff,  healthBuff,  utilityBuff,  changeCurrentHealth, GameEventType.buffUnit);
    }
    public static GameEvent CreatePoisonAddingEvent(List<ChessboardPiece> targetPieces, int amountOfToxinToAdd)
    {
        GameEvent temp = new GameEvent(targetPieces, amountOfToxinToAdd);
        temp.theType = GameEventType.AddPoisonToPieces;
        temp.requiresAnimation = true;
        return temp;
    }
    public GameEvent(Chesspiece unitToBuff,int attackBuff,int healthBuff,int utilityBuff, bool changeCurrentHealth, GameEventType changeType)// for buffing or nerfing stats
    {
        relevantBuffs = new List<UnitBuff>();
        requiresAnimation = true;
        theType = changeType;
        relevantUnits = new List<Chesspiece>() { unitToBuff };
        attack = attackBuff;
        utility = utilityBuff;
        defence = healthBuff;
        relevantBool = changeCurrentHealth;
    }
    public static GameEvent GetIncomeEventForPlayer(Player p)
    {
        GameEvent temp = new GameEvent(GameEventType.playerIncome);
        temp.targetPlayer = p;
        return temp;
    }
    public GameEvent(GameEventType emptyEventType)//for things that dont require any actual doing but need to be announced for listeners, i.e.: begin turn
    {
        relevantBuffs = new List<UnitBuff>();
        requiresAnimation = true;
        theType = emptyEventType;
    }
}
public enum GameEventType 
{ 
    moveUnit, attackUnit, playCard, summonUnit,summonDyingUnit ,drawCard, destroyMinion,
    moveAttack,dealDamage,applyOnDeathEffect, changeHealth,changeAttack,activateEventListener, 
    beginTurn,beginGame,endTurn,discardCard,reshuffleDeck,researchCard,onPlayEffect,pushUnits,
    applyPoison,buffUnit, AddPoisonToPieces,throwUnit,setupFence,throwGrenade,interceptGrenade,
    playerIncome,playerChoosesFromCards,playerChoosesACard,playerViewsCards
}
public class Counter
{
    public float currentTime = 0f;
    public float endTime = 0f;
    public bool hasFinished = false;
    public Counter (float theEndTime)
    {
        endTime = theEndTime;
        currentTime = 0f;
    }
    public void ResetCounter()
    {
        currentTime = 0f;
        hasFinished = false;
    }
    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
        if(currentTime >= endTime) { currentTime = endTime;hasFinished = true; }
    }
    public float GetPercentageDone()
    {
        return currentTime / endTime;
    }
}
public class UnitReferenceModifier //for changing or manipulating the storefront. if we want to make something available after research is done this is how it is done
{
    public UnitReferenceModifierType theType;
    public int attack = 0;
    public int health = 0;
    public int cost = 0;
    public List<CardReference> relevantCards;
    public Player owner = MainScript.neutralPlayer;//the owner dictates who the modifier affects. if you get an upgrade and add a modifier to a card reference it then affects only you. neutral player means it affects everyone
    public void SetAttack(int atk) { attack = atk; }
    public void SetHealth(int hlt) { health = hlt; }
    public void SetCost(int cos) { cost = cos; }

    public UnitReferenceModifier(UnitReferenceModifierType t, List<CardReference> cards,Player owns) { theType = t; relevantCards = cards; owner = owns; }
}
public enum UnitReferenceModifierType { addToStorefront,addToAbilities,removeFromAbilities,removeFromStorefront,addToResearchableCards,removeFromResearchableCards, addStats}
public class UnitReference
{
    public bool hasEventListeners = false;
    public bool hasOnPlayAnimation = false;
    public BuildingReference buildingRefParent;
    public AttackType attackType = AttackType.shoot;
    public List<UnitReferenceModifier> modifiers;
    public List<CardReference> storeFront = new List<CardReference>();
    public List<CardReference> researchStoreFront = new List<CardReference>();
    public int attack;
    public int defence;
    public int utility;
    public int cost;
    public int attackRange;
    public string unitPrefabName;
    public string unitCardSpriteName;
    public string unitName;
    public bool canAttack = true;
    public bool canMove = true;
    public bool applyOnDeathEffect = false;
    public bool applyOnPlayEffect = false;
    public CardTargetType onPlayEffectTargetType = CardTargetType.requiresNoTarget;
    public ChessPieceMovementAbilityType movementType = ChessPieceMovementAbilityType.none;
    public int movementDistance = 0;
    public bool canMoveOverUnits = false;
    public bool requiresSecondTarget = false;
    public List<GameEventType> eventListeners;
    public CardReference cardReference; //is added later when the Library makes a card Reference for every Unit Reference
    public UnitReference(string nameOfUnit, int atk,int def,int util,int cos,string prefabName, int moveDist, ChessPieceMovementAbilityType theMoveType,int atkRange,string cardSpriteName,AttackType atkType)
    {
        attackType = atkType;
        researchStoreFront = new List<CardReference>();
        modifiers = new List<UnitReferenceModifier>();
        storeFront = new List<CardReference>();
        unitName = nameOfUnit;
        attackRange = atkRange;
        attack = atk;
        defence = def;
        utility = util;
        cost = cos;
        unitPrefabName = prefabName;
        movementDistance = moveDist;
        movementType = theMoveType;
        unitCardSpriteName = cardSpriteName;
        eventListeners = new List<GameEventType>();
    }
    public void SetupToDieAtTurnEnd()
    {
        //eventListeners.Add
    }
    /*public static UnitReference GetBuilding(UnitReference r)
    {
        UnitReference temp = new UnitReference(r.unitName, r.attack, r.defence, r.utility, r.cost, r.unitPrefabName, r.movementDistance, r.movementType, r.attackRange, r.unitCardSpriteName);
    return temp;
    }*/
    public UnitReference(string nameOfUnit, int atk, int def, int util, int cos, string prefabName, int moveDist, ChessPieceMovementAbilityType theMoveType, int atkRange, string cardSpriteName,List<GameEventType> listeners)
    {
        researchStoreFront = new List<CardReference>();
        modifiers = new List<UnitReferenceModifier>();
        storeFront = new List<CardReference>();
        eventListeners = listeners;
        unitName = nameOfUnit;
        attackRange = atkRange;
        attack = atk;
        defence = def;
        utility = util;
        cost = cos;
        unitPrefabName = prefabName;
        movementDistance = moveDist;
        movementType = theMoveType;
        unitCardSpriteName = cardSpriteName;
    }
    public UnitReference(string nameOfUnit, int atk, int def, int util, int cos, string prefabName, int moveDist, ChessPieceMovementAbilityType theMoveType, int atkRange, string cardSpriteName,bool hasOnDeathEffect)
    {
        researchStoreFront = new List<CardReference>();
        modifiers = new List<UnitReferenceModifier>();
        storeFront = new List<CardReference>();
        unitName = nameOfUnit;
        attackRange = atkRange;
        attack = atk;
        defence = def;
        utility = util;
        cost = cos;
        unitPrefabName = prefabName;
        movementDistance = moveDist;
        movementType = theMoveType;
        unitCardSpriteName = cardSpriteName;
        applyOnDeathEffect = hasOnDeathEffect;
        eventListeners = new List<GameEventType>();
    }
    public UnitReference(string nameOfUnit, int atk, int def, int util, int cos, string prefabName, int moveDist, ChessPieceMovementAbilityType theMoveType, int atkRange, string cardSpriteName, bool hasOnDeathEffect, bool hasOnPLayEffect,CardTargetType requiredOnPlayTarget,AttackType atkType)
    {
        attackType = atkType;
        researchStoreFront = new List<CardReference>();
        modifiers = new List<UnitReferenceModifier>();
        storeFront = new List<CardReference>();
        unitName = nameOfUnit;
        attackRange = atkRange;
        attack = atk;
        defence = def;
        utility = util;
        cost = cos;
        unitPrefabName = prefabName;
        movementDistance = moveDist;
        movementType = theMoveType;
        unitCardSpriteName = cardSpriteName;
        applyOnDeathEffect = hasOnDeathEffect;
        applyOnPlayEffect = hasOnPLayEffect;
        //requiresSecondTarget = hasOnPLayEffect;
        onPlayEffectTargetType = requiredOnPlayTarget;
        if(onPlayEffectTargetType != CardTargetType.requiresNoTarget)
        {
            //Debug.Log(nameOfUnit + " require a secondary target");
            requiresSecondTarget = true;//defaults to false
        }
        eventListeners = new List<GameEventType>();
    }
    public List<CardReference> GetStorefront()
    {
        List<CardReference> temp = new List<CardReference>();
        foreach(CardReference c in storeFront)
        {
            temp.Add(c);
        }
        foreach(UnitReferenceModifier uMod in modifiers)
        {
            switch (uMod.theType)
            {
                case UnitReferenceModifierType.addToStorefront:
                    foreach (CardReference c in uMod.relevantCards)
                    {
                        if (!temp.Contains(c)) { temp.Add(c); }
                    }
                        
                    break;
                case UnitReferenceModifierType.removeFromStorefront:
                    foreach(CardReference c in uMod.relevantCards)
                    {
                        if (temp.Contains(c)) { temp.Remove(c); }
                    }
                    break;
            }
        }
        return temp;
    }
    public List<CardReference> GetResearchStorefront(Player p)
    {
        List<CardReference> temp = new List<CardReference>();
        foreach (CardReference c in researchStoreFront)
        {
            temp.Add(c);
        }
        foreach (UnitReferenceModifier uMod in modifiers)
        {
            switch (uMod.theType)
            {
                case UnitReferenceModifierType.addToResearchableCards:
                    foreach (CardReference c in uMod.relevantCards)
                    {
                        if (!temp.Contains(c)) { temp.Add(c); }
                    }
                    break;
                case UnitReferenceModifierType.removeFromResearchableCards:
                    foreach (CardReference c in uMod.relevantCards)
                    {
                        if (temp.Contains(c)) { temp.Remove(c); }
                    }
                    break;
            }
        }
        return temp;
    }
    public List<CardReference> GetAbilities(List<CardReference> originalAbilities)
    {
        List<CardReference> temp = new List<CardReference>();
        temp.AddRange(originalAbilities);
        foreach (UnitReferenceModifier uMod in modifiers)
        {
            switch (uMod.theType)
            {
                case UnitReferenceModifierType.addToAbilities:
                    foreach (CardReference c in uMod.relevantCards)
                    {
                        if (!temp.Contains(c)) { temp.Add(c); }
                    }
                    break;
                case UnitReferenceModifierType.removeFromAbilities:
                    foreach (CardReference c in uMod.relevantCards)
                    {
                        if (temp.Contains(c)) { temp.Remove(c); }
                    }
                    break;
            }
        }
        return temp;
    }
    public int GetCostForOwner(Player owns)
    {
        int baseCost = cardReference.cost;//get the base cost
        foreach(UnitReferenceModifier uMod in modifiers)
        {
            if(uMod.theType == UnitReferenceModifierType.addStats)
            {
                baseCost += uMod.cost;
            }
        }
        return baseCost;
    }
    public int GetAttackForOwner(Player owns)
    {
        int baseAttack = attack;
        foreach(UnitReferenceModifier mod in modifiers)
        {
            if(mod.owner.playerNumber == owns.playerNumber && mod.theType == UnitReferenceModifierType.addStats)
            {
                baseAttack += mod.attack;
            }
        }
        return baseAttack;
    }
    public int GetDefenceForOwner(Player owns)
    {
        int baseAttack = defence;
        foreach (UnitReferenceModifier mod in modifiers)
        {
            if (mod.owner.playerNumber == owns.playerNumber && mod.theType == UnitReferenceModifierType.addStats)
            {
                baseAttack += mod.health;
            }
        }
        return baseAttack;
    }
    public void ApplyOnDeathEffect() { applyOnDeathEffect = true; }
}
public class EventListener
{
    public List<GameEventType> typeOfEventToListenFor;
    public string listenerName;
    public Chesspiece unitOwner = MainScript.nullUnit;
    public List<Chesspiece> unitOwners;
    public Player playerOwner = MainScript.neutralPlayer;
    public bool destroyOnActivation = false;
    public bool poolAllListenersTogether = false;
    public bool waitingToDie = false;
    public List<ChessboardPiece> targets;
    public bool disabledUntilEndOfQueue = false;// for when an interupting listener needs to make sure it doesn't constantly activate over and over if it interupts and delays the event that triggers it
    public EventListener(Chesspiece unitWithListener, List<GameEventType> eventsToWatchFor)
    {
        unitOwners = new List<Chesspiece>();
        listenerName = unitWithListener.unitRef.unitName;
        unitOwner = unitWithListener;
        typeOfEventToListenFor = eventsToWatchFor;
    }
    public EventListener(UnitReference u,List<GameEventType> eventsToWatchFor)
    {
        listenerName = u.unitName;
        unitOwners = new List<Chesspiece>();
        typeOfEventToListenFor = eventsToWatchFor;
    }
    public EventListener (Player ownerOfPlayer, List<GameEventType> eventsToWatchFor, string nameOfListener,Chesspiece unitOwner)//use null unit if it doesn't have a unit like draw cards begin turn
    {
        unitOwners = new List<Chesspiece>();
        listenerName = nameOfListener;
        playerOwner = ownerOfPlayer;
        typeOfEventToListenFor = eventsToWatchFor;
    }
    public static EventListener GetMultipleUnitListener(List<Chesspiece> owners,string nameOfListener,List<GameEventType> eventsToWatchFor)
    {
        EventListener temp = new EventListener(owners[0].owner, eventsToWatchFor, nameOfListener, owners);
        temp.listenerName = nameOfListener;
        return temp;
    }
    public EventListener(Player ownerOfPlayer, List<GameEventType> eventsToWatchFor, string nameOfListener, List<Chesspiece> owners)//use null unit if it doesn't have a unit like draw cards begin turn
    {
        unitOwners = owners;
        unitOwner = unitOwners[0];
        listenerName = nameOfListener;
        playerOwner = ownerOfPlayer;
        typeOfEventToListenFor = eventsToWatchFor;
    }
    public EventListener()
    {

        unitOwners = new List<Chesspiece>();
        typeOfEventToListenFor = new List<GameEventType>();
        unitOwner = MainScript.nullUnit;
        listenerName = "none";
    }
}