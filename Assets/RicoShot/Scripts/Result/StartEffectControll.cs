using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

public class StartEffectControll : MonoBehaviour
{
    [SerializeField] private float Length = 1.7f;
    
    private void Start()
    {
        var vfx = GetComponent<VisualEffect>();
        UniTask.Create(async () =>
        {
            await UniTask.Delay((int)(Length * 1000));
            vfx.enabled = true;
        }).Forget();
    }
}
