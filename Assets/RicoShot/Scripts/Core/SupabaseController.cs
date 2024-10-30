using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using RicoShot.Core;
using Supabase;
using Unity.Collections;
using UnityEngine;


namespace RicoShot.Core
{
    public class SupabaseController : ISupabaseController
    {
        [SerializeField] private string supabaseURL;
        [SerializeField] private string supabaseKey;
        private Client _supabaseClient;
        
        private async UniTask<ProfileContainer> GetProfile(string userID)
        {
            var response = await _supabaseClient.From<ProfileContainer>().Get();
            return response.Models.Find(x => x.id == userID);
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

        public async UniTask ConnectSupabase(string url, string key)
        {
            var options = new SupabaseOptions()
            {
                AutoConnectRealtime = true
            }; 
            _supabaseClient = new Client(url, key, options);
            await _supabaseClient.InitializeAsync();
        }

        public void Initialize()
        {
            ConnectSupabase(supabaseURL, supabaseKey).Forget();
        }

        public async UniTask<CharacterParams> FetchPlayerProfile(string userID)
        {
            ProfileContainer container = await GetProfile(userID);
            CharacterPreset preset = JsonConvert.DeserializeObject<CharacterPreset>(container.character_setting);
            CharacterParams param = new CharacterParams(preset.chibiIndex, preset.colorCode, preset.costumeVariant,
                preset.accessory);
            return param;
        }

        public void UpsertTeam(Team team, FixedString32Bytes teamID, FixedString32Bytes matchingID, bool isWin)
        {
            TeamContainer container = new TeamContainer();
            container.id = teamID.ToString();
            container.is_win = isWin;
            container.matching_result_id = matchingID.ToString();
            _supabaseClient.From<TeamContainer>().Upsert(container);
        }

        public void UpsertMatching(FixedString32Bytes matchingID, DateTime startTime, DateTime endTime)
        {
            MatchingResultContainer container = new MatchingResultContainer();
            container.id = matchingID.ToString();
            container.start_at = startTime;
            container.end_at = endTime;
            _supabaseClient.From<MatchingResultContainer>().Upsert(container);
        }

        public void UpsertPlayerResult(FixedString32Bytes userID, int score, FixedString32Bytes teamID,
            FixedString32Bytes matchingID)
        {
            PlayerContainer container = new PlayerContainer();
            container.id = Guid.NewGuid().ToString();
            container.user_id = userID.ToString();
            container.matching_result_id = matchingID.ToString();
            container.team_id = teamID.ToString();
            container.score = score;
            _supabaseClient.From<PlayerContainer>().Upsert(container);
        }
    }
}