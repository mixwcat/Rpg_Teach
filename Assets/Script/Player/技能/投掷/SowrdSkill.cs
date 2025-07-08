using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SwordType
{
    Regular,
    Bouncing,
    Pierce,
    Spin
}

public class SowrdSkill : Skill
{
    public SwordType swordType = SwordType.Regular;
    [Header("Bounce Info")]
    [SerializeField] private int amountOfBounce;
    private float bounceGravity = 1f;


    [Header("Pierce Info")]
    [SerializeField] private int _pierceAmount;


    [Header("Sowrd Skill")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] protected Vector2 launchForce;
    [SerializeField] private float swordGravity;
    private Vector2 finalDir;

    [Header("Spin Info")]
    [SerializeField] private float _hitCooldown = .35f;
    [SerializeField] private float _maxTravelDistance = 3;
    [SerializeField] private float _spinDuration = 2f;
    [SerializeField] private float spinGravity = 1f;


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
        SetupGravity();
    }

    protected override void Update()
    {
        //速度
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


    #region 初始化剑
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillControllor newSwordScript = newSword.GetComponent<SwordSkillControllor>();
        newSwordScript.SetupSword(finalDir, swordGravity, player.facingDir);
        if (swordType == SwordType.Bouncing)
        {
            swordGravity = bounceGravity;
            newSwordScript.SetupBounce(true, amountOfBounce);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = 0f;
            newSwordScript.SetupPierce(_pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
            newSwordScript.SetupSpin(true, _maxTravelDistance, _spinDuration, _hitCooldown);
        }
        player.AssignSword(newSword);
        DotsActive(false);
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bouncing)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = 0f;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
        else if (swordType == SwordType.Regular)
        {
            swordGravity = 1f;
        }
    }

    //获得方向
    #endregion


    #region 抛物线点
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerPosition);

        return direction;
    }
    //传入Space_between_DOt,计算抛物线
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
    #endregion
}