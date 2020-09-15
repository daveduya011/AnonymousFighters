using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public int currentRoundNumber = 0;
    public EnemyRound[] rounds;
    public RectTransform gameOverPanel;
    public ParticleSystem spawnEffect;
    public float spawnTime = 0.5f;
    private EnemyRound currentRound;
    private bool isEnemiesSpawned;
    private float areaSize;
    private PlayerController player;
    public SpriteRenderer spawnArea; 
    private bool isInNextArea;
    private bool isBoundariesMoved = true;
    private bool isBoundariesMoving;
    private bool isSpawnAreaReached;
    public Transform leftBoundary;
    public Transform rightBoundary;
    public int levelsFinished = 0;
    public RectTransform goSignPanel;

    private Vector3 targetLeftBoundaryPos;
    private Vector3 targetRightBoundaryPos;

    void Awake() {
        if (GameManager.Instance == null) {
            SceneManager.LoadScene(0);
        } else {
            SpawnPlayer();
        }
    }
    void Start() {
        currentRound = rounds[currentRoundNumber];

        Transform bLeft = Camera.main.GetComponent<CameraController>().boundaryLeft;
        Transform bRight = Camera.main.GetComponent<CameraController>().boundaryRight;

        areaSize = spawnArea.bounds.size.x;
        ChangeBoundaryPositions();
    }

    private void ChangeBoundaryPositions() {
        isBoundariesMoving = true;
        Vector3 pos = leftBoundary.position;

        pos.x = (levelsFinished * (areaSize + areaSize / 2)) - (areaSize / 2);
        pos.x -= 5f;
        targetLeftBoundaryPos = pos;

        pos.x = pos.x + areaSize + (areaSize / 2);
        pos.x += 5f;
        targetRightBoundaryPos = pos;
    }

    private Vector3 GenerateRandomPosition() {
        return new Vector3(Random.Range(leftBoundary.position.x + areaSize + 4f,
            rightBoundary.position.x - 4f), transform.position.y, transform.position.z);
    }

    void Update() {
        if (isBoundariesMoving) {
            leftBoundary.transform.position = Vector3.Lerp(leftBoundary.transform.position, targetLeftBoundaryPos, 0.05f);
            rightBoundary.transform.position = targetRightBoundaryPos;
            isBoundariesMoving = leftBoundary.transform.position != targetLeftBoundaryPos;
        }

        if (player.transform.position.x > rightBoundary.position.x - 2f) {
            isInNextArea = true;
        } else {
            isInNextArea = false;
        }

        // If enemies in the current round is less than or equal to 0
        if (GameManager.Instance.numOfEnemies <= 0) {
            goSignPanel.gameObject.SetActive(true);

            // Go to next round of enemies
            if (isInNextArea) {
                ChangeBoundaryPositions();
                isBoundariesMoved = true;
            }

            if (isBoundariesMoved) {
                isSpawnAreaReached = (player.transform.position.x > leftBoundary.position.x + 10f);
                if (isSpawnAreaReached)
                    GoToNextRound();
            }
            
        }
    }

    private void GoToNextRound() {
        isBoundariesMoved = false;
        if (!isEnemiesSpawned) {
            isEnemiesSpawned = true;

            GameManager.Instance.numOfEnemies = 0;
            if (currentRoundNumber == rounds.Length) {
                currentRoundNumber = 0;
            }
            currentRound = rounds[currentRoundNumber];
            Invoke("SpawnEnemies", 0.5f);
        }


    }

    private void SpawnEnemies() {
        goSignPanel.gameObject.SetActive(false);

        isEnemiesSpawned = false;
        currentRoundNumber++;
        levelsFinished++;
        GameManager.Instance.levelsFinished++;
        foreach (EnemyRound.EnemySet enemySet in currentRound.enemySet) {
            for (int i = 0; i < enemySet.numOfEnemies; i++) {
                GameManager.Instance.numOfEnemies++;
                Vector3 randomPosition = GenerateRandomPosition();
                StartCoroutine(SpawnEnemy(enemySet.enemyPrefab, randomPosition, spawnTime * (i + 1)));
            }
        }
    }

    IEnumerator SpawnEnemy(Enemy enemyPrefab, Vector3 pos, float delay) {
        yield return new WaitForSeconds(delay);
        Enemy enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

        if (spawnEffect != null) {
            Instantiate(spawnEffect, pos, Quaternion.identity);
        }
    }

    private void SpawnPlayer() {
        PlayerController playerPrefab = GameManager.Instance.characterObject;
        
        if (playerPrefab != null) {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        } 
    }
}
