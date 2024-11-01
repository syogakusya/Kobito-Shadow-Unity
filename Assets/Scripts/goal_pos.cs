using UnityEngine;

public class RandomPositionMover : MonoBehaviour
{
    public float moveRange = 5f; // �ړ��͈�
    public float interval = 5f; // �ړ��Ԋu
    public float fixedZ = 0f; // �Œ肷��z���W

    private void Start()
    {
        // �ŏ��̈ʒu��ݒ�
        MoveToRandomPosition();
        // 5�b���Ƃ�MoveToRandomPosition���Ăяo��
        InvokeRepeating(nameof(MoveToRandomPosition), interval, interval);
    }

    private void MoveToRandomPosition()
    {
        // �����_���Ȉʒu�𐶐�
        Vector3 randomPosition = new Vector3(
            Random.Range(-moveRange, moveRange),
            transform.position.y, // y���͂��̂܂�
            fixedZ // �Œ肳�ꂽz���W
        );

        // �V�����ʒu�ɃI�u�W�F�N�g���ړ�
        transform.position = randomPosition;
    }
}
