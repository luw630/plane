#pragma warning disable 0169, 0414
using TemplateLibrary;

class SkillOffset
{
    [Compress, Title("id"), Desc("id")]
    int id;

    [Limit(1, 5), Title("位置坐标"), Desc("位置相对飞机中心点的坐标")]
    List<Point> point;
}

class Skill
{
    [Compress, Title("技能id"), Desc("技能Id")]
    int id;

    [Title("技能属性表"), Limit(0, 20), Desc("[属性:值] 键值对数组")]
    List<Property> properties;
}
