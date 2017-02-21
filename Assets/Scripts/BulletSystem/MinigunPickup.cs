using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunPickup : IBulletPickup
{

	public void Awake()
	{
		this.ownType = TankDefs.BulletType.Minigun;
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
		PickupManager.Instance.UnRegisterPickup(this.ownType, this);
	}
}
