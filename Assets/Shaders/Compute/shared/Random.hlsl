float random(float value, float seed = 32.435)
{
    // magical hardcoded randomness
    float2 seedVector = float2(sin(value), cos(value));

    const float a = 12.9898;
    const float b = 78.233;
    const float c = 43758.543123;

    float d = dot(seedVector, float2(a,b)) + seed;
    float s = sin(d);

    return frac(s * c);
}

float2 random2(float value)
{
    return float2(
        random(value, 433.059),
        random(value, 92.123)
    );
}
