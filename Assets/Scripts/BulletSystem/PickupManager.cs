﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : IManagerBase<PickupManager>
{

    private string bulletPrefabsFolder = "Prefabs/Bullets/";

    public GameObject normalBulletPrefab;
    public string normalBulletPrefabName = "TankBullet";

    public GameObject longlivingBulletPrefab;
    public string longlivingBulletPrefabName = "LonglivingBullet";

    public GameObject shotgunBulletPrefab;
    public string shotgunBulletPrefabName = "ShotgunBullet";


    public int maxPickupSpawns = 10;
    public float minPickupProbability = 2; // Muts be smaller than maxPickupSpawns

    public float pickupCheckTimer = 0f;
    public float pickupIntervals = 5f;

    public List<LonglivingShotPickup> longlivingShotPickupsSpawned;
    public float longlivingShotPickupTimer = 0f;

    public List<MinigunPickup> minigunPickupsSpawned;
    public float minigunPickupTimer = 0f;
    
    public List<ShotgunPickup> shotgunPickupsSpawned;
    public float shotgunPickupTimer = 0f;

    public List<ShieldPickup> shieldPickupsSpawned;
    public float shieldPickupTimer = 0f;

  
    new public void Awake()
    {
        pickupCheckTimer = 0f;

        longlivingShotPickupsSpawned = new List<LonglivingShotPickup>();
        minigunPickupsSpawned = new List<MinigunPickup>();
        shotgunPickupsSpawned = new List<ShotgunPickup>();
        shieldPickupsSpawned = new List<ShieldPickup>();
    }

    public void Update()
    {
        /*if (pickupCheckTimer <= 0)
		{
			SetRandomSpawn();
		} else
		{
			pickupCheckTimer -= Time.deltaTime;
		}*/
    }

    public void RegisterNewPickup(TankDefs.BulletType bulletType, BaseBulletPickup pickupBehaviour)
    {
        switch (bulletType)
        {
            case TankDefs.BulletType.LonglivingShot:

                LonglivingShotPickup lsp = pickupBehaviour.GetComponent<LonglivingShotPickup>();
                if (lsp != null && !longlivingShotPickupsSpawned.Contains(lsp))
                    longlivingShotPickupsSpawned.Add(lsp);

                break;

            case TankDefs.BulletType.Minigun:

                MinigunPickup mp = pickupBehaviour.GetComponent<MinigunPickup>();
                if (mp != null && !minigunPickupsSpawned.Contains(mp))
                    minigunPickupsSpawned.Add(mp);

                break;

            case TankDefs.BulletType.Shotgun:

                ShotgunPickup sp = pickupBehaviour.GetComponent<ShotgunPickup>();
                if (sp != null && !shotgunPickupsSpawned.Contains(sp))
                    shotgunPickupsSpawned.Add(sp);

                break;

            case TankDefs.BulletType.Shield:

                ShieldPickup slp = pickupBehaviour.GetComponent<ShieldPickup>();
                if (slp != null && !shieldPickupsSpawned.Contains(slp))
                    shieldPickupsSpawned.Add(slp);

                break;

        }
    }

    public void UnRegisterPickup(TankDefs.BulletType bulletType, BaseBulletPickup pickupBehaviour)
    {
        switch (bulletType)
        {
            case TankDefs.BulletType.LonglivingShot:

                LonglivingShotPickup lsp = pickupBehaviour.GetComponent<LonglivingShotPickup>();
                if (lsp != null && longlivingShotPickupsSpawned.Contains(lsp))
                    longlivingShotPickupsSpawned.Remove(lsp);

                break;

            case TankDefs.BulletType.Minigun:

                MinigunPickup mp = pickupBehaviour.GetComponent<MinigunPickup>();
                if (mp != null && minigunPickupsSpawned.Contains(mp))
                    minigunPickupsSpawned.Remove(mp);

                break;

            case TankDefs.BulletType.Shotgun:

                ShotgunPickup sp = pickupBehaviour.GetComponent<ShotgunPickup>();
                if (sp != null && shotgunPickupsSpawned.Contains(sp))
                    shotgunPickupsSpawned.Remove(sp);

                break;


            case TankDefs.BulletType.Shield:

                ShieldPickup slp = pickupBehaviour.GetComponent<ShieldPickup>();
                if (slp != null && shieldPickupsSpawned.Contains(slp))
                    shieldPickupsSpawned.Remove(slp);

                break;
        }
    }

    public GameObject GetBulletPrefab(TankDefs.BulletType bulletType)
    {
        GameObject retBullet = null;

        switch (bulletType)
        {
            case TankDefs.BulletType.Normal:
                if (normalBulletPrefab == null)
                    retBullet = Resources.Load(bulletPrefabsFolder + normalBulletPrefabName) as GameObject;
                else
                    retBullet = Instantiate(normalBulletPrefab);
                break;

            case TankDefs.BulletType.LonglivingShot:
                if (longlivingBulletPrefab == null)
                    retBullet = Resources.Load(bulletPrefabsFolder + longlivingBulletPrefabName) as GameObject;
                else
                    retBullet = Instantiate(longlivingBulletPrefab);
                break;

            case TankDefs.BulletType.Shotgun:
                if (shotgunBulletPrefab == null)
                    retBullet = Resources.Load(bulletPrefabsFolder + shotgunBulletPrefabName) as GameObject;
                else
                    retBullet = Instantiate(shotgunBulletPrefab);
                break;
        }

        return retBullet;
    }

    public void SetRandomSpawn()
    {
        Debug.LogError("TODO: Implement PickupManager.SetRandomSpawn() !!!");

        /*
		 * Fasssssssssssssáááááááááááááááááááááááááág
		 * 
		 * int allPickups = shotgunPickupsSpawned.Count + shieldPickupsSpawned.Count;
		if (allPickups >= maxPickupSpawns)
			return;

		float dontSpawnTreshold = 1f - ((float)maxPickupSpawns - minPickupProbability)/(float)maxPickupSpawns * (1 + 0.1f * (float)allPickups) ;

		float rndChoose = Random.Range(0f, (float)maxPickupSpawns + dontSpawnTreshold);

		if (rndChoose <= dontSpawnTreshold)
		{
			pickupCheckTimer = Random.Range(1f, pickupIntervals/2);
			return;
		} else if (rndChoose >= maxPickupSpawns + dontSpawnTreshold - shieldPickupsSpawned.Count)
		{
			SpawnShieldPickup();
			pickupCheckTimer = pickupIntervals;
		}
		else if (rndChoose >= maxPickupSpawns + dontSpawnTreshold - shieldPickupsSpawned.Count - shotgunPickupsSpawned.Count)
		{
			SpawnShotgunPickup();
			pickupCheckTimer = pickupIntervals;
		}*/
    }

    public void SpawnShotgunPickup()
    {
        Debug.LogError("Spaned ShotgunPickup!");
    }

    public void SpawnShieldPickup()
    {
        Debug.LogError("Spaned ShieldnPickup!");
    }
}
