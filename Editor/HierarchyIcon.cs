using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyIcon
{
    static HierarchyIcon()
    {
        EditorApplication.hierarchyWindowItemOnGUI += ReplaceGameObjectIcon;
    }

    private static void ReplaceGameObjectIcon(int instanceID, Rect selectionRect)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 1 && selectionRect.Contains(e.mousePosition))
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null)
            {
                // Show the icon selection window when right-clicking on the GameObject icon
                IconSelectionWindow.ShowWindow(gameObject);
                e.Use();
            }
        }

        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null)
        {
            // Check if the icon of the GameObject has been set
            Texture icon = EditorGUIUtility.ObjectContent(obj, obj.GetType()).image;
            if (icon != null)
            {
                // Draw the custom icon
                Rect iconRect = new Rect(selectionRect);
                iconRect.width = 16;
                iconRect.height = 16;
                GUI.DrawTexture(iconRect, icon);
            }
            else
            {
                // If no custom icon is set, draw an empty texture to hide the default icon
                Rect iconRect = new Rect(selectionRect);
                iconRect.width = 16;
                iconRect.height = 16;
                GUI.DrawTexture(iconRect, Texture2D.whiteTexture);
            }
        }
    }
}
