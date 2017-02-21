using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : BaseBulletPickup
{
	new public void Awake()
	{
		this.ownType = TankDefs.BulletType.Shield;
        base.Awake();
    }

}
