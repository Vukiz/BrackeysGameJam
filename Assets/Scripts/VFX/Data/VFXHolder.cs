using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VFX.Data
{
    [CreateAssetMenu(fileName = "VFXHolder", menuName = "VFX/VFXHolder")]
    public class VFXHolder : ScriptableObject
    {
        [SerializeField] private List<VFXData> _vfxData;

        public List<VFXData> VFXDatas => _vfxData.ToList();

        public ParticleSystem GetVFXByType(VFXType vfxType)
        {
            return _vfxData.FirstOrDefault(vfx => vfx.VFXType == vfxType)?.VFXPrefab;
        }
    }
}