using UnityEngine;

// スコアに応じて背景を切り替えるスクリプト
public class BackgroundManager : MonoBehaviour
{
    // シングルトン（他のスクリプトから参照できるようにする）
    public static BackgroundManager Instance;

    // 現在宇宙ゾーン（背景⑤以降）かどうかを外部から参照できる
    public bool isSpaceZone = false;

    // 背景のSpriteRendererコンポーネント
    [SerializeField] private SpriteRenderer backgroundRenderer;

    // 7枚の背景画像（Inspectorで設定）
    [SerializeField] private Sprite[] backgrounds;

    // 何枚乗るごとに背景が切り替わるか
    [SerializeField] private int changeInterval = 15;

    // 現在表示中の背景インデックス
    private int currentIndex = 0;

    void Awake()
    {
        Instance = this; // シングルトンを登録
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // スコアに応じた背景インデックスを計算
        int newIndex = Mathf.Min(
            GameManager.Instance.score / changeInterval,
            backgrounds.Length - 1
        );

        // 背景が変わった時だけ更新
        if (newIndex != currentIndex)
        {
            currentIndex = newIndex;
            backgroundRenderer.sprite = backgrounds[currentIndex];
        }

        // 背景⑤以降なら宇宙ゾーンフラグをONにする
        isSpaceZone = (currentIndex >= 3);
    }
}