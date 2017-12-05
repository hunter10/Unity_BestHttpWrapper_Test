using System;
using System.Collections.Generic;
using UnityEngine;
using REST;

public sealed class RESTClient : ScriptableObject
{
    private static Action OnInitialized;
    private static IREST _impl;
    private static bool _isInitCalled = false;
    private static string _accessToken;
    private static string _userId;

    private static IREST Impl
    {
        get
        {
            return _impl;
        }

        set
        {
            if (value == null)
            {
                throw new NullReferenceException("REST object is not yet loaded.  Did you call REST.Init()?");
            }
            _impl = value;
        }
    }

    public static string AccessToken
    {
        get
        {
            return (_impl != null) ? _impl.AccessToken : string.Empty;
        }
        set
        {
            if (value != null) _impl.AccessToken = value;
        }
    }

    public static string UserID
    {
        get
        {
            return (_impl != null) ? _impl.UserID : string.Empty;
        }
        set
        {
            _impl.UserID = value;
        }
    }

    public static bool IsLoggedIn
    {
        get
        {
            return (_impl != null) && _impl.IsLoggedIn;
        }
    }

    public static bool IsInitialized
    {
        get
        {
            return (_impl != null) && _impl.IsInitialized;
        }
    }

    public static TimeSpan RequestTimeout
    {
        get
        {
            return (_impl != null) ? _impl.RequestTimeout : TimeSpan.FromSeconds(60);
        }
        set
        {
            if (value != null) _impl.RequestTimeout = value;
        }
    }

    public static void Api(string query, HTTPMethod method, Action<Result> callback, Dictionary<string, string> formData)
    {
        if (IsInitialized)
        {
            //NetworkManager.DebugLog("HOST : " + Impl.Host);
            Debug.Log("HOST : " + Impl.Host);
            _impl.Api(query, method, callback, formData);
        }
    }

    public static void JsonApi(string query, string jsonString, Action<Result> callback)
    {
        if (IsInitialized)
        {
            _impl.JsonApi(query, jsonString, callback);
        }
    }

    public static void Init(Action callback)
    {
        if (!_isInitCalled)
        {
            OnInitialized += callback;
            ComponentFactory.GetComponent<RESTClientLoader>();
            _isInitCalled = true;
            return;
        }

        Debug.LogWarning("RESTClient.Init() has already been called.  You only need to call this once and only once.");

        // Init again if possible just in case something bad actually happened.
        if (Impl != null)
        {
            OnDllLoaded();
        }
    }

    public static void Login(Action<Result> callback, Dictionary<string, string> formData)
    {
        if (_impl != null)
        {
            _impl.Login(callback, formData);
        }

    }

    public static void Logout(Action<Result> callback)
    {
        if (_impl != null)
        {
            _impl.Logout(callback);
        }
    }

    private static void OnDllLoaded()
    {
        Impl.Init(OnInitialized);
    }

    public class RESTClientLoader : MonoBehaviour
    {
        protected IREST RestClient
        {
            get { return ComponentFactory.GetComponent<RESTImpl>(); }
        }

        private void Start()
        {
            RESTClient.Impl = RestClient;
            //if (GameInfoManager.Instance.isStageSever)
            {
                //GameInfoManager.Instance.isDevServer = false;
                //Impl.Host = "https://127.0.0.1:8010";
            }
            //else if (GameInfoManager.Instance.isDevServer)
            {
                //Impl.Host = "http://127.0.0.1:8010/";
            }

            Impl.Host = "https://jsonplaceholder.typicode.com";

            //NetworkManager.DebugLog("HOST : " + Impl.Host);
            Impl.UserID = "1400000002";
            Impl.AccessToken = "unitytest";
            Impl.RequestTimeout = TimeSpan.FromSeconds(20);
            RESTClient.OnDllLoaded();
            Destroy(this);
        }
    }
}
