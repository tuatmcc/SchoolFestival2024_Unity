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
    public bool Connected { get; private set; } = false;

    public async UniTask Connect()
    {
        await UniTask.WaitForSeconds(0.1f);
    }

    public async UniTask<(string displayName, CharacterParams characterParams)> FetchPlayerProfile(string userID)
    {
        await UniTask.WaitForSeconds(0.1f);
        return ("Test Character", CharacterParams.GetRandomCharacterParams());
    }

    public async UniTask UpsertMatching(FixedString32Bytes matchingID, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public async UniTask UpsertPlayerResult(FixedString32Bytes userID, int score, FixedString32Bytes teamID, FixedString32Bytes matchingID)
    {
        throw new NotImplementedException();
    }

    public async UniTask UpsertTeam(Team team, FixedString32Bytes teamID, FixedString32Bytes matchingID, bool isWin)
    {
        throw new NotImplementedException();
    }
}
