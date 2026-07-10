using UnityEngine;
using UnityEngine.SceneManagement; // シーンの読み込みに必要

// タイトル画面の管理スクリプト
public class TitleManager : MonoBehaviour
{
    // スカイツリーモード（20枚）が選ばれた時
    public void OnEasyClicked()
    {
        // クリア枚数を20に設定して保存
        PlayerPrefs.SetInt("ClearScore", 20);
        // ゲームシーンに移動
        SceneManager.LoadScene("PlayScene");
    }

    // 宇宙モード（50枚）が選ばれた時
    public void OnNormalClicked()
    {
        PlayerPrefs.SetInt("ClearScore", 40);
        SceneManager.LoadScene("PlayScene");
    }

    // 月モード（100枚）が選ばれた時
    public void OnHardClicked()
    {
        PlayerPrefs.SetInt("ClearScore", 70);
        SceneManager.LoadScene("PlayScene");
    }

    // エンドレスモードが選ばれた時（クリアなし）
    public void OnEndlessClicked()
    {
        PlayerPrefs.SetInt("ClearScore", 0);
        SceneManager.LoadScene("PlayScene");
    }
}