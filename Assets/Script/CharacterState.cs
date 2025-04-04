using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour
{
    public Stats damage;
    public Stats maxHp;
    public float HpFade;
    public float FadeTimer;
    public int FadeTime = 1;
    public int currentHp;
    public Image HealthIg;
    public Image HealthFade;

    private void Start()
    {
        currentHp = maxHp.GetValue();
        damage.AddModifier(4);
    }

    private void Update()
    {
        FadeTimer += Time.deltaTime;
        if (currentHp < HpFade && FadeTimer >= FadeTime)
        {
            HpFade = Mathf.Lerp(HpFade, currentHp, Time.deltaTime * 2);
        }
        HealthIg.fillAmount = currentHp / 100f;
        HealthFade.fillAmount = HpFade / 100f;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}