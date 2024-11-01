using UnityEngine;

public class RandomPositionMover : MonoBehaviour
{
    public float moveRange = 5f; // 移動範囲
    public float interval = 5f; // 移動間隔
    public float fixedZ = 0f; // 固定するz座標

    private void Start()
    {
        // 最初の位置を設定
        MoveToRandomPosition();
        // 5秒ごとにMoveToRandomPositionを呼び出す
        InvokeRepeating(nameof(MoveToRandomPosition), interval, interval);
    }

    private void MoveToRandomPosition()
    {
        // ランダムな位置を生成
        Vector3 randomPosition = new Vector3(
            Random.Range(-moveRange, moveRange),
            transform.position.y, // y軸はそのまま
            fixedZ // 固定されたz座標
        );

        // 新しい位置にオブジェクトを移動
        transform.position = randomPosition;
    }
}
