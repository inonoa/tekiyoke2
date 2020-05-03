using UnityEditor;
using UnityEngine;

public class RenameFieldAttribute : PropertyAttribute
{
    public string Name { get; }

    public RenameFieldAttribute( string name ) => Name = name;

#if UNITY_EDITOR
    [CustomPropertyDrawer( typeof( RenameFieldAttribute ) )]
    public class FieldNameDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            string[] path = property.propertyPath.Split( '.' );
            
            //isArray判定書き換えたけどこれであってるか分からん
            bool isArray = path.Length > 1 && path[ path.Length - 2 ] == "Array";

            if ( !isArray && attribute is RenameFieldAttribute fieldName ){
                label.text = fieldName.Name;
            }

            EditorGUI.PropertyField( position, property, label, true );
        }
    }
#endif
}
