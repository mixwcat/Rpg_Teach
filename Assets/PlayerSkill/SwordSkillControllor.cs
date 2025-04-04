using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControllor : SowrdSkill
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D collid;
    private float timer = 1f;
    private bool isReturning;
    private float returnSpeed = 1f;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collid = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    private new void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            ReturnSword();
        }
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < .1f)
                player.ClearSword();
        }
    }

    public void SetupSword(Vector2 dir, float gravityScale, float facingDir)
    {
        rb.gravityScale = gravityScale;
        rb.velocity = new Vector2(dir.x, dir.y);
        anim.SetBool("Rotate", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Rotate", false);
        collid.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
}