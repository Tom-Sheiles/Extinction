using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerItem
{
    int GetMaximumStackSize();

    int GetCurrentStackSize();
}
