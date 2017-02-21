using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LonglivingShotPickup : BaseBulletPickup
{

    new public void Awake()
    {
        this.ownType = TankDefs.BulletType.LonglivingShot;
        base.Awake();
    }


}

