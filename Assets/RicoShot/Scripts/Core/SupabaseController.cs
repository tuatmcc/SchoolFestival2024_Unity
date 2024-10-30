using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using Supabase;
using UnityEngine;


namespace RicoShot.SupabaseController
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

        private TeamContainer ConstructuTeam(Team team, Guid matchingID)
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

        public async UniTask<PlayerProfile> FetchPlayerProfile(string userID)
        {
            ProfileContainer container  = await GetProfile(userID);
            PlayerProfile profile = new PlayerProfile(container.user_id, container.display_name);
            // profile.ParsePreset(container.character_preset);
            return profile;
        }

        public void UpsertResult(MatchingResult result)
        {
            MatchingResultContainer matchingResultContainer = new MatchingResultContainer();
            matchingResultContainer.id = result.MatchingID.ToString();
            matchingResultContainer.start_at = result.StartTime;
            matchingResultContainer.end_at = result.EndTime;
            UpsertMatching(matchingResultContainer);
            
            TeamContainer teamA = ConstructuTeam(result.TeamA, result.MatchingID);
            UpsertTeam(teamA);
            TeamContainer teamB = ConstructuTeam(result.TeamB, result.MatchingID);
            UpsertTeam(teamB);

            foreach (var profile in result.TeamA.Members)
            {
                PlayerContainer player = ConstructPlayer(profile, result.TeamA.TeamID, result.MatchingID);
                UpsertPlayer(player);
            }
            
            foreach (var profile in result.TeamB.Members)
            {
                PlayerContainer player = ConstructPlayer(profile, result.TeamB.TeamID, result.MatchingID);
                UpsertPlayer(player);
            }
        }

    }
}