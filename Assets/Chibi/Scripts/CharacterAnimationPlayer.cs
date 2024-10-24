using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Chibi
{
    [RequireComponent(typeof(CharacterSettingsController))]
    public class CharacterAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private AnimationClip animationClip;
        private Animator _animator;
        private PlayableGraph _graph;
        private CharacterSettingsController _settingsController;

        private void Awake()
        {
            _settingsController = GetComponent<CharacterSettingsController>();
            _animator = GetComponent<Animator>();
            // _animator.applyRootMotion = false;

            _graph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", _animator);
            var playable = AnimationClipPlayable.Create(_graph, animationClip);
            playableOutput.SetSourcePlayable(playable);
        }

        private void Start()
        {
            _animator.avatar = _settingsController.animator.avatar;
            _graph.Play();
        }
    }
}