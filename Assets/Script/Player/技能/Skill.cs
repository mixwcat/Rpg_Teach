using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    //计算技能冷却
    [SerializeField] protected float cooldown;
    protected Player player;
    protected float cooldownTimer;

    // Start is called before the first frame update

    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("Skill is cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
    }
}