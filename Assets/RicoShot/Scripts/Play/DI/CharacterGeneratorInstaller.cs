using RicoShot.Play.Tests;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.DI
{
    public class CharacterGeneratorInstaller : MonoInstaller
    {
        [SerializeField] private TestCharacterGenerator testCharacterGenerator;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TestCharacterGenerator>().FromInstance(testCharacterGenerator);
        }
    }
}