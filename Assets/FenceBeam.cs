using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBeam : MonoBehaviour
{
    public ChessboardPiece postA;
    public ChessboardPiece postB;
    public Transform transform;
    ChessboardPiece currentMoveTarget;
    float beamSpeed = 6.7f;
    // Start is called before the first frame update
    public FenceBeam(ChessboardPiece firstFencePost, ChessboardPiece secondFencePost, Transform t)
    {
        postA = firstFencePost;
        postB = secondFencePost;
        transform = t;
        currentMoveTarget = postB;
    }
    public void UpdateFenceBeam()
    {
        float distanceTravelled = beamSpeed * Time.deltaTime;
        Vector3 finalDestination = new Vector3(currentMoveTarget.transform.position.x, transform.position.y, currentMoveTarget.transform.position.z);
        float distanceToCurrentTarget = Vector3.Distance(transform.position, finalDestination);
        if (distanceTravelled > distanceToCurrentTarget)
        {
            distanceTravelled -= distanceToCurrentTarget;
            transform.position = new Vector3(currentMoveTarget.transform.position.x, transform.position.y, currentMoveTarget.transform.position.z);
            finalDestination = new Vector3(currentMoveTarget.transform.position.x, transform.position.y, currentMoveTarget.transform.position.z);
            if(currentMoveTarget == postA)
            {
                currentMoveTarget = postB;
            }
            else
            {
                currentMoveTarget = postA;
            }
        }
        Vector3 moveDirect = (finalDestination - transform.position).normalized;
        transform.position = transform.position + (moveDirect * distanceTravelled);
    }
}
