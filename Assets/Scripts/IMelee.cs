using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMelee
{
    float getDamage();
    float getAttackSpeed();
    float getAttackRange();
    float getChaseSpeed();
    Transform getTargetTransform();
}
