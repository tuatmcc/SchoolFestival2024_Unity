using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RicoShot.Core;
using RicoShot.Core.Interface;
using Supabase;
using Unity.Collections;
using UnityEngine;
using Zenject;

namespace RicoShot.Core
{
    public class SupabaseController : ISupabaseController
    {
        public bool Connected { get; private set; } = false;

        private Client _supabaseClient;
        
        [Inject] private readonly IGameStateManager _gameStateManager;

        // Supabaseに接続する関数
        public async UniTask Connect()
        {
            var url = _gameStateManager.GameConfig.SupabaseURL;
            var key = _gameStateManager.GameConfig.SupabaseSecretKey;
            var options = new SupabaseOptions()
            {
                AutoConnectRealtime = true
            }; 
            _supabaseClient = new Client(url, key, options);
            await _supabaseClient.InitializeAsync();
            Debug.Log("Supabase connected");
            Connected = true;
        }

        private async UniTask<ProfileContainer> GetProfile(string userID)
        {
            var response = await _supabaseClient.From<ProfileContainer>().Where(x => x.UserID == userID).Single();
            return response;
        }
        
        // UUIDから表示名とキャラクターのパラメーターを取得する関数
        public async UniTask<(string displayName, CharacterParams characterParams)> FetchPlayerProfile(string userID)
        {
            var container = await GetProfile(userID);
            // 存在しない場合
            if (container == null) { return (string.Empty, null); }
            // 存在する場合(Jsonのパースは勝手にやってくれる)
            var characterParams = new CharacterParams()
            {
                ChibiIndex = container.CharacterPreset.character,
                CostumeVariant = container.CharacterPreset.costume,
                HairColor = container.CharacterPreset.hair,
                Accessory = container.CharacterPreset.accessory,
            };
            return (container.DisplayName, characterParams);
        }

        public async UniTask UpsertTeam(Team team, string teamID, string matchingID, bool isWin)
        {
            TeamContainer container = new TeamContainer();
            container.id = teamID;
            container.is_win = isWin;
            container.matching_result_id = matchingID;
            await _supabaseClient.From<TeamContainer>().Upsert(container);
        }

        public async UniTask UpsertMatching(string matchingID, DateTime startTime, DateTime endTime)
        {
            MatchingResultContainer container = new MatchingResultContainer();
            container.id = matchingID;
            container.start_at = startTime;
            container.end_at = endTime;
            await _supabaseClient.From<MatchingResultContainer>().Upsert(container);
        }

        public async UniTask UpsertPlayerResult(string userID, int score, string teamID, string matchingID)
        {
            PlayerContainer container = new PlayerContainer();
            container.id = Guid.NewGuid().ToString();
            container.user_id = userID;
            container.matching_result_id = matchingID;
            container.team_id = teamID;
            container.score = score;
            await _supabaseClient.From<PlayerContainer>().Upsert(container);
        }

        /*
        private TeamContainer ConstructTeam(Team team, Guid matchingID)
        {
            TeamContainer teamContainer = new TeamContainer();
            teamContainer.id = team.TeamID.ToString();
            teamContainer.is_win = team.IsWin;
            teamContainer.matching_result_id = matchingID.ToString();
            return teamContainer;
        }

        private PlayerContainer ConstructPlayer(PlayerProfile profile, Guid teamID, Guid matchingID)
        {
            PlayerContainer playerContainer = new PlayerContainer();
            playerContainer.id = Guid.NewGuid().ToString();
            playerContainer.user_id = profile.UserID;
            playerContainer.team_id = teamID.ToString();
            playerContainer.matching_result_id = matchingID.ToString();
            playerContainer.score = profile.Score;
            return playerContainer;
        }

        private async void UpsertMatching(MatchingResultContainer result)
        {
            await _supabaseClient.From<MatchingResultContainer>().Upsert(result);
        }

        private async void UpsertTeam(TeamContainer team)
        {
            await _supabaseClient.From<TeamContainer>().Upsert(team);
        }


        private async void UpsertPlayer(PlayerContainer player)
        {
            await _supabaseClient.From<PlayerContainer>().Upsert(player);
        }
        */
    }
}