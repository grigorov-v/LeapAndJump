using System;
using System.Reflection;
using UnityEditor;

public class ExpectNotNullValidator : IValidator
{
	public Type AttributeType => typeof(ExpectNotNullAttribute);

	public void Validate(SerializedProperty property, FieldInfo fieldInfo, Attribute attribute)
	{
		var type = fieldInfo.FieldType;
		UnityEngine.Debug.Log(type);
	}
}