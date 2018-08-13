using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    void OnGUI()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Event.current.type == EventType.ScrollWheel)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Event.current.delta.y / 5);
        }
        else
        {
            if (Event.current.type == EventType.ScrollWheel)
                transform.position = new Vector3(transform.position.x + Event.current.delta.y / 5, transform.position.y, transform.position.z);
        }
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                transform.position = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                transform.position = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
            }
        }
    }
}
