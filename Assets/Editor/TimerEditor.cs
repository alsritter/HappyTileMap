using AlsRitter.Utilities;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 计时器 编辑器
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(Timer))]
public class TimerEditor : Editor {

    public override void OnInspectorGUI() {
        Timer script = (Timer)target;

        // 重绘GUI
        EditorGUI.BeginChangeCheck();

        // 公开属性
        drawProperty("delay", "延迟时间(秒)");
        drawProperty("interval", "间隔时间(秒)");
        drawProperty("repeatCount", "重复次数");
        if (script.repeatCount <= 0) EditorGUILayout.LabelField(" ", "<=0 时无限重复", GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginHorizontal();
        drawProperty("autoStart", "自动计时");
        drawProperty("autoDestory", "自动销毁");
        EditorGUILayout.EndHorizontal();

        // 只读属性
        GUI.enabled = false;
        drawProperty("currentTime", "当前时间(秒)");
        drawProperty("currentCount", "当前次数");
        GUI.enabled = true;

        // 回调事件
        drawProperty("onIntervalEvent", "计时间隔事件");
        drawProperty("onCompleteEvent", "计时完成事件");
        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
    }

    private void drawProperty(string property, string label) {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label), true);
    }

}
