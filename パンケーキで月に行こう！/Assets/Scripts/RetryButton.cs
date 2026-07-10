using UnityEngine;
using UnityEngine.SceneManagement; // シーンの読み込みに必要

// リトライボタンの処理
public class RetryButton : MonoBehaviour
{
    // ボタンが押された時に呼ばれる
    public void OnRetryClicked()
    {
        AudioManager.Instance?.PlayButtonClick();

        // 現在のシーンを再読み込みしてゲームをリセット
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // タイトルに戻る
    public void OnTitleClicked()
    {
        AudioManager.Instance?.PlayButtonClick();

        SceneManager.LoadScene("TitleScene");
    }
}