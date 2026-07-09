using UnityEngine;
using TMPro; // TextMeshProを使うために必要

// UIの更新を管理するスクリプト
public class UIManager : MonoBehaviour
{
    // スコア表示用のテキスト
    [SerializeField] private TextMeshProUGUI scoreText;

    // ライフ表示用のテキスト
    [SerializeField] private TextMeshProUGUI lifeText;

    void Update()
    {
        // GameManagerが存在しない場合は何もしない
        if (GameManager.Instance == null) return;

        // スコアを表示
        scoreText.text = "スコア: " + GameManager.Instance.score;

        // ライフをハートの数で表示
        string hearts = "";
        for (int i = 0; i < GameManager.Instance.life; i++)
        {
            hearts += "♥";
        }
        lifeText.text = "ライフ: " + hearts;
    }
}