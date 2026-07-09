using UnityEngine; // Unityの基本機能を使えるようにする

public class PancakeController : MonoBehaviour // MonoBehaviourを継承してUnityのオブジェクトとして動かす
{
    [SerializeField] private float speed = 3f; // 左に動く速さ（Inspectorで調整できる）
    private bool landed = false;               // 少年が乗ったかどうかのフラグ

    void Update() // 毎フレーム呼ばれる
    {
        if (landed) return; // 乗られていたら動かさない

        // 左方向に speed の速さで移動する（Time.deltaTimeをかけることでフレームレートに依存しない動きにする）
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < -10f) // 画面左端より外に出たら
        {
            GameManager.Instance.OnPancakeMissed(); // ゲームマネージャーに「パンケーキが外に出た」と伝える
            Destroy(gameObject);                    // このオブジェクトを削除する
        }
    }

    public void OnLanded() // 少年が乗った時にPlayerControllerから呼ばれるメソッド
    {
        if (landed) return; // すでに乗られていたら何もしない（二重カウント防止）
        landed = true;                          // 乗られた状態にする（移動を止める）
        GameManager.Instance.OnPancakeLanded(); // ゲームマネージャーに「乗られた」と伝える
    }
    // パンケーキの移動を即座に止める
    public void Stop()
    {
        landed = true;
    }
}