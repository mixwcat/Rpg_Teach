using System.Collections;
using UnityEngine;
using Cinemachine; // 引入 Cinemachine 命名空间

public class AttackEffect : MonoBehaviour
{
    // 全局调用，攻击振动和停顿特效
    #region 单例模式
    private static AttackEffect instance;

    public static AttackEffect Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AttackEffect>();
            }
            return instance;
        }
    }
    #endregion

    // 攻击特效
    public void HitEffect(int Pauseduration, float Shakeduration, float strength)
    {
        StartCoroutine(Pause(Pauseduration));
        StartCoroutine(Shake(Shakeduration, strength));
    }

    #region 攻击顿帧
    private IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1f;
    }
    #endregion

    #region 攻击震动
    private IEnumerator Shake(float duration, float strength)
    {
        // 获取主摄像机或虚拟摄像机的 Transform
        Transform cameraTransform = GetCameraTransform();

        if (cameraTransform == null)
        {
            Debug.LogWarning("未找到主摄像机或虚拟摄像机，无法执行 Shake 效果！");
            yield break;
        }

        Vector3 startPosition = cameraTransform.localPosition;

        while (duration > 0)
        {
            // 让摄像机在一个随机范围内抖动
            cameraTransform.localPosition = startPosition + Random.insideUnitSphere * strength;

            duration -= Time.unscaledDeltaTime; // 使用 unscaledDeltaTime 确保不受 Time.timeScale 影响
            yield return null;
        }

        // 恢复摄像机到初始位置
        cameraTransform.localPosition = startPosition;
    }

    private Transform GetCameraTransform()
    {
        // 优先获取 Cinemachine Virtual Camera 的 Follow 目标
        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null && virtualCamera.Follow != null)
        {
            return virtualCamera.Follow;
        }

        // 如果没有虚拟摄像机，尝试获取主摄像机
        if (Camera.main != null)
        {
            return Camera.main.transform;
        }

        // 如果都没有，返回 null
        return null;
    }
    #endregion
}