using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RicoShot.Matching.UI
{
    public class MatchingTeamOption : MonoBehaviour
    {
        [SerializeField] private TMP_Text teamName;
        [SerializeField] private TMP_Text teamNameShadow;
        [SerializeField] private Image cursor;

        public void SetTeamName(string name)
        {
            teamName.text = name;
            teamNameShadow.text = name;
        }

        public void SetCursorActive(bool active)
        {
            cursor.gameObject.SetActive(active);
        }
    }
}