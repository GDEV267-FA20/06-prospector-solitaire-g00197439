using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An enum to handle all the possible scoring events
public enum eScoreEvent
{
    draw,
    mine,
    mineGold,
    gameWin,
    gameLoss
}

// ScoreManager handles all of the scoring
public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;

    static public int scoreFromPrevRound = 0;
    static public int highScore = 0;

    [Header("Set Dynamically")]
    // Fields to track score info
    public int chain = 0;
    public int scoreRun = 0;
    public int score = 0;

    private void Awake()
    {
        S = this; // Set the private singleton

        // Check for a high score in PlayerPrefs
        if (PlayerPrefs.HasKey("ProspectorHighScore"))
        {
            highScore = PlayerPrefs.GetInt("ProspectorHighScore");
        }
        // Add the score from the last round, which will be >0 if it was a win
        score += scoreFromPrevRound;
        // And reset the score from the previous round
        scoreFromPrevRound = 0;
    }

    static public void EVENT(eScoreEvent evt) {
        try {  // try-catch stops an error from breaking your program
            S.Event(evt);
        } catch (System.NullReferenceException nre) {
            Debug.LogError("ScoreManager:Event() called while S = null.\n"+nre);
        }
    }

    void Event (eScoreEvent evt)
    {
        switch(evt)
        {
            // Same things need to happen whether it's a draw, a win, or a loss
            case eScoreEvent.draw:
            case eScoreEvent.gameWin:
            case eScoreEvent.gameLoss:
                chain = 0;
                score += scoreRun;
                scoreRun = 0;
                break;

            case eScoreEvent.mine:
                chain++;
                scoreRun += chain;
                break;
        }

        // This second switch statement handles round wins and losses
        switch(evt)
        {
            case eScoreEvent.gameWin:
                // If it's a win, add the score to the next round
                // static field are NOT reset by SceneManager.LoadScene()
                scoreFromPrevRound = score;
                print("You won this round! Round score: " + score);
                break;

            case eScoreEvent.gameLoss:
                // If it's a loss, check against the high score
                if(highScore <= score)
                {
                    print("You got the high score! High score: " + score);
                    highScore = score;
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                } else
                {
                    print("Your final score for the game was: " + score);
                }
                break;

            default:
                print("score: " + score + " scoreRun:" + scoreRun + " chain:" + chain);
                break;
        }
    }

    static public int CHAIN { get { return S.chain; } }
    static public int SCORE { get { return S.score; } }
    static public int SCORE_RUN { get { return S.scoreRun; } }
}