using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : IBulletPickup
{
	public void Awake()
	{
		this.ownType = TankDefs.BulletType.Shield;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		MyTank hitTank = collision.GetComponent<MyTank>();

		if (hitTank != null)
		{
			hitTank.TriggerPickup(this.ownType);
			Destroy(gameObject);
		}
	}

}
