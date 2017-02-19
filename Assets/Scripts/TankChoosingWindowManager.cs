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

	public int rowLimit = 2;
	public int windowsInLineLimit = 4;
	public List<TankConfigWindowRow> rowObjects = new List<TankConfigWindowRow>();


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
			foreach (TankConfigurationSetupWindow startingConfig in startingPlayers)
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

		if (currentConfigWindow != null)
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

	public void OnConfigRemoveClicked(TankConfigurationSetupWindow removedConfig)
	{
		if (addedPlayerConfigs.Contains(removedConfig))
		{
			RemoveConfigFromRow(removedConfig);
			addedPlayerConfigs.Remove(removedConfig);
			// TODO: Window reorganisation
		}
	}

	public void OnAddNewPlayerClicked()
	{
		if (addedPlayerConfigs.Count == rowLimit * windowsInLineLimit)
			return;

		GameObject newGO;
		TankConfigurationSetupWindow newConfigWindow = null;

		if (tankConfigurationPrefab != null)
		{
			newGO = Instantiate(tankConfigurationPrefab) as GameObject;
			newConfigWindow = newGO.GetComponent<TankConfigurationSetupWindow>();
		} else {
			newGO = new GameObject("Player " + (addedPlayerConfigs.Count + 1) + " - Tank Config Window");
			newConfigWindow = newGO.AddComponent<TankConfigurationSetupWindow>();
		}

		if (newConfigWindow != null)
		{
			RegisterPlayerConfig(newConfigWindow);
			AddNewConfigWindow(newConfigWindow);
		}
	}

	public void OnAddNewMousePlayerClicked()
	{
		Debug.LogError("TODO: Add Mouse Player not implemented !!!");
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

	public void RegisterPlayerConfig(TankConfigurationSetupWindow playerConfig)
	{
		if (!addedPlayerConfigs.Contains(playerConfig))
		{
			addedPlayerConfigs.Add(playerConfig);
			// TODO: Window reorganisation
		}
	}

	public void UnregisterPlayerConfig(TankConfigurationSetupWindow playerConfig)
	{
		if (addedPlayerConfigs.Contains(playerConfig))
		{
			addedPlayerConfigs.Remove(playerConfig);
			// TODO: Window reorganisation
		}
	}

	public void AddNewConfigWindow(TankConfigurationSetupWindow newConfigWindow)
	{
		if (rowObjects == null || rowObjects.Count == 0)
			return;
		
		int i = 0;
		while (i < rowObjects.Count && rowObjects[i].childConfigWindows.Count >= windowsInLineLimit && !rowObjects[i].childConfigWindows.Contains(newConfigWindow))
		{
			i++;
		}
		if (i < rowObjects.Count && !rowObjects[i].childConfigWindows.Contains(newConfigWindow))
		{
			AddConfigToRown(i, newConfigWindow);
		}

	}

	public void AddConfigToRown(int rowNum, TankConfigurationSetupWindow newConfigWindow)
	{
		if (rowObjects == null || rowObjects.Count < rowNum || rowObjects[rowNum] == null)
			return;

		rowObjects[rowNum].childConfigWindows.Add(newConfigWindow);
		newConfigWindow.transform.parent = rowObjects[rowNum].transform;
	}

	public void RemoveConfigFromRow(TankConfigurationSetupWindow configWindow)
	{
		foreach (TankConfigWindowRow row in rowObjects)
		{
			if (row.childConfigWindows.Contains(configWindow))
				row.childConfigWindows.Remove(configWindow);
		}
	}

}
