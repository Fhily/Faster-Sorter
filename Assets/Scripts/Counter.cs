using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CounterText;

    public GameManager gameManager;

    private int count = 0;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkColor("Box Red", "SRed", other))
        {
            count++;
            CounterText.text = "Red: " + count;
        }
        else if (checkColor("Box Green", "SGreen", other))
        {
            count++;
            CounterText.text = "Green: " + count;
        }
        else if (checkColor("Box Blue", "SBlue", other))
        {
            count++;
            CounterText.text = "Blue: " + count;
        }
        else
        {
            gameManager.GameOver();
        }
    }

    private bool checkColor(string boxColor, string sphereColor, Collider other)
    {
        if (gameObject.name.CompareTo(boxColor) == 0 && other.gameObject.CompareTag(sphereColor))
            return true;
        else 
            return false;
    }
}
