namespace RicoShot.Core.Interface
{
    public interface ILocalPlayerManager
    {
        public string LocalPlayerUUID { get; set; }
        public CharacterParams CharacterParams { get; set; }
    }
}