#pragma warning disable 0169, 0414
using TemplateLibrary;
using xxlib;

// 模拟消息用
namespace Sim
{
    class Connect { }
    class Disconnect { }

    class Rotate
    {
        float angle;
        XY xyIncBase;
    }
}
