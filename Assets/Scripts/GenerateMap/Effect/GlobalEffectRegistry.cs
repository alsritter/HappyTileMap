﻿using System.Collections;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame.TileEffect;
using AlsRitter.GenerateMap.CustomTileFrame.TileEffect.PhysicsEffects;
using AlsRitter.GenerateMap.CustomTileFrame.TileEffect.SpecialEffects;
using UnityEngine;


namespace AlsRitter.GenerateMap.CustomTileFrame.TileEffect
{
    public class GlobalEffectRegistry
    {
        private static readonly Dictionary<string, BaseObjectEffect> allEffect;


        /// <summary>
        /// 静态代码块，负责把全部效果注册进来
        /// </summary>
        static GlobalEffectRegistry()
        {
            allEffect = new Dictionary<string, BaseObjectEffect>();
            RegistrySpecial();
            RegistryPhysics();
        }

        /// <summary>
        /// 用于负责注册特殊效果
        /// </summary>
        private static void RegistrySpecial()
        {
            // 返回一个空效果
            allEffect.Add("00000", new EmptyEffect());
            // 胜利
            allEffect.Add("99999", new GameVictoryEffect());
        }


        /// <summary>
        /// 用于负责注册物理相关的效果
        /// </summary>
        private static void RegistryPhysics()
        {
            // 向左移动的传送带
            allEffect.Add("00001", new ConveyorEffect(true, 15));
            // 向右移动的传送带
            allEffect.Add("00002", new ConveyorEffect(false, 15));
            // 蹦床效果
            allEffect.Add("00003", new TrampolineEffect(3));
        }

        public static BaseObjectEffect GetEffect(string key)
        {
            // 判断是否存在，不存在则返回空效果
            if (allEffect.ContainsKey(key))
            {
                return allEffect[key];
            }
            else
            {
                Debug.LogWarning($"The effect was not found, Please check the id {key}");
                return allEffect["00000"];
            }
        }
    }
}