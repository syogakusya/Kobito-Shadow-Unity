using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    void Start()
    {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 入力を取得
        float moveHorizontal = Input.GetAxis("Horizontal");

        // 動きのベクトルを作成
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // 向きの変更
        if (moveHorizontal > 0)
        {
            // 右に移動している場合、スプライトを右向きに
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (moveHorizontal < 0)
        {
            // 左に移動している場合、スプライトを左向きに
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        // アニメーションの制御
        if (moveHorizontal != 0)
        {
            // 動いている場合
            animator.SetBool("isWalking", true);
        }
        else
        {
            // 動いていない場合
            animator.SetBool("isWalking", false);
        }
    }
}
