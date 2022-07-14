using System;
using UnityEditor;
using UnityEngine;

namespace Track.Editor
{
    [CustomEditor(typeof(Mesher))]
    class RoadTool : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
        
            base.OnInspectorGUI();
        
            if (EditorGUI.EndChangeCheck())
                ((Mesher)target).Loft();
        }
    }
}
