using UnityEngine;

public interface IPlayerItem
{
    string GetItemName();

    int GetReserveItemAmount();

    int GetCurrentItemAmount();

    AudioClip GetItemSound();

    bool HasSound();
}
