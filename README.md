# Unity NEAT
Unity games with NEAT algorithm

This project was build with [SharpNEAT port to Unity](https://github.com/lordjesus/UnityNEAT)

### How to use it
1. Download this project
2. Extract it and open it with your version of Unity
3. Open Scene in CarRacer or in CarPlatform (which ever game you want)
4. Run the game and hit Start

### How to make your own game
1. Make a new folder in Assets folder where all your game files will be
2. Copy Template Scene from Template folder to your game folder
3. In Evaluator, set the variables to desired value (number of inputs, outputs, population size, etc.) and place object you want to train with in "Unit" field
4. On the same object place a new script component and copy-paste following code:
```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpNeat.Phenomes;

public class yourScriptName : UnitController {
    bool IsRunning;
    IBlackBox box;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsRunning)
        {
            ISignalArray inputArr = box.InputSignalArray; // Set network input

            inputArr[0] = setFirstInput;
            inputArr[1] = setSecondInput;
            ...

            box.Activate(); //Activate network

            ISignalArray outputArr = box.OutputSignalArray; // Get network output

            readFirstOutput = outputArr[0];
            readSecondOutput = outputArr[1];
            ...
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
        // Implement a meaningful fitness function here, for each unit.
        return 0;
    }
}
```
5. Correctly assign inputs and outputs of the network in FixedUpdate function
6. Implement fitness function in GetFitness()
