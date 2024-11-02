using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DissableDashEffect : MonoBehaviour
{
    [SerializeField] private float Length = 1.7f;
    
    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        UniTask.Create(async () =>
        {
            await UniTask.Delay((int)(Length * 1000));
            spriteRenderer.enabled = false;
        }).Forget();
    }
}
