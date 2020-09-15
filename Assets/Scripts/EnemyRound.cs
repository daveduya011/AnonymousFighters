[System.Serializable]
public class EnemyRound
{
    public EnemySet[] enemySet;

    [System.Serializable]
    public class EnemySet
    {
        public Enemy enemyPrefab;
        public int numOfEnemies;
    }
}
