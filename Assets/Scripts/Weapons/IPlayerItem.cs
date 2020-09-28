using UnityEngine;

public interface IPlayerItem
{
    int GetReserveItemAmount();

    int GetCurrentItemAmount();

    AudioClip GetItemSound();

    bool HasSound();
}
