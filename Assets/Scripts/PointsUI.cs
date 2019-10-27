using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PointsUI : MonoBehaviour
{
    private Text textComp;
    private GameManager gameManager;
    private void Start()
    {

        gameManager = GameManager.Instance;
        textComp = GetComponent<Text>();
    }
    private void OnGUI()
    {
        textComp.text = gameManager.Points.ToString();
    }
}
