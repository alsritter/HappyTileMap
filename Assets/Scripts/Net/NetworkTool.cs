using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace AlsRitter.Net
{
    public class NetworkTool
    {
        private const string Url = "http://localhost:8683";

        public delegate void RequestCallback(string result);

        /// <summary>
        /// Get 请求
        /// </summary>
        /// <param name="path">请求路径</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IEnumerator GetRequest(string path, RequestCallback callback)
        {
            using (var webRequest = UnityWebRequest.Get(Url + path))
            {
                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Post 请求，发送表单
        /// </summary>
        /// <param name="path">请求地址</param>
        /// <param name="requestFrom">请求的表单</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IEnumerator PostFormRequest(
            string path,
            Dictionary<string, string> requestFrom,
            RequestCallback callback)
        {
            using (var webRequest = UnityWebRequest.Post(Url + path, requestFrom))
            {
                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Post 请求，发送 JSON
        /// </summary>
        /// <param name="path">请求地址</param>
        /// <param name="json">发送的 json 数据</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IEnumerator PostJsonRequest(
            string path,
            string json,
            RequestCallback callback)
        {
            // 默认的 POST 请求是 application/x-www-form-urlencoded 所以这里不能直接使用 POST 方法
            using (var webRequest = new UnityWebRequest(Url + path, UnityWebRequest.kHttpVerbPOST))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }
    }
}