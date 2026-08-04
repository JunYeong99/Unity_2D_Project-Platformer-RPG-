using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public Character character;
    Animator anim;
    SpriteRenderer[] spriteRenderer;
    public SelectCharacter[] chars;
    public GameObject[] spotLight;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        if (DataManager.instance.currentCharacter == character)
        {
            OnSelect();
        }
        else
        {
            OnDeSelect();
        }
    }

    private void Update()
    {
        if(DataManager.instance.currentCharacter == Character.Warrior)
        {
            spotLight[0].SetActive(true);
            spotLight[1].SetActive(false);
            spotLight[2].SetActive(false);
        }
        else if (DataManager.instance.currentCharacter == Character.Archer)
        {
            spotLight[0].SetActive(false);
            spotLight[1].SetActive(true);
            spotLight[2].SetActive(false);
        }
        else
        {
            spotLight[0].SetActive(false);
            spotLight[1].SetActive(false);
            spotLight[2].SetActive(true);
        }
    }

    private void OnMouseUpAsButton()
    {
        DataManager.instance.currentCharacter = character;
        OnSelect();

        for(int i = 0; i < chars.Length; i++)
        {
            if(chars[i] != this)
            {
                chars[i].OnDeSelect();
            }
        }
    }
    void OnSelect()
    {
        anim.SetBool("isWalk", true);

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 1);          
        }
    }

    void OnDeSelect()
    {
        anim.SetBool("isWalk", false);

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(spriteRenderer[i].color.r, spriteRenderer[i].color.g, spriteRenderer[i].color.b, 0.5f);
        }
    }  
}
