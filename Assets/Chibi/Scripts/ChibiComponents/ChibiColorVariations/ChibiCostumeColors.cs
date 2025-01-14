using UnityEngine;

namespace Chibi.ChibiComponents.ChibiColorVariations
{
    public abstract class ChibiCostumeColors : MonoBehaviour
    {
        public abstract string primary { get; }
        public abstract string secondary { get; }
        public abstract string tertiary { get; }

        // parse the color string(hex) to a Color object
        public Color GetColor(int colorIndex)
        {
            switch (colorIndex)
            {
                case 0:
                    return ParseColor(primary);
                case 1:
                    return ParseColor(secondary);
                case 2:
                    return ParseColor(tertiary);
                default:
                    Debug.LogWarning("Color index out of range");
                    return Color.white;
            }
        }

        private Color ParseColor(string colorString)
        {
            var color = Color.white;
            ColorUtility.TryParseHtmlString(colorString, out color);
            return color;
        }
    }
}