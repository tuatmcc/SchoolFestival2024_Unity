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

    /*
     * ユーザーがWeb上で作ったキャラクターのデータをJSONからパースするためのコンテナクラス
     */
    public class CharacterPreset
    {
        public int chibiIndex { get; set; }
        public FixedString32Bytes colorCode { get; set; }
        public int costumeVariant { get; set; }
        public int accessory { get; set; }
    }

    /*
     * プレイヤーのデータを格納しておくためのクラス
     * 主な用途は以下2つ
     *   - JSONデータのパース
     *   - 結果をSupabase上のIDをリンクさせて保持
     */
    /*
    public class PlayerProfile
    {
        private string _userID;
        private string _userName;
        private CharacterPreset _characterPreset;
        private int _score = 0;

        public PlayerProfile(string userID, string userName)
        {
            _userID = userID;
            _userName = userName;
        }
        
        public string UserID { get { return _userID; } }
        public string UserName { get { return _userName; } }
        public CharacterPreset CharacterPreset { get { return _characterPreset; } }
        public int Score { get { return _score; } }
        
        public void SetScore(int score) { _score = score; }

        public void ParsePreset(string presetData)
        {
            CharacterPreset preset = JsonConvert.DeserializeObject<CharacterPreset>(presetData);
        }
    }
    
    public class Team
    {
        private Guid _teamID;
        private List<PlayerProfile> _members;
        private bool _isWin = false;

        public Team()
        {
            _teamID = Guid.NewGuid();
        }
        
        public Guid TeamID { get { return _teamID; } }
        public List<PlayerProfile> Members { get { return _members; } }
        public bool IsWin { get { return _isWin; } }

        public void AddMember(PlayerProfile member) { _members.Add(member); }
        public void RemoveMember(PlayerProfile member) { _members.Remove(member); }
        public void SetIsWon(bool isWin) { _isWin = isWin; }
    }
    
    public class MatchingResult
    {
        private Guid _matchingID;
        private DateTime _startTime;
        private DateTime _endTime;
        private Team _teamA;
        private Team _teamB;

        public MatchingResult()
        {
            _matchingID = Guid.NewGuid();
        }
        
        public Guid MatchingID { get { return _matchingID; } }
        public DateTime StartTime { get { return _startTime; } }
        public DateTime EndTime { get { return _endTime; } }
        public Team TeamA { get { return _teamA; } }
        public Team TeamB { get { return _teamB; } }
        
        public void SetStartTime(DateTime startTime) { _startTime = startTime; }
        public void SetEndTime(DateTime endTime) { _endTime = endTime; }
        public void SetTeamA(Team teamA) { _teamA = teamA; }
        public void SetTeamB(Team teamB) { _teamB = teamB; }
    }
    */
    
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