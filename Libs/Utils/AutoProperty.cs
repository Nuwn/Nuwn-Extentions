using System.Collections;
using UnityEngine;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RequiredAttribute : PropertyAttribute
{ }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(RequiredAttribute))]
public class RequiredDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var obj = property.GetParent() as Component;
        if (property.exposedReferenceValue)
            return;

        var c = obj.GetComponent(fieldInfo.FieldType);
        if (c)
            fieldInfo.SetValue(obj, c);
        else
        {
            var s = new GUIStyle(EditorStyles.label);
            s.normal.textColor = Color.red;
            GUI.Label(position, "Missing: " + fieldInfo.FieldType.Name, s);
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        var obj = property.GetParent() as Component;
        var c = obj.GetComponent(fieldInfo.FieldType);
        if (c)
            return 0;
        else
            return base.GetPropertyHeight(property, label);

    }

}

public static class PropertyDrawerExtensions
{

    public static object GetParent(this SerializedProperty prop)
    {

        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');

        foreach (var element in elements.Take(elements.Length - 1))
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
                obj = GetValue(obj, element);

        return obj;

    }

    public static object GetValue(this object source, string name)
    {

        if (source == null)
            return null;

        var type = source.GetType();
        var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (f == null)
        {
            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null)
                return null;
            return p.GetValue(source, null);
        }

        return f.GetValue(source);

    }

    public static object GetValue(object source, string name, int index)
    {

        var enumerable = GetValue(source, name) as IEnumerable;
        var enm = enumerable.GetEnumerator();

        while (index-- >= 0)
            enm.MoveNext();
        return enm.Current;

    }

}
#endif
