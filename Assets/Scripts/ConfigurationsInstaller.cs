using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = nameof(ConfigurationsInstaller), menuName = "Installers/ConfigurationsInstaller")]
public class ConfigurationsInstaller : ScriptableObjectInstaller<ConfigurationsInstaller>
{
    [SerializeField] private List<ScriptableObject> _configurations;

    public override void InstallBindings()
    {
        foreach (var configuration in _configurations)
        {
            var type = configuration.GetType();
            Container.Bind(type).To(type).FromInstance(configuration).AsSingle();
        }
    }
}