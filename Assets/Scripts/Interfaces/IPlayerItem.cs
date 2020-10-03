using UnityEngine;

public interface IPlayerItem
{
    string GetItemName();

    int GetReserveItemAmount();

    int GetCurrentItemAmount();

    void Add(int amount);

    AudioClip GetItemSound();

    bool HasSound();
}
