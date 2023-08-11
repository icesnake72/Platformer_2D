using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private AudioSource collectSound;

    private int countCherry;


    private void Awake()
    {
        // 이전 씬에서 저장한 데이터를 가져옴
        int cherryCount = PlayerPrefs.GetInt("Cherry");

        countCherry = cherryCount;
        SetCherryCount();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Cherry") )
        {
            collectSound.Play();

            Destroy(collision.gameObject);
            SetCherryCount();
        }
    }

    private void SetCherryCount()
    {
        string chr = $"Cherry : {++countCherry}";
        text.SetText(chr);
    }

    public int GetCherryCount()
    {
        return countCherry;
    }
}
