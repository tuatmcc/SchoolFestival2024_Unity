using System.Collections.Generic;
using UnityEngine;
namespace Shooting_test
{
    public class ScoreManagerImpl : IScoreManager
    {
        private int total_score = 0;

        public void AddScore(int score)
        {
            total_score += score;
        }

        public int GetScore()
        {
            return total_score;
        }
    }
}