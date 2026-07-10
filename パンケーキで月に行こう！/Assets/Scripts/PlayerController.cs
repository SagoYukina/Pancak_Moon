using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite idleSpriteSpace;
    [SerializeField] private Sprite jumpSpriteSpace;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // BackgroundManagerの現在の背景が宇宙ゾーンかどうかで待機スプライトを返す
    private Sprite GetIdleSprite()
    {
        if (BackgroundManager.Instance != null && BackgroundManager.Instance.isSpaceZone)
            return idleSpriteSpace;
        return idleSprite;
    }

    // BackgroundManagerの現在の背景が宇宙ゾーンかどうかでジャンプスプライトを返す
    private Sprite GetJumpSprite()
    {
        if (BackgroundManager.Instance != null && BackgroundManager.Instance.isSpaceZone)
            return jumpSpriteSpace;
        return jumpSprite;
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        // 着地中は毎フレームスプライトを確認・更新（背景が変わった瞬間に服も切り替わる）
        if (isGrounded)
        {
            spriteRenderer.sprite = GetIdleSprite();
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            // ジャンプ音を鳴らす
            AudioManager.Instance?.PlayJump();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            spriteRenderer.sprite = GetJumpSprite();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PancakeController pc = collision.gameObject.GetComponent<PancakeController>();
        if (pc != null)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    pc.OnLanded();
                    spriteRenderer.sprite = GetIdleSprite();
                    return;
                }
            }

            if (GameManager.Instance.life <= 1)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                pc.Stop();
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                Destroy(collision.gameObject);
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            isGrounded = true;
            spriteRenderer.sprite = GetIdleSprite();
        }
    }
}