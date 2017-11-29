using BestHTTP;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace REST
{
    public class RESTImpl : MonoBehaviour, IREST
    {
        public string Host { get; set; }

        public string AccessToken { get; set; }

        public string UserID { get; set; }

        public bool IsLoggedIn { get; private set; }

        public bool IsInitialized { get; private set; }

        public TimeSpan RequestTimeout { get; set; }

        private Action<Result> LoginCallback;
        private Action<Result> LogoutCallback;

        private void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
        }

        public void Init(Action callback)
        {
            HTTPManager.IsCachingDisabled = true;
            HTTPManager.ConnectTimeout = TimeSpan.FromSeconds(10);
            HTTPManager.RequestTimeout = RequestTimeout;
            this.IsInitialized = true;
            if (null != callback)
            {
                callback.Invoke();
            }
        }

        public void Login(Action<Result> callback, Dictionary<string, string> formData)
        {
            LoginCallback = callback;

            this.Api(
            query: "member/info",
            method: HTTPMethod.GET,
            callback: (result) =>
            {
                if (LoginCallback != null)
                    LoginCallback.Invoke(result);
            },
            formData: formData
            );
        }

        public void Logout(Action<Result> callback)
        {
            LogoutCallback = callback;

            this.Api(
            query: "user/info/" + this.AccessToken,
            method: HTTPMethod.DELETE,
            callback: (result) =>
            {
                if (LogoutCallback != null)
                    LogoutCallback.Invoke(result);
            },
            formData: null
            );
        }

        private HTTPRequest RequestCreate(Uri uri, HTTPMethods method, OnRequestFinishedDelegate callback)
        {
            HTTPRequest request = new HTTPRequest(
                uri: uri,
                methodType: method,
                isKeepAlive: true,
                disableCache: true,
                callback: callback
                );

            request.AddHeader("Authorization", this.AccessToken);
            request.AddHeader("UserID", this.UserID);
            return request;
        }

        private HTTPRequest RequestCreateToJson(Uri uri, HTTPMethods method, OnRequestFinishedDelegate callback, string jstring)
        {
            HTTPRequest request = new HTTPRequest(
                uri: uri,
                methodType: method,
                isKeepAlive: true,
                disableCache: true,
                callback: callback
                );

            request.RawData = ConvertToByteArray(jstring, Encoding.UTF8);
            request.AddHeader("UserID", this.UserID);
            request.AddHeader("Authorization", this.AccessToken);
            request.AddHeader("content-type", "application/json");
            return request;
        }

        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        private void Put(string query, Action<Result> callback, Dictionary<string, string> formData)
        {
            var endpoint = QueryBuilder.Create(this.Host, query, formData);
            HTTPRequest request;
            request = RequestCreate(endpoint, HTTPMethods.Put, (req, res) =>
            {
                if (callback != null)
                {
                    callback.Invoke(new Result(res));
                }
            });
            request.Send();
        }

        private void Post(string query, Action<Result> callback, Dictionary<string, string> formData)
        {
            var endpoint = QueryBuilder.Create(this.Host, query, null);
            HTTPRequest request;
            request = RequestCreate(endpoint, HTTPMethods.Post, (req, res) =>
            {
                if (callback != null)
                {
                    callback.Invoke(new Result(res));
                }
            });

            if (formData != null)
            {
                foreach (var pair in formData)
                {
                    request.AddField(pair.Key, pair.Value);
                }
            }

            request.Send();
        }

        private void Get(string query, Action<Result> callback, Dictionary<string, string> formData)
        {
            var endpoint = QueryBuilder.Create(this.Host, query, formData);
            HTTPRequest request;
            request = RequestCreate(endpoint, HTTPMethods.Get, (req, res) =>
            {
                if (callback != null)
                {
                    callback.Invoke(new Result(req.Response));
                }
            });
            request.Send();
        }

        private void Delete(string query, Action<Result> callback, Dictionary<string, string> formData)
        {
            var endpoint = QueryBuilder.Create(this.Host, query, formData);
            HTTPRequest request;
            request = RequestCreate(endpoint, HTTPMethods.Delete, (req, res) =>
            {
                if (callback != null)
                {
                    callback.Invoke(new Result(res));
                }
            });
            request.Send();
        }

        public void Api(string query, HTTPMethod method, Action<Result> callback, Dictionary<string, string> formData)
        {
            switch ((HTTPMethods)method)
            {
                case HTTPMethods.Post:	// Create
                    this.Post(query, callback, formData);
                    break;

                case HTTPMethods.Get:	// Read
                    this.Get(query, callback, formData);
                    break;

                case HTTPMethods.Put:	// Update
                    this.Put(query, callback, formData);
                    break;

                case HTTPMethods.Delete:
                    this.Delete(query, callback, formData);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }

        public void JsonApi(string query, string jsonString, Action<Result> callback)
        {
            var endpoint = QueryBuilder.Create(this.Host, query, null);
            HTTPRequest request;
            request = RequestCreateToJson(endpoint, HTTPMethods.Put, (req, res) =>
            {
                if (callback != null)
                {
                    callback.Invoke(new Result(res));
                }
            }, jsonString);
            request.Send();
        }
    }
}