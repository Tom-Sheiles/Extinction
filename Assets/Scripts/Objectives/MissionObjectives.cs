using System.Collections.Generic;
using UnityEngine;

public class MissionObjectives : MonoBehaviour
{
    private static Dictionary<string, Objective[]> allMissionObjectives = new Dictionary<string, Objective[]>()
    {
        {
            "Level01_GriffithUniversity",
            new Objective[]
            {
                new Objective("Make it across the bridge"),
                new Objective("Find the entrance to the laboratory"),
                new Objective("Plant the explosives in the laboratory"),
                new Objective("Escape the laboratory"),
                new Objective("Make it to the extraction point")
            }
        }
    };

    public static Objective[] GetMissionObjectives(string levelName)
    {
        return allMissionObjectives[levelName];
    }
}