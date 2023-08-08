using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private int countCherry;


    private void Awake()
    {
        countCherry = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Cherry") )
        {
            Destroy(collision.gameObject);
            string chr = $"Cherry : {++countCherry}";
            text.SetText(chr);
        }
    }
}
