using System;
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
    public Transform rearReference;
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

    //Movement params
    Vector2 forceDirection;
    float accelerometerCalibrationPoint;
    float accelerometerInput;
    float motorbikeAngleZ;

    //Wheels
    Collider2D[] overlapColliders = new Collider2D[1];//used int the TouchingGround()
    RaycastHit2D groundHit;
    bool frontWheelGrounded;
    bool rearWheelGrounded;

    private void Awake()
    {
        motorbikeBody = GetComponent<Rigidbody2D>();
        rearCollider = rearWheel.GetComponent<CircleCollider2D>();
        frontCollider = frontWheel.GetComponent<CircleCollider2D>();
        rearWheelBody = rearWheel.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        accelerometerInput = Input.acceleration.x - accelerometerCalibrationPoint;
        motorbikeAngleZ = Mathf.Clamp(motorbikeBody.transform.eulerAngles.z, 0.1f, motorbikeBody.transform.eulerAngles.z);
        rearWheelGrounded = TouchingGround(rearWheel, rearCollider);
        frontWheelGrounded = TouchingGround(frontWheel, frontCollider);

        if (rearWheelGrounded) groundHit = Physics2D.Raycast(rearReference.position, -rearReference.up, 5f, LayerMask.GetMask("Ground"));
    }

    void FixedUpdate()
    {
        UpdateBrakes();

        UpdateMovement();

        UpdateWheelie();

        LimitateAngularVelocity(rearWheelGrounded ? maxAngularVelocityGrounded : maxAngularVelocityAir);
    }

    void UpdateMovement()
    {
        //ACCELERATE
        if (TouchInputManager.accelerate && !TouchInputManager.brake && rearWheelGrounded)
        {
            #region OLD MOVE
            //float movement = acceleration * Time.fixedDeltaTime;
            ////accelerationValue.text = rearWheelBody.angularVelocity.ToString();
            //if ((rearWheelBody.angularVelocity / 1000) * acceleration > maxSpeed)
            //{
            //    rearWheelBody.AddTorque(movement * -1);
            //}
            //else
            //{
            //    rearWheelBody.AddTorque(movement);
            //}
            #endregion
            forceDirection = new Vector2(groundHit.normal.y, -groundHit.normal.x);
            //Debug.DrawRay(rear.transform.position, forceDirection * 40, Color.green);
            motorbikeBody.AddForce(forceDirection * acceleration * Time.fixedDeltaTime);
        }
    }

    void UpdateWheelie()
    {
        if (rearWheelGrounded)
        {
            if (accelerometerInput < 0)
            {
                //rotate left the motorbike body
                motorbikeBody.AddTorque(accelerometerInput * -groundedWheelieFactor * 100 * Time.fixedDeltaTime);
                //motorbikeBody.AddTorque(accelerometerInput * (1 / motorbikeAngleZ) * -groundedWheelieFactor * 100 * Time.fixedDeltaTime);

            }
            else if (accelerometerInput > 0 && !frontWheelGrounded)
            {
                //rotate right the motorbike body
                motorbikeBody.AddTorque(accelerometerInput * (-groundedWheelieFactor * 0.50f) * 100 * Time.fixedDeltaTime);
                //motorbikeBody.AddTorque(accelerometerInput * (motorbikeAngleZ / 100) * -groundedWheelieFactor * 100 * Time.fixedDeltaTime);
            }
        }
        else if (!frontWheelGrounded)
        {
            if (accelerometerInput < 0)
            {
                //rotate left the motorbike body
                motorbikeBody.AddTorque(-accelerometerInput * airWheelieFactor * 100 * Time.fixedDeltaTime);
            }
            else if (accelerometerInput > 0)
            {
                //rotate right the motorbike body
                motorbikeBody.AddTorque(accelerometerInput * -airWheelieFactor * 100 * Time.fixedDeltaTime);
            }
        }
    }

    void UpdateBrakes()
    {
        if (TouchInputManager.brake)
        {
            if (rearWheelGrounded)
            {
                motorbikeBody.drag = 10;
                rearWheelBody.freezeRotation = true;
            }
            else
            {
                motorbikeBody.drag = 0.1f;
                rearWheelBody.freezeRotation = false;
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

    public void CalibrateAccelerometer()
    {
        accelerometerCalibrationPoint = Input.acceleration.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.instance.UpdateCoinsCounter();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Finish"))
        {
            GameManager.instance.FinshLineReached();
        }
    }
}