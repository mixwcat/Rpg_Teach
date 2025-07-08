using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public bool canGrow;
    public bool canShrink;
    private float blackHoleDuration = 3f;

    private bool cloneAttackReleased;
    public int amountOfAttacks = 4;
    public float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    public List<Transform> targets;
    private List<GameObject> CreatedHotKeys = new List<GameObject>();

    // Start is called before the first frame update
    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        blackHoleDuration -= Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.F) || blackHoleDuration <= 0)
        {
            cloneAttackReleased = true;
            DestroyHotKeys();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }


    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            if (targets.Count != 0)
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex]);
            }

            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                cloneAttackReleased = false;
                canShrink = true;
                amountOfAttacks = 4;
                PlayerManager.instance.player.ExitBlackHole();
            }
        }
    }

    private void DestroyHotKeys()
    {
        if (CreatedHotKeys.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < CreatedHotKeys.Count; i++)
        {
            Destroy(CreatedHotKeys[i]);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !targets.Contains(other.transform))
        {
            other.GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", 3f);
            //targets.Add(other.transform);
            CreatHotKey(other);
        }
    }

    private void CreatHotKey(Collider2D other)
    {
        if (keyCodeList.Count == 0)
        {
            return;
        }

        if (cloneAttackReleased)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, .5f, 0), Quaternion.identity);

        CreatedHotKeys.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        newHotKey.GetComponent<BlackHoleHotKey>().SetupHotKey(choosenKey, other.transform, this);
    }
}