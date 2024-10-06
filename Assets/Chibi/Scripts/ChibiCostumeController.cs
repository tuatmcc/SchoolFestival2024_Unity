using UnityEngine;

namespace Chibi
{
    public class ChibiCostumeController : MonoBehaviour
    {
        [SerializeField] private int chibiIndex;
        [SerializeField] private Color hairColor;
        [SerializeField] private Team team;
        [SerializeField] private ChibiColorController[] chibiColorControllers;
        private readonly Color _lightBlue = new(0.5f, 0.7f, 1);
        private readonly Color _pink = new(1, 0.5f, 0.7f);

        private void OnEnable()
        {
            if (chibiIndex < 0 || chibiIndex >= chibiColorControllers.Length)
            {
                Debug.LogError("Invalid chibi index");
                return;
            }

            // enable only the chibi with the specified index
            foreach (var chibiColorController in chibiColorControllers)
                chibiColorController.gameObject.SetActive(false);
            chibiColorControllers[chibiIndex].gameObject.SetActive(true);

            // set the color of the chibi with the specified index
            chibiColorControllers[chibiIndex].SetColors(hairColor, team == Team.Alpha ? _pink : _lightBlue);
        }

        // 仮のチーム分け
        private enum Team
        {
            Alpha,
            Bravo
        }
    }
}