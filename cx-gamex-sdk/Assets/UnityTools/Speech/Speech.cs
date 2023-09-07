using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using UnityEngine.Windows.Speech;//引入命名空间  利用
#endif

/// <summary>
/// 语音识别（主要是别关键字）
/// </summary>
public class Speech : MonoBehaviour
{
    private static Speech _instance;
    public static Speech Instance
    {
        get { return _instance; }
    }


#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    // 短语识别器
    private PhraseRecognizer m_PhraseRecognizer;
#endif

    // 关键字
    [HideInInspector]
    public string[] keywords = { "你好", "开始", "停止" };

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    // 可信度
    public ConfidenceLevel m_confidenceLevel = ConfidenceLevel.Medium;
#endif

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// 设置指令
    /// </summary>
    /// <param name="args"></param>
    public void SetKeys(params string[] args)
    {
        keywords = args;
    }

    /// <summary>
    /// 创建语音解析器
    /// </summary>
    public void Create()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (m_PhraseRecognizer == null)
        {
            //创建一个识别器
            m_PhraseRecognizer = new KeywordRecognizer(keywords, m_confidenceLevel);
            //通过注册监听的方法
            m_PhraseRecognizer.OnPhraseRecognized += M_PhraseRecognizer_OnPhraseRecognized;
            //开启识别器
            m_PhraseRecognizer.Start();

            Debug.Log("创建识别器成功");
        }
#endif
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    /// <summary>
    ///  当识别到关键字时，会调用这个方法
    /// </summary>
    /// <param name="args"></param>
    private void M_PhraseRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        SpeechRecognition(args.text);
    }
#endif


    private void OnDestroy()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        //判断场景中是否存在语音识别器，如果有，释放
        if (m_PhraseRecognizer != null)
        {
            //用完应该释放，否则会带来额外的开销
            m_PhraseRecognizer.Dispose();
        }
#endif
    }

    /// <summary>
    /// 识别到语音的操作
    /// </summary>
    void SpeechRecognition(string key)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        //Game.Instance.Loader.Speech(key);
#endif
    }
}
