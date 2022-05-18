using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyReadableCardPrefabScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite playerOne;
    public Sprite aiPlayer;
    public CardNumber attack;
    public CardNumber health;
    public CardNumber cost;
    public CardNumber attackBuff;
    public CardNumber healthBuff;
    public AbilityNotification notifier;
    public UnityEngine.UI.Image notifierImage;
    public List<Sprite> notificationSprites;
    public List<Transform> cardNumberTransforms;
    public UnityEngine.UI.Image render;
    public Counter fadeCounter;
    public EmptyReadableCardPrefabScript otherCard;
    void Start()
    {
        SetupReadableCard();
    }
    public void SetupReadableCard()
    {

        //Debug.Log("setup readable card");
        fadeCounter = new Counter(2f);
        render = GetComponent<UnityEngine.UI.Image>();
        attack = cardNumberTransforms[0].GetComponent<CardNumberScript>().SetupNumber(20);
        health = cardNumberTransforms[1].GetComponent<CardNumberScript>().SetupNumber(1);
        cost = cardNumberTransforms[2].GetComponent<CardNumberScript>().SetupNumber(5);
        attackBuff = cardNumberTransforms[3].GetComponent<CardNumberScript>().SetupNumber(0);
        healthBuff = cardNumberTransforms[4].GetComponent<CardNumberScript>().SetupNumber(0);
        attackBuff.Disable();
        healthBuff.Disable();
        transform.localScale = MainScript.cardScale *  2.35f;//makes this card 2.65 times bigger for easier reading
        render.enabled = false;
        attack.Disable();
        health.Disable();
        cost.Disable();
        notifier = new AbilityNotification(notificationSprites, notifierImage);
        notifier.Disable();
    }
    public void AppearAsPlayer(Player p)
    {
        attack.Disable();
        cost.Disable();
        health.Disable();
        fadeCounter.ResetCounter();
        render.enabled = true;
        if (p.theType == PlayerType.localHuman)
        {
            render.sprite = playerOne;
        }
        else
        {
            render.sprite = aiPlayer;
        }
    }
    public void ShowNotifierOnly(AbilityNotificationState state, Color c)
    {
        //DisableEmptyReadableCard();
        fadeCounter.ResetCounter();
        notifier.SetupNotification(state, c);
        //notifier.SetupColor(c);
    }
    public void AppearAsThisCard(CardReference c,Player owns)
    {
        attackBuff.Disable();
        healthBuff.Disable();
        //Debug.Log(c.cardSpriteName);
        cost.Enable();
        cost.SetInt(c.cost);
        fadeCounter.ResetCounter();
        render.sprite = Resources.Load<Sprite>("CardSprites/" + c.cardSpriteName);
        render.enabled = true;
        if(c.cardType == CardType.minion)
        {
            attack.Enable();
            health.Enable();
            attack.SetInt(c.reference.GetAttackForOwner(MainScript.currentGame.currentPlayer));
            health.SetInt(c.reference.GetDefenceForOwner(MainScript.currentGame.currentPlayer));
            cost.SetInt(c.GetCost(owns));
            AppearAsThisUnitReference(c.reference,owns);
        }
        else if(c.cardType == CardType.attackUnit || c.cardType == CardType.moveUnit)
        {
            //cost.Disable();
            //attack.Disable();
            //health.Disable();
        }
        else
        {
            //cost.SetInt(c.reference.GetCostForOwner(owns));
            attack.Disable();
            health.Disable();
        }
    }
    public void SetupAttackBuff(int buffAmt)
    {
        attackBuff.Disable();
        attackBuff.transform.position = attack.transform.position;
        attackBuff.Enable();
        attackBuff.SetInt(buffAmt);
        if(Mathf.Sign(buffAmt) == 1f) { attackBuff.SetColor(Color.green); } else { attackBuff.SetColor(Color.red); }
    }
    public void SetupHealthBuff(int buffAmt)
    {
        healthBuff.Disable();
        healthBuff.transform.position = health.transform.position;
       healthBuff.Enable();
       healthBuff.SetInt(buffAmt);
        if (Mathf.Sign(buffAmt) == 1f) { healthBuff.SetColor(Color.green); } else { healthBuff.SetColor(Color.red); }
    }
    public void DisableEmptyReadableCard()
    {
        attackBuff.Disable();
        healthBuff.Disable();
        render.enabled = false;
        attack.Disable();
        health.Disable();
        cost.Disable();
        notifier.Disable();
    }
    public void UpdateEmptyReadableCard(float timePassed)
    {
        fadeCounter.AddTime(timePassed);
        if (healthBuff.render.enabled) { healthBuff.transform.Translate(Vector2.up * (MainScript.cardHeight * 0.15f)); }
        if (attackBuff.render.enabled) { attackBuff.transform.Translate(Vector2.up * (MainScript.cardHeight * 0.15f)); }
        if (fadeCounter.hasFinished) { DisableEmptyReadableCard(); }
    }
    public void AppearAsThisUnitReference(UnitReference u,Player owns)
    {
        attackBuff.Disable();
        healthBuff.Disable();
        cost.Enable();
        attack.Enable();
        health.Enable();
        render.sprite = Resources.Load<Sprite>("CardSprites/" + u.unitCardSpriteName);
        render.enabled = true;
        fadeCounter.ResetCounter();
        cost.SetInt(u.cardReference.GetCost(owns));
        attack.SetInt(u.GetAttackForOwner(owns));
        health.SetInt(u.GetDefenceForOwner(owns));
        
    }
    public void AppearAsThisUnit(Chesspiece u)
    {
        attackBuff.Disable();
        healthBuff.Disable();
        cost.Enable();
        attack.Enable();
        health.Enable();
        render.sprite = Resources.Load<Sprite>("CardSprites/" + u.unitRef.unitCardSpriteName);
        render.enabled = true;
        fadeCounter.ResetCounter();
        cost.SetInt(u.unitRef.cardReference.GetCost(u.owner));
        attack.SetInt(u.attack);
        health.SetInt(u.currentHealth);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class AbilityNotification
{
    public List<Sprite> notificationSprites;
    public UnityEngine.UI.Image render;
    public AbilityNotificationState currentState;
    public AbilityNotification(List<Sprite> notifications,UnityEngine.UI.Image r)
    {
        render = r;
        notificationSprites = notifications;
    }
    public void SetupColor(Color c)
    {
        render.color = c;
    }
    public void SetupNotification(AbilityNotificationState state, Color c)
    {
        currentState = state;
        Enable();
        switch (state)
        {
            case AbilityNotificationState.eventActivatedAbility:
                render.sprite = notificationSprites[0];
                break;
            case AbilityNotificationState.onDeathEffect:
                render.sprite = notificationSprites[1];
                break;
            case AbilityNotificationState.onPlayEffect:
                render.sprite = notificationSprites[2];
                break;
            case AbilityNotificationState.dealDamage:
                render.sprite = notificationSprites[3];
                break;
            case AbilityNotificationState.enemyTurn:
                render.sprite = notificationSprites[4];
                break;
            case AbilityNotificationState.localHumanTurn:
                render.sprite = notificationSprites[5];
                break;
            case AbilityNotificationState.discarding:
                render.sprite = notificationSprites[6];
                break;
            case AbilityNotificationState.playCard:
                render.sprite = notificationSprites[7];
                break;
            case AbilityNotificationState.reshuffling:
                render.sprite = notificationSprites[8];
                break;
            case AbilityNotificationState.endOfTurn:
                render.sprite = notificationSprites[9];
                break;
            case AbilityNotificationState.drawCard:
                render.sprite = notificationSprites[10];
                break;
            case AbilityNotificationState.attacksUnit:
                render.sprite = notificationSprites[11];
                break;
            case AbilityNotificationState.defending:
                render.sprite = notificationSprites[12];
                break;
            case AbilityNotificationState.summonMinion:
                render.sprite = notificationSprites[13];
                break;
            case AbilityNotificationState.unitDies:
                render.sprite = notificationSprites[14];
                break;
            case AbilityNotificationState.upgrading:
                render.sprite = notificationSprites[15];
                break;
            case AbilityNotificationState.applyingPoison:
                render.sprite = notificationSprites[16];
                break;
        }
        SetupColor(c);
    }
    public void Enable()
    {
        render.enabled = true;
    }
    public void Disable()
    {
        render.enabled = false;
    }
}
public enum AbilityNotificationState { onPlayEffect,onDeathEffect,eventActivatedAbility,localHumanTurn,enemyTurn,dealDamage,discarding,playCard,drawCard,endOfTurn,reshuffling,summonMinion,attacksUnit,defending,unitDies,upgrading,applyingPoison}