#pragma warning disable 0169, 0414
using TemplateLibrary;

[Struct]
struct Point
{
    [Compress]
    int x, y;
}

[Struct]
struct Size
{
    [Compress]
    int width, height;
}

[Struct]
struct Range
{
    [Compress]
    int min, max;
}
