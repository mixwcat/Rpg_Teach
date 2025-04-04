using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("ÁôÏÂ¿ËÂ¡")]
    [SerializeField] protected GameObject clonePrefab;

    public void CreateClone(Transform newtransform)
    {
        GameObject newClone = Instantiate(clonePrefab, player.transform.position, Quaternion.identity);
        newClone.GetComponent<CloneDismiss>().FaceClosetTarget();
    }
}