using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BouncySurface : MonoBehaviour
{
    public enum ForceType
    {
        Additive,
        Multiplicative,
    }

    public ForceType forceType = ForceType.Additive;
    public float bounceStrength = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            switch (forceType)
            {
                case ForceType.Additive:
                    ball.currentSpeed += bounceStrength;
                    return;

                case ForceType.Multiplicative:
                    ball.currentSpeed *= bounceStrength;
                    return;
            }
        }
    }
}
