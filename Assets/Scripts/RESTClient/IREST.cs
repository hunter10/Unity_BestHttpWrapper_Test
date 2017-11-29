using System;
using System.Net;
using System.Collections.Generic;

namespace REST
{
    public interface IREST
    {
        string Host { get; set; }

        string AccessToken { get; set; }

        string UserID { get; set; }

        TimeSpan RequestTimeout { get; set; }

        bool IsLoggedIn { get; }

        bool IsInitialized { get; }

        void Init(Action callback);

        void Login(Action<Result> callback, Dictionary<string, string> formData);

        void Logout(Action<Result> callback);

        void Api(string query, HTTPMethod method, Action<Result> callback, Dictionary<string, string> formData);

        void JsonApi(string query, string jsonString, Action<Result> callback);
    }
}
