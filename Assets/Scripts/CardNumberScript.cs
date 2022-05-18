using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class CardNumberScript : MonoBehaviour
{
    // Start is called before the first frame update
    public CardNumber thisCardNumber;
    public List<RectTransform> digits;
    public List<Sprite> possibleSprites;
    void Start()
    {
        
    }
    public CardNumber SetupNumber(int number)
    {
        thisCardNumber = new CardNumber(number, digits,transform);
        return thisCardNumber;
    }
    public void ChangeSprite(int refInt)
    {
        thisCardNumber.render.sprite = possibleSprites[refInt];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class CardNumber
{
    public int number = 0;
    public List<Sprite> numberSprites;
    public Transform transform;
    public List<RectTransform> childTransforms;
    public CardNumberTransformShiftType currentShiftType = CardNumberTransformShiftType.none;
    public float transformShiftSpeed = 0f;
    public Counter transformShiftingCounter;// a counter for if we want a card number to pulsate over time or what have yah
    public static float distanceBetweenNumbers;
    public Vector3 originalScale;
    //SpriteRenderer] renders;
    public List<UnityEngine.UI.Image> childRenders;
    public UnityEngine.UI.Image render;
    
    
    public CardNumber(int num, List<RectTransform> digits,Transform tran)
    {
        childRenders = new List<UnityEngine.UI.Image>();
        childTransforms = digits;
        transform = tran;
        foreach(RectTransform t in digits)
        {
            childRenders.Add(t.GetComponent<UnityEngine.UI.Image>());
        }
        //childTransforms = new Transform[digits.Length];
        render = transform.GetComponent<UnityEngine.UI.Image>();
        number = num;
        numberSprites = Camera.main.GetComponent<MainScript>().numberSprites;
        Enable();
        SetInt(number);
    }
    public void Disable()
    {
        render.enabled = false;
        SetColor(Color.white);
        foreach(UnityEngine.UI.Image i in childRenders)
        {
            i.enabled = false;
        }
    }
    public void SetupCardNumberTransformShift(CardNumberTransformShiftType type,float timeToShift,float shiftSpeed)
    {
        //Debug.Log("setting up card number transform shit " + type + " and speed of " + shiftSpeed);
        currentShiftType = type;
        if (MainScript.transformShiftingCardNumbers.Contains(this))
        {
            transformShiftingCounter.ResetCounter();
            transformShiftingCounter.endTime = timeToShift;
            transformShiftSpeed = shiftSpeed;
        }
        else
        {
            currentShiftType = type;
            originalScale = transform.localScale;
            transformShiftingCounter = new Counter(timeToShift);
            transformShiftSpeed = shiftSpeed;
            MainScript.transformShiftingCardNumbers.Add(this);
        }
        
    }
    public void ReturnToOriginalScale()
    {
        transform.localScale = originalScale;
    }
    public void SetOriginalScale()
    {
        originalScale = transform.localScale;
    }
    public void UpdateCardNumberTransformShift(float timeToAdd)
    {
        transformShiftingCounter.AddTime(timeToAdd);
        float currentPercent = transformShiftingCounter.GetPercentageDone();
        if (!transformShiftingCounter.hasFinished)
        {
            switch (currentShiftType)
            {
                case CardNumberTransformShiftType.pulsate:
                    float currentPointInTime = transformShiftSpeed * transformShiftingCounter.currentTime;
                    float cos = Mathf.Cos(currentPointInTime) * 0.335f;
                    float sin = Mathf.Sin(currentPointInTime) * 0.335f;
                    cos += 1.25f;
                    sin += 1.25f;
                    Vector3 newScale = new Vector3(cos * originalScale.x, sin * originalScale.y, originalScale.z);
                    transform.localScale = newScale;
                    break;
                case CardNumberTransformShiftType.inflateBriefly:
                    Vector3 inflateScale = new Vector3(originalScale.x * (0.5f + (currentPercent * 1.25f)), originalScale.y * (0.5f + (currentPercent * 1.25f)), originalScale.z);
                    transform.localScale = inflateScale;
                    break;
            }
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
    public void Enable()
    {
        render.enabled = true;
        foreach (UnityEngine.UI.Image i in childRenders)
        {
            i.enabled = true;
        }
    }
    public void SetColor(Color colorOfNumber)
    {
       foreach(Image r in childRenders)
        {
            r.color = colorOfNumber;
        }
    }
    public void SetInt(int newInt)
    {
        int absoluteNewInt = Mathf.Abs(newInt);
        int numberOfDigits = 1;
        if (absoluteNewInt > 999) { absoluteNewInt = 999; }
        number = newInt;
        int currentMultiple = 10;
        int remainder = absoluteNewInt % currentMultiple;
        //Debug.Log("original number is " + newInt.ToString() + "first remainder is " + remainder.ToString());

        childRenders[numberOfDigits - 1].sprite = numberSprites[remainder];
        childRenders[numberOfDigits - 1].enabled = true;
        if (absoluteNewInt > 9)
        {
            numberOfDigits++;
            currentMultiple *= 10;
            int secondRemainder = absoluteNewInt % currentMultiple;
            //Debug.Log("second remainder is " + secondRemainder.ToString());
            secondRemainder -= remainder;
            secondRemainder = secondRemainder / 10;
            childRenders[numberOfDigits - 1].sprite = numberSprites[secondRemainder];
            childRenders[numberOfDigits - 1].enabled = true;
            if (absoluteNewInt > 99)
            {
                numberOfDigits++;
                int thirdRemainder = (absoluteNewInt - (remainder + secondRemainder)) / 100;
                //Debug.Log("third remainder is " + thirdRemainder.ToString());
                childRenders[numberOfDigits - 1].sprite = numberSprites[thirdRemainder];
                childRenders[numberOfDigits - 1].enabled = true;
            }
            else
            {
                childRenders[2].enabled = false;
                //childRenders[2].color = MainScript.invisibleColor;
            }
        }
        else
        {
            childRenders[1].enabled = false;
            childRenders[2].enabled = false;
            //childRenders[1].color = MainScript.invisibleColor;
            //childRenders[2].color = MainScript.invisibleColor;
        }
        //Debug.Log("setting number, there is " + numberOfDigits.ToString());
        Vector3 origin = new Vector3(0f + (distanceBetweenNumbers * (float)(numberOfDigits - 1) * 50f), 0f, 0f);
        for (int i = 0; i < numberOfDigits; i++)
        {
            Transform t = childTransforms[i];
            t.localPosition = origin + (Vector3.left * (float)i * distanceBetweenNumbers * 100f);
        }
    }
}
public enum CardNumberTransformShiftType { pulsate,inflateBriefly,none}