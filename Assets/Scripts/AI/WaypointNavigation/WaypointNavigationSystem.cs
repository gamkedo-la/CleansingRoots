﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.WaypointNavigation
{
    public class WaypointNavigationSystem : IService
    {
        private List<WaypointNode> _nodes = new List<WaypointNode>();
        private PlayerWaypointNode _playerNode;
        private float updateRate = .2f;
        private Coroutine updateCoroutine;

        public WaypointNavigationSystem()
        {
            updateCoroutine = ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(UpdateNav());
        }
        public void RegisterWaypoint(WaypointNode nodeToRegister)
        {
            _nodes.Add(nodeToRegister);
        }

        public void UnregisterWaypoint(WaypointNode nodeToUnregister)
        {
            _nodes.Remove(nodeToUnregister);
        }
        
        public void RegisterPlayerNode(PlayerWaypointNode playerWaypointNode)
        {
            _playerNode = playerWaypointNode;
        }

        private void ClearAllNodes()
        {
            foreach (var node in _nodes)
            {
                node.Reset();
            }
        }

        private IEnumerator UpdateNav()
        {
            while (true)
            {
                if (_playerNode != null && _nodes.Count > 0)
                {
                    ClearAllNodes();
                    _playerNode.ScoreNodes();
                }
                yield return new WaitForSeconds(updateRate);
            }
        }
    }
}