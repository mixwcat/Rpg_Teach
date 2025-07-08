using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKey : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHoleSkillController blackHole;
    

    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, BlackHoleSkillController _blackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
        myEnemy = _myEnemy;
        blackHole = _blackHole;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.targets.Add(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
  