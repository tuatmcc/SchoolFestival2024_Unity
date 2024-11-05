using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DashBackControll : MonoBehaviour
{
    [SerializeField] private float StopTime = 1f;

    private Material _material;
    private void Start()
    {
        var color1Id = Shader.PropertyToID("_Color1");
        var color2Id = Shader.PropertyToID("_Color2");
        var materials = new List<Material>();
        GetComponent<MeshRenderer>().GetMaterials(materials);
        _material = materials[0];
        var color2 = _material.GetColor(color2Id);
        UniTask.Create(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(StopTime));
            _material.SetColor(color1Id, color2);
        }).Forget();
    }
}
