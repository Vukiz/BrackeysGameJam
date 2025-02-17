using UnityEngine;

namespace Level.Implementation
{
    public class CameraProvider
    {
        public Camera MainCamera { get; private set; }

        public CameraProvider(Camera camera)
        {
            MainCamera = camera;
        }
    }
}