using UnityEngine;

// カメラを少年に追従させるスクリプト
public class CameraController : MonoBehaviour
{
    // 追従対象（少年）
    [SerializeField] private Transform target;

    // 画面の何割の位置で追従を開始するか（0.5 = 中央）
    [SerializeField] private float followThreshold = 0.4f;

    // メインカメラの参照
    private Camera cam;

    void Start()
    {
        // カメラコンポーネントを取得
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // ターゲットがいなければ何もしない
        if (target == null) return;

        // 少年のワールド座標をビューポート座標（0〜1）に変換
        Vector3 viewPos = cam.WorldToViewportPoint(target.position);

        // 少年が画面上部40%より上に来たらカメラを上に動かす
        if (viewPos.y > followThreshold)
        {
            // カメラのY座標だけを更新（XとZはそのまま）
            Vector3 camPos = transform.position;
            camPos.y = target.position.y - (cam.orthographicSize * (2f * followThreshold - 1f));
            transform.position = camPos;
        }
    }
}