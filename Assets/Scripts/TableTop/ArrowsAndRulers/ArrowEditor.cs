﻿using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace TableTop
{

    public enum OPTIONSSIGN
    {
        positive = 1,
        negative = -1
    }


    [CustomEditor(typeof(Arrow))]
    public class ArrowEditor : UnityEditor.Editor
    {
        private Arrow arrow;

        public OPTIONS op;

        public OPTIONSSIGN sign = OPTIONSSIGN.positive;

        [MenuItem("Examples/Editor GUILayout Popup usage")]

        void OnEnable()
        {
            this.arrow = (Arrow)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();


            EditorGUILayout.PropertyField(serializedObject.FindProperty("thickness"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("thickenssZ"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("thickLength"), true);

            OPTIONS newop = (OPTIONS)EditorGUILayout.EnumPopup("Direction:", op);

            if (newop != op)
            {
                op = newop;
                directionCompute();
            }

            OPTIONSSIGN newsign = (OPTIONSSIGN)EditorGUILayout.EnumPopup("Sign:", sign);

            if (newsign != sign)
            {
                sign = newsign;
                directionCompute();
            }

            if (GUILayout.Button("generate "))
            {
                arrow.Generate();
            }

            void directionCompute()
            {
                switch (op)
                {
                    case OPTIONS.X:
                        arrow.Direction = Vector3.right * (int)sign;
                        break;
                    case OPTIONS.Y:
                        arrow.Direction = Vector3.forward * (int)sign;
                        break;
                }
            }


            serializedObject.ApplyModifiedProperties();

        }
    }


}
#endif