using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HPviewer : MonoBehaviour
{
    [SerializeField]
    private Enemy_StatePattern enemy_StatePattern;
    [SerializeField]
    private Slider slider;

    private void Update()
    {
        slider.value = enemy_StatePattern.CurrentHP / enemy_StatePattern.MAXHP;     
    }
}
