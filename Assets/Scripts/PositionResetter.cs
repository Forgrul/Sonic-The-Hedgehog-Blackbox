using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Giometric.UniSonic.Objects;

namespace Giometric.UniSonic
{
    public class PositionResetter : MonoBehaviour
    {
        [SerializeField] Movement player;
        List<Vector3> positions = new List<Vector3>();

        void Start()
        {
            foreach(Transform pos in transform)
            {
                positions.Add(pos.position);
            }
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetPosition();
            }
        }

        void ResetPosition()
        {
            int targetIndex = 0;
            float minDistance = 10000000f;

            for(int i = 0; i < positions.Count; i++)
            {
                float distance = (player.transform.position - positions[i]).magnitude;
                if(distance < minDistance)
                {
                    targetIndex = i;
                    minDistance = distance;
                }
            }

            player.transform.position = positions[targetIndex];
            player.ResetMovement();
        }
    }
}
