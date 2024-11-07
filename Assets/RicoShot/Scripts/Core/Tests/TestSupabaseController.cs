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

    public async UniTask<(string displayName, CharacterParams characterParams, int playCount, int highScore)> FetchPlayerProfile(string userID)
    {
        await UniTask.WaitForSeconds(0.1f);
        return ("Test Character", CharacterParams.GetRandomCharacterParams(), 10, 100);
    }

    public async UniTask UpsertMatching(string matchingID, DateTime startTime, DateTime endTime)
    {
        await UniTask.WaitForSeconds(0.1f);
    }

    public async UniTask UpsertPlayerResult(string userID, int score, string teamID, string matchingID)
    {
        await UniTask.WaitForSeconds(0.1f);
    }

    public async UniTask UpsertTeam(Team team, string teamID, string matchingID, bool isWin)
    {
        await UniTask.WaitForSeconds(0.1f);
    }
}
