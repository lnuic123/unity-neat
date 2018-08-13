using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControler : MonoBehaviour {
    private float wheelRadius;
    // Use this for initialization
    void Start ()
    {
        /*
        wheelRadius = Random.Range(0.35f, 1.5f);

        transform.localScale = new Vector3(wheelRadius, wheelRadius, transform.localScale.z);
        */
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Car") || collision.gameObject.tag.Equals("Wheel"))
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}
