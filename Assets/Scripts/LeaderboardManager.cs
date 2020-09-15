using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public RectTransform scoreCanvas;
    public RectTransform loadingPanel;
    public ScoreRow scoreRowPrefab;

    public List<ScoreRow> offlineScoreRows;
    public List<ScoreRow> onlineScoreRows;

    public Color colorPlayer;

    private bool isOfflineScoresShown;
    
    void Start() {
    }
    public void ShowOfflineScores() {
        loadingPanel.gameObject.SetActive(false);

        foreach (ScoreRow row in onlineScoreRows) {
            row.gameObject.SetActive(false);
        }
        foreach (ScoreRow row in offlineScoreRows) {
            row.gameObject.SetActive(true);
        }

        if (isOfflineScoresShown) {
            return;
        }

        offlineScoreRows = new List<ScoreRow>();
        isOfflineScoresShown = true;

        PlayerData data = SaveSystem.LoadPlayer("player");
        for (int i = 0; i < data.highScores.Length; i++) {
            ScoreRow score = Instantiate(scoreRowPrefab, scoreCanvas.transform);
            score.UpdateScore(data.highScoreNames[i], data.highScores[i], i+1);
            offlineScoreRows.Add(score);
        }
    }

    public void ShowOnlineScores() {
        if (GameManager.Instance.isLoggedIn) {
            loadingPanel.gameObject.SetActive(true);
        } else {
            foreach (ScoreRow row in offlineScoreRows) {
                row.gameObject.SetActive(false);
            }

            GameManager.Instance.ShowTextPrompt("Online scores are not available. Turn on internet and restart game to sign in to Google Play");
            return;
        }
        
        foreach (ScoreRow row in offlineScoreRows) {
            row.gameObject.SetActive(false);
        }

        ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
        lb.timeScope = TimeScope.AllTime;
        lb.id = GPGSIds.leaderboard_online_scores;
        lb.userScope = UserScope.Global;
        lb.LoadScores(ok => {
            if (ok) {
                LoadUsersAndDisplay(lb);
            }
            else {
                GameManager.Instance.ShowTextPrompt("Failed Fetching Scores. Please try again");
            }
        });
    }

    private void LoadUsersAndDisplay(ILeaderboard lb) {
        // get the user ids
        List<string> userIds = new List<string>();

        for (int i = 0; i < lb.scores.Length; i++) {
            userIds.Add(lb.scores[i].userID);
        }

        // load the profiles and display (or in this case, log)
        Social.LoadUsers(userIds.ToArray(), (users) => {
            foreach (ScoreRow row in onlineScoreRows) {
                row.gameObject.SetActive(false);
            }
            onlineScoreRows = new List<ScoreRow>();
            loadingPanel.gameObject.SetActive(false);

            for (int i = 0; i < users.Length; i++) {
                ScoreRow score = Instantiate(scoreRowPrefab, scoreCanvas.transform);
                score.UpdateScore(users[i].userName, lb.scores[i].value, i+1);

                if (users[i].id == lb.localUserScore.userID) {
                    score.GetComponent<Image>().color = colorPlayer;
                }

                onlineScoreRows.Add(score);

            }
        });
    }
}
