using RicoShot.Core;

namespace RicoShot.Result.Interface
{
    public interface IResultSceneTester
    {
        bool TestEnabled { get; }

        public CharacterParams CharacterParams { get; }

        public int CameraWorkIndex { get; }
    }
}