using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab; //水晶预制体
    private GameObject currentCrystal; //当前水晶实例
    [SerializeField] private float crystalDuration; //技能持续时间

    public override void UseSkill()
    {
        base.UseSkill(); //调用基类的技能使用方法

        if (currentCrystal == null) //如果当前没有水晶实例
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity); //实例化水晶
            CrystalSkillControllor currentCrystalScript = currentCrystal.GetComponent<CrystalSkillControllor>(); //获取水晶控制器组件

            currentCrystalScript.SetupCrystal(crystalDuration); //设置水晶存在时间
        }
        else //如果当前有水晶实例
        {
            Vector3 positionOfPlayer = player.transform.position; //记录玩家位置
            player.transform.position = currentCrystal.transform.position; //将玩家位置移动到水晶位置
            currentCrystal.transform.position = positionOfPlayer; //将水晶位置移动到玩家位置
            currentCrystal.GetComponent<CrystalSkillControllor>().FinishCrystal(); //销毁水晶实例
        }
    }

    public void CreateCrystal(Transform newtransform)
    {
        GameObject newCrystal = Instantiate(crystalPrefab, newtransform.position, Quaternion.identity); //实例化水晶
        newCrystal.GetComponent<CrystalSkillControllor>().SetupCrystal(crystalDuration); //设置水晶存在时间
    }
}
