using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {
    
    public DrawNet NeuralNetwork;
    
    void Update () {
        if (Input.GetKey(KeyCode.LeftControl)) // zoom controls
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
                NeuralNetwork.transform.position = new Vector3(transform.position.x - 6f, NeuralNetwork.transform.position.y, NeuralNetwork.transform.position.z);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                transform.position = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
                NeuralNetwork.transform.position = new Vector3(transform.position.x - 6f, NeuralNetwork.transform.position.y, NeuralNetwork.transform.position.z);
            }
        }
    }
}
