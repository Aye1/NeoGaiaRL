using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class CameraBehaviour : MonoBehaviour {

    public static readonly Vector3 minResolution = new Vector3(1024f, 576f, 0);
    private float targetRatio = 16f / 9f;

    private static CameraBehaviour instance = null;
    private Camera _camera;
    private int _width;
    private int _height;

    private static readonly string CameraColliderTag = "CameraCollider";
    private static readonly string CameraColliderTagLeft = "CameraColliderLeft";
    private static readonly string CameraColliderTagRight = "CameraColliderRight";
    private static readonly string CameraColliderTagTop = "CameraColliderTop";
    private static readonly string CameraColliderTagBottom = "CameraColliderBottom";

    // Camera dynamic boundaries
    private bool _hasLeftBoundary = false;
    private bool _hasRightBoundary = false;
    private bool _hasTopBoundary = false;
    private bool _hasBottomBoundary = false;
    private float _leftBoundary = float.MinValue;
    private float _rightBoundary = float.MaxValue;
    private float _topBoundary = float.MaxValue;
    private float _bottomBoundary = float.MinValue;

    public PlayerMovement player;
    public bool isCinematicMode = false;
    public Vector3 targetPosition;
    public float smoothTime;

    [Header("Debug settings")]
    public bool displayPosInCamera = false;
    public bool displayCameraSize = false;

    #region Accessors
    // Width of the camera
    public int Width
    {
        get
        {
            return _width;
        }

        set
        {
            if (value != _width)
            {
                _width = value;
                AdjustScreenScale();
            }
        }
    }

    // Height of the camera
    public int Height
    {
        get
        {
            return _height;
        }

        set
        {
            if (value != _height)
            {
                _height = value;
                AdjustScreenScale();
            }
        }
    }


    #endregion

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        _camera = GetComponent<Camera>();
        DontDestroyOnLoad(_camera);
        ObjectChecker.CheckNullity(player, "Camera can't find the Player");
        GoToPlayer();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Width = Screen.width;
        Height = Screen.height;
        if (!isCinematicMode)
        {
            BasicFollow();
        }
    }

    // Instantly goes to the player position
    private void GoToPlayer()
    {
        _camera.transform.position = new Vector3(player.transform.position.x
            , player.transform.position.y
            , _camera.transform.position.z);
    }

    public void GoToPosition(Vector3 pos)
    {
        GoToPosition(pos, 1);
    }

    public void GoToPosition(Vector3 pos, float time)
    {
        Debug.Log("Going to " + pos.ToString());
        StartCoroutine(SmoothMoveToPosition(pos, time));
    }

    public IEnumerator SmoothMoveToPosition(Vector3 pos, float time)
    {
        Vector3 currentPos = transform.position;
        float t = 0.0f;
        while (t<1)
        {
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPos, pos, t);
            yield return null;
        }
    }

    // Follow the player
    // Offset left/right depending of the player last movement
    // Offset down if the player is crouched
    private void BasicFollow()
    {
        float offsetX = 5.0f;
        float offsetY = 2.0f;
        Vector3 camPos = _camera.transform.position;
        Vector3 playerPos = player.transform.position;
        float crouchOffset = player.IsCrouching() ? -1.0f : 0.0f;

        Vector3 newPosition = new Vector3(playerPos.x + player.GetLatestDirectionX() * offsetX, 
            playerPos.y + crouchOffset * offsetY,
            camPos.z);

        // Clamp camera position if colliding
        newPosition.x = Mathf.Clamp(newPosition.x, _leftBoundary, _rightBoundary);
        newPosition.y = Mathf.Clamp(newPosition.y, _bottomBoundary, _topBoundary);

        Vector3 velocity = Vector3.zero;
        _camera.transform.position = Vector3.SmoothDamp(camPos, newPosition, ref velocity, 0.1f);
    }

    /// <summary>
    /// Computes the new scale of the screen
    /// </summary>
    private void AdjustScreenScale()
    {
        // determine the game window's current aspect ratio
        float windowaspect = Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetRatio;


        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = _camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            _camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = _camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
        if (collision.CompareTag(CameraColliderTagLeft))
        {
            _hasLeftBoundary = true;
            _leftBoundary = collisionPoint.x;
        }
        if (collision.CompareTag(CameraColliderTagRight))
        {
            _hasRightBoundary = true;
            _rightBoundary = collisionPoint.x;
        }
        if (collision.CompareTag(CameraColliderTagTop))
        {
            _hasTopBoundary = true;
            _topBoundary = collisionPoint.y;
        }
        if (collision.CompareTag(CameraColliderTagBottom))
        {
            _hasBottomBoundary = true;
            _bottomBoundary = collisionPoint.y;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(CameraColliderTagLeft))
        {
            _hasLeftBoundary = false;
            _leftBoundary = float.MinValue;
        }
        if (collision.CompareTag(CameraColliderTagRight))
        {
            _hasRightBoundary = false;
            _rightBoundary = float.MaxValue;
        }
        if (collision.CompareTag(CameraColliderTagTop))
        {
            _hasTopBoundary = false;
            _topBoundary = float.MaxValue;
        }
        if (collision.CompareTag(CameraColliderTagBottom))
        {
            _hasBottomBoundary = false;
            _bottomBoundary = float.MinValue;
        }
    }
}
