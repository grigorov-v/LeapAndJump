using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class Validator {
	public static void Validate(IEnumerable<Object> objects) {
		foreach ( var obj in objects ) {
			Validate(obj);
		}
	}

	public static void Validate(Object obj) {
		var serializedObject = new SerializedObject(obj);
		Validate(serializedObject);
	}

	static void Validate(SerializedObject serializedObject) {
		var targetObjectType = serializedObject.targetObject.GetType();
		var property = serializedObject.GetIterator();
		while ( property.NextVisible(true) ) {
			var field = GetFieldInfo(property);
			if ( field == null ) {
				continue;
			}

			var attributes = field.CustomAttributes;
			foreach ( var attr in attributes ) {
				var validators = ValidatorBox.Validators.FindAll(v => v.AttributeType == attr.AttributeType);
				var attribute = field.GetCustomAttribute(attr.AttributeType);
				if ( attribute == null ) {
					continue;
				}

				validators.ForEach(v => v.Validate(property, field, attribute));
			}
		}
	}

	static FieldInfo GetFieldInfo(SerializedProperty property) {
		var parentType = property.serializedObject.targetObject.GetType();
		var fieldInfo = parentType.GetField(property.propertyPath,
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		return fieldInfo;
	}

	[MenuItem("Validator/ValidateSelections")]
	static void TestValidate() {
		foreach ( var go in Selection.gameObjects ) {
			Validate(go.GetComponentsInChildren<MonoBehaviour>());
		}
	}
}