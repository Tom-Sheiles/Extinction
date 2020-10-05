using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    public Text kills;

    public Text loot;

    public Text objectives;

    public Text headShots;

    public Text timeTaken;

    public Text finalScore;


    private Score score;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Score").GetComponent<Score>())
        {
            score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
            SetScores();
        }
    }

    public void Continue()
    {
        // Would get the next level here if there was one
        SceneManager.LoadScene("Menu");
    }

    // Sets the information for the level completed screen
    private void SetScores()
    {
        kills.text = score.GetTotalKills().ToString();
        loot.text = score.GetLootFound().ToString();
        objectives.text = $"{score.GetObjectivesCompleted()} / {score.GetObjectivesCompleted()}";
        headShots.text = score.GetHeadShotsHit().ToString();
        timeTaken.text = FormatTime(score.GetTotalTimeTaken());
        finalScore.text = score.GetOverallScore().ToString();
    }
    
    // Formats the time from seconds to mins and seconds. Don't expect hours will be required.
    private string FormatTime(int seconds)
    {
        int minutes = seconds / 60;

        return minutes > 0 ? $"{minutes} mins {seconds % 60} secs" : $"{seconds % 60} secs";
    }
}
