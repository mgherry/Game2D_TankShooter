using UnityEngine;
using System.Collections;
using System;

public class Tank : MonoBehaviour 
{
    public string TestMessage = "Basic";

    public enum MainDirection
    {
        Top,
        TopRight,
        TopLeft,
        Bottom,
        BottomRight,
        BottomLeft,
        Right,
        Left,
        NULL,
    }

	[Header("Stats")]
	public int id;							//The unique identifier for this player.
	public int health;						//The current health of the tank.
	public int maxHealth;					//The maximum health of this tank.
	public int damage;						//How much damage this tank can do when shooting a projectile.
	public float moveSpeed;					//How fast the tank can move.
	public float turnSpeed;					//How fast the tank can turn.
	public float projectileSpeed;			//How fast the tank's projectiles can move.
	public float reloadSpeed;				//How many seconds it takes to reload the tank, so that it can shoot again.
	private float reloadTimer;				//A timer counting up and resets after shooting.

	[HideInInspector]
	public Vector3 direction;				//The direction that the tank is facing. Used for movement direction.

	[Header("Bools")]
	public bool canMove;					//Can the tank move if it wants to?
	public bool canShoot;					//Can the tank shoot if it wants to?

    public bool readyForRespawn;            //The tank died and wants to start it over.

	[Header("Components / Objects")]
	public Rigidbody2D rig;					//The tank's Rigidbody2D component. 
	public GameObject projectile;			//The projectile prefab of which the tank can shoot.
	public GameObject deathParticleEffect;	//The particle effect prefab that plays when the tank dies.
	public Transform muzzle;				//The muzzle of the tank. This is where the projectile will spawn.
	public Game game;                       //The Game.cs script, located on the GameManager game object.
    public Collider2D ownCollider = null;

    [Header("Blocked Sides")]
    public bool isCollidedTop = false;
    public bool isCollidedBot = false;
    public bool isCollidedRight = false;
    public bool isCollidedLeft = false;

	void Start ()
	{
		direction = Vector3.up; //Sets the tank's direction up, as that is the default rotation of the sprite.
        ownCollider = GetComponent<Collider2D>();
    }

	//Called by the Game.cs script when the game starts.
	public void InitializeaTank ()
	{
		//Sets the tank's stats based on the Game.cs start values.
		health = game.tankStartHealth;
		maxHealth = game.tankStartHealth;
		damage = game.tankStartDamage;
		moveSpeed = game.tankStartMoveSpeed;
		turnSpeed = game.tankStartTurnSpeed;
		projectileSpeed = game.tankStartProjectileSpeed;
		reloadSpeed = game.tankStartReloadSpeed;
	}

	void Update ()
    {
        if (rig == null)
        {
            rig = GetComponent<Rigidbody2D>();
            if (rig == null)
            {
                rig = gameObject.AddComponent<Rigidbody2D>();
            }
        }

        rig.velocity = Vector2.zero;


        reloadTimer += Time.deltaTime;
	}

	//Called by the Controls.cs script. When a player presses their movement keys, it calls this function
	//sending over a "y" value, set to either 1 or 0, depending if they are moving forward or backwards.
	public void Move (float y)
	{
        if (rig == null)
        {
            rig = GetComponent<Rigidbody2D>();
            if (rig == null)
            {
                rig = gameObject.AddComponent<Rigidbody2D>();
            }
        }

        rig.velocity = Vector2.zero;

        Vector3 moveVector = Vector3.zero;
        //Vector3 moveVector = direction * y * moveSpeed;

        CheckIfCollidersStillHitting(moveVector);

        if (!(isCollidedTop || isCollidedBot || isCollidedRight  || isCollidedLeft))
        {
            transform.position += moveVector;
        } else
        {
            MainDirection movementDirection = GetDirection(moveVector);
            moveVector = GetMovementAdjustment(moveVector, movementDirection);

            transform.position += moveVector;
        }
    }

    private void ResetCollisions()
    {
        isCollidedBot = false;
        isCollidedLeft = false;
        isCollidedRight = false;
        isCollidedTop = false;
    }

    private Vector3 GetMovementAdjustment(Vector3 moveVector, MainDirection movementDirection)
    {
        Vector3 retVector = moveVector;
        
        switch (movementDirection)
        {
            case MainDirection.NULL:
                retVector = Vector3.zero;
                break;
            case MainDirection.Top:
                if (isCollidedTop)
                    retVector = Vector3.zero;
                break;
            case MainDirection.Bottom:
                if (isCollidedBot)
                    retVector = Vector3.zero;
                break;
            case MainDirection.Right:
                if (isCollidedRight)
                    retVector = Vector3.zero;
                break;
            case MainDirection.Left:
                if (isCollidedLeft)
                    retVector = Vector3.zero;
                break;

            case MainDirection.TopRight:
                if (isCollidedTop && isCollidedRight)
                    retVector = Vector3.zero;
                else if (isCollidedTop)
                    retVector.y = 0; // -0; // 0.01f;
                else if (isCollidedRight)
                    retVector.x = 0; // -0; // 0.01f;
                break;

            case MainDirection.TopLeft:
                if (isCollidedTop && isCollidedLeft)
                    retVector = Vector3.zero;
                else if (isCollidedTop)
                    retVector.y = 0; // -0; // 0.01f;
                else if (isCollidedLeft)
                    retVector.x = 0; // 0.01f;
                break;

            case MainDirection.BottomRight:
                if (isCollidedBot && isCollidedRight)
                    retVector = Vector3.zero;
                else if (isCollidedBot)
                    retVector.y = 0; // 0.01f;
                else if (isCollidedRight)
                    retVector.x = 0; // -0; // 0.01f;
                break;

            case MainDirection.BottomLeft:
                if (isCollidedBot && isCollidedLeft)
                    retVector = Vector3.zero;
                else if (isCollidedBot)
                    retVector.y = 0; // 0.01f;
                else if (isCollidedLeft)
                    retVector.x = 0; // 0.01f;
                break;
        }

        /*if (retVector != moveVector)
            ResetCollisions();*/

        retVector.z = 0;
        return retVector;
    }

    public MainDirection GetDirection(Vector3 baseVector)
    {
        if (baseVector.x == 0)
        {
            if (baseVector.y == 0)
                return MainDirection.NULL;
            else if (baseVector.y > 0)
                return MainDirection.Top;
            else if (baseVector.y < 0)
                return MainDirection.Bottom;
        }
        else if (baseVector.x > 0)
        {
            if (baseVector.y == 0)
                return MainDirection.Right;
            else if (baseVector.y > 0)
                return MainDirection.TopRight;
            else if (baseVector.y < 0)
                return MainDirection.BottomRight;
        }
        else if (baseVector.x < 0)
        {
            if (baseVector.y == 0)
                return MainDirection.Left;
            else if (baseVector.y > 0)
                return MainDirection.TopLeft;
            else if (baseVector.y < 0)
                return MainDirection.BottomLeft;
        }
        return MainDirection.NULL;
    }

    public void CheckIfCollidersStillHitting(Vector3 movementVector)
    {
        if (isCollidedTop)
            if (movementVector.y < 0)
                isCollidedTop = false;

        if (isCollidedBot)
            if (movementVector.y > 0)
                isCollidedBot = false;

        if (isCollidedRight)
            if (movementVector.x < 0)
                isCollidedRight = false;

        if (isCollidedLeft)
            if (movementVector.x > 0)
                isCollidedLeft = false;


        if (isCollidedTop)
        {

        }

    }

    //Called by the Controls.cs script. When a player presses their turn keys, it calls this function
    //sending over an "x" value, set to either 1 or 0, depending if they are moving left or right.
    public void Turn (float x)
	{
		transform.Rotate(-Vector3.forward * x * turnSpeed);
		direction = transform.rotation * Vector3.up;
	}

	//Called by the Contols.cs script. When a player presses their shoot key, it calls this function, making the tank shoot.
	public void Shoot ()
	{
		if(reloadTimer >= reloadSpeed){													//Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
			GameObject proj = Instantiate(projectile, muzzle.transform.position, Quaternion.identity) as GameObject;	//Spawns the projectile at the muzzle.
			Projectile projScript = proj.GetComponent<Projectile>();					//Gets the Projectile.cs component of the projectile object.
			projScript.tankId = id;														//Sets the projectile's tankId, so that it knows which tank it was shot by.
			projScript.damage = damage;													//Sets the projectile's damage.
			projScript.game = game;														

			//projScript.rig.velocity = direction * projectileSpeed * Time.deltaTime;		//Makes the projectile move in the same direction that the tank is facing.

			reloadTimer = 0.0f;															//Sets the reloadTimer to 0, so that we can't shoot straight away.
		}
	}

	//Called when the tank gets hit by a projectile. It sends over a "dmg" value, which is how much health the tank will lose. 
	public void Damage (int dmg)
	{
		if(game.oneHitKill){	//Is the game set to one hit kill?
			Die();				//If so instantly kill the tank.
			return;
		}

		if(health - dmg <= 0){	//If the tank's health will go under 0 when it gets damaged.
			Die();				//Kill the tank since its health will be under 0.
		}else{					//Otherwise...
			health -= dmg;		//Subtract the dmg from the tank's health.
		}
	}

	//Called when the tank's health is or under 0.
	public void Die ()
	{
		if(id == 0){				//If the tank is player 1.
			game.player2Score++;	//Add 1 to player 2's score.
		}
		if(id == 1){				//If the tank is player 2.
			game.player1Score++;	//Add 1 to player 1's score.
		}

		canMove = false;			//The tank can now not move.
		canShoot = false;			//The tank can now not shoot.

		//Particle Effect
		GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity) as GameObject;	//Spawn the death particle effect at the tank's position.
		Destroy(deathEffect, 1.5f);						//Destroy that effect in 1.5 seconds.

		transform.position = new Vector3(0, 100, 0);	//Set the tanks position outside of the map, so that it is not visible when dead.

		StartCoroutine(RespawnTimer());					//Start the RespawnTimer coroutine.
	}

    public void SetRespawn()
    {
        readyForRespawn = true;
    }

    //Called when the tank has been dead and is ready to rejoin the game.
    public void Respawn ()
	{
		canMove = true;
		canShoot = true;
        readyForRespawn = false;

        health = maxHealth;

		transform.position = game.spawnPoints[UnityEngine.Random.Range(0, game.spawnPoints.Count)].transform.position;	//Sets the tank's position to a random spawn point.
	}

	//Called when the tank dies, and needs to wait a certain time before respawning.
	IEnumerator RespawnTimer ()
	{
		yield return new WaitForSeconds(game.respawnDelay);	//Waits how ever long was set in the Game.cs script.

        // SetRespawn();                                       // Respawns all of the tanks at the same time. We set the flag for this one.
         Respawn();											//Respawns the tank.
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TestMessage = "Collision [Enter] Registered";
    }

    void OnCollisionLeave(Collision2D collision)
    {
        TestMessage = "Collision [Leave] Registered";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ownCollider == null)
        {
            ownCollider = GetComponent<Collider2D>();
            if (ownCollider == null)
                return;
        }

        WallColliderSensor collidedWall = other.gameObject.GetComponent<WallColliderSensor>();

        if (collidedWall != null)
        {
            Bounds wallBounds = other.bounds;

            Bounds ownBounds = ownCollider.bounds;

            Vector3 wallMaxPoint = wallBounds.max;
            Vector3 wallMinPoint = wallBounds.min;

            Vector3 maxPoint = ownBounds.max;
            Vector3 minPoint = ownBounds.min;

            Vector3 topRightCrn = maxPoint;
            Vector3 topLeftCrn = new Vector3(minPoint.x, maxPoint.y, 0);
            Vector3 bottomRightCrn = new Vector3(maxPoint.x, minPoint.y, 0);
            Vector3 bottomLeftCrn = minPoint;

            bool topRightBool = false;
            bool topLeftBool = false;
            bool bottomRightBool = false;
            bool bottomLeftBool = false;
            
            if (wallBounds.Contains(topRightCrn))
                topRightBool = true;
            if (wallBounds.Contains(topLeftCrn))
                topLeftBool = true;
            if (wallBounds.Contains(bottomRightCrn))
                bottomRightBool = true;
            if (wallBounds.Contains(bottomLeftCrn))
                bottomLeftBool = true;
            
            if (!topRightBool && !topLeftBool && !bottomRightBool && !bottomLeftBool)
            {
                if (MinisculeDistance(topRightCrn.y - wallBounds.min.y) || MinisculeDistance(topRightCrn.x - wallBounds.min.x))
                    topRightBool = true;
                if (MinisculeDistance(topLeftCrn.y - wallBounds.min.y) || MinisculeDistance(topLeftCrn.x - wallBounds.max.x))
                    topLeftBool = true;
                if (MinisculeDistance(bottomRightCrn.y - wallBounds.max.y) || MinisculeDistance(topLeftCrn.x - wallBounds.min.x))
                    bottomRightBool = true;
                if (MinisculeDistance(bottomLeftCrn.y - wallBounds.max.y) || MinisculeDistance(topLeftCrn.x - wallBounds.max.x))
                    bottomLeftBool = true;
            }


            if (topRightBool && topLeftBool)
                isCollidedTop = true;
            if (bottomRightBool && bottomLeftBool)
                isCollidedBot = true;
            if (topRightBool && bottomRightBool)
                isCollidedRight = true;
            if (bottomLeftBool && topLeftBool)
                isCollidedLeft = true;
            
            Vector3 dist = other.transform.position - transform.position;

            if (topRightBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedTop = true;
                else
                    isCollidedRight = true;
            }

            if (topLeftBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedTop = true;
                else
                    isCollidedLeft = true;
            }

            if (bottomRightBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedBot = true;
                else
                    isCollidedRight = true;
            }

            if (topLeftBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedBot = true;
                else
                    isCollidedLeft = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (ownCollider == null)
        {
            ownCollider = GetComponent<Collider2D>();
            if (ownCollider == null)
                return;
        }

        WallColliderSensor collidedWall = other.gameObject.GetComponent<WallColliderSensor>();

        if (collidedWall != null)
        {
            isCollidedBot = false;
            isCollidedTop = false;
            isCollidedRight = false;
            isCollidedLeft = false;

            Bounds wallBounds = other.bounds;

            Bounds ownBounds = ownCollider.bounds;

            Vector3 wallMaxPoint = wallBounds.max;
            Vector3 wallMinPoint = wallBounds.min;

            Vector3 maxPoint = ownBounds.max;
            Vector3 minPoint = ownBounds.min;

            Vector3 topRightCrn = maxPoint;
            Vector3 topLeftCrn = new Vector3(minPoint.x, maxPoint.y, 0);
            Vector3 bottomRightCrn = new Vector3(maxPoint.x, minPoint.y, 0);
            Vector3 bottomLeftCrn = minPoint;

            bool topRightBool = false;
            bool topLeftBool = false;
            bool bottomRightBool = false;
            bool bottomLeftBool = false;

            if (wallBounds.Contains(topRightCrn))
                topRightBool = true;
            if (wallBounds.Contains(topLeftCrn))
                topLeftBool = true;
            if (wallBounds.Contains(bottomRightCrn))
                bottomRightBool = true;
            if (wallBounds.Contains(bottomLeftCrn))
                bottomLeftBool = true;

            if (!topRightBool && !topLeftBool && !bottomRightBool && !bottomLeftBool)
            {
                if (MinisculeDistance(topRightCrn.y - wallBounds.min.y) || MinisculeDistance(topRightCrn.x - wallBounds.min.x))
                    topRightBool = true;
                if (MinisculeDistance(topLeftCrn.y - wallBounds.min.y) || MinisculeDistance(topLeftCrn.x - wallBounds.max.x))
                    topLeftBool = true;
                if (MinisculeDistance(bottomRightCrn.y - wallBounds.max.y) || MinisculeDistance(topLeftCrn.x - wallBounds.min.x))
                    bottomRightBool = true;
                if (MinisculeDistance(bottomLeftCrn.y - wallBounds.max.y) || MinisculeDistance(topLeftCrn.x - wallBounds.max.x))
                    bottomLeftBool = true;
            }


            if (topRightBool && topLeftBool)
                isCollidedTop = true;
            if (bottomRightBool && bottomLeftBool)
                isCollidedBot = true;
            if (topRightBool && bottomRightBool)
                isCollidedRight = true;
            if (bottomLeftBool && topLeftBool)
                isCollidedLeft = true;

            Vector3 dist = other.transform.position - transform.position;

            if (topRightBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedTop = true;
                else
                    isCollidedRight = true;
            }

            if (topLeftBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedTop = true;
                else
                    isCollidedLeft = true;
            }

            if (bottomRightBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedBot = true;
                else
                    isCollidedRight = true;
            }

            if (topLeftBool)
            {
                if (Mathf.Abs(dist.y) > Mathf.Abs(dist.x))
                    isCollidedBot = true;
                else
                    isCollidedLeft = true;
            }
        }
    }

    private bool MinisculeDistance(float dist)
    {
        if (dist <= 0.01)
            return true;
        else
            return false;
    }
}
