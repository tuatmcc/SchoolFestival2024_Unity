using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Chibi
{
    [RequireComponent(typeof(Animator))]
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField] private AnimationClip animationClip;
        private Animator _animator;
        private PlayableGraph _graph;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _graph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", _animator);
            var playable = AnimationClipPlayable.Create(_graph, animationClip);
            playableOutput.SetSourcePlayable(playable);
        }

        private void Start()
        {
            _graph.Play();
        }
    }
}