using System;

namespace RicoShot.Play.Interface
{
    public interface IHpHolder
    {
        public event Action<int> OnHpChanged;

        public int Hp { get; }

        public void DecreaseHp(int damage);
    }
}
