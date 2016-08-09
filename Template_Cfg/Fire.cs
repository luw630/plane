#pragma warning disable 0169, 0414
using TemplateLibrary;

class Fire
{
    [Compress, Title("攻击类型id"), Desc("攻击类型Id")]
    int id;

    [Title("攻击属性"), Limit(0, 20), Desc("攻击属性")]
    List<Property> properties;
}

