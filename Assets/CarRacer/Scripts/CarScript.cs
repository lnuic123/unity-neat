using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;
using System;

public class CarScript : UnitController {
    private float maxCarSpeed = 1.5f;
    public float CarVelocity;
    private float CarRotation;
    private int CurrentCheckpoint;
    private int LapCounter;
    private float WallHitCounter;
    public float SensorRange = 10;
    public int CheckpointCount;
    private RaycastHit2D hit;
    bool IsRunning;
    IBlackBox box;
    // Use this for initialization
    void Start ()
    {
        CarRotation = 90f;
        CarVelocity = 0f;
        LapCounter = 0;
        CurrentCheckpoint = 0;
        WallHitCounter = 0;
	}

    // Update is called once per frame
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float RayLength = 5f;
        Vector3 start = transform.position + transform.up * 0.8f;
        Gizmos.DrawRay(start, transform.TransformDirection(new Vector3(0, RayLength, 0)));
        Gizmos.DrawRay(start, transform.TransformDirection(new Vector3(RayLength, 0, 0)));
        Gizmos.DrawRay(start, transform.TransformDirection(new Vector3(-RayLength, 0, 0)));
        Gizmos.DrawRay(start, transform.TransformDirection(new Vector3(-RayLength, RayLength * 0.75f, 0)));
        Gizmos.DrawRay(start, transform.TransformDirection(new Vector3(RayLength, RayLength * 0.75f, 0)));
    }
    void FixedUpdate()
    {
        /*
        CarRotation += Input.GetKey(KeyCode.A) ? 3f : Input.GetKey(KeyCode.D) ? -3f : 0f;

        if (Input.GetKey(KeyCode.W) && CarVelocity < maxCarSpeed + 10f)
        {
            CarVelocity += 0.1f;
        }
        else if (CarVelocity > 0f)
        {
            CarVelocity -= 0.1f;
        }

        if (Input.GetKey(KeyCode.S) && CarVelocity > -maxCarSpeed + 10f)
        {
            CarVelocity -= 0.1f;
        }
        else if (CarVelocity < 0f)
        {
            CarVelocity += 0.1f;
        }

        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, CarRotation));
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * CarVelocity;
        */
        if (IsRunning)
        {
            float frontSensor = 0;
            float leftFrontSensor = 0;
            float leftSensor = 0;
            float rightFrontSensor = 0;
            float rightSensor = 0;

            //Cast a ray in the direction specified in the inspector.
            Vector3 start = transform.position + transform.up * 0.8f;

            hit = Physics2D.Raycast(start, transform.TransformDirection(new Vector3(0, 1, 0)), SensorRange);
            if (hit.collider != null && hit.collider.tag.Equals("Wall"))
            {
                frontSensor = 1 - hit.distance / SensorRange;
            }
            //Front-right sensor
            hit = Physics2D.Raycast(start, transform.TransformDirection(new Vector3(1, 0.75f, 0)), SensorRange);
            if (hit.collider != null && hit.collider.tag.Equals("Wall"))
            {
                rightFrontSensor = 1 - hit.distance / SensorRange;
            }
            //Front-left sensor
            hit = Physics2D.Raycast(start, transform.TransformDirection(new Vector3(-1, 0.75f, 0)), SensorRange);
            if (hit.collider != null && hit.collider.tag.Equals("Wall"))
            {
                leftFrontSensor = 1 - hit.distance / SensorRange;
            }
            //Left sensor
            hit = Physics2D.Raycast(start, transform.TransformDirection(new Vector3(-1, 0, 0)), SensorRange);
            if (hit.collider != null && hit.collider.tag.Equals("Wall"))
            {
                leftSensor = 1 - hit.distance / SensorRange;
            }
            //Right sensor
            hit = Physics2D.Raycast(start, transform.TransformDirection(new Vector3(1, 0, 0)), SensorRange);
            if (hit.collider != null && hit.collider.tag.Equals("Wall"))
            {
                rightSensor = 1 - hit.distance / SensorRange;
            }

            ISignalArray inputArr = box.InputSignalArray;
            inputArr[0] = frontSensor;
            inputArr[1] = leftFrontSensor;
            inputArr[2] = leftSensor;
            inputArr[3] = rightFrontSensor;
            inputArr[4] = rightSensor;

            box.Activate();

            ISignalArray outputArr = box.OutputSignalArray;

            var rotation = ((float)outputArr[0] * 2 - 1) * 5f;
            var velocity = ((float)outputArr[1] * 2 - 1) / 20;

            CarRotation += rotation;

            if (Math.Abs(CarVelocity) < maxCarSpeed)
            {
                CarVelocity += velocity;
            }

            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, CarRotation));
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * CarVelocity;

        }
    }

    public override void Stop()
    {
        this.IsRunning = false;
    }

    public override void Activate(IBlackBox box)
    {
        this.box = box;
        this.IsRunning = true;
    }

    public override float GetFitness()
    {
        if (WallHitCounter < (10f / CheckpointCount) * CurrentCheckpoint + LapCounter * 10)
        {
            return (10f / CheckpointCount) * CurrentCheckpoint + LapCounter * 10 - WallHitCounter;
        }
        else
        {
            return 0;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag.Equals("Checkpoint"))
        {
            if (CurrentCheckpoint == col.gameObject.GetComponent<CheckpointScript>().id - 1)
            {
                CurrentCheckpoint = col.gameObject.GetComponent<CheckpointScript>().id;
            }
        }
        else if (col.gameObject.tag.Equals("Lap") && CurrentCheckpoint == CheckpointCount)
        {
            LapCounter++;
            CurrentCheckpoint = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Car")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
        else if(collision.gameObject.tag == "Wall")
        {
            WallHitCounter += 0.1f;
        }

    }
}
