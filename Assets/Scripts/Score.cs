using System.Collections;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Base score of a kill
    private int kill = 30;

    // Base score of finding a lootbox
    private int loot = 15;

    // Base score of completing an objective
    private int objective = 10;

    // Base score of planting explosives
    private int explosivePlanted = 100;

    // Base score of successful detonation 
    private int explosiveDetonation = 500;

    // Cumulative total score of kills
    private int killScore = 0;

    // Cumulative total score of loot found
    private int lootScore = 0;

    // Cumulative total of objectives completed
    private int objectiveScore = 0;

    // Cumulative total score related to the explosive
    private int explosiveScore = 0;

    // time that the level has taken in seconds
    private int timeTakenInSeconds = 0;

    // Flag to indicate if a time update is in progress.
    private bool updatingTimer = false;

    // The total amount of infected killed during the level
    private int totalKills = 0;

    // The number of headshot the player achieved
    private int headShots = 0;

    // The number of loot boxes found during the level
    private int lootFound = 0;

    // The number of mission objectives completed
    private int objectivesCompleted = 0;

    // Total number of objectives in the level
    private int totalObjectives = 0;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (!updatingTimer)
        {
            updatingTimer = true;
            StartCoroutine(AddOneSecond());
        }
    }

    public int GetTotalKills()
    {
        return totalKills;
    }

    public int GetLootFound()
    {
        return lootFound;
    }

    public int GetObjectivesCompleted()
    {
        return objectivesCompleted;
    }

    public int GetTotalObjectives()
    {
        return totalObjectives;
    }

    public void SetTotalObjectives(int objectives)
    {
        totalObjectives = objectives;
    }


    public int GetHeadShotsHit()
    {
        return headShots;
    }

    public int GetTotalTimeTaken()
    {
        return timeTakenInSeconds;
    }

    public int GetOverallScore()
    {
        return killScore + lootScore + objectiveScore + explosiveScore;
    }

    // Update the kill score when an enemy is killed
    public void KilledEnemy(bool headShot)
    {
        totalKills += 1;
        headShots += headShot ? 1 : 0;
        killScore += headShot ? kill * 5 : kill;
    }

    // Updates score when loot is found
    public void FoundLoot()
    {
        lootFound += 1;
        lootScore += loot;
    }

    // Updates score when an objective is completed
    public void CompletedObjective(int objectiveNumber)
    {
        objectivesCompleted += 1;
        objectiveScore += objective * objectiveNumber;
    }

    // Updates the score when the explosives are planted
    public void ExplosivePlanted()
    {
        explosiveScore += explosivePlanted * timeTakenInSeconds;
    }

    // Updates the score when the explosives are detonated.
    public void ExplosiveDetonated()
    {
        explosiveScore += explosiveDetonation * timeTakenInSeconds;
    }

    // Reset the scores
    public void ResetScore()
    {
        killScore = lootScore = objectiveScore = explosiveScore = timeTakenInSeconds = totalKills = headShots = lootFound = objectivesCompleted = 0;
    }

    // Increments the time taken in seconds by 1 each second
    private IEnumerator AddOneSecond()
    {
        yield return new WaitForSeconds(1);
        timeTakenInSeconds += 1;
        updatingTimer = false;
    }
}
