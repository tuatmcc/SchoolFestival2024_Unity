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
        private Client _client = null;

        public async void Awake()
        {
            Debug.Log(supabaseURL);
            Debug.Log(supabaseKey);
            _client = new Client(supabaseURL, supabaseKey);
            await _client.InitializeAsync();
        }

        public void Start()
        {
            ;
        }

        public async UniTask<ProfileContainer> GetProfile(string userID)
        {
            var response = await _client?.From<ProfileContainer>().Get()!;
            return response.Models.Find(x => x.id == userID);
        }

        public async void UpsertProfile(ProfileContainer profile)
        {
            await _client.From<ProfileContainer>().Upsert(profile);
        }

        public async UniTask<MatchingResultContainer> GetMatching(string userID)
        {
            var response = await _client?.From<MatchingResultContainer>().Get()!;
            return response.Models.Find(x => x.id == userID);
        }

        public async void UpsertMatching(MatchingResultContainer result)
        {
            await _client.From<MatchingResultContainer>().Upsert(result);
        }

        public async UniTask<TeamContainer> GetTeam(string teamID)
        {
            var response = await _client?.From<TeamContainer>().Get()!;
            return response.Models.Find(x => x.id == teamID);
        }

        public async void UpsertTeam(TeamContainer team)
        {
            await _client.From<TeamContainer>().Upsert(team);
        }

        public async UniTask<PlayerContainer> GetPlayer(string userID)
        {
            var response = await _client?.From<PlayerContainer>().Get()!;
            return response.Models.Find(x => x.id == userID);
        }

        public async void UpsertPlayer(PlayerContainer player)
        {
            await _client.From<PlayerContainer>().Upsert(player);
        }

        public async UniTask<List<ProfilesWithStatsContainer>> GetProfilesWithStats()
        {
            var response = await _client?.From<ProfilesWithStatsContainer>().Get()!;
            return response.Models.ToList();
        }
    }
}