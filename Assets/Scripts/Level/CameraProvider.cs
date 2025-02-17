using UnityEngine;

namespace Level
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