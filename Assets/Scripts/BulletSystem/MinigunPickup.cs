using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunPickup : BaseBulletPickup
{

	new public void Awake()
	{
		this.ownType = TankDefs.BulletType.Minigun;
        base.Awake();
	}
	
}
