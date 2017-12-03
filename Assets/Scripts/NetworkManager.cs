using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using LitJson;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using REST;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public enum CONNECT_NAME
    {
        CHECK_FRIENDS_STAGE,
    }

    public string basePath = "https://jsonplaceholder.typicode.com";


    public void Start()
    {
        if(!RESTClient.IsInitialized)
        {
            RESTClient.Init(OnInitialized);
        }
    }

    private void OnInitialized()
    {
        Debug.Log("network Init");
    }
    public void OnClick()
    {
        AutoConnectToServer(CONNECT_NAME.CHECK_FRIENDS_STAGE);
    }

    public void AutoConnectToServer(CONNECT_NAME _connectName, Dictionary<string, string> tempDic = null, GameObject target = null)
    {
        //NetworkManager.DebugLog(UserInfoManager.instance.user_No);
        //if (UserInfoManager.instance.user_No != 1000000001)
        {
            //Member member = SyncManager.instance.GetMember();
            switch (_connectName)
            {

                case CONNECT_NAME.CHECK_FRIENDS_STAGE:
                    //RESTClient.Api("friend/stage/last/info/" + "적당한값" + "/" + "적당한값", HTTPMethod.GET, (r) =>
                    //{
                    //}, tempDic);

                    string tempQuery = basePath + "/posts";
                    RESTClient.Api(tempQuery, HTTPMethod.GET, (r) =>
                    {
                        Debug.Log(r.StatusCode);
                        Debug.Log(r.Text);
                        //if (r.StatusCode == 200)
                        //{

                        //}
                        //else
                        //{

                        //}
                        
                        int a = 0;
                    }, tempDic);

                    break;
            }
        }
    }
}
