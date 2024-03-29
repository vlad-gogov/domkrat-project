﻿using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace WireBuilder
{
    [ExecuteInEditMode]
    [AddComponentMenu("Wire Network/Connector")]
    [HelpURL("http://staggart.xyz/unity/wire-builder/wb-docs/?section=components")]
    public class WireConnector : MonoBehaviour
    {
        [Tooltip("The group the connector is attached part of")]
        public WireConnectorGroup group;
        [Tooltip("Wires pulled out from this connector will be created using this type.")]
        public WireType wireType;
        [Tooltip("All the wires currently connected to the connector")]
        public List<Wire> wires = new List<Wire>();

        public void OnEnable()
        {
            WireManager.AddConnector(this);
        }

        public void OnDisable()
        {
            WireManager.RemoveConnector(this);
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            //Avoid drawing gizmo when parent is selected
            if (Selection.activeGameObject != this.gameObject) return;

            UnityEditor.Handles.DrawWireDisc(this.transform.position, Camera.current.transform.forward, 0.2f);
#endif
        }


    }
}