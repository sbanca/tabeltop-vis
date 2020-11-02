﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR

namespace TableTop
{


    [CustomEditor(typeof(Rulers))]
    public class RulersEditor : UnityEditor.Editor
    {
        private Rulers rulers;


        [MenuItem("Examples/Editor GUILayout Popup usage")]

        void OnEnable()
        {
            this.rulers = (Rulers)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("map"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("RangeticksX"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("RangeticksY"), true);


            if (GUILayout.Button("Generate "))
            {
                rulers.CreateRulers();
            }


            if (GUILayout.Button("Delete "))
            {
                rulers.DeleteRulers();
            }



            serializedObject.ApplyModifiedProperties();

        }
    }
}

#endif