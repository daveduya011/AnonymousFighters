using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Resolution resolution;
    public PlayerController characterObject;
    public int scoreMultiplier = 10;
    public int reviveCost = 2;
    public Coin coinPrefab;
    public Gem gemPrefab;
    public PickableHealth pickableHealthPrefab;
    public DamageText enemyDamageText;
    public DamageText playerDamageText;
    public DamageText healthIncreaseText;
    public TextPrompt textPrompt;

    [HideInInspector]
    public long[] highScores;
    public string[] highScoreNames;
    [HideInInspector]
    public int scoreIndex;
    [HideInInspector]
    public bool isLoggedIn;


    [HideInInspector]
    public bool isNewHighScore;


    [Header("Universal RP Assets")]
    public UniversalRenderPipelineAsset lowQualityAsset;
    public UniversalRenderPipelineAsset midQualityAsset;
    public UniversalRenderPipelineAsset highQualityAsset;

    [HideInInspector]
    public int levelsFinished;

    [HideInInspector]
    public int score = 0;
    [HideInInspector]
    public int numOfEnemies = 0;
    [HideInInspector]
    public int highScore = 0;
    [HideInInspector]
    public int coins = 0;
    [HideInInspector]
    public int gems = 0;
    [HideInInspector]
    public int tempScore;
    [HideInInspector]
    public int tempCoins;
    [HideInInspector]
    public int tempGems;
    [HideInInspector]
    public int difficulty;
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null) {
            Destroy(this);
        } else {
            SettingsData settings = SaveSystem.LoadSettings();
            ChangeGraphics(settings.graphics);
            resolution = Screen.currentResolution;

            PlayerData data = SaveSystem.LoadPlayer("player");
            highScores = data.highScores;
            highScoreNames = data.highScoreNames;

            SignIn();

            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
    }

    private void SignIn() {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success => {
            if (success) {
                isLoggedIn = true;
            }
            else
                isLoggedIn = false;
        });
    }

    void Start() {
        
    }
    // Update is called once per frame
    void Update() {

    }

    public void EnemyDied() {
        numOfEnemies--;
        score += scoreMultiplier;
    }
    public void GameOver() {
        for (int i = 0; i < highScores.Length; i++) {
            if (score > highScores[i]) {
                for (int j = highScores.Length - 1; i < j; j--) {
                    highScores[j] = highScores[j - 1];
                    highScoreNames[j] = highScoreNames[j - 1];
                }

                highScores[i] = score;
                scoreIndex = i;
                isNewHighScore = true;

                break;
            }
        }

        PlayerData player = new PlayerData(highScoreNames, highScores);
        SaveSystem.SavePlayer(player);

        int lastCoinsCount = PlayerPrefs.GetInt("Coins");
        int lastGemsCount = PlayerPrefs.GetInt("Gems");
        PlayerPrefs.SetInt("Coins", coins + lastCoinsCount);
        PlayerPrefs.SetInt("Gems", gems + lastGemsCount);
        tempScore = score;
        tempCoins = coins;
        tempGems = gems;
        ResetValues();
        Invoke("QuitGame", 1f);
    }

    public void ResetValues() {
        Time.timeScale = 1;
        numOfEnemies = 0;
        coins = 0;
        gems = 0;
        score = 0;
        levelsFinished = 0;
    }

    private void QuitGame() {
        SceneManager.LoadScene("GameOver");
    }
    public void AddCoin(int coinValue) {
        coins += coinValue;
    }
    public void AddGem(int value) {
         gems += value;
    }

    public void ShowTextPrompt(string message) {
        TextPrompt text = Instantiate(textPrompt, Vector3.zero, Quaternion.identity);
        text.SetMessage(message);
    }

    public void ChangeGraphics(int graphicsIndex) {
        switch (graphicsIndex) {
            case 0:
                Screen.SetResolution(resolution.width / 4, resolution.height / 4, true);
                GraphicsSettings.renderPipelineAsset = lowQualityAsset;
                break;
            case 1:
                GraphicsSettings.renderPipelineAsset = midQualityAsset;
                Screen.SetResolution(resolution.width, resolution.height, true);
                break;
            case 2:
                Screen.SetResolution(resolution.width, resolution.height, true);
                GraphicsSettings.renderPipelineAsset = highQualityAsset;
                break;
        }
    }
}
