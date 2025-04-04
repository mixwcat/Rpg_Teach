using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DashSkill : Skill
{
    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("use dash skill");
    }
}