using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : IManagerBase<InputManager>
{
    public static InputState currentInputState = InputState.NULL;

    public enum InputState
    {
        NULL,
        NewKeySelection,
        TankInArena,

    }

    public InputManager GetInstance()
    {
        return Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentInputState == InputState.NewKeySelection)
        {
            if (Input.anyKeyDown)
            {
                TankChoosingWindowManager.Instance.ReturnKeyValue(GetPressedKeyCode());
            }
        }
    }

    public KeyCode GetPressedKeyCode()
    {
        List<KeyCode> allKeyCodes = new List<KeyCode>();

        allKeyCodes.Add(KeyCode.A);
        allKeyCodes.Add(KeyCode.B);
        allKeyCodes.Add(KeyCode.C);
        allKeyCodes.Add(KeyCode.D);
        allKeyCodes.Add(KeyCode.E);
        allKeyCodes.Add(KeyCode.F);
        allKeyCodes.Add(KeyCode.G);
        allKeyCodes.Add(KeyCode.H);
        allKeyCodes.Add(KeyCode.I);
        allKeyCodes.Add(KeyCode.J);
        allKeyCodes.Add(KeyCode.K);
        allKeyCodes.Add(KeyCode.L);
        allKeyCodes.Add(KeyCode.M);
        allKeyCodes.Add(KeyCode.N);
        allKeyCodes.Add(KeyCode.O);
        allKeyCodes.Add(KeyCode.P);
        allKeyCodes.Add(KeyCode.Q);

        allKeyCodes.Add(KeyCode.R);
        allKeyCodes.Add(KeyCode.S);
        allKeyCodes.Add(KeyCode.T);
        allKeyCodes.Add(KeyCode.U);
        allKeyCodes.Add(KeyCode.V);
        allKeyCodes.Add(KeyCode.W);
        allKeyCodes.Add(KeyCode.Y);
        allKeyCodes.Add(KeyCode.X);
        allKeyCodes.Add(KeyCode.Z);

        allKeyCodes.Add(KeyCode.Comma);
        allKeyCodes.Add(KeyCode.Period);

        allKeyCodes.Add(KeyCode.UpArrow);
        allKeyCodes.Add(KeyCode.DownArrow);
        allKeyCodes.Add(KeyCode.RightArrow);
        allKeyCodes.Add(KeyCode.LeftArrow);
        
        foreach (KeyCode key in allKeyCodes)
            if (Input.GetKey(key))
                return key;

        return KeyCode.None;
    }

}
