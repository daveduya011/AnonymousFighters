using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
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
        PlayerData data = SaveSystem.LoadPlayer("player");

        for (int i = 0; i < scorePanels.Length; i++) {
            score[i].text = data.highScores[i].ToString();
            scoreboardName[i].text = data.highScoreNames[i];
        }

        //scorePanels[GameManager.Instance.scoreIndex].GetComponent<Image>().color = highScoreColor;
    }
    
}
