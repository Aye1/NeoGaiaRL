using UnityEngine;
using Assets.Scripts.Helpers;

public class CameraBehaviour : MonoBehaviour {

    public static readonly Vector3 minResolution = new Vector3(1024f, 576f, 0);
    private float targetRatio = 16f / 9f;

    private static CameraBehaviour instance = null;
    private Camera _camera;
    private int _width;
    private int _height;

    public PlayerMovement player;

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

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Width = Screen.width;
        Height = Screen.height;
        //SmoothFollow();
        BasicFollow();
    }

    private void BasicFollow()
    {
        float offsetX = 5.0f;
        float offsetY = 2.0f;
        Vector3 camPos = _camera.transform.position;
        Vector3 playerPos = player.transform.position;
        float crouchOffset = player.crouching ? -1.0f : 0.0f;

        Vector3 newPosition = new Vector3(playerPos.x + player.lastDirectionX * offsetX, 
            playerPos.y + crouchOffset * offsetY,
            camPos.z);

        Vector3 velocity = Vector3.zero;
        _camera.transform.position = Vector3.SmoothDamp(camPos, newPosition, ref velocity, 0.1f);
        //_camera.transform.position = newPosition;
    }

    private void SmoothFollow()
    {
        Vector3 posInCamera = _camera.WorldToScreenPoint(player.transform.position);

        // Debug only
        if (displayPosInCamera)
        {
            Debug.Log("Pos: " + posInCamera);
        }
        if (displayCameraSize)
        {
            Debug.Log("Camera: " + Width + "x" + Height);
        }
        
        // Distance to trigger the camera animation
        float offsetX = Width*0.3f;
        float offSetY = Height*0.3f;

        // Offset to have more space above the player than below
        float floorOffset = 2f;

        Vector3 camPos = _camera.transform.position;
        Vector3 destination = camPos;
        if (posInCamera.y + offSetY >= Height || posInCamera.y - offSetY <= 0)
        {
            float direction = posInCamera.y > Height / 2.0f ? 1.0f : -1.0f;
            destination.y = player.transform.position.y + floorOffset + 2.0f/3.0f * Height * direction;
        }
        if (posInCamera.x + offsetX >= Width || posInCamera.x - offsetX <= 0)
        {
            float direction = posInCamera.x > Width / 2.0f ? 1.0f : -1.0f;
            destination.x = player.transform.position.x + 2.0f/3.0f * Width * direction ;
        }
        Vector3 velocity = Vector3.zero;
        _camera.transform.position = Vector3.SmoothDamp(camPos, destination, ref velocity, GetSmoothTime(posInCamera));
    }

    private float GetSmoothTime(Vector3 posInCamera)
    {
        float defaultSmoothTime = 1.0f;
        //float fastSmoothTime = 0.01f;
        //float velocityThreshold = 20.0f;

        /*Debug.Log("velocity:" + player.GetComponent<Rigidbody2D>().velocity.y);

        if (posInCamera.y >= Height || posInCamera.y <= 0)
        {
            return fastSmoothTime;
        }*/

        /*if (Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y) >= velocityThreshold)
        {
            return fastSmoothTime;
        }*/
        return defaultSmoothTime;
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

    public void OnColliderEnter2D(Collider2D collider)
    {
        Debug.Log("Camera colliding");
    }
}
