using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour 
{
	[Header("Player 1 Controls")]
	public KeyCode p1MoveForward;
	public KeyCode p1MoveBackwards;
	public KeyCode p1TurnLeft;
	public KeyCode p1TurnRight;
	public KeyCode p1Shoot;

	[Header("Player 2 Controls")]
	public KeyCode p2MoveForward;
	public KeyCode p2MoveBackwards;
	public KeyCode p2TurnLeft;
	public KeyCode p2TurnRight;
	public KeyCode p2Shoot;

	[Header("Components")]
	public Game game;

	public void Awake()
	{
		InitializeTankControlls();
	}

	public void InitializeTankControlls()
	{
		if (TankChoosingMenu.Instance.allTankConfigurations != null && TankChoosingMenu.Instance.allTankConfigurations.Count >= 2)
		{
			TankConfiguration player1Config = TankChoosingMenu.Instance.allTankConfigurations[0];
			p1MoveForward = player1Config.MoveForward;
			p1MoveBackwards = player1Config.MoveBackwards;
			p1TurnLeft = player1Config.TurnLeft;
			p1TurnRight = player1Config.TurnRight;
			p1Shoot = player1Config.Shoot;

			TankConfiguration player2Config = TankChoosingMenu.Instance.allTankConfigurations[1];
			p2MoveForward = player2Config.MoveForward;
			p2MoveBackwards = player2Config.MoveBackwards;
			p2TurnLeft = player2Config.TurnLeft;
			p2TurnRight = player2Config.TurnRight;
			p2Shoot = player2Config.Shoot;
		}
	}

	void FixedUpdate ()
	{
        //Time.deltaTime;

		//Quit Game
		if(Input.GetKeyDown(KeyCode.Escape)){
            //MenuGuiManager.Instance

            MenuGuiManager.Instance.GoToMenu();
		}

        //Player 1
        //game.player1Tank.rig.velocity = Vector2.zero;

        int dir = 1;

        if (Input.GetKey(p1MoveForward))
        {
            game.Player1Tank.Move(dir * Time.fixedDeltaTime);
            //game.Player1Tank.Move(dir * Time.deltaTime);
        }
        if (Input.GetKey(p1MoveBackwards))
        {
            dir = -1;
            game.Player1Tank.Move(dir * Time.fixedDeltaTime);
            //game.Player1Tank.Move(dir * Time.deltaTime);
        }
        if (Input.GetKey(p1TurnLeft))
        {
            game.Player1Tank.Turn(-1 * dir * Time.fixedDeltaTime);
            //game.Player1Tank.Turn(-1 * dir * Time.deltaTime);
        }
        if (Input.GetKey(p1TurnRight))
        {
            game.Player1Tank.Turn(1 * dir * Time.fixedDeltaTime);
            //game.Player1Tank.Turn(1 * dir * Time.deltaTime);
        }

        if (Input.GetKeyDown(p1Shoot))
        {
            game.Player1Tank.Shoot();
        }

        //Player 2
        //game.player2Tank.rig.velocity = Vector2.zero;
        dir = 1;
        if (Input.GetKey(p2MoveForward))
        {
            game.Player2Tank.Move(1 * Time.deltaTime);
        }
        if (Input.GetKey(p2MoveBackwards))
        {
            dir = -1;
            game.Player2Tank.Move(-1 * Time.deltaTime);
        }
        if (Input.GetKey(p2TurnLeft))
        {
            game.Player2Tank.Turn(-1 * dir * Time.deltaTime);
        }
        if (Input.GetKey(p2TurnRight))
        {
            game.Player2Tank.Turn(1 * dir * Time.deltaTime);
        }

        if (Input.GetKeyDown(p2Shoot))
        {
            game.Player2Tank.Shoot();
        }

    }
}
