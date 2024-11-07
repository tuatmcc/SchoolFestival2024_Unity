using RicoShot.Core;
using RicoShot.InputActions;
using System;

namespace RicoShot.Title.Interface
{
    public interface ITitleSceneManager
    {
        public event Action<TitleState> OnTitleStateChanged;
        public event Action OnReadNotUUID;
        public event Action OnReadUUIDNotExist;

        public TitleInputs TitleInputs { get; }
        public TitleState TitleState { get; set; }
        public string FetchedDisplayName { get;}
        public CharacterParams FetchedCharacterParams { get; }
        public int FetchedPlayCount { get; }
        public int FetchedHighScore { get; }

        public void FetchData(string uuid);
    }
}