using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Score_UI_text;
    [Inject] private IScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Score_UI_text.text = scoreManager.GetScore().ToString();
    }
}
