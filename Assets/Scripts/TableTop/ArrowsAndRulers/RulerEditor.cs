﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR

namespace TableTop
{

    public enum OPTIONS
    {
        X = 0,
        Y = 1
    }


    [CustomEditor(typeof(Ruler))]
    public class RuleEditor : UnityEditor.Editor
    {
        private Ruler ruler;

        public OPTIONS op;

        public RulerCoordinateType type;

        [MenuItem("Examples/Editor GUILayout Popup usage")]

        void OnEnable()
        {
            this.ruler = (Ruler)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("rangeTicks"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ticksNumber"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("length"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("center"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("thickness"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("thickenssZ"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("thickLength"), true);

            OPTIONS newop = (OPTIONS)EditorGUILayout.EnumPopup("Direction:", op);

            

            if (newop != op)
            {
                op = newop;
                directionCompute();
            }

   
            if (GUILayout.Button("Generate "))
            {
                ruler.Generate();
            }

            void directionCompute()
            {
                switch (op)
                {
                    case OPTIONS.X:
                        ruler.Direction = Vector3.right;
                        break;
                    case OPTIONS.Y:
                        ruler.Direction = Vector3.forward;
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();

        }
    }
}

#endif