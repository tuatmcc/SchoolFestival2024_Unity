using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RicoShot.SupabaseController
{
    public interface ISupabaseController
    {
        UniTask ConnectSupabase(string url, string key);
        /*
         * DBの "public.profiles" テーブルからプレイヤーのキャラ情報等のデータを取得し、パース結果を返す
         * (userIDはQRコードで渡されることを想定)
         */
        UniTask<PlayerProfile> FetchPlayerProfile(string userID);
        
        /*
         * MatchingResult内のデータから "対戦結果"、"チーム情報"、"各プレイヤーのスコア" をそれぞれSupabaseに送信する
         */
        void UpsertResult(MatchingResult result);
    }
}