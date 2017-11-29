using BestHTTP;
using System;
using UnityEngine;

namespace REST
{
    public class Result : IDisposable
    {
        public int StatusCode { get; set; }

        public string Error { get; private set; }

        public string Text { get; private set; }

        public Texture2D Texture { get; private set; }

        public void Dispose()
        {
        }

        public Result(HTTPResponse response)
        {
            //if (response == null)
            //    throw new ArgumentNullException("response");
            if (response == null)
            {
                this.Error = "Can not connect to the network.";
                this.StatusCode = 600;
                this.Text = "{\"errCode\":\"99999\",\"errMsg\":\"Can not connect to the network\",\"errLocation\":\"\"}";
            }
            else
            {
                if (!response.IsSuccess)
                    this.Error = response.Message;
                this.StatusCode = response.StatusCode;
                this.Text = response.DataAsText;
                this.Texture = response.DataAsTexture2D;
            }
        }

        public void setText(string text)
        {
            this.Text = text;
        }
    }
}