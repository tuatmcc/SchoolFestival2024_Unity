using DG.Tweening;
using UnityEngine;

namespace RicoShot.Play
{
    public class TakinaAroundRotate : MonoBehaviour
    {
        private void Start()
        {
            transform.DORotate(new Vector3(0f, 360f, 0f), 5f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}