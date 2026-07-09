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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            // SpawnPancake の呼び出しを削除
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PancakeController pc = collision.gameObject.GetComponent<PancakeController>();
        if (pc != null)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 上から乗った場合
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    pc.OnLanded();
                    return;
                }
            }
            // 横から当たった場合
            if (GameManager.Instance.life <= 1)
            {
                // ライフがなくなる → そのまま落ちていく
                GameManager.Instance.GameOver();
            }
            else
            {
                // ライフが残る → パンケーキを止めて消す
                pc.Stop();
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                Destroy(collision.gameObject);
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            // 地面など
            isGrounded = true;
        }
    }
}