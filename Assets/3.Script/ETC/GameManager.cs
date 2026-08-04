using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Player player;
    public bool isGameOver = false;
    public GameObject GameOverUI;

    private void Start()
    {
        
        GameOverUI = GameObject.Find("GameOver").GetComponent<GameObject>();
    }

    private void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (player.CurrentHP <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
