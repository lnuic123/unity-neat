using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointScript : MonoBehaviour
{
    public WheelJoint2D WheelJoint;
    JointMotor2D wheelMotor;
    public GameObject wheelPrefab;
    private GameObject wheelobject;
    public float speedF, speedB, torqueF, torqueB;
    private float WheelRadius = 0f;

    // Use this for initialization
    void Start()
    {
        float newObjPositionX = transform.position.x + Mathf.Cos(2f * Mathf.PI) * transform.localScale.y / 3;
        float newObjPositionY = transform.position.y + Mathf.Sin(2f * Mathf.PI) * transform.localScale.y / 3;

        Vector3 wheelposition = new Vector3(newObjPositionX * 2f - 1f, newObjPositionY * 2f, transform.position.z);

        wheelobject = Instantiate(wheelPrefab, wheelposition, Quaternion.identity, transform.parent);

        WheelJoint.connectedBody = wheelobject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (WheelRadius != 0f)
        {
            wheelobject.GetComponent<Transform>().localScale = new Vector3(WheelRadius, WheelRadius, transform.localScale.z);
            WheelRadius = 0f;
        }
        
            wheelMotor.motorSpeed = -speedF;
            wheelMotor.maxMotorTorque = torqueF;
            WheelJoint.motor = wheelMotor;
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            wheelMotor.motorSpeed = -speedB;
            wheelMotor.maxMotorTorque = torqueB;
            WheelJoint.motor = wheelMotor;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Car") || collision.gameObject.tag.Equals("Wheel"))
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }
    public void SetWheelRadius(float r)
    {
        WheelRadius = r * 1.15f + 0.35f;
    }
}
