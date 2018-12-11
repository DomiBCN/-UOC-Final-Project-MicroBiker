using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        //offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void FixedUpdate()
    {
        //// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        //if (player != null)
        //{
        //    transform.position = player.transform.position + offset;
        //}
        Vector3 desiredPos = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
