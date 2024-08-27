using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Watermelon.SquadShooter
{
    public class RoomPresetSaveWindow : WatermelonWindow
    {
        private static RoomPresetSaveWindow window;
        private Action<string> calback;
        private string presetName;

        public static void CreateRoomPresetSaveWindow(Action<string> calback)
        {
            window = (RoomPresetSaveWindow)GetWindow(typeof(RoomPresetSaveWindow));
            window.minSize = new Vector2(300, 56);
            window.maxSize = new Vector2(700, 56);
            window.calback = calback;
            window.ShowPopup();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            presetName = EditorGUILayout.TextField("Preset name:", presetName);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Cancel", EditorStylesExtended.button_03))
            {
                window.Close();
            }

            if (GUILayout.Button("Save", EditorStylesExtended.button_02))
            {
                calback?.Invoke(presetName);
                window.Close();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

    }
}
