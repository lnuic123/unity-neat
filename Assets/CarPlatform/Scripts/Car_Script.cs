using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;

public class Car_Script : UnitController {
    bool IsRunning;
    private int JointCount = 0;
    public GameObject JointPrefab;
    public float JointAngleDelta = 0.5f;
    IBlackBox box;
    
    void FixedUpdate()
    {
        if (IsRunning)
        {
            // input array assained to constant because without it output is constantly 0.5f
            ISignalArray inputArr = box.InputSignalArray;

            for (var i = 0; i < 25; i++) inputArr[i] = 0.5f;

            box.Activate();

            ISignalArray outputArr = box.OutputSignalArray;

            JointCount = (int)Mathf.Round((float)outputArr[0] * 5 + 1);
            /*
            string tests = "";
            for(var i = 0; i < 21; i++)
            {
                tests += i + ": " + (float)outputArr[i] + " ";
            }
            Debug.Log(tests);
            */

            for (int i = 0; i < JointCount; i++) // spawning joints
            {
                // get joint position
                float JointPositionX = Mathf.Cos(i * 2f * Mathf.PI / JointCount);
                float JointPositionY = Mathf.Sin(i * 2f * Mathf.PI / JointCount);
                Vector3 JointPosition = new Vector3(JointPositionX, JointPositionY, 0f);

                // get joint rotation
                float JointRotationZ = 90f + i * (360 / JointCount);
                Quaternion JointRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, JointRotationZ));

                //create joint
                GameObject obj = Instantiate(JointPrefab, JointPosition, JointRotation, transform);

                //change joint scale
                obj.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + (float)outputArr[i + 1], transform.localScale.z);

                //fix position based on scale
                float newObjPositionX = obj.transform.position.x + Mathf.Cos(i * 2f * Mathf.PI / JointCount) * obj.transform.localScale.y / 3;
                float newObjPositionY = obj.transform.position.y + Mathf.Sin(i * 2f * Mathf.PI / JointCount) * obj.transform.localScale.y / 3;
                obj.transform.position = new Vector3(newObjPositionX, newObjPositionY, obj.transform.position.z);

                //send wheel radius to joint
                obj.GetComponent<JointScript>().SetWheelRadius((float)outputArr[i + 11]);
                
                SetupHingeJointComponent(obj);
            }

            Stop();
        }
    }

    void SetupHingeJointComponent(GameObject obj)
    {
        HingeJoint2D hj = gameObject.AddComponent<HingeJoint2D>() as HingeJoint2D;
        hj.connectedBody = obj.GetComponent<Rigidbody2D>();
        hj.useLimits = true;

        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = 180f - JointAngleDelta;
        limits.max = 180f + JointAngleDelta;
        hj.limits = limits;
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
        return transform.position.x > 0 ? Mathf.Round(transform.position.x / 10) : 0;
    }
}
