using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int score = GameManager.Instance.tempScore;
        GetComponent<Text>().text = score.ToString();

        Social.ReportScore(score, GPGSIds.leaderboard_online_scores, success => {
        });
    }

}
