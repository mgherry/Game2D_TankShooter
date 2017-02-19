using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankChoosingWindowManager : IManagerBase<TankChoosingWindowManager> {

    public GameObject tankConfigurationPrefab;

    public TankConfigurationSetupWindow currentConfigWindow = null;
    public TankConfigurationSetupWindow.ButtonKeyName waitingForKey = TankConfigurationSetupWindow.ButtonKeyName.NULL;

    public GameObject ColorSelectionWindow;
    public GameObject KeySelectionWindow;

    public List<TankConfigurationSetupWindow> addedPlayerConfigs = new List<TankConfigurationSetupWindow>();

    new public void Start()
    {
        base.Start();

        if (ColorSelectionWindow != null)
            ColorSelectionWindow.SetActive(false);

        if (KeySelectionWindow != null)
            KeySelectionWindow.SetActive(false);

        TankConfigurationSetupWindow[] startingPlayers = GetComponentsInChildren<TankConfigurationSetupWindow>();
        if (startingPlayers != null && startingPlayers.Length != 0)
        {
            foreach(TankConfigurationSetupWindow startingConfig in startingPlayers)
            {
                addedPlayerConfigs.Add(startingConfig);
            }
        }
    }


    public void OnSetKeyRequest(TankConfigurationSetupWindow requestingWindow, TankConfigurationSetupWindow.ButtonKeyName keyType)
    {
        if (InputManager.currentInputState == InputManager.InputState.NewKeySelection)
            return;

        InputManager.currentInputState = InputManager.InputState.NewKeySelection;
        InputManager.Instance.GetInstance();

        currentConfigWindow = requestingWindow;
        waitingForKey = keyType;

        if (KeySelectionWindow != null)
            KeySelectionWindow.SetActive(true);
    }

    public void ReturnKeyValue(KeyCode newKeyCode)
    {
        if (newKeyCode == KeyCode.None)
            return;

        if (KeySelectionWindow != null)
            KeySelectionWindow.SetActive(false);

        InputManager.currentInputState = InputManager.InputState.NULL;

        if (currentConfigWindow!= null)
            currentConfigWindow.SetNewKey(newKeyCode, waitingForKey);
    }

    public void OnSetColorRequest(TankConfigurationSetupWindow requestingWindow)
    {
        currentConfigWindow = requestingWindow;

        if (ColorSelectionWindow != null)
            ColorSelectionWindow.SetActive(true);
    }

    public void ReturnColorValue(Color newColor)
    {
        if (ColorSelectionWindow != null)
            ColorSelectionWindow.SetActive(false);

        if (currentConfigWindow != null)
            currentConfigWindow.SetNewColor(newColor);
    }

    #region Game Scene Chaning Button Recievers

    public void OnStartButton()
    {
        TankChoosingMenu.Instance.OnStartGame(this);
    }

    public void OnReturnButton()
    {

    }

    #endregion

}
