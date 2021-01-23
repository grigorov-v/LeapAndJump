using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExpectNotNullValidator : IValidator {
	public Type AttributeType => typeof(ExpectNotNullAttribute);

	public void Validate(SerializedProperty property, FieldInfo fieldInfo, Attribute attribute) {
		var type = fieldInfo.FieldType;
		Debug.Log(type);
	}
}