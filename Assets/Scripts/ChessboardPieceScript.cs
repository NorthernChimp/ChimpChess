using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessboardPieceScript : MonoBehaviour
{
    // Start is called before the first frame update
    public MeshRenderer transparentRender;
    public ChessboardPiece thisChessboardPiece;
    public GameObject artilleryTargetPrefab;//for artillery targets or anything that symbolizes a space is going to be damaged or targetted somehow
    public GameObject gasCloudPrefab;// for poison gas clouds
    public GameObject scienceSymbolPrefab;//for when you research something or sciencey spells that want to add an animation
    public GameObject defendSymbolPrefab;//for a space defending from attack
    public GameObject attackSymbolPrefab;//for a space attacking
    public GameObject arrowSymbolPrefab;//to draw attention towards a space, summoning
    public GameObject moneyGenerationPrefab;//money generation effects or money-ish spells that want an animation
    public GameObject healthGenerationPrefab;
    public GameObject attackGenerationPrefab;
    public GameObject utilityGenerationPrefab;
    public GameObject cardGenerationPrefab;
    public GameObject hammerWrenchSymbolPrefab;
    public GameObject stunnedSymbolPrefab;
    public GameObject gasBubblesPrefab;
    public GameObject healthIconPrefab;
    public GameObject bigHeartSymbolPrefab;
    public Transform gasCloudTransform;
    public SkinnedMeshRenderer gasCloudRender;
    public CardNumber unitHealthNumber;

    void Start()
    {
        
    }
    public void CreateAnimationPrefab(GameObject prefab,float speedDivide, bool permanent)
    {
        Vector3 instantiatePos = transform.position;
        //Debug.Log("instantiating at " + instantiatePos);
        Transform t = Instantiate(prefab, instantiatePos, prefab.transform.rotation).transform;
        Animator temp = t.GetComponentInChildren<Animator>();
        temp.Play("idle");
        temp.speed =  GetAnimationLength("idle", temp)/ 1f;
        //temp.speed =  GetAnimationLength("idle", temp)/ speedDivide;
        //temp.speed = speedDivide / GetAnimationLength("idle", temp);
        if (!permanent)
        {
            MainScript.nonAnimatedProjectileTransforms.Add(t);
        }
        else
        {
            //MainScript.artilleryTargetTransforms.Add(t);
            if(prefab == stunnedSymbolPrefab)
            {
                thisChessboardPiece.currentChessPiece.stunTransform = t;
                t.SetParent(thisChessboardPiece.currentChessPiece.transform);
            }
        }
        
    }
    public void CreateHealthGenerationSymbol()
    {
        CreateAnimationPrefab(healthGenerationPrefab, 0.5f,false);
    }
    public void CreateBigHeartSymbol()
    {
        CreateAnimationPrefab(bigHeartSymbolPrefab, 1f, false);
    }
    public void CreateHammerWrenchSymbol()
    {
        CreateAnimationPrefab(hammerWrenchSymbolPrefab, 1f, false);
    }
    public void CraeteStunnedSymbol()
    {
        CreateAnimationPrefab(stunnedSymbolPrefab, 1f,true);
    }
    public void CreateCardGenerationSymbol()
    {
        CreateAnimationPrefab(cardGenerationPrefab, 2f,false);
    }
    public void CreateAttackSymbol()
    {
        CreateAnimationPrefab(attackSymbolPrefab, 2f,false);
    }
    public void CreateDefendSymbol()
    {
        CreateAnimationPrefab(defendSymbolPrefab, 4f,false);
    }
    public void CreateArrowPrefab()
    {
        CreateAnimationPrefab(arrowSymbolPrefab, 4f,false);
    }
    public void CreateScienceSymbol()
    {
        Vector3 instantiatePos = transform.position;
        Transform t = Instantiate(scienceSymbolPrefab, instantiatePos, Quaternion.identity).transform;
        Animator temp = t.GetComponentInChildren<Animator>();
        temp.Play("idle");
        temp.speed = 3.5f / GetAnimationLength("idle", temp);
        MainScript.nonAnimatedProjectileTransforms.Add(t);
        //Debug.Log(t.position);
    }
    public void CreateGasBubbles()
    {
        Transform t = Instantiate(gasBubblesPrefab, transform.position, Quaternion.identity).transform;
        Animator temp = t.GetComponentInChildren<Animator>();
        temp.Play("bubblesRising");
        temp.speed = 2f / GetAnimationLength("bubblesRising", temp);
        MainScript.nonAnimatedProjectileTransforms.Add(t);
        //MainScript.projectileTransforms.Add(t);
    }
    public float GetAnimationLength(string animationName, Animator thisAnim)
    {
        for (int i = 0; i < thisAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (thisAnim.runtimeAnimatorController.animationClips[i].name == animationName)
            {
                return thisAnim.runtimeAnimatorController.animationClips[i].length;
            }
        }
        return 0f;
    }
    public void CreateMoneyGeneration(float timeForAnim)
    {
        Transform t = Instantiate(moneyGenerationPrefab, transform.position + (Vector3.up * MainScript.distanceBetweenUnitAndBoardPiece * 1.5f), moneyGenerationPrefab.transform.rotation).transform;
        Animator temp = t.GetComponent<Animator>();
        temp.Play("idle");
        temp.speed = GetAnimationLength("idle",temp)/timeForAnim;
        MainScript.nonAnimatedProjectileTransforms.Add(t);
    }
    public void CreateHealthGenerationSymbol(float timeforAnim)
    {

    }
    public void CreateAttackGenerationSymbol(float timeforAnim)
    {

    }
    public void CreateUtilityGenerationSymbol(float timeforAnim)
    {

    }
    public void SetupArtilleryTarget()
    {
        Transform t = Instantiate(artilleryTargetPrefab, thisChessboardPiece.transform.position + (Vector3.up * MainScript.distanceBetweenUnitAndBoardPiece * 0.45f), Quaternion.LookRotation(Vector3.up, Vector3.forward)).transform;
        t.GetComponent<Animator>().Play("idle");
        MainScript.artilleryTargetTransforms.Add(t);
        thisChessboardPiece.hasArtillery = true;
    }
    public void SetupGasCloud(int amount)
    {
        int oldAmount = thisChessboardPiece.toxicCloudAmount;
        thisChessboardPiece.toxicCloudAmount += amount;
        
        //Debug.Log("setting up gas cloud for " + amount + " and it used to be " + oldAmount + " and now it is " + thisChessboardPiece.toxicCloudAmount);
        if (thisChessboardPiece.toxicCloudAmount > 0)
        {
            if (!thisChessboardPiece.poisonIcon.render.enabled) { thisChessboardPiece.poisonIcon.Enable(); thisChessboardPiece.poisonIcon.SetInt(thisChessboardPiece.toxicCloudAmount); }
            //Debug.Log("greater than zero");
            thisChessboardPiece.hasToxicCloud = true;
            int relativeGasAmount = thisChessboardPiece.toxicCloudAmount;
            if(relativeGasAmount > 5) { relativeGasAmount = 5; }
            if (!gasCloudTransform)
            {
                gasCloudTransform = Instantiate(gasCloudPrefab, transform.position + (Vector3.up * MainScript.distanceBetweenBoardPieces), Quaternion.identity).transform;
                gasCloudRender = gasCloudTransform.GetComponentInChildren<SkinnedMeshRenderer>();
            }
            Color currentColor = gasCloudRender.material.color;
            gasCloudRender.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.125f * relativeGasAmount);
        }
        else
        {
            //Debug.Log("amount equals zero");
            //if (thisChessboardPiece.hasToxicCloud) { thisChessboardPiece.hasToxicCloud = false; }
            thisChessboardPiece.toxicCloudAmount = 0;
            thisChessboardPiece.hasToxicCloud = false;
            thisChessboardPiece.poisonIcon.Disable();
            
            if (gasCloudTransform) { Destroy(gasCloudTransform.gameObject); }
            
        }
        //Debug.Log("has toxic cloud = " + thisChessboardPiece.hasToxicCloud + " and the amount is " + thisChessboardPiece.toxicCloudAmount);

        //thisChessboardPiece.hasToxicCloud = true;
    }
    public void SetupChessboardPiece(int x, int y)
    {
        thisChessboardPiece = new ChessboardPiece(x, y, transform, transparentRender);
        if (healthIconPrefab == null) { healthIconPrefab = (GameObject)Resources.Load("HealthIconPrefab"); }
        Transform t = Instantiate(healthIconPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f), Quaternion.identity, MainScript.theCanvas).transform; ;        ///GameObject healthIconPrefab = Resources.L
        t.localScale = new Vector3(0.25f, 0.25f, 1f);
        thisChessboardPiece.healthIcon = t.GetComponent<CardNumberScript>().SetupNumber(0);
        thisChessboardPiece.healthIcon.Disable();
        t = Instantiate(healthIconPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f), Quaternion.identity, MainScript.theCanvas).transform; ;        ///GameObject healthIconPrefab = Resources.L
        t.localScale = new Vector3(0.25f, 0.25f, 1f);
        thisChessboardPiece.attackIcon = t.GetComponent<CardNumberScript>().SetupNumber(0);
        thisChessboardPiece.attackIcon.transform.SendMessage("ChangeSprite",1);
        thisChessboardPiece.attackIcon.Disable();

        t = Instantiate(healthIconPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f), Quaternion.identity).transform;
        thisChessboardPiece.poisonIcon = t.GetComponent<CardNumberScript>().SetupNumber(0);
        thisChessboardPiece.poisonIcon.transform.SendMessage("ChangeSprite", 2);
        thisChessboardPiece.poisonIcon.Enable();
        t.SetParent(MainScript.theCanvas);
        t.localScale = new Vector3(0.25f, 0.25f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class ChessboardPiece
{
    public int xPos = 0;
    public int yPos = 0;
    public bool hasChessPiece = false;
    public bool hasToxicCloud = false;
    public int toxicCloudAmount = 0;
    public bool hasArtillery = false;
    public bool hasStun = false;
    public Chesspiece currentChessPiece;
    public Transform transform;
    public MeshRenderer transparentRender;
    public Player owner = MainScript.neutralPlayer;
    public MeshRenderer ownerBoxRender;
    public MeshRenderer render;
    public Counter transparentRenderCounter;
    public CardNumber healthIcon;
    public CardNumber attackIcon;
    public CardNumber poisonIcon;
    
    public ChessboardPiece(int x, int y, Transform t,MeshRenderer r)
    {
        
        
        xPos = x;
        yPos = y;
        transform = t;
        transparentRender = r;
        transparentRenderCounter = new Counter(1.35f);
        transparentRenderCounter.hasFinished = true;
        transparentRender.enabled = false;
        transparentRender.material.color = new Color(0f, 0f, 0f, 0f);
        ownerBoxRender = t.GetChild(0).GetComponent<MeshRenderer>();
        ownerBoxRender.material.color = ChessboardPiece.GetAlphaOfColor(Color.white);
        transform.GetComponent<ChessboardPieceScript>().thisChessboardPiece = this;
        render = t.GetComponent<MeshRenderer>();
        render.material.color = Color.white;
        //ChangeTransparentRenderColor(new Color(0f,255f,0f,0.125f));
    }
    public void LogCoordinates(string preceedingNote)
    {
        Debug.Log(preceedingNote + " X: " + xPos + " Y: " + yPos);
    }
    public void CreateStunnedSymbol()
    {
        transform.SendMessage("CraeteStunnedSymbol");
    }
    public void CreateScienceSymbol()
    {
        transform.SendMessage("CreateScienceSymbol");
    }
    public void CreateBigHeartSymbol()
    {
        transform.SendMessage("CreateBigHeartSymbol");
    }
    public void CreateHammerWrenchSymbol()
    {
        transform.SendMessage("CreateHammerWrenchSymbol");
    }
    public void CreateCardGenerationSymbol()
    {
        transform.SendMessage("CreateCardGenerationSymbol");
    }
    public void SetupAttackSymbol()
    {
        transform.SendMessage("CreateAttackSymbol");
    }
    public void SetupDefendSymbol()
    {
        transform.SendMessage("CreateDefendSymbol");
    }
    public void CreateHealthSymbol()
    {
        transform.SendMessage("CreateHealthGenerationSymbol");
    }
    public void SetupArrowSymbol()
    {
        transform.SendMessage("CreateArrowPrefab");
    }
    public void CreateGasBubbles()
    {
        transform.SendMessage("CreateGasBubbles");
    }
    public void CreateMoneyGeneration(float timeForAnim)
    {
        transform.SendMessage("CreateMoneyGeneration",timeForAnim);
    }
    public void SetupGasCloud(int amount)
    {
        transform.SendMessage("SetupGasCloud", amount);
    }
    public void SetupArtillery()
    {
        transform.SendMessage("SetupArtilleryTarget");
    }
    public void UpdateUnitStats()
    {
        
        if (hasChessPiece)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(currentChessPiece.transform.position);
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(currentChessPiece.transform.position);
            float distanceFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);
            if(distanceFromCenter < 0.4f)
            {
                if (!healthIcon.render.enabled) { healthIcon.Enable(); attackIcon.Enable(); healthIcon.SetInt(currentChessPiece.currentHealth); attackIcon.SetInt(currentChessPiece.attack); }
                if (healthIcon.number != currentChessPiece.currentHealth) { healthIcon.SetInt(currentChessPiece.currentHealth); }
                if(attackIcon.number != currentChessPiece.attack) { attackIcon.SetInt(currentChessPiece.attack); }
                healthIcon.transform.position = screenPoint + (Vector3.right * 25f) + (Vector3.down * 25f);
                attackIcon.transform.position = screenPoint + (Vector3.right * -25f) + (Vector3.down * 25f);
            }else if(healthIcon.render.enabled) { healthIcon.Disable(); attackIcon.Disable(); }

        }
        else if (healthIcon.render.enabled) { healthIcon.Disable();attackIcon.Disable(); }
        if (hasToxicCloud)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);
            float distanceFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);
            if (distanceFromCenter < 0.4f)
            {
                if (!poisonIcon.render.enabled) { poisonIcon.Enable();}
                if(poisonIcon.number != toxicCloudAmount) { poisonIcon.SetInt(toxicCloudAmount); }
                poisonIcon.transform.position = screenPoint + (Vector3.down * 0f);
            }
            else
            {
                if (poisonIcon.render.enabled) { poisonIcon.Disable(); }
            }   
        }
        else
        {
            if (poisonIcon.render.enabled) { poisonIcon.Disable(); }
        }
    }
    public void ChangeColor(Color newColor)
    {
        render.material.color = newColor;
    }
    public void ChangeTransparentRenderColor(Color c)
    {
        MainScript.boardPiecesWithTransparentRenderActive.Add(this);
        transparentRender.enabled = true;
        transparentRender.material.color = new Color(c.r,c.g,c.b,0.25f);
        transparentRenderCounter.ResetCounter();
    }
    public void ChangeOwner(Player newOwner, Color playerColor)
    {
        owner = newOwner;
        owner.ownedBoardPieces.Add(this);
        ownerBoxRender.material.color = ChessboardPiece.GetAlphaOfColor(playerColor);
    }
    public ChessboardPiece(Transform tr)
    {
        xPos = 420;
        yPos = 420;
        transform = tr;
    }
    public static Color GetAlphaOfColor(Color c)
    {
        return new Color(c.r, c.g, c.b, 0.5f);
    }
}
