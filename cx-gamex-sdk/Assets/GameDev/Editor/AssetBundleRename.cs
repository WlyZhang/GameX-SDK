using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace SH.AssetBundleRename
{
    /*
    * 将现有的AB资源重新命名，保持原有Bundle名字 ，前面增加了分类路径
    */
    #region Bundle 资源重新命名

    public class AssetBundleRename
    {
       
        const string AnimationClip = "animationClip";
        const string Audio       = "audio";
        const string Image       = "image";
        const string Model       = "model";
        const string Scene       = "scene";
        const string Video       = "video";
        const string Effect      = "effect";
        const string Config      = "config";
        const string UIPanel     = "uiPanel";
        const string UIAtlas     = "uiAtlas";
        const string Share       = "share";
        const string Spectrum    = "spectrum";
        const string UIAtlasInfo   = "uiAtlasInfo";
        const string RunAnimaControll = "runAnimaControll";
        const string DollCard = "dollcard";
        const string SoundPreview = "soundPreview";


        #region 资源单个打包
        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + DollCard)]
        static void RenameDollCard()
        {
            ReNmaeAsset(DollCard); 
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Audio)]
        static void RenameAudio()
        {
            ReNmaeAsset(Audio);
        }


        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Image)]
        static void RenameImage()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(Image);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + RunAnimaControll)]
        static void RenameAnimation()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(RunAnimaControll);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Model)]
        static void RenameModle()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(Model);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Scene)]
        static void RenameScene()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(Scene);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Video)]
        static void RenameVideo()
        {
            ReNmaeAsset(Video);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Effect)]
        static void RenameEffect()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(Effect);
        }


        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + UIPanel)]
        static void RenameUIPanel()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(UIPanel);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + UIAtlas)]
        static void RenameUIAtlas()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(UIAtlas);
        }


        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Share)]
        static void RenameShare()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(Share);
        }


        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + AnimationClip)]
        static void RenameAnimationClip()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(AnimationClip);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + Config)]
        static void RenameConfigData()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset(Config);
        }

        [MenuItem("Assets/AssetBundleRename_SingleAsset/" + "Material")]
        static void RenameMaterialData()
        {
            Object[] objs = Selection.objects;

            ReNmaeAsset("Material");
        }

        #endregion

        #region 资源打进整包

        [MenuItem("Assets/AssetBundleRename_Bundle/" + "Texture")]
        static void RenameAnimationTexture()
        {
            Object[] objs = Selection.objects;

            ReNameBundle("Texture");
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + "Mat")]
        static void RenameAnimationMat()
        {
            Object[] objs = Selection.objects;

            ReNameBundle("material/mat");
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + "Font")]
        static void RenameAnimationFont()
        {
            Object[] objs = Selection.objects;

            ReNameBundle("/font/Font");
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + Config)]
        static void RenameConfig()
        {
            Object[] objs = Selection.objects;

            ReNameBundle("config/json");
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + UIAtlasInfo)]
        static void RenameAtlasInfo()
        {
            Object[] objs = Selection.objects;

            ReNameBundle(string.Format("{0}/{1}", UIAtlasInfo, "atlasInfo"));
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + "FaceAnimation")]
        static void RenameAnimationClipBundle()
        {
            Object[] objs = Selection.objects;

            ReNameBundle("AnimationBundle/FaceAnimation");
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + "UIBundle")]
        static void RenameUIBundle()
        {
            Object[] objs = Selection.objects;

            ReNameBundle("UI/UIBundle");
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + "Shader")]
        static void ShaderBundle()
        {
            ReNameBundle("shader/shader");
        }


        [MenuItem("Assets/AssetBundleRename_Bundle/" + "Matrilal")]
        static void MaterialBundle()
        {
            ReNameBundle("material/material"); 
        }

        [MenuItem("Assets/AssetBundleRename_Bundle/" + SoundPreview)]
        static void RenameSoundPreviewBundle()
        {
            ReNameBundle("sound/"+SoundPreview);
        }

        #endregion

        static void ReNmaeAsset(string dir)
        {
            Object[] objs = Selection.objects;

            int totalLength = objs.Length;
            int index = 0;
            float progress = 0f;

            foreach (var v in objs)
            {
                index++;
                progress = (float)index / (float)totalLength;
                EditorUtility.DisplayProgressBar("资源命名", "请稍等..." + index.ToString() + "/" + totalLength.ToString(), progress);

                string fileName;
                string path = AssetDatabase.GetAssetPath(v);
                AssetImporter imp = AssetImporter.GetAtPath(path);

                //if (imp.assetBundleName != string.Empty)
                //{
                //    string bundleName = imp.assetBundleName.TrimStart(' ');
                //    bundleName = bundleName.TrimEnd(' ');
                //    string[] splitName = bundleName.Split('/');

                //    if (splitName.Length > 1)
                //    {
                //        fileName = splitName[splitName.Length - 1];
                //    }
                //    else
                //    {
                //        fileName = imp.assetBundleName;
                //    }
                //}
                //else
                //{
                //    fileName = v.name;
                //}

                imp.assetBundleName = $"{dir}/{v.name.Trim()}";

                imp.SaveAndReimport();
            }

            UnityEditor.EditorUtility.ClearProgressBar();
        }

        static void ReNameBundle(string dir)
        {
            Object[] objs = Selection.objects;

            int totalLength = objs.Length;
            int index = 0;
            float progress = 0f;

            foreach (var v in objs)
            {
                index++;
                progress = (float)index / (float)totalLength;
                EditorUtility.DisplayProgressBar("资源命名", "请稍等..." + index.ToString() + "/" + totalLength.ToString(), progress);

                string path = AssetDatabase.GetAssetPath(v);
                AssetImporter imp = AssetImporter.GetAtPath(path);

                imp.assetBundleName = $"{dir.Trim()}";

                imp.SaveAndReimport();
            }

            AssetDatabase.Refresh();

            UnityEditor.EditorUtility.ClearProgressBar();
        }
    }

    #endregion

    /*
    public class AssetBundle_Sound_Rename : EditorWindow
    {
        static string temppath;
        [MenuItem("Tools/剧情对话音频命名")]
        static void Rename()
        {
            List<string> chapterpath = new List<string>();//所有章节路径
            List<string> fullpath = new List<string>();//临时全路径
            List<Object> tempobjects = new List<Object>();
            Object[] objects;
            string packagePath = UnityEditor.EditorUtility.OpenFolderPanel("Select Package Path", UnityEngine.Application.dataPath, "");
            string str = packagePath;
            if (str != "")
            {
                temppath = str;
                if (!System.IO.Directory.Exists(temppath + "/101"))
                {
                    Debug.LogError("不存在" + temppath + "101");
                    Debug.LogError("请选择art文件夹下的Design/Audio/Story");
                }
                else
                {
                    Debug.LogError("路径选择正确");
                    string[] dirs = System.IO.Directory.GetFileSystemEntries(temppath);
                    Debug.LogError("文件夹个数"+dirs.Length);

                    chapterpath.Clear();
                    //获取所有文件夹以及路径
                    foreach (string subdir in Directory.GetDirectories(temppath))
                    {
                        chapterpath.Add(subdir);
                        Debug.LogError(subdir);
                    }
                    for (int i = 0; i < chapterpath.Count; i++)
                    {
                        //获取文件信息
                        DirectoryInfo direction = new DirectoryInfo(chapterpath[i]);

                        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

                        for (int j = 0; j < files.Length; j++)
                        {
                            //过滤掉临时文件
                            if (files[j].Name.EndsWith(".meta"))
                            {
                                continue;
                            }
                            string tempstr= files[j].Directory.ToString() + "/" + files[j].Name;
                            string[] sArray = tempstr.Split(new string[] { "Assets" }, System.StringSplitOptions.RemoveEmptyEntries);
                            
                            fullpath.Add("Assets" + sArray[1]);
                            Debug.LogError("Assets" + sArray[1]);
                        }
                        Debug.LogError("最终长度" + fullpath.Count);
                        for (int j = 0; j < fullpath.Count; j++)
                        {
                            tempobjects.Add(AssetDatabase.LoadAssetAtPath(@fullpath[j], typeof(Object)) as Object);
                        }
                    fullpath.Clear();
                    Debug.LogError("长度" + chapterpath.Count);
                    Debug.LogError(chapterpath[i].Substring(chapterpath[i].Length - 3));
                    ReNmaeAssetsound("StorySound", chapterpath[i].Substring(chapterpath[i].Length - 3),tempobjects); 
                        tempobjects.Clear();
                    }
                }
            }
            else
            {
                Debug.LogError("请选择正确的路径");
            }
        }

        static void ReNmaeAssetsound(string dir,string name, List<Object> objects)
        {
            Debug.LogError(@dir+"/"+ name);
            List<Object> objs = objects;
            int totalLength = objs.Count;
            int index = 0;
            float progress = 0f;

            foreach (var v in objs)
            {
                index++;
                progress = (float)index / (float)totalLength;
                EditorUtility.DisplayProgressBar("资源命名", "请稍等..." + index.ToString() + "/" + totalLength.ToString(), progress);

                string path = AssetDatabase.GetAssetPath(v);
                AssetImporter imp = AssetImporter.GetAtPath(path);
                imp.assetBundleName = dir + "/" + name;

                imp.SaveAndReimport();
            }

            UnityEditor.EditorUtility.ClearProgressBar();
        }
    }

    */
}
