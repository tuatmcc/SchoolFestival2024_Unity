using Cysharp.Threading.Tasks;
using RicoShot.Core;
using RicoShot.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TestSupabaseController : ISupabaseController
{
    public async UniTask<CharacterParams> FetchPlayerProfile(string userID)
    {
        throw new NotImplementedException();
    }

    public void UpsertMatching(FixedString32Bytes matchingID, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public void UpsertPlayerResult(FixedString32Bytes userID, int score, FixedString32Bytes teamID, FixedString32Bytes matchingID)
    {
        throw new NotImplementedException();
    }

    public void UpsertTeam(Team team, FixedString32Bytes teamID, FixedString32Bytes matchingID, bool isWin)
    {
        throw new NotImplementedException();
    }
}
