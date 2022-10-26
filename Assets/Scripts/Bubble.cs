using UnityEngine;

public struct Bubble
{
    public Vector2 position;
    public Vector2 velocity;
    public float radius;
}

public static class Bubbles
{
    private const float Velocity = 100f;
    private const float HalfVelocity = Velocity / 2;
    private const float MinRadius = 5.0f;
    private const float MaxRadius = 20.0f;
    private const float RadiusRange = MaxRadius - MinRadius;

    public static Bubble[] CreateBubbles(int amount, float boundsX, float boundsY)
    {
        var bubbles = new Bubble[amount];

        for (var i = 0; i < bubbles.Length; i++)
        {
            var bubble = bubbles[i];

            bubble.position.x = Random.value * boundsX;
            bubble.position.y = Random.value * boundsY;

            bubble.velocity.x = (Random.value * Velocity) - HalfVelocity;
            bubble.velocity.y = (Random.value * Velocity) - HalfVelocity;

            bubble.radius = (Random.value * RadiusRange) + MinRadius;

            bubbles[i] = bubble;
        }

        return bubbles;
    }
}
