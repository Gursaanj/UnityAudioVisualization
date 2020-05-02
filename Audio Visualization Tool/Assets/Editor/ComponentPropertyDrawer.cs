using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Component), true)]
public class ComponentPropertyDrawer : ObjectPropertyViewer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Component component = property.serializedObject.targetObject as Component;
        if (component == null || property.isArray)
        {
            base.OnGUI(position, property, label);
            return;
        }

        position.width -= WIDTH_OF_BUTTON * 3;
        base.OnGUI(position, property, label);
        position.x = position.xMax;
        position.width = WIDTH_OF_BUTTON;

        if (GUI.Button(position, new GUIContent("G", "Assign value based on GetComponent on this GameObject")))
        {
            System.Type type = GetPropertyType(property);

            if (type != null)
            {
                HandlePropertyButtonClicked(property, component.GetComponents(type));
            }
            else
            {
                property.objectReferenceValue = null;
            }
        }

        position.x += WIDTH_OF_BUTTON;
        if (GUI.Button(position, new GUIContent("P", "Assign value based on GetCompent on the parent of this GameObject")))
        {
            System.Type type = GetPropertyType(property);
            if (type != null)
            {
                HandlePropertyButtonClicked(property, component.GetComponentsInParent(type), component.gameObject);
            }
            else
            {
                property.objectReferenceValue = null;
            }
        }

        position.x += WIDTH_OF_BUTTON;
        if (GUI.Button(position, new GUIContent("C", "Assign value based on GetCompent on the children of this GameObject")))
        {
            System.Type type = GetPropertyType(property);
            if (type != null)
            {
                HandlePropertyButtonClicked(property, component.GetComponentsInChildren(type), component.gameObject);
            }
            else
            {
                property.objectReferenceValue = null;
            }
        }
    }

    private void HandlePropertyButtonClicked(SerializedProperty property, Component[] components, GameObject exclude = null)
    {
        GenericMenu menu = new GenericMenu();
        Component defaultCompnent = null;
        foreach (Component component in components)
        {
            if (component.gameObject == exclude)
            {
                continue;
            }

            defaultCompnent = component;
            menu.AddItem(new GUIContent(string.Format("{0} ~ {1}", component.name, component.GetType().Name)), false, delegate ()
            {
                property.objectReferenceValue = component;
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        if (menu.GetItemCount() > 1)
        {
            menu.ShowAsContext();
        }
        else
        {
            property.objectReferenceValue = defaultCompnent;
        }
    }
}
