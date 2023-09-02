using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;


public class UIPanelTool : MonoBehaviour
{
    private Dictionary<string, List<string>> pathList = new Dictionary<string, List<string>>();
    
    private const string buttonKey = "Button";
    private const string inputfieldKey = "InputField";
    private const string textKey = "Text";
    private const string imageKey = "Image";


    private string luaCode = string.Empty;


    [Button("创建Lua组件")]
    [GUIColor(0,1,0)]
    public void GenLuaComponent()
    {
        Debug.Log($"<color=green>{gameObject.name}: 创建Lua组件</color>");

        GenLuaCode();

        Debug.Log(luaCode);


        string path = EditorUtility.SaveFilePanel("创建Lua组件",Application.dataPath+"/", gameObject.name, "lua");
        File.WriteAllText(path, luaCode);

        luaCode = string.Empty;
        pathList.Clear();
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 整合Lua代码
    /// </summary>
    private void GenLuaCode()
    {
        //Lua框架设定
        string luaName = gameObject.name;
        string luaUnity = "unity";
        string luaPanel = "panel";
        string panel = $"{luaName}.{luaPanel}";
        string onClickFuncName = $"{luaName}.OnClickCallback";

        luaCode = "\n" + luaName + " = {}\n\n";
        luaCode += $"function {luaName}:Create()\n\n";
        luaCode += $"\t{panel} = UIManager:OpenAndCloseOther('{luaName}')\n\n";


        #region InputField 代码生成
        //InputField
        List<InputField> inputFieldList = GetInputFieldList();
        FindInputFieldPath(inputFieldList);

        List<string> tempInputFieldList = pathList["InputField"];
        for (int i = 0; i < tempInputFieldList.Count; i++)
        {
            luaCode += $"\t{luaName}.{inputFieldList[i].name} = {panel}.transform:Find('{tempInputFieldList[i]}'):GetComponent('InputField')\n\n";
        }

        #endregion
        
        #region Button 代码生成
        //InputField
        List<Button> buttonList = GetButtonList();
        FindButtonPath(buttonList);

        List<string> tempButtonList = pathList["Button"];
        for (int i = 0; i < tempButtonList.Count; i++)
        {
            luaCode += $"\t{luaName}.{buttonList[i].name} = {panel}.transform:Find('{tempButtonList[i]}'):GetComponent('Button')\n\n";
        }

        #endregion

        luaCode += "end\n\n";

        #region Button 点击事件代码生成
        luaCode += $"function {onClickFuncName}()\n\n";

        for (int i = 0; i < buttonList.Count; i++)
        {
            luaCode += $"\t--Button: {buttonList[i].name} 点击事件\n";
            luaCode += $"\t{luaName}.{buttonList[i].name}.onClick:AddListener(function()\n\t\t\n";
            luaCode += "\tend)\n\n";
        }
        #endregion

        luaCode += "end\n\n";
        luaCode += $"\n\nreturn {luaName}";
    }

    #region 按钮组件
    /// <summary>
    /// 获取按钮列表
    /// </summary>
    private List<Button> GetButtonList()
    {
        if (!pathList.ContainsKey(buttonKey))
        {
            pathList.Add(buttonKey, new List<string>());
        }

        Button[] btns = transform.GetComponentsInChildren<Button>();
        List<Button> list = new List<Button>(btns);

        return list;
    }

    /// <summary>
    /// 查找按钮路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private void FindButtonPath(List<Button> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform child = list[i].transform;
            string path = string.Empty;

            GetParent<Button>(path, child);
        }
    }
    #endregion

    #region 输入框组件
    /// <summary>
    /// 获取输入框列表
    /// </summary>
    private List<InputField> GetInputFieldList()
    {
        if (!pathList.ContainsKey(inputfieldKey))
        {
            pathList.Add(inputfieldKey, new List<string>());
        }

        InputField[] inf = transform.GetComponentsInChildren<InputField>();
        List<InputField> list = new List<InputField>(inf);

        return list;
    }

    /// <summary>
    /// 查找输入框路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private void FindInputFieldPath(List<InputField> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform child = list[i].transform;
            string path = string.Empty;

            GetParent<InputField>(path, child);
        }
    }
    #endregion

    #region 文本组件
    /// <summary>
    /// 获取按钮列表
    /// </summary>
    private List<Text> GetTextList()
    {
        if (!pathList.ContainsKey(textKey))
        {
            pathList.Add(textKey, new List<string>());
        }

        Text[] txts = transform.GetComponentsInChildren<Text>();
        List<Text> list = new List<Text>(txts);

        return list;
    }

    /// <summary>
    /// 查找按钮路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private void FindTextPath(List<Text> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform child = list[i].transform;
            string path = string.Empty;

            GetParent<Text>(path, child);
        }
    }
    #endregion


    #region 图片组件
    /// <summary>
    /// 获取按钮列表
    /// </summary>
    private List<Image> GetImageList()
    {
        if (!pathList.ContainsKey(imageKey))
        {
            pathList.Add(imageKey, new List<string>());
        }

        Image[] imgs = transform.GetComponentsInChildren<Image>();
        List<Image> list = new List<Image>(imgs);

        return list;
    }

    /// <summary>
    /// 查找按钮路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private void FindImagePath(List<Image> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform child = list[i].transform;
            string path = string.Empty;

            GetParent<Image>(path, child);
        }
    }
    #endregion




    #region 公用函数
    /// <summary>
    /// 获取父对象路径
    /// </summary>
    /// <param name="key"></param>
    /// <param name="path"></param>
    /// <param name="child"></param>
    private void GetParent<T>(string path, Transform child)
    {
        if (child.parent.Equals(transform.parent))
        {
            string key = typeof(T).Name;
            pathList[key].Add(path);
        }
        else
        {
            if(string.IsNullOrEmpty(path))
            {
                path = $"{child.name}";
            }
            else
            {
                path = $"{child.name}/{path}";
            }
            
            GetParent<T>(path, child.parent);
        }
    }
    #endregion

}
#endif