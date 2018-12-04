using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WheelJoint2D))]
public class MotorbikeController : MonoBehaviour
{

    [Header("Motorbike components")]
    public Transform rearWheel;
    public Transform frontWheel;
    Rigidbody2D motorbikeBody;
    Rigidbody2D rearWheelBody;
    CircleCollider2D rearCollider;
    CircleCollider2D frontCollider;


    [Header("Movement")]
    public float maxSpeed = 100;
    public float acceleration = 500;

    [Header("Rotation")]
    public float groundedWheelieFactor = 200.0f;
    public float airWheelieFactor = 60.0f;
    public float maxAngularVelocityGrounded = 50;
    public float maxAngularVelocityAir = 200;

    public Text accelerationValue;

    //Screen touch zones reference points
    float screenSplit;
    float screenSectionBrake;
    float screenSectionAccelerate;

    //Movement params
    float dir = 0f; //horizontal movement
    bool brake;

    //Wheels
    Collider2D[] overlapColliders = new Collider2D[1];//used int the TouchingGround()
    RaycastHit2D hit;
    bool frontWheelGrounded;
    bool rearWheelGrounded;

    private void Awake()
    {
        motorbikeBody = GetComponent<Rigidbody2D>();
        rearCollider = rearWheel.GetComponent<CircleCollider2D>();
        frontCollider = frontWheel.GetComponent<CircleCollider2D>();
        rearWheelBody = rearWheel.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization 
    void Start()
    {
        screenSplit = Screen.width / 4;
        screenSectionBrake = screenSplit;
        screenSectionAccelerate = screenSplit * 3;
    }

    private void Update()
    {
        InputTouchChecker();

        rearWheelGrounded = TouchingGround(rearWheel, rearCollider);
        frontWheelGrounded = TouchingGround(frontWheel, frontCollider);
    }

    void FixedUpdate()
    {
        UpdateBrakes();

        UpdateMovement();

        UpdateWheelie();

        LimitateAngularVelocity(rearWheelGrounded ? maxAngularVelocityGrounded : maxAngularVelocityAir);
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
                        dir = 1;
                    }
                    else if (myTouch.position.x > screenSectionAccelerate && (myTouch.phase == TouchPhase.Ended || myTouch.phase == TouchPhase.Canceled))
                    {
                        dir = 0;
                    }
                }
            }
        }
    }

    void UpdateMovement()
    {
        accelerationValue.text = ((rearWheelBody.angularVelocity / 1000) * acceleration).ToString();
        //ACCELERATE
        if (dir != 0 && !brake)
        {
            float movement = acceleration * Time.fixedDeltaTime;
            //accelerationValue.text = rearWheelBody.angularVelocity.ToString();
            if ((rearWheelBody.angularVelocity / 1000) * acceleration > maxSpeed)
            {
                rearWheelBody.AddTorque(movement * -1);
            }
            else
            {
                rearWheelBody.AddTorque(movement);
            }
        }
    }

    void UpdateWheelie()
    {
        if (rearWheelGrounded)
        {
            if (Input.acceleration.x < 0)
            {
                //rotate left the motorbike body
                motorbikeBody.AddTorque(Mathf.Abs((Input.acceleration.x * (1/motorbikeBody.transform.eulerAngles.z)) * groundedWheelieFactor * 100 * Time.fixedDeltaTime));
            }
            else if (Input.acceleration.x > 0 && !frontWheelGrounded)
            {
                //rotate right the motorbike body
                motorbikeBody.AddTorque(Input.acceleration.x * (1 / motorbikeBody.transform.eulerAngles.z) * -groundedWheelieFactor * 100 * Time.fixedDeltaTime);
            }
        }
        else if (!frontWheelGrounded)
        {
            if (Input.acceleration.x < 0)
            {
                //rotate left the motorbike body
                motorbikeBody.AddTorque(-Input.acceleration.x * airWheelieFactor * 100 * Time.fixedDeltaTime);
            }
            else if (Input.acceleration.x > 0)
            {
                //rotate right the motorbike body
                motorbikeBody.AddTorque(Input.acceleration.x * -airWheelieFactor * 100 * Time.fixedDeltaTime);
            }
        }
    }

    void UpdateBrakes()
    {
        if (brake)
        {
            if (rearWheelGrounded)
            {
                motorbikeBody.drag = 10;
                rearWheelBody.freezeRotation = true;
            }
        }
        else
        {
            motorbikeBody.drag = 0.1f;
            rearWheelBody.freezeRotation = false;
        }
    }

    void LimitateAngularVelocity(float maxAngularVelocity)
    {
        //LIMIT ANGLULAR VELOCITY
        if (motorbikeBody.angularVelocity > 0 && motorbikeBody.angularVelocity > maxAngularVelocity)
        {
            motorbikeBody.angularVelocity = maxAngularVelocity;
        }
        if (motorbikeBody.angularVelocity < 0 && motorbikeBody.angularVelocity < -maxAngularVelocity)
        {
            motorbikeBody.angularVelocity = -maxAngularVelocity;
        }
    }

    bool TouchingGround(Transform wheel, CircleCollider2D wheelCollider)
    {
        //given that we are calling this method a lot and in order to save memory usage, I'm using OverlapCircleNonAlloc instead of OverlapCircle.
        //It allows us to pass the same array for the results, with a given length(it won't be resized) and reuse it. 

        return Physics2D.OverlapCircleNonAlloc(wheel.position, wheelCollider.radius + (wheelCollider.radius * 0.14f), overlapColliders, LayerMask.GetMask("Ground")) > 0;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}