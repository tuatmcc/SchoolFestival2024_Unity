using System.Collections;
using System.Collections.Generic;
using Postgrest.Attributes;
using Postgrest.Models;
using UnityEngine;

namespace Supabase
{
    [Table("score")]
    public class ScoreContainer : BaseModel
    {
        [PrimaryKey("id", false)] public string id { get; set; }
        [Column("score")] public int score { get; set; }
        [Column("username")] public string username { get; set; }
    }
}