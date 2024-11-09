using System.Collections.Generic;
using System.Linq;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Result.UI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        // [SerializeField] private TMP_Text[] rankingPlayers;

        [Inject] private readonly IGameStateManager _gameStateManager;
        [Inject] private readonly INetworkController _networkController;
        [Inject] private readonly ILocalPlayerManager _localPlayerManager;

        [SerializeField] private RankingPlayer[] rankingPlayers;

        private void Start()
        {
            List<ScoreData> scores = new();
            foreach (var score in _networkController.ScoreManager.ScoreList) scores.Add(score);

            List<ClientData> clients = new();
            foreach (var client in _networkController.ClientDataList) clients.Add(client);

            scores.Sort((a, b) => -a.Score + b.Score);
            for (var i = 0; i < scores.Count; ++i)
            {
                var rank = scores.Count(x => x.Score > scores[i].Score) + 1;
                if (scores[i].IsNpc)
                {
                    rankingPlayers[i].SetPlayerData("NPC", scores[i].Team, scores[i].Score, false, rank);
                }
                else
                {
                    var index = clients.FindIndex(x => x.UUID.ToString() == scores[i].UUID.ToString());
                    rankingPlayers[i].SetPlayerData(
                        clients[index].Name.ToString(),
                        clients[index].Team,
                        scores[i].Score,
                        _gameStateManager.NetworkMode == NetworkMode.Client && clients[index].UUID.ToString() ==
                        _localPlayerManager.LocalPlayerUUID,
                        rank);
                }
            }
        }
    }
}