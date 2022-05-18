using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 directionToMove;
    Vector3 destination;
    bool hasReachedDestination = false;
    public float speed = 10f;
    public Animator anim;
    public MeshRenderer r;

    void Start()
    {
        
    }
    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetupProjectile(Vector3 direction)
    {
        anim.Play("shoot");
        //float time = 
        destination = transform.position + direction;
        directionToMove = direction.normalized;
    }
    public void UpdateProjectile()
    {
        //transform.position += (directionToMove * speed * Time.deltaTime);
        //transform.Translate(directionToMove * speed * Time.deltaTime);
        if (hasReachedDestination)
        {
            Color temp = r.material.color;
            if (temp.a != 0f)
            {
                float newAlptha = temp.a - (Time.deltaTime * anim.speed * 1.5f);
                if (newAlptha < 0f) { newAlptha = 0f;  }
                r.material.color = new Color(temp.r, temp.g, temp.b, newAlptha);
            }
        }
        float distanceToDestination = Vector3.Distance(transform.position, destination);
        float amountToMove = speed * Time.deltaTime;
        if(amountToMove < distanceToDestination && !hasReachedDestination)
        {
            transform.Translate(directionToMove * speed * Time.deltaTime, Space.World);
        }
        else
        {
            anim.Play("death");
            anim.speed = 5f;
            transform.position = destination;
            hasReachedDestination = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
