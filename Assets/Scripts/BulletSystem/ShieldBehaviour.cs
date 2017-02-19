using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour, IBulletHittable
{
	public Collider2D ownCollider = null;
	public SpriteRenderer ownSprite = null;

	public MyTank parentTank = null;

	public bool active = false;

	public void Awake()
	{
		active = false;

		if (ownCollider == null)
			ownCollider = GetComponent<Collider2D>();
		if (ownSprite == null)
			ownSprite = GetComponent<SpriteRenderer>();

		if (parentTank == null)
			parentTank = GetComponentInParent<MyTank>();
	}

	public void RegisterTank(MyTank newTank)
	{
		parentTank = newTank;
		if (newTank.shielded)
			TurnOnShield();
		else
			TurnOffShield();
	}

	public void TurnOnShield()
	{
		active = true;

		if (ownCollider != null)
		{
			ownCollider.enabled = true;
		}

		if (ownSprite != null)
		{
			ownSprite.enabled = true;
		}
	}

	public void TurnOffShield()
	{
		active = false;

		if (ownCollider != null)
		{
			ownCollider.enabled = false;
		}

		if (ownSprite != null)
		{
			ownSprite.enabled = false;
		}
	}

	public void HandleBulletHit(TankDefs.BulletType bulletType)
	{
		if (active && parentTank != null)
		{
			parentTank.TurnOffShield();
		}
	}
}
