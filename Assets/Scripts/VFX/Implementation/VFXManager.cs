using UnityEngine;
using VFX.Data;
using VFX.Infrastructure;

namespace VFX.Implementation
{
    public class VFXManager : IVFXManager
    {
        private readonly VFXHolder _vfxHolder;

        public VFXManager(VFXHolder vfxHolder)
        {
            _vfxHolder = vfxHolder;
        }

        public void SpawnVFX(VFXType vfxType, Vector3 position)
        {
            var vfx = _vfxHolder.GetVFXByType(vfxType);
            if (vfx == null)
            {
                Debug.LogError($"VFX Not found for {vfxType}");
                return;
            }

            Object.Instantiate(vfx, position, Quaternion.identity);

            // object will automatically be destroyed by a script on it
        }
    }
}