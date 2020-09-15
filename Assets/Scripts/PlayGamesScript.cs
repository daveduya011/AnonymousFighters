using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class PlayGamesScript : MonoBehaviour
{
    public static void AddScoreToLeaderboard(string leaderboardId, long score) {
        Social.ReportScore(score, leaderboardId, success => { }); 
    }

    public void ShowLeaderboardUI() {
        Social.ShowLeaderboardUI();
    }

}
