using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RicoShot.Play.Animations
{
    public class ThrowBehaviour : MonoBehaviour
    {
        [SerializeField] private AnimationClip clip;
        private int _animIDThrow = Animator.StringToHash("Throw");

        private PlayableGraph _graph;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _graph = _animator.playableGraph;

            var clipPlayable = AnimationClipPlayable.Create(_graph, clip);
            var output = _graph.GetOutput(0);
            var layer = AnimationMixerPlayable.Create(_graph, 2);
            layer.ConnectInput(1, clipPlayable, 0);
            layer.SetInputWeight(1, 1);
            output.SetSourcePlayable(layer);
        }

        private void Update()
        {
            Debug.Log("ThrowBehaviour Update");
            if (_animator.GetBool(_animIDThrow))
            {
                _animator.SetBool(_animIDThrow, false);
                _graph.Play();
                Debug.Log("Throw from ThrowBehaviour");
            }
        }
    }
}