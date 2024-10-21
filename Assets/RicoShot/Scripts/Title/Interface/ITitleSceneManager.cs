using RicoShot.InputActions;
using System;

namespace RicoShot.Title.Interface
{
    public interface ITitleSceneManager
    {
        public event Action<TitleState> OnTitleStateChanged;

        public TitleInputs TitleInputs { get; }
        public TitleState TitleState { get; set; }
    }
}