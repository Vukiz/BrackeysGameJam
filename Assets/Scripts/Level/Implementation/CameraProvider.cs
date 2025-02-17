using UnityEngine;

namespace Level.Implementation
{
    public class CameraProvider
    {
        public Camera MainCamera { get; private set; }

        public void SetMainCamera(Camera mainCamera)
        {
            MainCamera = mainCamera;
        }
    }
}