using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float currentSurfaceY = -4.0f;
    public float pancakeThickness = 0.3f;
    public bool isGameOver = false;
    public int score = 0;
    public int life = 3;
    public int clearScore = 10;

    [SerializeField] private GameObject pancakePrefab;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject scoreUI;
    [SerializeField] private GameObject lifeUI;
    [SerializeField] private GameObject scoreBord;

    // ResultPanelのImageコンポーネント（背景画像の切り替えに使う）
    [SerializeField] private Image resultBackground;

    // 難易度別のクリア画像
    [SerializeField] private Sprite resultImageEasy;   // Easyクリア画像（雲・虹）
    [SerializeField] private Sprite resultImageNormal; // Normalクリア画像（宇宙）
    [SerializeField] private Sprite resultImageHard;   // Hardクリア画像（月面）

    private bool canSpawn = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int savedClearScore = PlayerPrefs.GetInt("ClearScore", 10);

        if (savedClearScore > 0)
            clearScore = savedClearScore;
        else
            clearScore = int.MaxValue;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);

        Invoke("SpawnPancake", 3.0f);
    }

    private void HideGameUI()
    {
        if (scoreUI != null) scoreUI.SetActive(false);
        if (lifeUI != null) lifeUI.SetActive(false);
        if (scoreBord != null) scoreBord.SetActive(false);
    }

    // 難易度に応じたクリア画像を返す（エンドレスモードはnull）
    private Sprite GetResultImage()
    {
        if (clearScore == int.MaxValue) return null; // エンドレスモードは画像なし
        if (clearScore <= 20) return resultImageEasy;   // Easy（20枚）
        if (clearScore <= 50) return resultImageNormal; // Normal（50枚）
        return resultImageHard;                          // Hard（100枚）以上
    }

    public void SpawnPancake()
    {
        if (!canSpawn || isGameOver) return;
        float spawnY = currentSurfaceY + pancakeThickness / 2f;
        Instantiate(pancakePrefab, new Vector3(10f, spawnY, 0f), Quaternion.identity);
        canSpawn = false;
    }

    public void OnPancakeLanded()
    {
        score++;
        currentSurfaceY += pancakeThickness;
        canSpawn = true;
        Debug.Log("スコア: " + score);

        if (score >= clearScore)
        {
            isGameOver = true;
            HideGameUI();
            AudioManager.Instance?.PlayClear();

            if (resultPanel != null)
            {
                if (resultBackground != null)
                {
                    Sprite img = GetResultImage();
                    if (img != null)
                    {
                        // 難易度別画像を設定して完全表示
                        resultBackground.sprite = img;
                        resultBackground.color = Color.white;
                    }
                    else
                    {
                        // エンドレスモードは背景画像を透明にする
                        resultBackground.color = new Color(1, 1, 1, 0);
                    }
                }

                resultPanel.SetActive(true);
                resultText.text = "クリア！\nスコア: " + score;
            }
        }
        else
        {
            Invoke("SpawnPancake", 1.0f);
        }
    }

    public void OnPancakeMissed()
    {
        canSpawn = true;
        Invoke("SpawnPancake", 1.0f);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        life--;
        Debug.Log("ライフ残り: " + life);

        if (life <= 0)
        {
            AudioManager.Instance?.PlayLifeZero();
            isGameOver = true;
            HideGameUI();
            Debug.Log("ゲームオーバー！スコア: " + score);
            AudioManager.Instance?.PlayGameOver();
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }
        else
        {
            AudioManager.Instance?.PlayDamage();
            canSpawn = true;
            Invoke("SpawnPancake", 1.5f);
        }
    }
}