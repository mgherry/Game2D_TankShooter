using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuStateManager : IManagerBase<MenuStateManager>
{
    public GameStage staringGameStage = GameStage.NULL;
    private GameStage currentGameStage = GameStage.NULL;

    public enum GameStage
    {
        NULL,
        StartingMenu,
        MapChoosingMenu,
        TankChoosingMenu,
        GameArena,
    }

    private string[] GameStageNames =
    {
        "MainStartingScene",
        "MainStartingScene",
        "MapChoosingScene",
        "TankChoosing",
        "GameArena"
    };

    new public void Awake()
    {
        currentGameStage = GameStage.NULL;

        if (staringGameStage != GameStage.NULL)
        {
            ChangeScene_Internal(staringGameStage);
        } else
        {
            ChangeScene_Internal(GameStage.GameArena);
        }

		base.Awake();
    }

    public void ChangeScene(GameStage newGameStage)
    {
        ChangeScene_Internal(newGameStage);
    }

    private void ChangeScene_Internal(GameStage newGameStage)
    {
        if (currentGameStage != GameStage.NULL)
            SceneManager.UnloadSceneAsync(GameStageNames[(int)currentGameStage]);

        SceneManager.LoadScene(GameStageNames[(int)newGameStage], LoadSceneMode.Additive);
		currentGameStage = newGameStage;

	}
}
