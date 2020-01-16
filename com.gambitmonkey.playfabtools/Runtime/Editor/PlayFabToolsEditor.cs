#if UNITY_EDITOR && PLAYFABTOOLS
namespace GambitMonkey.PlayFabTools.Editors
{
    using Sirenix.OdinInspector.Editor;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;
    using Sirenix.Utilities;
    using System.Collections.Generic;
    using Sirenix.Utilities.Editor;
    using global::PlayFab.AuthenticationModels;
    using global::PlayFab;
    using global::PlayFab.MultiplayerModels;

    public class PlayFabToolsEditor : OdinMenuEditorWindow
    {
        #region Odin Editor
        [MenuItem("Window/GambitMonkey/Tools/PlayFab Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<PlayFabToolsEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);            
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);

            var customMenuStyle = new OdinMenuStyle
            {
                BorderPadding = 0f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Offset = 20f,
                Height = 23,
                IconPadding = 0f,
                BorderAlpha = 0.323f
            };

            tree.DefaultMenuStyle = customMenuStyle;

            tree.Config.DrawSearchToolbar = true;

            // Adds the custom menu style to the tree, so that you can play around with it.
            // Once you are happy, you can press Copy C# Snippet copy its settings and paste it in code.
            // And remove the "Menu Style" menu item from the tree.
            tree.AddObjectAtPath("Menu Style", customMenuStyle);
            tree.AddMenuItemAtPath("", new OdinMenuItem(tree, "PlayFab Shared Settings", Resources.FindObjectsOfTypeAll<PlayFabSharedSettings>().FirstOrDefault()));

            #region Build Summaries Menu
            var buildSummaryObject = new ListBuildSummarriesEditor();

            //foreach (BuildSummary summary in buildSummaryObject.BuildSummaries)
            //{

            //}
            //var buildSummaryItem = new CustomMen
            //tree.AddMenuItemAtPath("", new OdinMenuItem(tree, "PlayFab Build Summaries",typeof(ListBuildSummarriesEditor)));
            #endregion

            //tree.AddMenuItemAtPath("", new OdinMenuItem(tree, "PlayFab VM Instances", Resources.FindObjectsOfTypeAll<PlayFabSharedSettings>().FirstOrDefault()));
            // Add all scriptable object items.
            tree.AddAllAssetsAtPath("PlayFab VM Instances", "Assets/PlayFabTools", typeof(VMTool), true)
                .ForEach(this.AddDragHandles);           

            tree.EnumerateTree()
                .AddThumbnailIcons()
                .SortMenuItemsByName();

            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }

        //protected override void OnBeginDrawEditors()
        //{
        //    var selected = this.MenuTree.Selection.FirstOrDefault();
        //    var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

        //    // Draws a toolbar with the name of the currently selected menu item.
        //    SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
        //    {
        //        if (selected != null)
        //        {
        //            GUILayout.Label(selected.Name);
        //        }

        //        if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
        //        {
        //            //ScriptableObjectCreator.ShowDialog<Item>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Items", obj =>
        //            //{
        //            //    obj.Name = obj.name;
        //            //    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
        //            //});
        //        }

        //        if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
        //        {
        //            //ScriptableObjectCreator.ShowDialog<Character>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Character", obj =>
        //            //{
        //            //    obj.Name = obj.name;
        //            //    base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
        //            //});
        //        }
        //    }
        //    SirenixEditorGUI.EndHorizontalToolbar();
        //}

        //// The editor window itself can also be customized.
        //protected override void OnEnable()
        //{
        //    base.OnEnable();

        //    this.MenuWidth = 200;
        //    this.ResizableMenuWidth = true;
        //    this.WindowPadding = new Vector4(10, 10, 10, 10);
        //    this.DrawUnityEditorPreview = true;
        //    this.DefaultEditorPreviewHeight = 20;
        //    this.UseScrollView = true;
        //}
        #endregion    
    }
}
#endif
