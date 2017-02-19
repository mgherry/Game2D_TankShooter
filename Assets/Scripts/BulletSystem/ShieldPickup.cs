using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : IBulletPickup
{
	public void Awake()
	{
		this.ownType = TankDefs.BulletType.Shield;
		PickupManager.Instance.RegisterNewPickup(ownType, this);
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

	public void OnDestroy()
	{
		PickupManager.Instance.UnRegisterPickup(ownType, this);
	}
}
