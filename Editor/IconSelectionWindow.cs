using UnityEditor;
using UnityEngine;
using System.Linq;

public class IconSelectionWindow : EditorWindow
{
    private static Texture2D selectedIcon;
    private static GameObject selectedGameObject;

    private Texture2D[] icons;
    private Vector2 scrollPosition;

    private void OnEnable()
    {
        // Unity エディターのデフォルトリソースからアイコンを取得
        icons = Resources.FindObjectsOfTypeAll(typeof(Texture2D))
            .Where(x => AssetDatabase.GetAssetPath(x) == "Library/unity editor resources") 
            .Select(x => x.name)    
            .Distinct()             
            .OrderBy(x => x)        
            .Select(x => EditorGUIUtility.Load(x) as Texture2D) 
            .Where(x => x)          
            .ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Icon", EditorStyles.boldLabel);

        // スクロールビューの開始
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        int iconsPerRow = Mathf.FloorToInt(position.width / 60); // 1アイコンの横幅が60であると仮定
        int iconCount = 0;
        GUILayout.BeginHorizontal();
        foreach (var icon in icons)
        {
            if (GUILayout.Button(icon, GUILayout.Width(50), GUILayout.Height(50)))
            {
                selectedIcon = icon;
                SetIcon();
            }

            iconCount++;
            if (iconCount >= iconsPerRow)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                iconCount = 0;
            }
        }
        GUILayout.EndHorizontal();

        // スクロールビューの終了
        EditorGUILayout.EndScrollView();

        // Clear selection
        if (GUILayout.Button("Clear", GUILayout.Width(50)))
        {
            selectedIcon = null;
            SetIcon();
        }
    }

    private void SetIcon()
    {
        if (selectedGameObject != null)
        {
            Texture2D icon = null;
            if (selectedIcon != null)
            {
                icon = AssetPreview.GetMiniThumbnail(selectedIcon);
            }
            else
            {
                // デフォルトのアイコンを読み込む
                icon = EditorGUIUtility.Load("GameObject Icon") as Texture2D;
            }
            EditorGUIUtility.SetIconForObject(selectedGameObject, icon);
        }
    }

    public static void ShowWindow(GameObject gameObject)
    {
        selectedGameObject = gameObject;
        var window = GetWindow<IconSelectionWindow>("Icon Selection");
        window.minSize = new Vector2(200, 100);
    }
}
