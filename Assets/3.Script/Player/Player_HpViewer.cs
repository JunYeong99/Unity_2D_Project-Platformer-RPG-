using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HpViewer : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Slider slider;

    private void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        slider.value = player.CurrentHP / player.MAXHP;
    }
}
