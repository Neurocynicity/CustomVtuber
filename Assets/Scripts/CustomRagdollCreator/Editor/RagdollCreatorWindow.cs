using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomRagdollCreator
{
    public class RagdollCreatorWindow : EditorWindow
    {
        [MenuItem("Tools/Complex Ragdoll Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(RagdollCreatorWindow));
        }

        private CustomRagdoll _editingRagdoll;

        private void OnSelectionChange()
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            
            Repaint();
            if (!selectedGameObject)
                return;

            CustomRagdoll ragdollToEdit = selectedGameObject.GetComponentInChildren<CustomRagdoll>();
            ragdollToEdit ??= selectedGameObject.GetComponentInParent<CustomRagdoll>();

            if (!ragdollToEdit)
                return;

            _editingRagdoll = ragdollToEdit;
            Repaint();
        }

        private void OnEnable()
        {
            Undo.undoRedoPerformed += Repaint;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= Repaint;
        }

        private float _radiusSliderValue;
        private bool _symmetryMenuOpen;

        private Vector2 _scrollPos;

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            
            if (Selection.activeGameObject != null && GUILayout.Button("MAKE RAGDOLL!!!"))
            {
                GameObject newRagdollGameObject = Selection.activeGameObject;
                Undo.RegisterFullObjectHierarchyUndo(newRagdollGameObject, "Created custom ragdoll");
                _editingRagdoll = CustomRagdoll.GenerateCustomRagdoll(newRagdollGameObject);
                SymmetryDetector.EvaluateAllSymmetries(_editingRagdoll);
                _closeSymmetries = _editingRagdoll.CloseSymmetries;
            }

            if (!_editingRagdoll)
            {
                EditorGUILayout.EndScrollView();
                return;
            }
            
            DrawSeperator();

            string newDisplayName = EditorGUILayout.TextField("Currently Editing:", _editingRagdoll.DisplayName);

            if (newDisplayName != _editingRagdoll.DisplayName)
            {
                Undo.RecordObject(_editingRagdoll, "Changed Ragdoll Display Name");
                _editingRagdoll.DisplayName = newDisplayName;
            }
            
            DrawSeperator();

            EditorGUILayout.BeginHorizontal();
            
            _symmetryMenuOpen ^= GUILayout.Button("Symmetry Settings", EditorStyles.boldLabel);
            _symmetryMenuOpen ^= GUILayout.Button(_symmetryMenuOpen ? "ʌ" : "v");
            
            EditorGUILayout.EndHorizontal();

            if (_symmetryMenuOpen)
                DrawSymmetryMenu();
            
            EditorGUILayout.EndScrollView();
        }

        private List<CloseSymmetry> _closeSymmetries = new();
        private void DrawSymmetryMenu()
        {
            _closeSymmetries = _editingRagdoll.CloseSymmetries;
            
            _editingRagdoll.symmetryPosAllowance = EditorGUILayout.FloatField("Positional Symmetry Allowance",
                _editingRagdoll.symmetryPosAllowance);
            _editingRagdoll.symmetryRotAllowance = EditorGUILayout.FloatField("Rotational Symmetry Allowance",
                _editingRagdoll.symmetryRotAllowance);

            if (GUILayout.Button("Reevaluate Symmetry"))
            {
                SymmetryDetector.EvaluateAllSymmetries(_editingRagdoll, true);
                _closeSymmetries = _editingRagdoll.CloseSymmetries;
            }

            if (_closeSymmetries.Count == 0)
                return;
            
            EditorGUILayout.LabelField("Potential Symmetries:", EditorStyles.boldLabel);
            
            DrawSeperator();
            
            for (int i = 0; i < _closeSymmetries.Count; i++)
                DrawCloseSymmetry(_closeSymmetries[i]);
        }

        private void DrawCloseSymmetry(CloseSymmetry closeSymmetry)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.ObjectField(closeSymmetry.Bone1, typeof(RagdollBone), true);    
            EditorGUILayout.ObjectField(closeSymmetry.Bone2, typeof(RagdollBone), true);
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.FloatField("Pos Allowance", closeSymmetry.RequiredPositionalAllowance);
            EditorGUILayout.FloatField("Rot Allowance", closeSymmetry.RequiredRotationalAllowance);

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Add"))
            {
                Undo.RecordObject(_editingRagdoll, "Added Close Symmetry");
                Undo.RegisterFullObjectHierarchyUndo(closeSymmetry.Bone1.gameObject, "Added Close Symmetry");
                Undo.RegisterFullObjectHierarchyUndo(closeSymmetry.Bone2.gameObject, "Added Close Symmetry");
                
                _closeSymmetries.Remove(closeSymmetry);
                RagdollBoneSymmetry.CreateSymmetry(closeSymmetry.Bone1, closeSymmetry.Bone2);
            }

            if (GUILayout.Button("Remove"))
            {
                Undo.RecordObject(_editingRagdoll, "Ignored Close Symmetry");
                _closeSymmetries.Remove(closeSymmetry);
            }
            
            EditorGUILayout.EndHorizontal();

            DrawSeperator();
        }
        
        // this makes a ""slider"" which look decent as a line seperator
        private void DrawSeperator() =>
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
}