using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    void Start()
    {
        LoadManager.loadLevel("Level01_GriffithUniversity");
    }
}
