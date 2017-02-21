using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickup : BaseBulletPickup
{
	new public void Awake()
	{
		this.ownType = TankDefs.BulletType.Shotgun;
        base.Awake();
    }

}
