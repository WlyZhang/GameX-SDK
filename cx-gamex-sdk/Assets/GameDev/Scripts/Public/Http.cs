using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Http
{

    private static Http _instance = null;
    public static Http Instance => _instance;

    public static void Create()
    {
        _instance = new Http();
    }

    public async void Message(string type, string url, string key = null, string json = null, Action<string> callback = null)
    {
        if(type.Equals("post"))
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            if(!string.IsNullOrEmpty(json))
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            string res = await PostMessage(url, data);
            callback(res);
        }
        else if(type.Equals("post-str"))
        {
            string res = await PostMessage(url, key, json);
            callback(res);
        }
        else if(type.Equals("get"))
        {
            string res = await GetMessage(url);
            callback(res);
        }
    }

    public async void MessageToken(string url, int userId, string userToken, string key, string jsonData,Action<string> callback = null)
    {
        string res = await PostToken(url,userId,userToken,key,jsonData);
        callback(res);
    }

    public async void MessageForm(string url,WWWForm form,Action<string> callback)
    {
        string res = await PostForm(url,form);
        callback(res);
    }

    private async Task<string> PostToken(string url,int userId,string userToken,string key,string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("user-id", userId);
        form.AddField("user-token", userToken);

        if(!string.IsNullOrEmpty(key)&&!string.IsNullOrEmpty(jsonData))
        {
            form.AddField(key, jsonData);
        }

        WWW www = new WWW(url, form);

        while (!www.isDone)
        {
            await Task.Yield();
        }

        return www.text;
    }

    private async Task<string> PostMessage(string url, string key, string json)
    {
        WWWForm form = new WWWForm();
        form.AddField(key,json);

        WWW www = new WWW(url, form);

        while (!www.isDone)
        {
            await Task.Yield();
        }

        return www.text;
    }

    private async Task<string> PostMessage(string url, Dictionary<string, object> data)
    {
        WWWForm form = new WWWForm();

        foreach (var kv in data)
        {
            form.AddField(kv.Key, kv.Value.ToString());
        }

        WWW www = new WWW(url, form);

        while (!www.isDone)
        {
            await Task.Yield();
        }

        return www.text;
    }

    private async Task<string> PostForm(string url,WWWForm form)
    {
        WWW www = new WWW(url, form);

        while (!www.isDone)
        {
            await Task.Yield();
        }

        return www.text;
    }

    private async Task<string> GetMessage(string url)
    {

        WWW www = new WWW(url);

        while (!www.isDone)
        {
            await Task.Yield();
        }

        return www.text;
    }
}
