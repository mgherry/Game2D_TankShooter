using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBulletPickup : MonoBehaviour {
	public TankDefs.BulletType ownType;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        MyTank hitTank = collision.GetComponent<MyTank>();

        if (hitTank != null)
        {
            hitTank.TriggerPickup(this.ownType);
            Destroy(gameObject);
        }
    }

    internal void Awake()
    {
        PickupManager.Instance.RegisterNewPickup(ownType, this);
    }

    public void OnDestroy()
    {
        PickupManager.Instance.UnRegisterPickup(this.ownType, this);
    }
}
