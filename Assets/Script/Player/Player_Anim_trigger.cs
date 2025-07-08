using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player_Anim_trigger : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player => GetComponentInParent<Player>();

    private void AttackFinish()
    {
        player.attackOver = true;
    }


    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }

    private void CatchSword()
    {
        player.stateMachine.ChangeState(player.idleState);
    }
}