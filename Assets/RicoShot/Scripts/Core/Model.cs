using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;
using Unity.Collections;
using UnityEngine;

namespace RicoShot.Core
{
    [Table("profiles")]
    public class ProfileContainer : BaseModel
    {
        [PrimaryKey("id", false)] public string ID { get; set; }
        [Column("user_id")] public string UserID { get; set; }
        [Column("display_name")] public string DisplayName { get; set; }
        /*
         * キャラクターの設定情報を格納したJSONを格納するためのコンテナ
         */
        [Column("character_setting")] public CharacterParams CharacterSetting { get; set; }
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
        [Column("team_id")] public string team_id { get; set; }
        [Column("matching_result_id")] public string matching_result_id { get; set; }
        [Column("score")] public int score { get; set; }
    }
}