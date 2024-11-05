using System.Collections.Generic;
using System.Linq;
using R3;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Matching.Interface;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;


namespace RicoShot.Matching.UI
{
    public class MatchingUI : MonoBehaviour
    {
        [Inject] private IMatchingSceneManager _matchingSceneManager;
        [Inject] private INetworkController _networkController;
        [Inject] private ILocalPlayerManager _localPlayerManager;

        [SerializeField] private Image backgroundOverlayAlpha;
        [SerializeField] private Image backgroundOverlayBravo;

        [SerializeField] private MatchingTeamOption alphaTeamOption;
        [SerializeField] private MatchingTeamOption bravoTeamOption;

        [SerializeField] private MatchingPlayerPreviewFrame[] alphaPlayers;
        [SerializeField] private MatchingPlayerPreviewFrame[] bravoPlayers;

        private void Start()
        {
            Observable.FromEvent
                (h => _networkController.ClientDataList.OnDataChanged += h,
                    h => _networkController.ClientDataList.OnDataChanged -= h)
                .Subscribe(_ => UpdatePlayersUI()).AddTo(this);
        }

        private void UpdatePlayersUI()
        {
            var alphas = _networkController.ClientDataList.Where(x => x.Team == Team.Alpha).ToList();
            var bravos = _networkController.ClientDataList.Where(x => x.Team == Team.Bravo).ToList();

            UpdatePlayerFrames(alphaPlayers, alphas);
            UpdatePlayerFrames(bravoPlayers, bravos);
            UpdateTeamOptions();
        }

        private void UpdatePlayerFrames(MatchingPlayerPreviewFrame[] teamPlayerFrames, List<ClientData> players)
        {
            for (var i = 0; i < teamPlayerFrames.Length; i++)
                if (i < players.Count)
                {
                    teamPlayerFrames[i].SetReady(!players[i].IsNpc);
                    teamPlayerFrames[i].SetPlayer(players[i].Name.ToString());
                }
                else
                {
                    teamPlayerFrames[i].SetReady(false);
                }
        }

        private void UpdateTeamOptions()
        {
            foreach (var clientData in _networkController.ClientDataList)
                if (clientData.UUID.ToString() == _localPlayerManager.LocalPlayerUUID)
                {
                    backgroundOverlayAlpha.gameObject.SetActive(clientData.Team != Team.Alpha);
                    backgroundOverlayBravo.gameObject.SetActive(clientData.Team != Team.Bravo);
                    alphaTeamOption.SetCursorActive(clientData.Team == Team.Alpha);
                    bravoTeamOption.SetCursorActive(clientData.Team == Team.Bravo);
                }
        }
    }
}