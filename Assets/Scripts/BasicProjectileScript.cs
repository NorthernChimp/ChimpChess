using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectileScript : MonoBehaviour
{
    Vector3 directionToMove;
    Vector3 destination;
    bool hasReachedDestination = false;
    public float speed = 10f;
    public Animator anim;
    public Renderer r;
    public Vector3 randomRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetupProjectile(Vector3 direction)
    {
        randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        anim.Play("JarOfToxinShoot");
        MainScript.projectileTransforms.Add(transform);
        //r = transform.GetComponent<Renderer>();
        //float time = 
        destination = transform.position + direction;
        directionToMove = direction.normalized;
    }
    public void UpdateProjectile()
    {
        //transform.position += (directionToMove * speed * Time.deltaTime);
        //transform.Translate(directionToMove * speed * Time.deltaTime);
        transform.Rotate(randomRotation * Time.deltaTime * 215f);
        if (hasReachedDestination)
        {
            /*Color temp = r.material.color;
            if (temp.a != 0f)
            {
                float newAlptha = temp.a - (Time.deltaTime * anim.speed * 1.5f);
                if (newAlptha < 0f) { newAlptha = 0f; }
                r.material.color = new Color(temp.r, temp.g, temp.b, newAlptha);
            }*/
        }
        float distanceToDestination = Vector3.Distance(transform.position, destination);
        float amountToMove = speed * Time.deltaTime;
        if (amountToMove < distanceToDestination && !hasReachedDestination)
        {
            transform.Translate(directionToMove * speed * Time.deltaTime, Space.World);
        }
        else
        {
            anim.Play("death");
            r.enabled = false;
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
