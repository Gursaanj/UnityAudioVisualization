using System;
using System.Reflection;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityEngine.Object), true)]
public class ObjectPropertyViewer : PropertyDrawer
{
    protected const float WIDTH_OF_BUTTON = 20f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.width -= WIDTH_OF_BUTTON;
        EditorGUI.PropertyField(position, property, label, false);
        position.x = position.xMax;
        position.width = WIDTH_OF_BUTTON;

        if (GUI.Button(position, new GUIContent("X", "Sets value to null")))
        {
            property.objectReferenceValue = null;
        }
    }

    protected Type GetPropertyType(SerializedProperty prop)
    {
        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] stringElements = path.Split('.');
        if (stringElements.Length == 0)
        {
            return null;
        }

        for (int i = 0, len = stringElements.Length - 1; i < len; i++)
        {
            string element = stringElements[i];
            int indexOf = element.IndexOf("[");
            if (indexOf >= 0)
            {
                string stringElementName = element.Substring(0, indexOf);
                int index = Convert.ToInt32(element.Substring(indexOf).Replace("[", string.Empty).Replace("]", string.Empty));
                obj = GetValue(obj, stringElementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }

        return GetTypeOfField(obj, stringElements[stringElements.Length - 1]);

    }


    protected Type GetTypeOfField(object sourceObject, string name)
    {
        if (sourceObject == null)
        {
            return null;
        }

        Type type = sourceObject.GetType();
        FieldInfo info = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (info == null)
        {
            PropertyInfo pInfo = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (pInfo == null)
            {
                return null;
            }
            return pInfo.PropertyType;
        }

        return info.FieldType;
    }

    protected object GetValue(object sourceObject, string name)
    {
        if (sourceObject == null)
        {
            return null;
        }

        Type type = sourceObject.GetType();
        FieldInfo info = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (info == null)
        {
            PropertyInfo pInfo = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (pInfo == null)
            {
                return null;
            }
            return pInfo.GetValue(sourceObject, null);
        }

        return info.GetValue(sourceObject);
    }

    protected object GetValue(object sourceObject, string name, int index)
    {
        IEnumerable enumerable = GetValue(sourceObject, name) as IEnumerable;
        IEnumerator enumerator = enumerable.GetEnumerator();

        while (index-- >= 0)
        {
            if (!enumerator.MoveNext())
            {
                return null;
            }
        }

        return enumerator.Current;
    }
}
