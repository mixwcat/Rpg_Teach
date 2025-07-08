using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [Header("血条")]
    private Slider slider;
    private Entity entity;
    private CharacterState myStats;
    private RectTransform mytransform;


    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        entity = GetComponentInParent<Entity>();
        myStats = GetComponentInParent<CharacterState>();
        mytransform = GetComponent<RectTransform>();

        myStats.onHealthChange += UpdateHealthUI;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHp;
    }

    private void OnDisable()
    {
        myStats.onHealthChange -= UpdateHealthUI;
    }
}
