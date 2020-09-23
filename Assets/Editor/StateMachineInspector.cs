using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;

[CustomEditor(typeof(FiniteStateMachine)), CanEditMultipleObjects]
public class StateMachineInspector : Editor
{
    [SerializeField]List<MachineType> foundTypes;
    SerializedProperty selected;

    int selectedType = 0;

    private void OnEnable()
    {
        selected = serializedObject.FindProperty("stateIndex");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        FiniteStateMachine finiteStateMachine = (FiniteStateMachine)target;

        foundTypes = FindSubtypes.FindGenericSubtypes<MachineType>();
        string[] sTypes = foundTypes.ToArray().OfType<MachineType>().Select(t => t.ToString()).ToArray();

        selectedType = EditorGUILayout.Popup("Machine Types", selected.intValue, sTypes);
        selected.intValue = selectedType;

        EditorGUILayout.LabelField("Current State: " + finiteStateMachine.getCurrentState());

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Possible States:", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        foreach(var state in foundTypes[selectedType].possibleStates)
        {
            EditorGUILayout.LabelField(state.ToString());
        }

        serializedObject.ApplyModifiedProperties();
    }
}
