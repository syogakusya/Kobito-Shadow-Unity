using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    void Start()
    {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���͂��擾
        float moveHorizontal = Input.GetAxis("Horizontal");

        // �����̃x�N�g�����쐬
        Vector2 movement = new Vector2(moveHorizontal, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // �����̕ύX
        if (moveHorizontal > 0)
        {
            // �E�Ɉړ����Ă���ꍇ�A�X�v���C�g���E������
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (moveHorizontal < 0)
        {
            // ���Ɉړ����Ă���ꍇ�A�X�v���C�g����������
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        // �A�j���[�V�����̐���
        if (moveHorizontal != 0)
        {
            // �����Ă���ꍇ
            animator.SetBool("isWalking", true);
        }
        else
        {
            // �����Ă��Ȃ��ꍇ
            animator.SetBool("isWalking", false);
        }
    }
}
