using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackSence : MonoBehaviour
{
    private static AttackSence instance;

    public static AttackSence Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AttackSence>();
            }
            return instance;
        }
    }

    private bool isShake;

    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    private IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1f;
    }

    public void ShakeCamera(float duration, float strength)
    {
        StartCoroutine(Shake(duration, strength));
    }

    private IEnumerator Shake(float duration, float strength)
    {
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.localPosition;
        while (duration > 0)
        {
            camera.localPosition = startPosition + Random.insideUnitSphere * strength;
            duration -= Time.deltaTime;
            yield return null;
        }
        camera.position = startPosition;
    }
}