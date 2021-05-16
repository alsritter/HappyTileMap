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
        /// 这个 DownloadHandler 是响应里面的 body
        /// </summary>
        /// <param name="result"></param>
        public delegate void RequestSpriteCallback(Sprite result);

        /// <summary>
        /// Get 请求
        /// TODO: 这里未来需要添加一个 JWT 的
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

                    Debug.LogError(webRequest.error + path);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Get 请求
        /// </summary>
        /// <param name="fullPath">全路径</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IEnumerator GetRequestByFullPath(string fullPath, RequestCallback callback)
        {
            using (var webRequest = UnityWebRequest.Get(fullPath))
            {
                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.LogError(webRequest.error + fullPath);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// 用于下载图片
        /// </summary>
        /// <param name="fullPath">填入完整路径，不像其它的可以省略域名</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator GetSpriteRequest(string fullPath, RequestSpriteCallback callback)
        {
            using (var webRequest = new UnityWebRequest(fullPath))
            {
                var texDl = new DownloadHandlerTexture(true);
                webRequest.downloadHandler = texDl;
                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.LogError(webRequest.error + fullPath);
                }
                else
                {
                    // var imgWidth = texDl.texture.width;
                    // var imgHeight = texDl.texture.height;
                    // Debug.Log($"宽度：{imgWidth}  高度: {imgHeight}");
                    var tex = texDl.texture;
                    var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                    callback(sprite);
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
                    Debug.LogError(webRequest.error + path);
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
                    Debug.LogError(webRequest.error + path);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }
    }
}