using System;
using System.Collections.Generic;
using System.Text;

namespace REST
{
    public static class QueryBuilder
    {
        public static Uri Create(string host, string query, Dictionary<string, string> formData)
        {
            var builder = new StringBuilder();

            builder.Append(host);
            if (builder[builder.Length - 1] != '/') builder.Append("/");
            builder.Append(query);

            if (formData != null)
            {
                builder.Append("?");
                foreach (var iterator in formData)
                {
                    //UnityEngine.Debug.Log("formData key : " + iterator.Key + " value : " + iterator.Value);
                    builder.AppendFormat("{0}={1}", iterator.Key, iterator.Value);
                    builder.Append("&");
                }
                builder.Remove(builder.Length - 1, 1);
            }
            return new Uri(builder.ToString());
        }
    }
}