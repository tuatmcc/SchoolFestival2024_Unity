using RicoShot.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RicoShot.Result.UI
{
    public class RankingPlayer : MonoBehaviour
    {
        [SerializeField] private Image cursor;
        [SerializeField] private Image background;
        [SerializeField] private Image rankImageArea;
        [SerializeField] private TMP_Text displayNameArea;
        [SerializeField] private TMP_Text scoreTextArea;
        [SerializeField] private Sprite alphaBackground;
        [SerializeField] private Sprite bravoBackground;
        [SerializeField] private Sprite[] rankImages;
        [SerializeField] private Sprite[] teamImages;

        public void SetPlayerData(string displayName, Team team, int score, bool isLocalPlayer, int rank)
        {
            cursor.gameObject.SetActive(isLocalPlayer);
            displayNameArea.text = displayName;
            scoreTextArea.text = score.ToString();
            background.sprite = team == Team.Alpha ? alphaBackground : bravoBackground;
            for (var i = 0; i < rankImages.Length; ++i)
                rankImageArea.sprite = rankImages[rank - 1];
        }
    }
}