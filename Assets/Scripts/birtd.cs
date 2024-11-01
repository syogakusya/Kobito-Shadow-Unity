using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 2.0f; // 鳥の移動速度
    private Vector3 targetPosition; // 鳥の移動先
    private bool isWaiting = false; // 待機中かどうかのフラグ
    private float waitTime; // 待機時間

    // Start is called before the first frame update
    void Start()
    {
        // 鳥の初期位置を画面の左端に設定
        targetPosition = new Vector3(15, Random.Range(7.0f, 9.0f), transform.position.z);
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting) // 待機中でなければ移動を続ける
        {
            // 鳥を右へ移動
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 鳥が右端に到達したら、左端に戻る
            if (transform.position.x >= 15)
            {
                StartCoroutine(WaitAndChangePosition(-15));
            }
            // 鳥が左端に到達したら、右端に移動
            else if (transform.position.x <= -15)
            {
                StartCoroutine(WaitAndChangePosition(15));
            }
        }
    }

    private IEnumerator WaitAndChangePosition(float newX)
    {
        isWaiting = true; // 待機中フラグを立てる
        waitTime = Random.Range(5.0f, 10.0f); // 待機時間を5から10の間でランダムに設定
        yield return new WaitForSeconds(waitTime); // 指定時間待つ
        targetPosition = new Vector3(newX, Random.Range(7.0f, 9.0f), transform.position.z);
        isWaiting = false; // 待機中フラグを下げる
    }
}
