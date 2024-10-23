using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Postgrest.Models;
using Supabase;
using UnityEngine;


namespace RicoShot.Supabase
{
    public class SupabaseController : MonoBehaviour
    {
        [SerializeField] private string supabaseURL;
        [SerializeField] private string supabaseKey;
        private Client _supabaseClient;

        private async UniTask Connect()
        {
            await _supabaseClient.InitializeAsync();
            Debug.Log($"Connected to {supabaseURL}");
        }

        public void Start()
        {
            var options = new SupabaseOptions()
            {
                AutoConnectRealtime = true
            }; 
            _supabaseClient = new Client(supabaseURL, supabaseKey, options);
            Connect().Forget();
        }

        public void OnClick()
        {
            string userID = "0f5d546a-dd80-5406-a005-a4f3061b9fb4";
            string matchID = "5cbb8028-b7e1-5ec0-a482-cbaff11709b8";
            string teamID = "16736f78-c8d8-55ff-90d2-0300ffc54b4f";
            string playerID = "0eb05d63-02be-518c-acf4-e73c0c5cc1a3";
            Debug.Log(_supabaseClient is null);
            Debug.Log(userID);
            Debug.Log(teamID);
            Debug.Log(playerID);
            Debug.Log(matchID);
            var profile = GetProfile(userID).Result;
            // var match = GetMatching(matchID).Result;
            // var team = GetTeam(teamID).Result;
            // var player = GetPlayer(playerID).Result;
        }

        public async Task<ProfileContainer> GetProfile(string userID)
        {
            Debug.Log(_supabaseClient is null);
            var response = await _supabaseClient.From<ProfileContainer>().Get();
            return response.Models.Find(x => x.id == userID);
        }

        public async void UpsertProfile(ProfileContainer profile)
        {
            await _supabaseClient.From<ProfileContainer>().Upsert(profile);
        }

        public async Task<MatchingResultContainer> GetMatching(string userID)
        {
            var response = await _supabaseClient.From<MatchingResultContainer>().Get();
            return response.Models.Find(x => x.id == userID);
        }

        public async void UpsertMatching(MatchingResultContainer result)
        {
            await _supabaseClient.From<MatchingResultContainer>().Upsert(result);
        }

        public async Task<TeamContainer> GetTeam(string teamID)
        {
            var response = await _supabaseClient.From<TeamContainer>().Get();
            return response.Models.Find(x => x.id == teamID);
        }

        public async void UpsertTeam(TeamContainer team)
        {
            await _supabaseClient.From<TeamContainer>().Upsert(team);
        }

        public async Task<PlayerContainer> GetPlayer(string userID)
        {
            var response = await _supabaseClient.From<PlayerContainer>().Get();
            return response.Models.Find(x => x.id == userID);
        }

        public async void UpsertPlayer(PlayerContainer player)
        {
            await _supabaseClient.From<PlayerContainer>().Upsert(player);
        }

        public async Task<List<ProfilesWithStatsContainer>> GetProfilesWithStats()
        {
            var response = await _supabaseClient.From<ProfilesWithStatsContainer>().Get();
            return response.Models.ToList();
        }
    }
}