using UnityEngine;
using System.Collections;

public class TankConfiguration : MonoBehaviour {

    [Header("Keyboard Controls")]
    public KeyCode MoveForward;
    public KeyCode MoveBackwards;
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode Shoot;

    [Header("Keyboard Controls")]
    public Color TankColor;

	public void Awake()
	{
		TankChoosingMenu.Instance.RegisterTankConfig(this);
	}

	public void OnDestroy()
	{
		TankChoosingMenu.Instance.UnregisterTankConfig(this);
	}

	public override bool Equals(object other)
	{
		TankConfiguration otherTank = other as TankConfiguration;

		if (otherTank == null)
			return false;

		return
			MoveForward == otherTank.MoveForward &&
			MoveBackwards == otherTank.MoveBackwards &&
			TurnLeft == otherTank.TurnLeft &&
			TurnRight == otherTank.TurnRight &&
			Shoot == otherTank.Shoot &&
			TankColor == otherTank.TankColor;
	}

	public override int GetHashCode()
	{
		return (int)MoveForward * (int)MoveBackwards * (int)TurnLeft * (int)TurnRight * (int)Shoot * TankColor.GetHashCode();
	}

}
