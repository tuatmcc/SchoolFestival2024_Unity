using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RicoShot.Core;
using Unity.Collections;
using UnityEngine;

namespace RicoShot.Core.Interface
{
    public interface ISupabaseController
    {
        public bool Connected { get; }

        public UniTask Connect();

        /*
         * DBの "public.profiles" テーブルからプレイヤーのキャラ情報等のデータを取得し、パース結果を返す
         * (userIDはQRコードで渡されることを想定)
         */
        public UniTask<(string displayName, CharacterParams characterParams)> FetchPlayerProfile(string userID);

        public UniTask UpsertTeam(Team team, string teamID, string matchingID, bool isWin);
        
        public UniTask UpsertMatching(string matchingID, DateTime startTime, DateTime endTime);
        
        public UniTask UpsertPlayerResult(string userID, int score, string teamID, string matchingID);
    }
}