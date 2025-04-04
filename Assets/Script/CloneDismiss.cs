using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDismiss : CloneSkill
{
    private SpriteRenderer sr;
    [SerializeField] protected float colorLosingSpeed;
    [SerializeField] protected float cloneDuration;
    private float cloneTimer;
    private Transform closetEnemy;

    private void Awake()
    {
        cloneTimer = cloneDuration;
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0 && sr != null)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
        }
        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FaceClosetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closetDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closetDistance)
                {
                    closetEnemy = hit.transform;
                }
            }
        }
        if (closetEnemy != null)
        {
            if (closetEnemy.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}