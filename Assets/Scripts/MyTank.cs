using UnityEngine;
using System.Collections;

public class MyTank : MonoBehaviour
{
    public float moveSpeed;                 //How fast the tank can move.
    public float turnSpeed;					//How fast the tank can turn.
    public bool readyForRespawn;

    public float reloadSpeed;               //How many seconds it takes to reload the tank, so that it can shoot again.
    private float reloadTimer;              //A timer counting up and resets after shooting.

    public int maxBulletCount;
    public int currentBulletCount;

	public bool shielded = false;
	public bool dead = false;

	public TankDefs.BulletType bulletModifier = TankDefs.BulletType.Normal;
	public int modifierShots = 0;

	//[HideInInspector]
	public Vector3 direction;               //The direction that the tank is facing. Used for movement direction.
    public Transform muzzle;                //The muzzle of the tank. This is where the projectile will spawn.

    [Header("Components / Objects")]
    public Rigidbody2D rig;                 //The tank's Rigidbody2D component. 
    public Collider2D ownCollider = null;
	public SpriteRenderer ownSprite = null;

    public Game game;                       //The Game.cs script, located on the GameManager game object.
    public GameObject bulletPrefab;           //The projectile prefab of which the tank can shoot.

    void Awake()
    {
		InitializeaTank();
	}

    void Update()
    {
        if (rig == null)
        {
            rig = GetComponent<Rigidbody2D>();
            if (rig == null)
            {
                //   rig = gameObject.AddComponent<Rigidbody2D>();
            }
        }

        reloadTimer += Time.deltaTime;
    }

	public void InitializeaTank()
	{
		dead = false;
		currentBulletCount = 0;

		SetBulletType(bulletModifier);

		if (ownSprite == null)
		{
			ownSprite = gameObject.GetComponent<SpriteRenderer>();
			if (ownSprite == null)
			{
				ownSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
			}
		}

		if (ownSprite != null)
			ownSprite.enabled = true;
		
		if (ownCollider == null)
		{
			ownCollider = gameObject.GetComponent<Collider2D>();
			if (ownCollider == null)
			{
				ownCollider = gameObject.GetComponentInChildren<Collider2D>();
			}
		}

		if (ownCollider != null)
			ownCollider.enabled = true;

		if (rig == null)
		{
			rig = GetComponent<Rigidbody2D>();
		}
	}

    public void Move(float movementAmount)
	{
		if (dead)
			return;

		direction = transform.rotation * Vector3.up;
        Vector3 moveVector = direction * movementAmount * moveSpeed;
        transform.position += moveVector;
    }

    public void Turn(float x)
	{
		if (dead)
			return;

		transform.Rotate(-Vector3.forward * x * turnSpeed);
        direction = transform.rotation * Vector3.up;
    }

    public void Shoot()
    {
		if (dead)
			return;
		if (currentBulletCount >= maxBulletCount)
			return;
		if (reloadTimer < reloadSpeed)
			return;

		switch (this.bulletModifier)
		{
			case TankDefs.BulletType.Normal:
				ShootNormal();
				break;

			case TankDefs.BulletType.Shotgun:
				ShootShotgun();
				break;				
		}

		if (modifierShots <= 0 && bulletModifier != TankDefs.BulletType.Normal)
			SetBulletType(TankDefs.BulletType.Normal);
    }

    public void DecreaseCurrentBulletCount()
	{
		if (dead)
			return;

		if (currentBulletCount <= 0)
            return;

        currentBulletCount--;
    }

    public void SetRespawn()
    {
        readyForRespawn = true;
    }

    //Called when the tank has been dead and is ready to rejoin the game.
    public void Respawn()
    {
        readyForRespawn = false;
        transform.position = game.spawnPoints[UnityEngine.Random.Range(0, game.spawnPoints.Count)].transform.position;  //Sets the tank's position to a random spawn point.

		InitializeaTank();
    }

	public void HandleHit(TankDefs.BulletType bulletType)
	{
		Die();
	}

	public void Die()
	{
		if (ownSprite != null)
		{
			ownSprite.enabled = false;
			ownCollider.enabled = false;
			dead = true;
		}
	}

	public void TriggerPickup(TankDefs.BulletType bulletType)
	{
		SetBulletType(bulletType);
	}

	#region Shooter private functions per BulletType
	
	private void SetBulletType(TankDefs.BulletType bulletType)
	{
		switch (bulletType)
		{
			case TankDefs.BulletType.Normal:

				this.bulletModifier = TankDefs.BulletType.Normal;
				bulletPrefab = PickupManager.Instance.GetBulletPrefab(this.bulletModifier);
				
				maxBulletCount = 4;
				reloadSpeed = 1;

				break;

			case TankDefs.BulletType.Shotgun:

				this.bulletModifier = TankDefs.BulletType.Shotgun;
				bulletPrefab = PickupManager.Instance.GetBulletPrefab(this.bulletModifier);

				modifierShots = 4;
				maxBulletCount = 12;
				modifierShots = 4;
				reloadSpeed = 1.2f;        

				break;
		}
	}

	private void ShootNormal()
	{
		GameObject bulletGameObject = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity) as GameObject;    //Spawns the projectile at the muzzle.
		Bullet bullet = bulletGameObject.GetComponent<Bullet>();
		bullet.transform.position = muzzle.position;
		bullet.transform.rotation = transform.rotation;
		bullet.direction = direction;
		bullet.shooterTank = this;

		reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
		currentBulletCount++;
	}

	private void ShootShotgun()
	{
		GameObject bulletGameObject = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity) as GameObject;  
		Bullet bullet1 = bulletGameObject.GetComponent<Bullet>();
		bullet1.transform.position = muzzle.position;
		bullet1.transform.rotation = transform.rotation;
		bullet1.direction = direction;
		bullet1.shooterTank = this;

		bulletGameObject = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity) as GameObject;
		Bullet bullet2 = bulletGameObject.GetComponent<Bullet>();
		bullet2.transform.position = muzzle.position;
		bullet2.transform.rotation = Quaternion.Euler(0, 0, -25) * transform.rotation;
		bullet2.direction = direction;
		bullet2.shooterTank = this;

		bulletGameObject = Instantiate(bulletPrefab, muzzle.transform.position, Quaternion.identity) as GameObject;
		Bullet bullet3 = bulletGameObject.GetComponent<Bullet>();
		bullet3.transform.position = muzzle.position;
		bullet3.transform.rotation = Quaternion.Euler(0, 0, 25) * transform.rotation;
		bullet3.direction = direction;
		bullet3.shooterTank = this;

		reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
		currentBulletCount += 3;
		modifierShots -= 1;
	}

	#endregion

	#region Movement Helper Functions

	private bool MinisculeDistance(float dist)
	{
		if (dist <= 0.01)
			return true;
		else
			return false;
	}
	#endregion

}