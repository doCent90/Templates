using UnityEngine;

namespace Core
{
    public static class Distance
    {
        public static float GetDistance(Vector3 start, Vector3 end)
        {
            Vector3 direction = start - end;
            float distance = direction.sqrMagnitude;
            return distance;
        }

        public static float GetDistanceX(Vector3 start, Vector3 end)
        {
            end.y = start.y;
            end.z = start.z;
            Vector3 direction = start - end;
            float distance = direction.sqrMagnitude;
            return distance;
        }

        public static Vector2 Point(Collider2D collider)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[2];
            collider.GetContacts(contacts);
            return contacts[0].point;
        }
    }
}
