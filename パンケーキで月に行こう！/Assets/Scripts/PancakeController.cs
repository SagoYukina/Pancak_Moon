using UnityEngine;

public class PancakeController : MonoBehaviour
{
    [SerializeField] private float minSpeed = 2f; // 速さの最小値
    [SerializeField] private float maxSpeed = 6f; // 速さの最大値
    [SerializeField] private float speedIncreasePerScore = 0.1f; // スコア1につき加算される速さ
    private float speed;
    private bool landed = false;

    void Start()
    {
        // スコアに応じてmin/maxを底上げしてからランダムで決める
        float bonus = GameManager.Instance.score * speedIncreasePerScore;
        speed = Random.Range(minSpeed + bonus, maxSpeed + bonus);
    }

    void Update()
    {
        if (landed) return;
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < -10f)
        {
            GameManager.Instance.OnPancakeMissed();
            Destroy(gameObject);
        }
    }

    public void OnLanded()
    {
        if (landed) return;
        landed = true;
        GameManager.Instance.OnPancakeLanded();
    }

    public void Stop()
    {
        landed = true;
    }
}