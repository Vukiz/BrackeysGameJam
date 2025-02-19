using UnityEngine;
using VFX.Data;

namespace VFX.Infrastructure
{
    public interface IVFXManager
    {
        void SpawnVFX(VFXType vfxType, Vector3 position);
    }
}