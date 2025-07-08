using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterState
{
    private Player player;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        // 然后播放受击动画
    }
}
