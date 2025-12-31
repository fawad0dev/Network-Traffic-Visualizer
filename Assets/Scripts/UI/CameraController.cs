using UnityEngine;

namespace NetworkTrafficVisualizer.UI
{
    /// <summary>
    /// Controls camera movement and rotation for scene navigation
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float fastMoveMultiplier = 3f;
        [SerializeField] private float rotationSpeed = 100f;

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 50f;

        [Header("Mouse Control")]
        [SerializeField] private bool enableMouseRotation = true;
        [SerializeField] private float mouseSensitivity = 2f;

        private Vector3 dragOrigin;
        private bool isDragging = false;
        private float currentZoom = 20f;

        private void Start()
        {
            currentZoom = transform.position.magnitude;
        }

        private void Update()
        {
            HandleKeyboardMovement();
            HandleMouseControl();
            HandleZoom();
        }

        private void HandleKeyboardMovement()
        {
            float speed = moveSpeed;
            
            // Fast move with Shift
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed *= fastMoveMultiplier;
            }

            // WASD movement
            Vector3 movement = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W))
                movement += transform.forward;
            if (Input.GetKey(KeyCode.S))
                movement -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                movement -= transform.right;
            if (Input.GetKey(KeyCode.D))
                movement += transform.right;
            if (Input.GetKey(KeyCode.Q))
                movement += Vector3.down;
            if (Input.GetKey(KeyCode.E))
                movement += Vector3.up;

            transform.position += movement.normalized * speed * Time.deltaTime;
        }

        private void HandleMouseControl()
        {
            if (!enableMouseRotation) return;

            // Right mouse button for rotation
            if (Input.GetMouseButtonDown(1))
            {
                dragOrigin = Input.mousePosition;
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector3 difference = Input.mousePosition - dragOrigin;
                
                float rotationX = difference.y * mouseSensitivity;
                float rotationY = difference.x * mouseSensitivity;

                transform.Rotate(Vector3.right, -rotationX * Time.deltaTime * rotationSpeed, Space.Self);
                transform.Rotate(Vector3.up, rotationY * Time.deltaTime * rotationSpeed, Space.World);

                dragOrigin = Input.mousePosition;
            }

            // Arrow keys for rotation
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
            if (Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            if (Input.GetKey(KeyCode.UpArrow))
                transform.Rotate(Vector3.right, -rotationSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey(KeyCode.DownArrow))
                transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if (scroll != 0f)
            {
                currentZoom -= scroll * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
                
                // Move camera along its forward direction
                Vector3 direction = transform.forward.normalized;
                transform.position -= direction * scroll * zoomSpeed;
            }
        }

        /// <summary>
        /// Focus camera on a specific position
        /// </summary>
        public void FocusOn(Vector3 position, float distance = 20f)
        {
            Vector3 direction = (transform.position - position).normalized;
            transform.position = position + direction * distance;
            transform.LookAt(position);
        }

        /// <summary>
        /// Reset camera to default position
        /// </summary>
        public void ResetCamera()
        {
            transform.position = new Vector3(0, 20, -20);
            transform.rotation = Quaternion.Euler(30, 0, 0);
            currentZoom = 20f;
        }
    }
}
