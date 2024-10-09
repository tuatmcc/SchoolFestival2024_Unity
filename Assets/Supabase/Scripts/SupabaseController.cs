using System.Collections;
using System.Collections.Generic;
using Supabase;
using UnityEngine;


namespace Supabase
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
            var model = ConstructModel("supabase", 200, "supabase");
            RegisterResult(model);
        }

        public ScoreContainer ConstructModel(string userID, int score, string username)
        {
            return new ScoreContainer
            {
                id = userID,
                score = score,
                username = username
            };
        }

        public async void RegisterResult(ScoreContainer result)
        {
            await _client?.From<ScoreContainer>().Upsert(result)!;
        }

        public async void DeleteResult(ScoreContainer result)
        {
            await _client.From<ScoreContainer>().Delete(result)!;
        }
    }
}