using System;
using System.Collections;
using System.Collections.Generic;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play;
using RicoShot.Play.Interface;
using Ricoshot.Result;
using Unity.Collections;
using UnityEngine;
using Zenject;

//public class ResultSender : IResultSender
//{
//    [Inject] private ISupabaseController _supabaseController;
//    [Inject] private INetworkController _networkController;
//    [Inject] private INetworkScoreManager _networkScoreManager;

//    public void SendResult()
//    {
//        int alphaScore = 0, bravoScore = 0;
//        Dictionary<FixedString64Bytes, int> scoreDic = new Dictionary<FixedString64Bytes, int>();
//        foreach (var scoreData in _networkScoreManager.ScoreList)
//        {
//            scoreDic[scoreData.UUID] = scoreData.Score;
//            switch (scoreData.Team)
//            {
//                case Team.Alpha:
//                    alphaScore += scoreData.Score;
//                    break;
//                case Team.Bravo:
//                    bravoScore += scoreData.Score;
//                    break;
//            }
//        }

//        FixedString32Bytes alphaUUID = Guid.NewGuid().ToString();
//        FixedString32Bytes bravoUUID = Guid.NewGuid().ToString();
//        FixedString32Bytes matchingUUID = Guid.NewGuid().ToString();

//        // 同点の場合は両方とも勝ち
//        _supabaseController.UpsertTeam(Team.Alpha, alphaUUID, matchingUUID, ((alphaScore > bravoScore) || (alphaScore == bravoScore)));
//        _supabaseController.UpsertTeam(Team.Bravo, bravoUUID, matchingUUID, ((alphaScore < bravoScore) || (alphaScore == bravoScore)));

//        // 試合情報を送信(StartTimeとEndTimeが無いためコメントアウト)
//        // とりあえずNetworkScoreManagerにくっついていることを想定
//        // _supabaseController.UpsertMatching(matchingUUID, _networkScoreManager.StartTime, _networkScoreManager.EndTime);

//        foreach (var client in _networkController.ClientDataList)
//        {
//            switch (client.Team)
//            {
//                case Team.Alpha:
//                    _supabaseController.UpsertPlayerResult(client.UUID.ToString(), scoreDic[client.UUID], alphaUUID, matchingUUID);
//                    break;
//                case Team.Bravo:
//                    _supabaseController.UpsertPlayerResult(client.UUID.ToString(), scoreDic[client.UUID], bravoUUID, matchingUUID);
//                    break;
//            }
//        }
//    }
//}
