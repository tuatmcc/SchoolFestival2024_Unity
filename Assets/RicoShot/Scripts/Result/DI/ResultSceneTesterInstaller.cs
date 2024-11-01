using UnityEngine;
using Zenject;

namespace RicoShot.Result
{
    public class ResultSceneTesterInstaller : MonoInstaller
    {
        [SerializeField] private ResultSceneTester resultSceneTester;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResultSceneTester>().FromInstance(resultSceneTester);
        }
    }
}