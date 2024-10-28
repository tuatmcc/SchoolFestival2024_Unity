using RicoShot.InputActions;
using System;
using Unity.Collections;

namespace RicoShot.Title.Interface
{
    public interface ITitleSceneManager
    {
        public event Action<TitleState> OnTitleStateChanged;

        public TitleInputs TitleInputs { get; }
        public TitleState TitleState { get; set; }
        public string ReadUUID { get; set; }
    }
}