#pragma warning disable 0169, 0414
using TemplateLibrary;

class Enhance
{
    [Compress, Title("id"), Desc("效果Id")]
    int id;

    [Title("强化名称"), Desc("强化名称")]
    string name;

    [Title("强化属性"), Limit(0, 10), Desc("强化属性列表")]
    List<Property> properties;
}
