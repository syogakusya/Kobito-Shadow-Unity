using System.Collections;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particleSystem; // 対象のParticle System
    public float offDuration = 2f; // オフにする時間
    public float onDuration = 3f; // オンにする時間

    private void Start()
    {
        StartCoroutine(ControlParticleSystem());
    }

    private IEnumerator ControlParticleSystem()
    {
        while (true) // 永続的に繰り返す
        {
            // Particle Systemをオンにする
            particleSystem.Play();
            yield return new WaitForSeconds(onDuration); // 指定した時間待つ

            // Particle Systemをオフにする
            particleSystem.Stop();
            yield return new WaitForSeconds(offDuration); // 指定した時間待つ
        }
    }
}
