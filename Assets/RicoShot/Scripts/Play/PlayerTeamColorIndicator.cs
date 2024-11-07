using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using UnityEngine;
using Zenject;

namespace RicoShot.Play
{
    public class PlayerTeamColorIndicator : MonoBehaviour
    {
        [Inject] private readonly IPlaySceneManager _playSceneManager;
        [SerializeField] private GameObject alpha;
        [SerializeField] private GameObject bravo;

        public void SetTeamColor(Team team)
        {
            alpha.SetActive(team == Team.Alpha);
            bravo.SetActive(team == Team.Bravo);
        }
    }
}