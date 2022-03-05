//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityStandardAssets;

//namespace UnityStandardAssets.Utility.Inspector
//{

//    [CustomPropertyDrawer(typeof (WaypointList))]
//    public class WaypointListDrawer : PropertyDrawer
//    {
//        private float lineHeight = 18;
//        private float spacing = 4;


//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);

//            float x = position.x;
//            float y = position.y;
//            float inspectorWidth = position.width;


//            // Don't make child fields be indented
//            var indent = EditorGUI.indentLevel;
//            EditorGUI.indentLevel = 0;

//            WaypointList list = property.GetValue() as WaypointList;
//            var wayPoints = property.FindPropertyRelative("items");
//            var titles = new string[] {"Transform", "", "", ""};
//            var buttonsAndLabels = new string[] {"transform", "^", "v", "-"};
//            var widths = new float[] {.7f, .1f, .1f, .1f};
//            float lineHeight = 18;
//            bool changedLength = false;

//            //If there are more than zero waypoints
//            if (wayPoints.arraySize > 0)
//            {
//                for (int nWaypointIndex = 0; nWaypointIndex < wayPoints.arraySize; ++nWaypointIndex)
//                {
//                    var point = wayPoints.GetArrayElementAtIndex(nWaypointIndex);

//                    float rowX = x;
//                    for (int nLabelCounter = -1; nLabelCounter < buttonsAndLabels.Length; ++nLabelCounter)
//                    {
//                        if (nLabelCounter < 0)
//                        {
//                            nLabelCounter++;
//                        }

//                        float w = widths[nLabelCounter] *inspectorWidth;

//                        // Calculate rects
//                        Rect rect = new Rect(rowX, y, w, lineHeight);
//                        rowX += w;

//                        if (nLabelCounter == -1)
//                        {
//                            EditorGUI.LabelField(rect, titles[nLabelCounter]);
//                        }
//                        else
//                        {
//                            if (nLabelCounter == 0)
//                            {
                                
//                                    Debug.Log(point.objectReferenceValue);

//                                EditorGUI.ObjectField(rect, point.objectReferenceValue, typeof(Transform), true);
//                            }
//                            else
//                            {
//                                if (GUI.Button(rect, buttonsAndLabels[nLabelCounter]))
//                                {
//                                    switch (buttonsAndLabels[nLabelCounter])
//                                    {
//                                        case "-":
//                                            wayPoints.DeleteArrayElementAtIndex(nWaypointIndex);
//                                            changedLength = true;
//                                            break;
//                                        case "v":
//                                            if (nWaypointIndex > 0)
//                                            {
//                                                wayPoints.MoveArrayElement(nWaypointIndex, nWaypointIndex + 1);
//                                            }
//                                            break;
//                                        case "^":
//                                            if (nWaypointIndex < wayPoints.arraySize - 1)
//                                            {
//                                                wayPoints.MoveArrayElement(nWaypointIndex, nWaypointIndex - 1);
//                                            }
//                                            break;
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    y += lineHeight + spacing;
//                    if (changedLength)
//                    {
//                        break;
//                    }
//                }
//            }
            
//                // add button
//                var addButtonRect = new Rect((x + position.width) - widths[widths.Length - 1]*inspectorWidth, y,
//                                             widths[widths.Length - 1]*inspectorWidth, lineHeight);
//                if (GUI.Button(addButtonRect, "+"))
//                {
//                    wayPoints.InsertArrayElementAtIndex(wayPoints.arraySize);
//                }

//                y += lineHeight + spacing;
            

//            // add all button
//            var addAllButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
//            if (GUI.Button(addAllButtonRect, "Assign using all child objects"))
//            {
//                if (list.circuit == null)
//                {
//                    Debug.Log(property.displayName);
//                    Debug.LogError("Circuit was null.");
//                    return;
//                }
//                var children = new Transform[list.circuit.transform.childCount];
//                int n = 0;
//                foreach (Transform child in list.circuit.transform)
//                {
//                    children[n++] = child;
//                }
//                Array.Sort(children, new TransformNameComparer());
//                list.circuit.waypointList.items = new Transform[children.Length];
//                for (n = 0; n < children.Length; ++n)
//                {
//                    list.circuit.waypointList.items[n] = children[n];
//                }
//            }
//            y += lineHeight + spacing;

//            // rename all button
//            var renameButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
//            if (GUI.Button(renameButtonRect, "Auto Rename numerically from this order"))
//            {
//                //var circuit = property.FindPropertyRelative("circuit").objectReferenceValue as WaypointCircuit;
//                int n = 0;
//                foreach (Transform child in list.circuit.waypointList.items)
//                {
//                    child.name = "Waypoint " + (n++).ToString("000");
//                }
//            }
//            y += lineHeight + spacing;

//            // Set indent back to what it was
//            EditorGUI.indentLevel = indent;
//            EditorGUI.EndProperty();
//        }


//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            SerializedProperty items = property.FindPropertyRelative("items");
//            float lineAndSpace = lineHeight + spacing;
//            return 40 + (items.arraySize*lineAndSpace) + lineAndSpace;
//        }


//        // comparer for check distances in ray cast hits
//        public class TransformNameComparer : IComparer
//        {
//            public int Compare(object x, object y)
//            {
//                return ((Transform) x).name.CompareTo(((Transform) y).name);
//            }
//        }
//    }
//}