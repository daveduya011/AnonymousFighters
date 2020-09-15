public class EnemyWolfBoss : Enemy
{
    // Start is called before the first frame update
    public override void Start() {
        base.Start();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public override void Die() {
        GameManager.Instance.score += 100;
        base.Die();
    }
}
