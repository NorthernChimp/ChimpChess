using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    // Start is called before the first frame update
    Counter deathCounter;
    void Start()
    {
        
    }
    public void SetupFireball()
    {
        deathCounter = new Counter(1f);
    }
    public void UpdateSpell(float time)
    {
        deathCounter.AddTime(time);
        if (deathCounter.hasFinished)
        {
            //MainScript.spellTransforms.Remove(transform);
            //Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
