using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 2.0f; // ���̈ړ����x
    private Vector3 targetPosition; // ���̈ړ���
    private bool isWaiting = false; // �ҋ@�����ǂ����̃t���O
    private float waitTime; // �ҋ@����

    // Start is called before the first frame update
    void Start()
    {
        // ���̏����ʒu����ʂ̍��[�ɐݒ�
        targetPosition = new Vector3(15, Random.Range(7.0f, 9.0f), transform.position.z);
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting) // �ҋ@���łȂ���Έړ��𑱂���
        {
            // �����E�ֈړ�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // �����E�[�ɓ��B������A���[�ɖ߂�
            if (transform.position.x >= 15)
            {
                StartCoroutine(WaitAndChangePosition(-15));
            }
            // �������[�ɓ��B������A�E�[�Ɉړ�
            else if (transform.position.x <= -15)
            {
                StartCoroutine(WaitAndChangePosition(15));
            }
        }
    }

    private IEnumerator WaitAndChangePosition(float newX)
    {
        isWaiting = true; // �ҋ@���t���O�𗧂Ă�
        waitTime = Random.Range(5.0f, 10.0f); // �ҋ@���Ԃ�5����10�̊ԂŃ����_���ɐݒ�
        yield return new WaitForSeconds(waitTime); // �w�莞�ԑ҂�
        targetPosition = new Vector3(newX, Random.Range(7.0f, 9.0f), transform.position.z);
        isWaiting = false; // �ҋ@���t���O��������
    }
}
