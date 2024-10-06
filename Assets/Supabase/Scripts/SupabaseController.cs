using System.Collections;
using System.Collections.Generic;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;
using UnityEngine;

public class SupabaseController : MonoBehaviour
{
    [SerializeField] private string supabaseURL = "";
    [SerializeField] private string supabaseKey = "";
    [SerializeField] private Client _client = null;

    private async void Awake()
    {
        _client = new Client(supabaseURL, supabaseKey);
        await _client.InitializeAsync();
    }
}

[Table("score")]
public class ScoreModel : BaseModel
{
    [PrimaryKey("id", false)] public string id { get; set; }
    [Column("score")] public int score { get; set; }
    [Column("username")] public string username { get; set; }
}