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
    // -1にしておくと、ゲーム開始時にも最初のBGMが流れる
    private int currentIndex = -1;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (backgroundRenderer == null) return;
        if (backgrounds == null || backgrounds.Length == 0) return;

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

            // 背景に合わせてBGMを切り替える
            // 2回目以降の切り替えではステージ切り替え効果音も鳴る
            StageAudioType stage = GetStageAudioType(currentIndex);
            AudioManager.Instance?.PlayStageBgm(stage, true);
        }

        // 背景④以降なら宇宙ゾーンフラグをONにする
        isSpaceZone = (currentIndex >= 3);
    }

    // 背景番号から、どのBGMを流すかを決める
    private StageAudioType GetStageAudioType(int index)
    {
        // 背景1枚目：地面
        if (index == 0)
        {
            return StageAudioType.Ground;
        }

        // 背景2〜3枚目：雲
        if (index <= 2)
        {
            return StageAudioType.Cloud;
        }

        // 最後の背景：月
        if (index >= backgrounds.Length - 1)
        {
            return StageAudioType.Moon;
        }

        // それ以外：宇宙
        return StageAudioType.Space;
    }
}