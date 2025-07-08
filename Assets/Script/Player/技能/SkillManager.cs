using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //技能种类
    //中转调用技能
    public static SkillManager instance;

    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public SowrdSkill sword { get; private set; }
    public BlackHoleSkill blackHole { get; private set; }
    public CrystalSkill crystal { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SowrdSkill>();
        blackHole = GetComponent<BlackHoleSkill>();
        crystal = GetComponent<CrystalSkill>();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}