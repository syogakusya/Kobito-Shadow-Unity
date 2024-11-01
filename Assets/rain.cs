using System.Collections;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particleSystem; // �Ώۂ�Particle System
    public float offDuration = 2f; // �I�t�ɂ��鎞��
    public float onDuration = 3f; // �I���ɂ��鎞��

    private void Start()
    {
        StartCoroutine(ControlParticleSystem());
    }

    private IEnumerator ControlParticleSystem()
    {
        while (true) // �i���I�ɌJ��Ԃ�
        {
            // Particle System���I���ɂ���
            particleSystem.Play();
            yield return new WaitForSeconds(onDuration); // �w�肵�����ԑ҂�

            // Particle System���I�t�ɂ���
            particleSystem.Stop();
            yield return new WaitForSeconds(offDuration); // �w�肵�����ԑ҂�
        }
    }
}
