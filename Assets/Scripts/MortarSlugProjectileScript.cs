using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarSlugProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 15f;
    public Animator anim;
    public Vector3  destination;
    public Vector3 directionToMove = Vector3.down;
    public Vector3 startPosition;
    public MeshRenderer shellRender;
    public MeshRenderer explosionCloudRender;
    public bool hasReachedDestination = false;
    void Start()
    {
        
    }
    public void SetupProjectile(Vector3 direction)
    {
        anim.Play("idle");
        //float time = 
        shellRender.enabled = true;
        explosionCloudRender.enabled = false;
        startPosition = transform.position;
        destination = transform.position + direction;
        //directionToMove = direction.normalized;
    }
    public void UpdateProjectile()
    {
        //transform.position += (directionToMove * speed * Time.deltaTime);
        //transform.Translate(directionToMove * speed * Time.deltaTime);
        if (!hasReachedDestination)
        {
            /*Color temp = r.material.color;
            if (temp.a != 0f)
            {
                float newAlptha = temp.a - (Time.deltaTime * anim.speed * 1.5f);
                if (newAlptha < 0f) { newAlptha = 0f; }
                r.material.color = new Color(temp.r, temp.g, temp.b, newAlptha);
            }*/
            float distanceToDestination = Vector3.Distance(transform.position, destination);
            float amountToMove = speed * Time.deltaTime;
            if (amountToMove < distanceToDestination && !hasReachedDestination)
            {
                transform.Translate(directionToMove * speed * Time.deltaTime, Space.World);
                //Debug.Log("moving down");
            }
            else
            {
                //Debug.Log("exploding");
                anim.Play("exploding");
                shellRender.enabled = false;
                explosionCloudRender.enabled = true;
                anim.speed = 5f;
                transform.position = destination;
                hasReachedDestination = true;
            }
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
