using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour, IPointerDownHandler
{
    public GameObject pausePanel;
    private PlayerController player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pausePanel.SetActive(false);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            togglePause();
        }
    }
    private void PauseGame() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        enableScripts(false);
    }
    private void ContinueGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        enableScripts(true);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        togglePause();
    }
    private void togglePause() {
        if (!pausePanel.activeInHierarchy) {
            PauseGame();
        }
        else {
            ContinueGame();
        }
    }

    private void enableScripts(bool v) {
        player.enabled = v;
    }

    public void RestartGame() {
        GameManager.Instance.ResetValues();
        enableScripts(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Resume() {
        ContinueGame();
    }

    public void QuitGame() {
        int lastCoinsCount = PlayerPrefs.GetInt("Coins");
        int lastGemsCount = PlayerPrefs.GetInt("Gems");

        PlayerPrefs.SetInt("Coins", GameManager.Instance.coins + lastCoinsCount);
        PlayerPrefs.SetInt("Gems", GameManager.Instance.gems + lastGemsCount);
        SceneManager.LoadScene(0);
        GameManager.Instance.ResetValues();
    }
}