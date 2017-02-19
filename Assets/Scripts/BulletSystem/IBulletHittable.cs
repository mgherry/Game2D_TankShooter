using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBulletHittable
{
	void HandleBulletHit(TankDefs.BulletType bulletType);
}
