using UnityEngine;
using Factories.Infrastructure;

namespace VFX.Views
{
    public class SignalGlow : MonoBehaviour
    {
        private Material _glowMaterial;
        [SerializeField] private float _maxIntensity = 3f; // HDR intensity multiplier

        private IFactory _factory;
        [SerializeField] private Color _redColor = new(1f, 0f, 0f);
        [SerializeField] private Color _greenColor = new(0f, 1f, 0f);

        private void Start()
        {
            // If material not assigned in inspector, try to get it from the renderer
            if (_glowMaterial == null)
            {
                _glowMaterial = GetComponent<Renderer>().material;
            }

            // Initialize emission
            _glowMaterial.EnableKeyword("_EMISSION");
        }

        public void Initialize(IFactory factory)
        {
            _factory = factory;
        }

        private void Update()
        {
            if (_factory == null)
            {
                return;
            }

            var progress = _factory.SpawnProgress;
            var currentColor = Color.Lerp(_redColor, _greenColor, progress);
            _glowMaterial.SetColor("_EmissionColor", currentColor * _maxIntensity);
        }

        private void OnDestroy()
        {
            // Clean up material if it was created at runtime
            if (_glowMaterial != null)
            {
                Destroy(_glowMaterial);
            }
        }
    }
}