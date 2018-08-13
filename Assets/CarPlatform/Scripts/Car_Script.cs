using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;
using UnityEngine;

public class Car_Script : UnitController {
    bool IsRunning;
    private int JointCount = 0;
    public GameObject JointPrefab;
    public float JointAngleDelta = 5f;
    IBlackBox box;
    bool test = true;

    void Start()
    {

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
    void FixedUpdate()
    {

        if (IsRunning && test)
        {
            ISignalArray inputArr = box.InputSignalArray;
            for (var i = 0; i < 21; i++)
            {
                inputArr[i] = 1;
            }

            box.Activate();

            ISignalArray outputArr = box.OutputSignalArray;

            JointCount = (int)Mathf.Round((float)outputArr[0] * 9 + 1);
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

                float objXPosition = Mathf.Cos(i * 2f * Mathf.PI / JointCount);
                float objYPosition = Mathf.Sin(i * (2f * Mathf.PI) / JointCount);
                Vector3 objPosition = new Vector3(objXPosition, objYPosition, 0f);

                float objZRotation = 90f + i * (360 / JointCount);

                Quaternion objRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, objZRotation));

                GameObject obj = Instantiate(JointPrefab, objPosition, objRotation, transform);

                obj.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + (float)outputArr[i + 1], transform.localScale.z);
                float newObjPositionX = obj.transform.position.x + Mathf.Cos(i * 2f * Mathf.PI / JointCount) * obj.transform.localScale.y / 3;
                float newObjPositionY = obj.transform.position.y + Mathf.Sin(i * 2f * Mathf.PI / JointCount) * obj.transform.localScale.y / 3;

                obj.transform.position = new Vector3(newObjPositionX, newObjPositionY, obj.transform.position.z);
                obj.GetComponent<JointScript>().SetWheelRadius((float)outputArr[i + 11]);

                SetupHingeJointComponent(obj);

            }
            test = false;
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
        return transform.position.x > 0 ? Mathf.Round(transform.position.x / 10) : 0;
    }
}
