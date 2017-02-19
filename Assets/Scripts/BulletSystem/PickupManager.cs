using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : IManagerBase<PickupManager> {

	private string bulletPrefabsFolder = "Prefabs/Bullets/";

	public GameObject normalBulletPrefab;
	public string normalBulletPrefabName = "TankBullet";  

	public GameObject shotgunBulletPrefab;
	public string shotgunBulletPrefabName = "ShotgunBullet";
	
	public GameObject GetBulletPrefab(TankDefs.BulletType bulletType)
	{
		GameObject retBullet = null;

		switch (bulletType)
		{
			case TankDefs.BulletType.Normal:
				if (normalBulletPrefab != null)
					retBullet = Resources.Load(bulletPrefabsFolder + normalBulletPrefabName) as GameObject;
				else
					retBullet = Instantiate(normalBulletPrefab);
				break;

			case TankDefs.BulletType.Shotgun:
				if (shotgunBulletPrefab != null)
					retBullet = Resources.Load(bulletPrefabsFolder + shotgunBulletPrefabName) as GameObject;
				else
					retBullet = Instantiate(shotgunBulletPrefab);
				break;
		}

		return retBullet;
	}

}
