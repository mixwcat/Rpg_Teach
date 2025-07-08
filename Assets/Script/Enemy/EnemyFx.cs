using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFx : MonoBehaviour
{
    private SpriteRenderer sr;
    public Material hitMat;
    private Material originalMat;
    public float flashDuration;
    private Color[] igniteColor;

    // Start is called before the first frame update
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMat = sr.material;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public IEnumerator FlashFx()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)

            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxfor(float duration)
    {
        InvokeRepeating(nameof(IgniteColorFx), 0, 0.3f);
        Invoke(nameof(CancelColorChange), duration);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
}