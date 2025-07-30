using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject _dronePrefab; 
                                                   
    public override void InstallBindings()
    {
        Container.Bind<DroneStats>()
            .FromComponentInNewPrefab(_dronePrefab)
            .UnderTransformGroup("Drones")
            .AsTransient(); // her ihtiyaçta yeni Drone yarat
    }
}
