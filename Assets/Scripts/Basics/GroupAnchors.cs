using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName ="Anchors/Group")]
    public class GroupAnchors : ScriptableObject
    {
        List<GameObject> group = new();

        public List<GameObject> Group => group;


        public void Add(GameObject member)
        {
            if (group.Contains(member))
            {
                Debug.LogWarning($"Failed to add Member. Group already contains {member}.");
                return;
            }
            group.Add(member);
        }

        public void Remove(GameObject member)
        {
            if (!group.Contains(member))
            {
                Debug.LogWarning($"Cant remove {member}, it was not a member of the group.");
                return;
            }
            Remove(member);
        }
    }
}
