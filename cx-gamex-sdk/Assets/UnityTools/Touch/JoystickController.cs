using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// 摇杆控制类(GTA模式的第三人称-people+camera在同一个节点)
/// 摇杆控制类(双手模式的第三人称-people、camera在不同节点，并开启摄像机追踪)
/// </summary>
public class JoystickController : MonoBehaviour {

    /// <summary>
    /// 人物根节点
    /// </summary>
    public Transform m_Target;

    /// <summary>
    /// 摇杆
    /// </summary>
    private JoyStick js;

    /// <summary>
    /// 人物节点
    /// </summary>
    private Transform _peopleNode;

    /// <summary>
    /// 摄像机节点
    /// </summary>
    private Camera _cameraNode;

    /// <summary>
    /// 是否是GTA模式的第三人称
    /// </summary>
    private bool _isGTAThird = true;

    /// <summary>
    /// 摄像机跟随速度
    /// </summary>
    private float _followSpeed = 5;

    /// <summary>
    /// 人物节点移动速度
    /// </summary>
    private float _moveSpeed = 10;

    /// <summary>
    /// 人物节点旋转速度
    /// </summary>
    private float _rotationSpeed = 30 ;

    /// <summary>
    /// 摄像机跟随距离
    /// </summary>
    private float _cameraDistance = 5;

    void Awake() {

    }

    public void OnEnable()
    {
        js = GameObject.FindObjectOfType<JoyStick>();
        js.OnJoyStickTouchBegin += On_JoystickMoveStart;
        js.OnJoyStickTouchMove += On_JoystickMove;
        js.OnJoyStickTouchEnd += On_JoystickMoveEnd;


        //上、下、左、右、四个方向的手势滑动
        FingerGestures.OnFingerSwipe += OnFingerSwipe;
    }

    public void OnDisable()
    {
        js.OnJoyStickTouchBegin -= On_JoystickMoveStart;
        js.OnJoyStickTouchMove -= On_JoystickMove;
        js.OnJoyStickTouchEnd -= On_JoystickMoveEnd;


        //上、下、左、右、四个方向的手势滑动
        FingerGestures.OnFingerSwipe -= OnFingerSwipe;
    }
		
	void OnDestroy()
    {
        js.OnJoyStickTouchBegin -= On_JoystickMoveStart;
        js.OnJoyStickTouchMove -= On_JoystickMove;
        js.OnJoyStickTouchEnd -= On_JoystickMoveEnd;
    }

    void On_JoystickMoveStart(Vector2 move)
    {
        Debug.Log("Start");
    }
	
	void On_JoystickMoveEnd(){
        Debug.Log("End");
    }

    /// <summary>
    /// 控制器处于移动状态
    /// </summary>
    /// <param name="move"></param>
	void On_JoystickMove(Vector2 move)
    {
        Debug.Log(move);
    }

    private void OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
    {

    }

}
