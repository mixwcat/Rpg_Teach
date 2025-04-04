using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SowrdSkill : Skill
{
    [Header("Sowrd Skill")]
    [SerializeField] private GameObject swordPrefab;

    [SerializeField] protected Vector2 launchForce;
    [SerializeField] private float swordGravity;
    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private GameObject dotPrefab;

    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        newSword.GetComponent<SwordSkillControllor>().SetupSword(finalDir, swordGravity, player.facingDir);
        player.AssignSword(newSword);
        DotsActive(false);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerPosition);

        return direction;
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position +
            new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y) * t
        + 0.5f * t * t * Physics2D.gravity * swordGravity;
        return position;
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
        }
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(isActive);
        }
    }
}