using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("创建克隆")]
    [SerializeField] protected GameObject clonePrefab;
    [SerializeField] private bool creatCloneOnDashStart;
    [SerializeField] private bool creatCloneOnDashOver;

    [Header("水晶替代克隆")]
    [SerializeField] private bool replaceCrystalWithClone;

    public void CreateClone(Transform newtransform)
    {
        if (replaceCrystalWithClone)
        {
            SkillManager.instance.crystal.CreateCrystal(newtransform);
            return;
        }
        GameObject newClone = Instantiate(clonePrefab, newtransform.position, Quaternion.identity);
        newClone.GetComponent<Clone_Itself>().FaceClosetTarget();
    }

    public void CreatCloneOnDashStart()
    {
        if (creatCloneOnDashStart)
        {
            CreateClone(player.transform);
        }
    }
    
    public void CreatCloneOnDashOver()
    {
        if (creatCloneOnDashOver)
        {
            CreateClone(player.transform);
        }
    }
}