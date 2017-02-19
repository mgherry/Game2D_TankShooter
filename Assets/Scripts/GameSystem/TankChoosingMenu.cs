using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TankChoosingMenu : IManagerBase<TankChoosingMenu> {
    
    public List<TankConfiguration> allTankConfigurations = new List<TankConfiguration>();

    public void OnStartGame(TankChoosingWindowManager tankWindowManager)
    {
        allTankConfigurations = new List<TankConfiguration>();
        foreach (TankConfigurationSetupWindow tankConfigSet in tankWindowManager.addedPlayerConfigs)
        {
            GameObject newConfigGO = new GameObject("PlayerConfig");
            TankConfiguration newConfig = newConfigGO.AddComponent<TankConfiguration>();

            newConfig.MoveForward = tankConfigSet.configurationReference.MoveForward;
            newConfig.MoveBackwards = tankConfigSet.configurationReference.MoveBackwards;
            newConfig.TurnLeft = tankConfigSet.configurationReference.TurnLeft;
            newConfig.TurnRight = tankConfigSet.configurationReference.TurnRight;
            newConfig.Shoot = tankConfigSet.configurationReference.Shoot;
            newConfig.TankColor = tankConfigSet.configurationReference.TankColor;

            newConfig.transform.parent = transform;
            allTankConfigurations.Add(newConfig);
        }
        
        MenuStateManager.Instance.ChangeScene(MenuStateManager.GameStage.GameArena);
    }

    // TODO - GameArena OnStart függvényeknek interface-i


    public void OnReturnToMapMenu()
    {
        allTankConfigurations = new List<TankConfiguration>();
        MenuStateManager.Instance.ChangeScene(MenuStateManager.GameStage.MapChoosingMenu);
    }
}
