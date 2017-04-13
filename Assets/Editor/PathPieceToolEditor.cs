using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PathPieceBuildTool))]
public class PathPieceToolEditor : Editor {

    ReorderableList chunksList;
    ReorderableList surfaceList;
    ReorderableList enviroList;

    SerializedProperty turnEnviro;

    PathPieceBuildTool buildPiece { get { return target as PathPieceBuildTool; } }

    void OnEnable()
    {
        chunksList = new ReorderableList(serializedObject, 
            serializedObject.FindProperty("chunkPathObjects"), true, true, true, true);

        chunksList.drawHeaderCallback = OnDrawPiecesHeader;
        chunksList.elementHeightCallback = OnGetPiecesHeight;
        chunksList.drawElementCallback = OnDrawPathPiecesList;

        //chunksList.onChangedCallback = (list) => { buildPiece.RebuildPath(); };
        surfaceList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("surfaceChunkObjects"), true, true, true, true);

        surfaceList.drawHeaderCallback = OnDrawPiecesHeader;
        surfaceList.elementHeightCallback = OnGetPiecesHeight;
        surfaceList.drawElementCallback = OnDrawSurfacePiecesList;

        enviroList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("enviroChunkObjects"), true, true, true, true);

        enviroList.drawHeaderCallback = OnDrawPiecesHeader;
        enviroList.elementHeightCallback = OnGetPiecesHeight;
        enviroList.drawElementCallback = OnDrawEnviroPiecesList;

        turnEnviro = serializedObject.FindProperty("pathEndChunk");
    }
    
    void OnDrawPiecesHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Pieces (amount : " + serializedObject.FindProperty("chunkPathObjects").arraySize + ")");
    }

    float OnGetPiecesHeight(int index)
    {
        return 40f;
    }

    void OnDrawPathPiecesList(Rect rect, int index, bool active, bool focused)
    {
        SerializedProperty arrayElement = chunksList.serializedProperty.GetArrayElementAtIndex(index);

        rect.y += 2;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.size.x, EditorGUIUtility.singleLineHeight),
            arrayElement.FindPropertyRelative("thisPathChunkPiece"), GUIContent.none);

        /*
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.size.x, EditorGUIUtility.singleLineHeight),
            arrayElement.FindPropertyRelative("endRelativeToStart"), GUIContent.none);
            */
        serializedObject.ApplyModifiedProperties();
    }

    void OnDrawSurfacePiecesList(Rect rect, int index, bool active, bool focused)
    {
        SerializedProperty arrayElement = surfaceList.serializedProperty.GetArrayElementAtIndex(index);

        rect.y += 2;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.size.x, EditorGUIUtility.singleLineHeight),
            arrayElement.FindPropertyRelative("thisSurfaceChunkPiece"), GUIContent.none);

        /*
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.size.x, EditorGUIUtility.singleLineHeight),
            arrayElement.FindPropertyRelative("endRelativeToStart"), GUIContent.none);
            */
        serializedObject.ApplyModifiedProperties();
    }

    void OnDrawEnviroPiecesList(Rect rect, int index, bool active, bool focused)
    {
        SerializedProperty arrayElement = enviroList.serializedProperty.GetArrayElementAtIndex(index);

        rect.y += 2;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.size.x, EditorGUIUtility.singleLineHeight),
            arrayElement.FindPropertyRelative("thisEnviroChunk"), GUIContent.none);

        /*
        rect.y += EditorGUIUtility.singleLineHeight + 2;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.size.x, EditorGUIUtility.singleLineHeight),
            arrayElement.FindPropertyRelative("endRelativeToStart"), GUIContent.none);
            */
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        chunksList.DoLayoutList();
        surfaceList.DoLayoutList();
        enviroList.DoLayoutList();

        EditorGUILayout.PropertyField(turnEnviro);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Create Path"))
        {
            buildPiece.RebuildPath();
        }

        //chunksList
    }
}
