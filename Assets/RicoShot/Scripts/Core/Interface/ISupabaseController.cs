using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RicoShot.Core;
using Unity.Collections;
using UnityEngine;

namespace RicoShot.Core
{
    public interface ISupabaseController
    {
        UniTask ConnectSupabase(string url, string key);
        /*
         * DBの "public.profiles" テーブルからプレイヤーのキャラ情報等のデータを取得し、パース結果を返す
         * (userIDはQRコードで渡されることを想定)
         */
        UniTask<CharacterParams> FetchPlayerProfile(string userID);

        void UpsertTeam(Team team, FixedString32Bytes teamID, FixedString32Bytes matchingID, bool isWin);
        
        void UpsertMatching(FixedString32Bytes matchingID, DateTime startTime, DateTime endTime);
        
        void UpsertPlayerResult(FixedString32Bytes userID, int score, FixedString32Bytes teamID, FixedString32Bytes matchingID);
    }
}