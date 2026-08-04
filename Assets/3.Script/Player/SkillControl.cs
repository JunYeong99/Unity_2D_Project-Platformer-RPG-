using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillControl : MonoBehaviour
{
    public GameObject[] Skills;
    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public Text[] hideSkillTimeTexts;
    public Image[] hideSkillImage;
    public bool[] isHideSkills = { false , false , false };
    private float[] skillTimes = { 24, 30, 30 };
    private float[] getSkillTimes = { 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<Text>();
        }

        if(GameObject.Find("Warrior(Clone)"))
        {
            Skills[0].SetActive(true);
        }
        else if(GameObject.Find("Archer(Clone)"))
        {
            Skills[1].SetActive(true);
        }
        else
        { 
            Skills[2].SetActive(true);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && getSkillTimes[0] == 0)
        {
            if (GameObject.Find("Warrior(Clone)"))
                HideSkillSetting(0);
        }

        if (Input.GetKeyDown(KeyCode.Q) && getSkillTimes[1] == 0)
        {
            if (GameObject.Find("Archer(Clone)"))
                HideSkillSetting(1);
        }

        if (Input.GetKeyDown(KeyCode.Q) && getSkillTimes[2] == 0)
        {
            if (GameObject.Find("Magician(Clone)"))
                HideSkillSetting(2);
        }

        HideSkillChk();
    }

    public void HideSkillSetting(int skillNum)
    {
        hideSkillButtons[skillNum].SetActive(true);
        getSkillTimes[skillNum] = skillTimes[skillNum];
        //isHideSkills[skillNum] = true;
    }

    private void HideSkillChk()
    {
        if(isHideSkills[0] && GameObject.Find("Warrior(Clone)"))
        {
            StartCoroutine(SkillTimeChk(0));
        }

        if (isHideSkills[1] && GameObject.Find("Archer(Clone)"))
        {
            StartCoroutine(SkillTimeChk(1));
        }

        if (isHideSkills[2] && GameObject.Find("Magician(Clone)"))
        {
            StartCoroutine(SkillTimeChk(2));
        }
    }

    IEnumerator SkillTimeChk(int skillNum)
    {
        yield return null;

        if(getSkillTimes[skillNum] > 0)
        {
            getSkillTimes[skillNum] -= Time.deltaTime;

            if(getSkillTimes[skillNum] < 0)
            {
                getSkillTimes[skillNum] = 0;
                isHideSkills[skillNum] = false;
                hideSkillButtons[skillNum].SetActive(false);
            }

            hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00");

            float time = getSkillTimes[skillNum] / skillTimes[skillNum];
            hideSkillImage[skillNum].fillAmount = time;
        }
    }
}
