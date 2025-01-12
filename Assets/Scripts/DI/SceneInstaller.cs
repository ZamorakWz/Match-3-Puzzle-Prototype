using UnityEngine;
using Zenject;
using static Unity.Collections.AllocatorManager;
public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("Installing bindings...");

        Container.Bind<BlockSpawnManager>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<BlockObjectPool>().FromComponentInHierarchy().AsTransient().NonLazy();

        Container.Bind<GridManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}