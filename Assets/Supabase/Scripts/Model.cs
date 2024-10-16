using System;
using System.Collections;
using System.Collections.Generic;
using Postgrest.Attributes;
using Postgrest.Models;
using UnityEngine;

namespace RicoShot.Supabase
{
    [Table("profiles")]
    public class ProfileContainer : BaseModel
    {
        [PrimaryKey("id", false)] public string id { get; set; }
        [Column("user_id")] public string user_id { get; set; }
        [Column("display_name")] public string display_name { get; set; }
    }

    [Table("matching_results")]
    public class MatchingResultContainer : BaseModel
    {
        [PrimaryKey("id", false)] public string id { get; set; }
        [Column("start_at")] public DateTime start_at { get; set; }
        [Column("end_at")] public DateTime end_at { get; set; }
    }

    [Table("teams")]
    public class TeamContainer : BaseModel
    {
        [PrimaryKey("id", false)] public string id { get; set; }
        [Column("matching_result_id")] public string matching_result_id { get; set; }
        [Column("is_win")] public bool is_win { get; set; }
    }

    [Table("players")]
    public class PlayerContainer : BaseModel
    {
        [PrimaryKey("id", false)] public string id { get; set; }
        [Column("user_id")] public string user_id { get; set; }
        [Column("team_id")] public int team_id { get; set; }
        [Column("matching_result_id")] public string matching_result_id { get; set; }
        [Column("score")] public int score { get; set; }
    }

    [Table("profiles_with_stats")]
    public class ProfilesWithStatsContainer : BaseModel
    {
        [Column("high_score")] public int high_score { get; set; }
        [Column("play_count")] public int play_count { get; set; }
        [Column("rank")] public int rank { get; set; }
    }
}