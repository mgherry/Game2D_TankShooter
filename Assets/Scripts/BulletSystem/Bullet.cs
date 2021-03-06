﻿using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour
{
	public Vector3 direction;
    public float moveSpeed;

    public int maxBounceCount;
    public int currentBounceCount;

    public MyTank shooterTank;

	public float leaveShieldTimer = 0;


	public void Awake()
    {
        currentBounceCount = 0;
    }

    public void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    public void Update()
    {
		if (leaveShieldTimer > 0)
			leaveShieldTimer -= Time.deltaTime;

	}

    private void Move(float movementAmount)
    {
        direction = transform.rotation * Vector3.up;
        Vector3 moveVector = direction * movementAmount * moveSpeed;
        transform.position += moveVector;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<WallColliderSensor>() != null)
        {
			if (this.leaveShieldTimer > 0)
				this.leaveShieldTimer = 0;

            if (maxBounceCount <= currentBounceCount)
            {
                shooterTank.DecreaseCurrentBulletCount();
                Destroy(this.gameObject);
            }

            Vector3 contactPoint3D = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0.0f);
            Vector3 dir = Vector3.Reflect((contactPoint3D - transform.position).normalized, collision.contacts[0].normal);

            float angle = 0;

            if (dir.x == 0)
            {
                angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg + 180 - (2 * Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg);
            }
            else if (dir.y == 0)
            {
                angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg + 180 - (2 * Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg);
            }
            else {
                angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg + 180;
            }

            var targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = targetRot;

            currentBounceCount++;
        }

		IBulletHittable hitObject = collision.collider.GetComponent<IBulletHittable>();
		if (hitObject != null)
        {
			ShieldBehaviour myShield = collision.collider.GetComponent<ShieldBehaviour>();
			if (myShield != null && myShield.parentTank == shooterTank)
				return;

			shooterTank.DecreaseCurrentBulletCount();
			hitObject.HandleBulletHit(TankDefs.BulletType.Normal);

			Destroy(this.gameObject);
        }
    }

	public void SetShieldTimer(float waitSec = 1)
	{
		leaveShieldTimer = waitSec;
	}

	public void OnTriggerEnter2D(Collider2D collider)
	{
		IBulletHittable hitObject = collider.GetComponent<IBulletHittable>();
		if (hitObject != null)
		{
			ShieldBehaviour myShield = collider.GetComponent<ShieldBehaviour>();

			if (myShield != null && myShield.parentTank == shooterTank && leaveShieldTimer >= 0)
				return;

			shooterTank.DecreaseCurrentBulletCount();
			hitObject.HandleBulletHit(TankDefs.BulletType.Normal);

			Destroy(this.gameObject);
		}
	}
}
