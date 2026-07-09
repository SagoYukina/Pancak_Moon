using UnityEngine; // Unityの基本機能を使えるようにする

public class PlayerController : MonoBehaviour // MonoBehaviourを継承してUnityのオブジェクトとして動かす
{
    [SerializeField] private float jumpForce = 10f; // ジャンプの強さ（Inspectorで調整できる）

    private Rigidbody2D rb;       // 物理演算コンポーネントを入れる変数
    private bool isGrounded = false; // 地面またはパンケーキの上にいるかどうか

    void Start() // ゲーム開始時に1回だけ呼ばれる
    {
        rb = GetComponent<Rigidbody2D>(); // 自分自身のRigidbody2Dを取得して変数に入れる
    }

    void Update() // 毎フレーム呼ばれる
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        // 地面にいる時、かつSpaceキーまたはマウスクリックが押された時
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // 上方向に速度を与えてジャンプ
            isGrounded = false;                    // 空中にいる状態にする
            GameManager.Instance.SpawnPancake();   // パンケーキを生成するよう指示する
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // 何かに触れた瞬間に呼ばれる
    {
        // 触れたオブジェクトにPancakeControllerがついているか確認する
        PancakeController pc = collision.gameObject.GetComponent<PancakeController>();

        if (pc != null) // パンケーキに触れた場合
        {
            // 接触点のそれぞれについて、どの方向から当たったか調べる
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // 法線が上向き = 上から乗った
                {
                    isGrounded = true;  // 着地状態にする
                    pc.OnLanded();      // パンケーキ側に「乗られた」と伝える
                    return;             // ここで処理を終了する
                }
            }
            // 上から乗れなかった = 横から当たった → ゲームオーバー
            GameManager.Instance.GameOver();
        }
        else // パンケーキ以外（地面など）に触れた場合
        {
            isGrounded = true; // 着地状態にする
        }
    }
}