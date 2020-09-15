using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class OnlineScoreboard : MonoBehaviour
{
    public RectTransform[] scorePanels;
    private Text[] score;
    private Text[] scoreboardName;
    //public Color highScoreColor;

    void Awake() {
        score = new Text[scorePanels.Length];
        scoreboardName = new Text[scorePanels.Length];

        for (int i = 0; i < scorePanels.Length; i++) {
            score[i] = scorePanels[i].Find("score").GetComponent<Text>();
            scoreboardName[i] = scorePanels[i].Find("name").GetComponent<Text>();
        }
        UpdateScoreboard();
    }
    public void UpdateScoreboard() {
        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.timeScope = TimeScope.AllTime;
        lb.id = GPGSIds.leaderboard_online_scores;
        lb.userScope = UserScope.Global;
        lb.LoadScores(ok => {
            if (ok) {
                LoadUsersAndDisplay(lb);
            }
            else {
                
            }
        });

        //scorePanels[GameManager.Instance.scoreIndex].GetComponent<Image>().color = highScoreColor;
    }

    private void LoadUsersAndDisplay(ILeaderboard lb) {
        // get the user ids
        List<string> userIds = new List<string>();

        for (int i = 0; i < lb.scores.Length; i++) {
            if (i >= scorePanels.Length) 
                break;
            
            userIds.Add(lb.scores[i].userID);
        }

        // load the profiles and display (or in this case, log)
        Social.LoadUsers(userIds.ToArray(), (users) =>
        {
            foreach (IScore score in lb.scores) {
                
            }
            for (int i = 0; i < scorePanels.Length; i++) {
                score[i].text = lb.scores[i].value.ToString();
                scoreboardName[i].text = users[i].userName;
            }
        });
    }

}
