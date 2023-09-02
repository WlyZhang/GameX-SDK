using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader
{
    /// <summary>
    /// 加载脚本接口
    /// </summary>
    /// <param name="loaderType">加载模式</param>
    //void LoadScript(LoaderType loaderType);

    /// <summary>
    /// 物理循环函数
    /// </summary>
    void FixedUpdate();

    /// <summary>
    /// 循环函数
    /// </summary>
    void Update();

    /// <summary>
    /// 延迟循环函数
    /// </summary>
    void LateUpdate();

    /// <summary>
    /// 销毁函数
    /// </summary>
    void Destroy();

    /// <summary>
    /// Android / iOS 回调
    /// </summary>
    void Callback(string args);

    /// <summary>
    /// 识别语音指令 回调
    /// </summary>
    /// <param name="key"></param>
    void Speech(string key);

    /// <summary>
    /// 手势滑动方向
    /// </summary>
    /// <param name="fingerIndex">手指下标</param>
    /// <param name="startPos">开始位置</param>
    /// <param name="direction">方向枚举</param>
    /// <param name="velocity">速度</param>
    void FingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity);

    /// <summary>
    /// 手势连点
    /// </summary>
    /// <param name="fingerIndex">手指下标</param>
    /// <param name="fingerPos">手指点击位置</param>
    /// <param name="tapCount">连点次数</param>
    void FingerTap(int fingerIndex, Vector2 fingerPos, int tapCount);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fingerIndex">手指下标</param>
    /// <param name="fingerPos">手指点击位置</param>
    void FingerLongPress(int fingerIndex, Vector2 fingerPos);


    /// <summary>
    /// 开始触摸虚拟摇杆
    /// </summary>
    /// <param name="vec">坐标</param>
    void OnJoyStickBegin(Vector2 vec);

    /// <summary>
    /// 正在移动虚拟摇杆
    /// </summary>
    /// <param name="vec">坐标</param>
    void OnJoyStickMove(Vector2 vec);

    /// <summary>
    /// 触摸移动摇杆结束
    /// </summary>
    void OnJoyStickEnd();
}