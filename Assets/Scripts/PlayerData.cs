[System.Serializable]
public class PlayerData
{
    public string[] highScoreNames;
    public long[] highScores;

    public PlayerData(PlayerData data) {
        this.highScoreNames = data.highScoreNames;
        this.highScores = data.highScores;
    }
    public PlayerData() {
        this.highScoreNames = new string[20];
        this.highScores = new long[20];
    }

    public PlayerData(string[] highScoreNames, long[] highScores) {
        this.highScoreNames = highScoreNames;
        this.highScores = highScores;
    }
}
