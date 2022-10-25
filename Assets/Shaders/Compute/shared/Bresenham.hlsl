// https://www.geeksforgeeks.org/bresenhams-circle-drawing-algorithm/
// https://arcade.makecode.com/graphics-math/bresenham-circle

// Function to put pixels at subsequence points
void drawMirroredPixels(uint2 center, int2 pixel, float4 color, RWTexture2D<float4> output)
{
    output[uint2(center.x + pixel.x, center.y + pixel.y)] = color;
    output[uint2(center.x - pixel.x, center.y + pixel.y)] = color;
    output[uint2(center.x + pixel.x, center.y - pixel.y)] = color;
    output[uint2(center.x - pixel.x, center.y - pixel.y)] = color;
    output[uint2(center.x + pixel.y, center.y + pixel.x)] = color;
    output[uint2(center.x - pixel.y, center.y + pixel.x)] = color;
    output[uint2(center.x + pixel.y, center.y - pixel.x)] = color;
    output[uint2(center.x - pixel.y, center.y - pixel.x)] = color;
}

// Function for circle-generation using Bresenham's algorithm
void drawCircle(uint2 center, uint radius, float4 color, RWTexture2D<float4> output)
{
    int2 pixel = int2(0, radius);

    // circle equation is:
    // X^2 + Y^2 = RADIUS^2
    //
    // error equation for the two possible pixels:
    // A = (Xi + 1)^2 + Yi^2 - radius^2
    // B = (Xi + 1)^2 + (Yi - 1)^2 - radius^2
    //
    // d = 2 * (Xi + 1)^2 + Yi^2 + (Yi - 1)^2 - 2 * radius^2

    // when x=0 and y=radius
    int d = 3 - 2 * radius;

    drawMirroredPixels(center, pixel, color, output);

    while (pixel.y >= pixel.x)
    {
        // for each pixel we will draw all eight pixels
        pixel.x++;

        // check for decision parameter and correspondingly update d, x, y

        // the decision parameter is updated according to the equation:
        // Dj = Di + (expression for Dj) - (expression for Di)
        if (d > 0)
        {
            pixel.y--;
            // Di > 0: Dj = Di + 4 * (Xi - Yi) + 10
            d = d + 4 * (pixel.x - pixel.y) + 10;
        }
        else
        {
            // Di <= 0: Dj = Di + 4 * Xi + 6
            d = d + 4 * pixel.x + 6;
        }

        drawMirroredPixels(center, pixel, color, output);
    }
}
