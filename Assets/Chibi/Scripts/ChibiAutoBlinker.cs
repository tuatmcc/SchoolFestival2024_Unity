using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Chibi
{
    [RequireComponent(typeof(Animator))]
    public class ChibiAutoBlinker : MonoBehaviour
    {
        [SerializeField] private AnimationClip blinkAnimation;

        private Animator animator;

        private PlayableGraph graph;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            graph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
            var playable = AnimationClipPlayable.Create(graph, blinkAnimation);
            playableOutput.SetSourcePlayable(playable);
        }

        private void Start()
        {
            graph.Play();
        }
    }
}