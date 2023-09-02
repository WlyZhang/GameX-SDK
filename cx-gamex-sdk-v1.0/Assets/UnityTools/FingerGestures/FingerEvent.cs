using UnityEngine;
using System.Collections;

public class FingerEvent : MonoBehaviour
{
    private Camera cam;


    public enum PanMode
    {
        Disabled,
        OneFinger,
        TwoFingers
    }

    /// <summary>
    /// The object to orbit around
    /// </summary>
    public Transform target;

    /// <summary>
    /// Initial camera distance to target
    /// </summary>
    public float initialDistance = 10.0f;

    /// <summary>
    /// Minimum distance between camera and target
    /// </summary>
    public float minDistance = 1.0f;

    /// <summary>
    /// Maximum distance between camera and target
    /// </summary>
    public float maxDistance = 20.0f;

    /// <summary>
    /// Affects horizontal rotation speed
    /// </summary>
    public float yawSensitivity = 80.0f;

    /// <summary>
    /// Affects vertical rotation speed
    /// </summary>
    public float pitchSensitivity = 80.0f;

    /// <summary>
    /// Keep pitch angle value between minPitch and maxPitch?
    /// </summary>
    public bool clampPitchAngle = true;
    public float minPitch = -20;
    public float maxPitch = 80;

    /// <summary>
    /// Allow the user to affect the orbit distance using the pinch zoom gesture
    /// </summary>
    public bool allowPinchZoom = true;

    /// <summary>
    /// Affects pinch zoom speed
    /// </summary>
    public float pinchZoomSensitivity = 2.0f;

    /// <summary>
    /// Use smooth camera motions?
    /// </summary>
    public bool smoothMotion = true;
    public float smoothZoomSpeed = 3.0f;
    public float smoothOrbitSpeed = 4.0f;

    /// <summary>
    /// Two-Finger camera panning.
    /// Panning will apply an offset to the pivot/camera target point
    /// </summary>
    public bool allowPanning = false;
    public bool invertPanningDirections = false;
    public float panningSensitivity = 1.0f;
    public Transform panningPlane;  // reference transform used to apply the panning translation (using panningPlane.right and panningPlane.up vectors)
    public bool smoothPanning = true;
    public float smoothPanningSpeed = 8.0f;
    float lastPanTime = 0;

    float distance = 10.0f;
    float yaw = 0;
    float pitch = 0;

    float idealDistance = 0;
    float idealYaw = 0;
    float idealPitch = 0;

    Vector3 idealPanOffset = Vector3.zero;
    Vector3 panOffset = Vector3.zero;

    public float Distance
    {
        get { return distance; }
    }

    public float IdealDistance
    {
        get { return idealDistance; }
        set { idealDistance = Mathf.Clamp(value, minDistance, maxDistance); }
    }

    public float Yaw
    {
        get { return yaw; }
    }

    public float IdealYaw
    {
        get { return idealYaw; }
        set { idealYaw = value; }
    }

    public float Pitch
    {
        get { return pitch; }
    }

    public float IdealPitch
    {
        get { return idealPitch; }
        set { idealPitch = clampPitchAngle ? ClampAngle(value, minPitch, maxPitch) : value; }
    }

    public Vector3 IdealPanOffset
    {
        get { return idealPanOffset; }
        set { idealPanOffset = value; }
    }

    public Vector3 PanOffset
    {
        get { return panOffset; }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }




    /// ////////////////////////////////////////////////////////////////



    void OnEnable()
    {
        //启动时调用，这里开始注册手势操作的事件。

        //按下事件： OnFingerDown就是按下事件监听的方法，这个名子可以由你来自定义。方法只能在本类中监听。下面所有的事件都一样！！！
        FingerGestures.OnFingerDown += OnFingerDown;
        //抬起事件
        FingerGestures.OnFingerUp += OnFingerUp;
        //开始拖动事件
        FingerGestures.OnFingerDragBegin += OnFingerDragBegin;
        //拖动中事件...
        FingerGestures.OnFingerDragMove += OnFingerDragMove;
        //拖动结束事件
        FingerGestures.OnFingerDragEnd += OnFingerDragEnd;
        //上、下、左、右、四个方向的手势滑动
        FingerGestures.OnFingerSwipe += OnFingerSwipe;
        //连击事件 连续点击事件
        FingerGestures.OnFingerTap += OnFingerTap;
        //按下事件后调用一下三个方法
        FingerGestures.OnFingerStationaryBegin += OnFingerStationaryBegin;
        FingerGestures.OnFingerStationary += OnFingerStationary;
        FingerGestures.OnFingerStationaryEnd += OnFingerStationaryEnd;
        //长按事件
        FingerGestures.OnFingerLongPress += OnFingerLongPress;

        FingerGestures.OnTwoFingerDragMove += FingerGestures_OnTwoFingerDragMove;

        FingerGestures.OnPinchMove += FingerGestures_OnPinchMove;

    }

    void OnDisable()
    {
        //关闭时调用，这里销毁手势操作的事件
        //和上面一样
        FingerGestures.OnFingerDown -= OnFingerDown;
        FingerGestures.OnFingerUp -= OnFingerUp;
        FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= OnFingerDragEnd;
        FingerGestures.OnFingerSwipe -= OnFingerSwipe;
        FingerGestures.OnFingerTap -= OnFingerTap;
        FingerGestures.OnFingerStationaryBegin -= OnFingerStationaryBegin;
        FingerGestures.OnFingerStationary -= OnFingerStationary;
        FingerGestures.OnFingerStationaryEnd -= OnFingerStationaryEnd;
        FingerGestures.OnFingerLongPress -= OnFingerLongPress;

        FingerGestures.OnTwoFingerDragMove -= FingerGestures_OnTwoFingerDragMove;

        FingerGestures.OnPinchMove -= FingerGestures_OnPinchMove;
    }

    //按下时调用
    void OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        //int fingerIndex 是手指的ID 第一按下的手指就是 0 第二个按下的手指就是1。。。一次类推。
        //Vector2 fingerPos 手指按下屏幕中的2D坐标

        //将2D坐标转换成3D坐标
        //transform.position = GetWorldPos(fingerPos);
        Debug.Log(" OnFingerDown =" + fingerPos);
    }

    //抬起时调用
    void OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {

        Debug.Log(" OnFingerUp =" + fingerPos);
    }

    //开始滑动
    void OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
    {
        Debug.Log("OnFingerDragBegin fingerIndex =" + fingerIndex + " fingerPos =" + fingerPos + "startPos =" + startPos);
    }
    //滑动结束
    void OnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
    {

        Debug.Log("OnFingerDragEnd fingerIndex =" + fingerIndex + " fingerPos =" + fingerPos);
    }

    //上下左右四方方向滑动手势操作
    void OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
    {
        //结果是 Up Down Left Right 四个方向
        Debug.Log("OnFingerSwipe " + direction + " with finger " + fingerIndex);


        //调用Lua端对应函数
        //Game.Instance.Loader.FingerSwipe(fingerIndex, startPos, direction, velocity);
    }

    //连续按下事件， tapCount就是当前连续按下几次
    void OnFingerTap(int fingerIndex, Vector2 fingerPos, int tapCount)
    {

        Debug.Log("OnFingerTap " + tapCount + " times with finger " + fingerIndex);

        //连续按下事件
        //Game.Instance.Loader.FingerTap(fingerIndex, fingerPos, tapCount);

    }

    //按下事件开始后调用，包括 开始 结束 持续中状态只到下次事件开始！
    void OnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
    {

        Debug.Log("OnFingerStationaryBegin " + fingerPos + " times with finger " + fingerIndex);
    }
    
    void OnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
    {

        Debug.Log("OnFingerStationary " + fingerPos + " times with finger " + fingerIndex);

    }

    void OnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
    {

        Debug.Log("OnFingerStationaryEnd " + fingerPos + " times with finger " + fingerIndex);
    }


    //长按事件
    void OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
    {

        Debug.Log("OnFingerLongPress " + fingerPos);

        //长按事件
        //Game.Instance.Loader.FingerLongPress(fingerIndex, fingerPos);
    }

    //把Unity屏幕坐标换算成3D坐标
    Vector3 GetWorldPos(Vector2 screenPos)
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(transform.position.z - mainCamera.transform.position.z)));
    }

    //-------------------------------------


    //滑动中
    void OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
    {
        //transform.position = GetWorldPos(fingerPos);
        Debug.Log(" OnFingerDragMove =" + fingerPos);


        if (Time.time - lastPanTime < 0.25f)
            return;

        if (target)
        {
            IdealYaw += delta.x * yawSensitivity * 0.02f;
            IdealPitch -= delta.y * pitchSensitivity * 0.02f;
        }
    }

    void FingerGestures_OnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
    {
        if (allowPanning)
        {
            Vector3 move = -0.02f * panningSensitivity * (panningPlane.right * delta.x + panningPlane.up * delta.y);

            if (invertPanningDirections)
                IdealPanOffset -= move;
            else
                IdealPanOffset += move;

            lastPanTime = Time.time;
        }
    }


    void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
    {
        if (allowPinchZoom)
            IdealDistance -= delta * pinchZoomSensitivity;
    }


    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        if (!panningPlane)
            panningPlane = this.transform;

        Vector3 angles = transform.eulerAngles;

        distance = IdealDistance = initialDistance;
        yaw = IdealYaw = angles.y;
        pitch = IdealPitch = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        Apply();
    }


    void LateUpdate()
    {
        Apply();
    }
    void Apply()
    {
        if (smoothMotion)
        {
            distance = Mathf.Lerp(distance, IdealDistance, Time.deltaTime * smoothZoomSpeed);
            yaw = Mathf.Lerp(yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed);
            pitch = Mathf.Lerp(pitch, IdealPitch, Time.deltaTime * smoothOrbitSpeed);
        }
        else
        {
            distance = IdealDistance;
            yaw = IdealYaw;
            pitch = IdealPitch;
        }

        if (smoothPanning)
            panOffset = Vector3.Lerp(panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed);
        else
            panOffset = idealPanOffset;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = (target.position + panOffset) - distance * transform.forward;

        cam.orthographicSize = distance*0.5f;
    }
    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(310, 10, 80, 30), "放大"))
        {
            cam.orthographicSize -= 0.5f;
        }
        if (GUI.Button(new Rect(395, 10, 80, 30), "缩小"))
        {
            cam.orthographicSize += 0.5f;
        }

        GUI.Label(new Rect(50, 50, 200, 200), "distance:" + distance.ToString());
    }
    */

}
