using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour {

    public static bool accelerate; //accelerate
    public static bool brake;//brake motorbike
    public static bool shoot;//shot weapon

    //Screen touch zones reference points
    float screenSplit;
    float screenSectionBrake;
    float screenSectionAccelerate;

    // Use this for initialization
    void Start () {
        screenSplit = Screen.width / 4;
        screenSectionBrake = screenSplit;
        screenSectionAccelerate = screenSplit * 3;
    }
	
	// Update is called once per frame
	void Update () {
        InputTouchChecker();
    }

    void InputTouchChecker()
    {
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            if (touches.Length > 0)
            {
                foreach (var myTouch in touches)
                {
                    //Check if bake zone pressed
                    if (myTouch.position.x < screenSectionBrake && myTouch.phase != TouchPhase.Ended && myTouch.phase != TouchPhase.Canceled)
                    {
                        brake = true;
                    }
                    else if (myTouch.position.x < screenSectionBrake && (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled))
                    {
                        brake = false;
                    }
                    //Check if accelerate zone pressed
                    if (myTouch.position.x > screenSectionAccelerate && myTouch.phase != TouchPhase.Ended && myTouch.phase != TouchPhase.Canceled)
                    {
                        accelerate = true;
                    }
                    else if (myTouch.position.x > screenSectionAccelerate && (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled))
                    {
                        accelerate = false;
                    }
                    //Check if shoot zone pressed
                    if (myTouch.position.x > screenSectionBrake && myTouch.position.x < screenSectionAccelerate && myTouch.phase != TouchPhase.Ended && myTouch.phase != TouchPhase.Canceled)
                    {
                        shoot = true;
                    }
                    else if (myTouch.position.x > screenSectionBrake && myTouch.position.x < screenSectionAccelerate && (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled))
                    {
                        shoot = false;
                    }
                }
            }
        }
    }
}
