using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    float zoom;
    float zoomMultiplier = 2f;
    float minZoom = 0.5f;
    float maxZoom = 5f;
    float velocity = 0f;
    float smoothTime = 0.25f;

    [SerializeField] private Camera cam;

    private void Start()
    {
        zoom = cam.orthographicSize;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
    }
}