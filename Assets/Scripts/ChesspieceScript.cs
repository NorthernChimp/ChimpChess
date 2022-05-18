using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChesspieceScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Chesspiece thisChessPiece;
    public Animator thisAnimator;
    public SkinnedMeshRenderer thisMeshRenderer;
    
    public MeshRenderer render;
    
    void Start()
    {
        
    }
    public void SetupChesspiece()
    {
        thisChessPiece.skinRender = thisMeshRenderer;
        thisChessPiece.thisAnim = thisAnimator;
        thisChessPiece.render = render;
        thisChessPiece.SetupAnimation("idle", 4f);
        //Debug.Log("setting up chesspiece" + thisChessPiece.unitRef.cardReference.cardType);
    }
    private void OnMouseEnter()
    {
        thisChessPiece.mouseOver = true;
    }
    private void OnMouseExit()
    {
        thisChessPiece.mouseOver = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum AttackType { shoot,melee}
public class Chesspiece
{
    public Animator thisAnim;
    public ChessboardPiece currentBoardPiece;
    public ChessboardPiece deathTarget = MainScript.nullBoardPiece;
    public List<CardReference> cardsPlayedThisTurn = new List<CardReference>();
    public int numberOfAttacks = 1;//defaults to one, maybe can be upped through cards or whatever
    public int numberOfAttacksThisTurn = 0;
    public int numberOfMoves = 1;//defaults to one, maybe can be upped through cards or whatever
    public int numberOfMovesThisTurn = 0;
    public bool canShuffleStorefrontIntoDeck = false;
    public int attack = 0;
    public int defence = 0;
    public int utility = 0;
    public int evasion = 0;
    public int attackRange = 0;
    public int currentHealth;
    public int maxHealth;
    public AttackType attackType = AttackType.shoot;
    public Transform stunTransform = null;
    public bool canAttack = true;
    public bool canMove = true;
    public bool mouseOver = false;
    public bool alive = true;
    public bool queuedForDeletion = false;
    public bool stunned = false;
    public bool hasStunTransform = false;
    public List<Ability> allAbilities;
    public UnitReference unitRef;
    public Transform transform;
    public Player owner;
    public MeshRenderer render;
    public SkinnedMeshRenderer skinRender;
    public List<UnitBuff> buffs;
    public bool hasOnDeathEffect = false;
    public ChessPieceMovementAbilityType movementType = ChessPieceMovementAbilityType.none;
    public int movementdistance = 0;
    public List<EventListener> listeners;
    public Chesspiece()
    {

    }
    public void EndOfTurnUpdate()
    {
        cardsPlayedThisTurn = new List<CardReference>();
        numberOfAttacksThisTurn = 0;
        if (unitRef.canAttack) { canAttack = true; }
        if (unitRef.canMove) { canMove = true; }
    }
    public Chesspiece(ChessboardPiece startingPiece, Transform t,Player theOwner, UnitReference theRef)
    {
        buffs = new List<UnitBuff>();
        attackType = theRef.attackType;
        canAttack = theRef.canAttack;
        canMove = theRef.canMove;
        MainScript.currentGame.allUnits.Add(this);
        transform = t;
        render = t.GetComponent<MeshRenderer>();
        skinRender = t.GetComponent<SkinnedMeshRenderer>();
        currentBoardPiece = startingPiece;
        currentBoardPiece.currentChessPiece = this;
        currentBoardPiece.hasChessPiece = true;
        owner = theOwner;
        //owner.currentUnits.Add(this);
        unitRef = theRef;
        attack = unitRef.GetAttackForOwner(theOwner);
        currentHealth = unitRef.GetDefenceForOwner(theOwner);
        maxHealth = unitRef.GetDefenceForOwner(theOwner);
        movementType = theRef.movementType;
        attackRange = unitRef.attackRange;
        //movementType = unitRef.movementType;
        movementdistance = theRef.movementDistance;
        hasOnDeathEffect = theRef.applyOnDeathEffect;
        if (theRef.eventListeners.Count > 0)
        {
            listeners = new List<EventListener>() { new EventListener(this, theRef.eventListeners) };
            MainScript.allEventListeners.AddRange(listeners);
        }
        else { listeners = new List<EventListener>(); }
    }
    public float GetAnimationLength(string animationName)
    {
        //Debug.Log(unitRef.unitName);
        for(int i = 0;i < thisAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if(thisAnim.runtimeAnimatorController.animationClips[i].name == animationName)
            {
                return thisAnim.runtimeAnimatorController.animationClips[i].length;
            }
        }
        return 0f;
    }
    public bool GetBuilding() { return (unitRef.cardReference.cardType == CardType.building); }
    public Color GetColor()
    {
        if(unitRef.cardReference.cardType == CardType.building) { return render.material.color; } else
        {
            if (skinRender) { return skinRender.material.color; } else { return render.material.color; }
        }
    }
    public void SetupAnimation(string nameOfAnim, float timeForAnim)
    {
        //if(unitRef.cardReference.cardType != CardType.building)
        if(!LibraryScript.buildingLibrary.Contains(unitRef))
        {
             //Debug.Log("setting up animation " + nameOfAnim + " on " + unitRef.unitName);
            thisAnim.Play(nameOfAnim);
            float lengthOfAnim = GetAnimationLength(nameOfAnim);
            thisAnim.speed = lengthOfAnim / timeForAnim;
        }
        
    }
    public void SetupDeathAtEndOfTurn()
    {
        //Debug.Log("setting unit up to die at end of turn");
        EventListener killThisUnit = new EventListener(this, new List<GameEventType>() { GameEventType.endTurn });
        killThisUnit.listenerName = "dieAtTheEndOfTurn";
        MainScript.allEventListeners.Add(killThisUnit);
    }
    public void BasicAnimation(string nameOfAnim)
    {
        float timeForAnim = GetAnimationLength(nameOfAnim);
        if (unitRef.cardReference.cardType != CardType.building)
        {
            //Debug.Log("setting up animation " + nameOfAnim);
            thisAnim.Play(nameOfAnim);
            float lengthOfAnim = GetAnimationLength(nameOfAnim);
            thisAnim.speed = lengthOfAnim / timeForAnim;
        }

    }
    public void CalculateBuffs()
    {
        stunned = false;
        attack = unitRef.GetAttackForOwner(owner);
        maxHealth = unitRef.GetDefenceForOwner(owner);
        foreach(UnitBuff b in buffs)
        {
            attack += b.attackBuff;
            maxHealth += b.healthBuff;
            if (b.stunned) 
            { 
                stunned = true;  
            }
        }
        if(currentHealth > maxHealth) { currentHealth = maxHealth; }
        if(attack < 0) { attack = 0; }
        if(stunned && !currentBoardPiece.hasStun) { hasStunTransform = true; currentBoardPiece.hasStun = true;currentBoardPiece.CreateStunnedSymbol(); Debug.Log("was no stunned symbol when needed so added"); }
        else if(!stunned && hasStunTransform) 
        {
            Debug.Log("not stunned but have the transform");
            hasStunTransform = false;
            MainScript.nonAnimatedProjectileTransforms.Add(stunTransform);
            //stunTransform = null;
        }
    }
    public void BuffUnit(UnitBuff buffToapply)
    {
        attack += buffToapply.attackBuff;
        currentHealth += buffToapply.healthBuff;
        maxHealth += buffToapply.healthBuff;
        //if(buffToapply.healthBuff > 0) { currentHealth += buffToapply.healthBuff; }
        if (buffToapply.stunned)
        {
            //figure out stunning later
        }
        buffs.Add(buffToapply);
        CalculateBuffs();
    }
    public void RemoveBuff(UnitBuff buffToRemove)
    {
        if (buffs.Contains(buffToRemove))
        {
            //attack -= buffToRemove.attackBuff;
            //maxHealth -= buffToRemove.healthBuff;
            if(currentHealth > maxHealth) { currentHealth = maxHealth; }
            buffs.Remove(buffToRemove);
        }
        else { Debug.Log("the buff you are trying to remove is not in the buffs list of this unit"); }
    }
    public void ChangeHealth(int amountToChangeBy)//specifically for dealing damage or healing, this is not for increasing max health like Power word shield
    {
        if (alive)//if you're still alive    -you could die, and get healed before the game executes your death through the queue. you could reduce health to 0 then heal above 0 you would never register your death 
        {
            //Debug.Log("Changing health by " + amountToChangeBy.ToString());
            currentHealth += amountToChangeBy; //if damaging amount to change by should be negative, positive for healing
            if (currentHealth < 0) { currentHealth = 0; alive = false; } else if (currentHealth > maxHealth) { currentHealth = maxHealth; }
        }

    }
    public void ChangeColor(Color newColor)
    {
        //render.material.color = newColor;
        if(unitRef.cardReference.cardType == CardType.building)
        {
            render.material.color = newColor;
        }
        else
        {
            if (skinRender)
            {
                skinRender.material.color = newColor;
            }
            else
            {
                render.material.color = newColor;
            }
            
        }
        
    }
    public void DealDamage(int dmg)
    {
        if (alive) { currentHealth -= dmg; }
        if (currentHealth <= 0) { currentHealth = 0; alive = false; }
    }
}

public class Ability
{
    public int attack = 0;
    public int defence = 0;
    public int utility = 0;
    public int evasion = 0;
}
public enum AbilityType { active,passive,eventDriven }
public class ChessPieceMovementAbility
{
    int distance = 1;
    ChessPieceMovementAbilityType abilityType;
    public ChessPieceMovementAbility(ChessPieceMovementAbilityType theType,int theDistance)
    {
        distance = theDistance;
        abilityType = theType;
    }
    public List<ChessboardPiece> GetAllMoveableSquaresFromPoint(ChessboardPiece origin)
    {
        List<ChessboardPiece> tempList = new List<ChessboardPiece>();
        switch (abilityType){
            case ChessPieceMovementAbilityType.rook:
                //MainScript
                break;
        }
        return tempList;
    }
}
public enum ChessPieceMovementAbilityType { rook,bishop,queen,pawn,custom,none}

public class UnitBuff
{
    public int attackBuff = 0;
    public int healthBuff = 0;
    public bool stunned = false;
    public bool removeAtEndOfturn = false;
    public int remainingStunnedTurns = 0;
    public UnitBuff(int atk, int heal, bool stun, bool oneTurnOnly)
    {
        attackBuff = atk;
        healthBuff = heal;
        stunned = stun;
        removeAtEndOfturn = oneTurnOnly;
    }
}