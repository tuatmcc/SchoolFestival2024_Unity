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

        public UniTask UpsertTeam(Team team, FixedString32Bytes teamID, FixedString32Bytes matchingID, bool isWin);
        
        public UniTask UpsertMatching(FixedString32Bytes matchingID, DateTime startTime, DateTime endTime);
        
        public UniTask UpsertPlayerResult(FixedString32Bytes userID, int score, FixedString32Bytes teamID, FixedString32Bytes matchingID);
    }
}