using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(CircuitData))]
public class CircuitDataInspector : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		if (position.height > 16f)
		{
			position.height = 16f;
			EditorGUI.indentLevel += 1;
			contentPosition = EditorGUI.IndentedRect(position);
			contentPosition.y += 18f;
		}

		contentPosition.width *= 0.5f;
		EditorGUI.indentLevel = 0;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("PositionIndex"), GUIContent.none);
		contentPosition.x += contentPosition.width + 4;
		contentPosition.width /= 1f;
		EditorGUIUtility.labelWidth = 64f;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("RequiredConnections"), new GUIContent("Reqd"));
		EditorGUI.EndProperty();

	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float extraHeight = 18f;
		return base.GetPropertyHeight(property, label) + extraHeight;
	}
}
