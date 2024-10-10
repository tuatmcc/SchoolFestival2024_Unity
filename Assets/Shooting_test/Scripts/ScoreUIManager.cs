using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreUIManager : MonoBehaviour
{
    TextMeshProUGUI Score_UI_text;
    [Inject] private IScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        Score_UI_text = this.GetComponent < TextMeshProUGUI >();
    }

    // Update is called once per frame
    void Update()
    {
        Score_UI_text.text = scoreManager.GetScore().ToString();
    }
}
