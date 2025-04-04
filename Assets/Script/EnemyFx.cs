using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFx : MonoBehaviour
{
    private SpriteRenderer sr;
    public Material hitMat;
    private Material originalMat;
    public float flashDuration;

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
}