using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DashBackControll : MonoBehaviour
{
    [SerializeField] private float StartSpeed;
    [SerializeField] private float EndSpeed;
    [SerializeField] private float Alpha;
    [SerializeField] private Ease EaseType;
    [SerializeField] private float Length;
    [SerializeField] private Color Color1 = new Color(0.8f, 0.3f, 0.3f);
    [SerializeField] private Color Color2 = new Color(0.8f, 0.5f, 0.5f);
    
    private Material _material;

    private int _speedId;
    private int _alphaId;
    private int _color1Id;
    private int _color2Id;

    private void Start()
    {
        _material = GetComponent<Image>().material;
        _speedId = Shader.PropertyToID("_Speed");
        _alphaId = Shader.PropertyToID("_Alpha");
        _color1Id = Shader.PropertyToID("_Color1");
        _color2Id = Shader.PropertyToID("_Color2");
        _material.SetFloat(_speedId, StartSpeed);
        _material.SetFloat(_alphaId, Alpha);
        _material.SetColor(_color1Id, Color1);
        _material.SetColor(_color2Id, Color2);
        _material.DOFloat(EndSpeed, _speedId, Length)
            .SetEase(EaseType)
            .OnComplete(() =>
            {
                _material.SetFloat(_speedId, 0f);
                _material.DOColor(Color2, _color1Id, 0.1f);
            });
        _material.DOFloat(0f, _alphaId, 1f).SetEase(Ease.Linear);
        // DOTween.To(() => Speed, x => _material.SetFloat(_speedId, Speed), 1f, 3f);
    }

    private void Update()
    {
        // _material.SetFloat(_speedId, Speed);
        // _material.SetFloat(_alphaId, Alpha);
        Debug.Log($"Speed: {_material.GetFloat(_speedId)} Alpha: {_material.GetFloat(_alphaId)}");
    }
}
