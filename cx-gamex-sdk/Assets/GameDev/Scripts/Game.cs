using UnityEngine;

public class Game : MonoBehaviour
{
    /// <summary>
    /// 需要使用运行的 Module 模块，切换需要更改此字段
    /// </summary>
    public static string ModuleName = "Game";


    /// <summary>
    /// 公司或机构名称
    /// </summary>
    public static string CompanyName => Application.companyName;



    /// <summary>
    /// 开发者模式SDK标识
    /// </summary>
    public static string DevelopApp => Application.productName;
}
