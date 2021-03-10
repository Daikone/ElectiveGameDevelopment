using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Resources.Scripts.Matti_AI.Editor
{
    
    [CustomEditor(typeof(State), true)]
    public class MattiStateEditor : UnityEditor.Editor
    {
        
        private List<SerializedProperty> properties;

        private void OnEnable()
        {
            string[] hiddenProperties = new string[]{"gm", "agent","humanLayerCheck", "idleState", "chaseState"}; //fields you do not want to show go here
            properties = EditorHelper.GetExposedProperties(this.serializedObject,hiddenProperties);
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI(); - (standard way to draw base inspector)

            //We draw only the properties we want to display here
            foreach (SerializedProperty property in properties)
            {
                EditorGUILayout.PropertyField(property,true);
            }
            serializedObject.ApplyModifiedProperties();
        }
            
    }
}
