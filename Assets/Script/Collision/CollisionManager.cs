using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall { 
    public class CollisionManager : MonoBehaviour
    {
        List<CollisionMoveBasicAgent> _collisionBasicAgents;
        List<CollisionEffectAgent> _collisionEffectAgents;

        void Awake() {
            _collisionBasicAgents = new List<CollisionMoveBasicAgent>();
            _collisionEffectAgents = new List<CollisionEffectAgent>();
        }


        // Start is called before the first frame update
        void Start()
        {
        
        }


        // Update is called once per frame
        void Update()
        {                        
            // 计算移动
            for (int i = 0; i < _collisionBasicAgents.Count; i++) {
                _collisionBasicAgents[i].UpdatePosition(_collisionEffectAgents);                                       
            }            
        }

        public void AddCollisionMoveBasicAgent(CollisionMoveBasicAgent agent) {
            _collisionBasicAgents.Add(agent);
        }

        public void RemoveCollisionMoveBasicAgent(CollisionMoveBasicAgent agent)
        {
            _collisionBasicAgents.Remove(agent);
        }

        public void AddCollisionEffectAgent(CollisionEffectAgent agent)
        {
            _collisionEffectAgents.Add(agent);
        }

        public void RemoveCollisionEffectAgent(CollisionEffectAgent agent)
        {
            _collisionEffectAgents.Remove(agent);
        }

    }
}