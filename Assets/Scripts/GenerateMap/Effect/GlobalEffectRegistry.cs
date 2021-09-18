using System.Collections.Generic;
using AlsRitter.Utilities;
using AlsRitter.V3.CustomTileFrame.TileEffect.PhysicsEffects;
using AlsRitter.V3.CustomTileFrame.TileEffect.SpecialEffects;
using UnityEngine;


namespace AlsRitter.V3.CustomTileFrame.TileEffect {
    public class GlobalEffectRegistry : Singleton<GlobalEffectRegistry> {
        private readonly Dictionary<string, IBaseEffect> allEffect;


        /// <summary>
        /// 静态代码块，负责把全部效果注册进来
        /// </summary>
        public GlobalEffectRegistry() {
            allEffect = new Dictionary<string, IBaseEffect>();
            RegistrySpecial();
            RegistryPhysics();
        }

        /// <summary>
        /// 用于负责注册特殊效果
        /// </summary>
        private void RegistrySpecial() {
            // 返回一个空效果
            allEffect.Add("00000", new EmptyEffect());
            // 胜利
            allEffect.Add("99999", new GameVictoryEffect());
        }


        /// <summary>
        /// 用于负责注册物理相关的效果
        /// </summary>
        private void RegistryPhysics() {
            // 向左移动的传送带
            allEffect.Add("00001", new ConveyorEffect(true));
            // 向右移动的传送带
            allEffect.Add("00002", new ConveyorEffect(false));
            // 蹦床效果
            allEffect.Add("00003", new TrampolineEffect(20f));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public IBaseEffect GetEffect(string key) {
            // 判断是否存在，不存在则返回空效果
            if (allEffect.ContainsKey(key)) {
                return allEffect[key];
            }
            else {
                Debug.LogWarning($"The effect was not found, Please check the id {key}");
                return allEffect["00000"];
            }
        }
    }
}