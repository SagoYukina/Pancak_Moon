using UnityEngine;
using TMPro; // TextMeshProを使うために必要

// ゲーム全体を管理するシングルトン
public class GameManager : MonoBehaviour
{
    // 唯一のインスタンスを保持する静的変数
    public static GameManager Instance;

    // パンケーキが積み上がる現在の最上面のY座標
    public float currentSurfaceY = -4.0f;

    // パンケーキ1枚の厚さ
    public float pancakeThickness = 0.3f;

    // ゲームオーバー状態かどうか
    public bool isGameOver = false;

    // 現在のスコア（乗ったパンケーキの枚数）
    public int score = 0;

    // 残りライフ
    public int life = 3;

    // クリアに必要なパンケーキの枚数（仮）
    public int clearScore = 10;

    // パンケーキのPrefab
    [SerializeField] private GameObject pancakePrefab;

    // ゲームオーバーパネル
    [SerializeField] private GameObject gameOverPanel;

    // リザルトパネル
    [SerializeField] private GameObject resultPanel;

    // リザルトのテキスト
    [SerializeField] private TextMeshProUGUI resultText;

    // 次のパンケーキを出してよいか
    private bool canSpawn = true;

    void Awake()
    {
        // シングルトンとして自分自身を登録
        Instance = this;
    }

    void Start()
    {
        // PlayerPrefsからクリア枚数を読み込む（0ならエンドレス）
        int savedClearScore = PlayerPrefs.GetInt("ClearScore", 10);
        if (savedClearScore > 0)
            clearScore = savedClearScore;
        else
            clearScore = int.MaxValue; // エンドレスは事実上無限

        // 最初はゲームオーバーパネルを非表示
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 最初はリザルトパネルを非表示
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }
    // パンケーキを生成する
    public void SpawnPancake()
    {
        // 生成不可またはゲームオーバーなら何もしない
        if (!canSpawn || isGameOver) return;

        // 現在の最上面の少し上にパンケーキを出す
        float spawnY = currentSurfaceY + pancakeThickness / 2f;
        Instantiate(pancakePrefab, new Vector3(10f, spawnY, 0f), Quaternion.identity);
        canSpawn = false;
    }

    // パンケーキに乗った時の処理
    public void OnPancakeLanded()
    {
        score++;
        currentSurfaceY += pancakeThickness;
        canSpawn = true;
        Debug.Log("スコア: " + score);

        // クリアスコアに達したらリザルト画面を表示
        if (score >= clearScore)
        {
            isGameOver = true;
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
                resultText.text = "クリア！\nスコア: " + score;
            }
        }
    }

    // パンケーキを逃した時の処理
    public void OnPancakeMissed()
    {
        canSpawn = true;
    }

    // ゲームオーバー処理
    public void GameOver()
    {
        // すでにゲームオーバーなら何もしない
        if (isGameOver) return;

        // ライフを1減らす
        life--;
        Debug.Log("ライフ残り: " + life);

        // ライフが0以下になったら本当のゲームオーバー
        if (life <= 0)
        {
            isGameOver = true;
            Debug.Log("ゲームオーバー！スコア: " + score);

            // ゲームオーバーパネルを表示
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
        else
        {
            // ライフが残っていれば続行
            canSpawn = true;
        }
    }
}