using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AlsRitter.CallWeb {
    public class UnityToWeb : MonoBehaviour {
        //引入方法
        [DllImport("__Internal")]
        private static extern string getMapData();
        
        [DllImport("__Internal")]
        private static extern string ReportReady();


        private void Start() {
            ReportReady();
        }

        /**
         * 取得数据
         */
        public string GetJsonData() {
            var map = getMapData();
            Debug.Log(map);
            return map;
        }

        /**
         * 测试前端传数据进来
         */
        public void SetData(string data) {
            // Debug.Log(data);
        }
    }
}