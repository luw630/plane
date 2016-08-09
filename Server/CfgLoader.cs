using System.Collections.Generic;

public static class CfgLoader
{
    public static Cfgs Load()
    {
        // 此处开始造假.
        var cfgs = new Cfgs();

        // 一张地图
        cfgs.maps.Add(new Cfg.Map
        {
            id = 0,
            size = new Cfg.Size { width = 4500, height = 3000, },
            gasAreaSize = new Cfg.Size { width = 5, height = 5 },
            gasNumInit = new Cfg.Range { min = 30, max = 50 },//{ min = 20, max = 30 },
            gasScanInterval = 3000,
            gasScanThreshold = 20,//20,
            gasSupplementInterval = 100,
            gasNumSupplement = new Cfg.Range { min = 10, max = 20 },//{ min = 5, max = 10 },
                                                                  // 两种 gas 的生成权重
            gasWeights = new List<Cfg.GasWeight>
                {
                    new Cfg.GasWeight { id = 0, weight = 9 },
                    new Cfg.GasWeight { id = 1, weight = 1 },
                },
        });

        // 两种 gas
        cfgs.gass.Add(new Cfg.Gas { id = 0, radius = 10, energy = 100 });
        cfgs.gass.Add(new Cfg.Gas { id = 1, radius = 20, energy = 200 });


        // 四种飞机(基本, 坦克, 输出, 奶 )
        cfgs.planes.Add(new Cfg.Plane 
		{
            id = 0,
            hp = 100,
            armor = 1,
            shield = 0,
            shieldRecoverInterval = 100,
            shieldBeginRecoverInterval = 3000,
            isBuilding = false,                     // 当前不用理会
            rank = 0,                               // 当前不用理会

            moveSpeed = 100,
            colletRadius = 50,
            collisionRadius = 30,                   // 当前不用理会
            strikeRadius = 50,
            angleLimit = 1800,

            targetDetectDistance = 400,            // client only
            targetDetectAngle = 90,                 // client only
            shootOffsets = new List<Cfg.Point>
                {
                    new Cfg.Point { x = 50, y = 0 }     // 当前这个还可以不需要用矩阵旋转来算
                },
            skillIds = new List<int>
                {
                    0
                },

            evolutionIds = new List<int>            // 当前设计为  三种飞机可以相互进化
                {
                    1,
                    3
                },
            evolutionEnergyCost = 1500,
            enhanceEnergyCosts = new List<int>
                {
                    50,
                    80,
                    100,
                    200
                },
            enhanceIds = new List<int>
                {
                    0,
                    1,
                    2
                },

            bornShieldLifespan = 5000,
            bornInterval = 10000,

            name = "基本",
            vision = 100,                             // client only
        });

        cfgs.planes.Add(new Cfg.Plane {
            id = 1,
            hp = 100,
            armor = 1,
            shield = 0,
            shieldRecoverInterval = 0,
            shieldBeginRecoverInterval = 0,
            isBuilding = false,                     // 当前不用理会
            rank = 0,                               // 当前不用理会

            moveSpeed = 120,
            colletRadius = 50,
            collisionRadius = 30,                   // 当前不用理会
            strikeRadius = 50,
            angleLimit = 900,

            targetDetectDistance = 400,            // client only
            targetDetectAngle = 90,                 // client only
            shootOffsets = new List<Cfg.Point>
                {
                    new Cfg.Point { x = 50, y = 0 }     // 当前这个还可以不需要用矩阵旋转来算. 主要是普通攻击技能用
                },
            skillIds = new List<int>
                {
                    0,
                    3
                },

            evolutionIds = new List<int>            // 当前设计为  三种飞机可以相互进化
                {
                    3
                },
            evolutionEnergyCost = 2500,
            enhanceEnergyCosts = new List<int>
                {
                    50,
                    100,
                    200,
                    500
                },
            enhanceIds = new List<int>
                {
                    3,
                    4,
                    5
                },

            bornShieldLifespan = 5000,
            bornInterval = 10000,

            name = "奶型",
            vision = 100,                             // client only
        });


        cfgs.planes.Add(new Cfg.Plane {
            id = 2,
            hp = 80,
            armor = 1,
            shield = 0,
            shieldRecoverInterval = 0,
            shieldBeginRecoverInterval = 0,
            isBuilding = false,                     // 当前不用理会
            rank = 0,                               // 当前不用理会

            moveSpeed = 150,
            colletRadius = 50,
            collisionRadius = 30,                   // 当前不用理会
            strikeRadius = 50,
            angleLimit = 1800,

            targetDetectDistance = 600,            // client only
            targetDetectAngle = 90,                 // client only
            shootOffsets = new List<Cfg.Point>
                {
                    new Cfg.Point { x = 50, y = 0 }     // 当前这个还可以不需要用矩阵旋转来算
                },
            skillIds = new List<int>
                {
                    4,
                    5
                },

            evolutionIds = new List<int>            // 当前设计为  三种飞机可以相互进化
                {
                    1,
                    3
                },
            evolutionEnergyCost = 2500,
            enhanceEnergyCosts = new List<int>
                {
                    50,
                    100,
                    200,
                    500
                },
            enhanceIds = new List<int>
                {
                    6,
                    7,
                    8
                },

            bornShieldLifespan = 5000,
            bornInterval = 10000,

            name = "攻型",
            vision = 120,                             // client only
        });

        cfgs.planes.Add(new Cfg.Plane {
            id = 3,
            hp = 100,
            armor = 3,
            shield = 0,
            shieldRecoverInterval = 100,
            shieldBeginRecoverInterval = 3000,
            isBuilding = false,                     // 当前不用理会
            rank = 0,                               // 当前不用理会

            moveSpeed = 100,
            colletRadius = 50,
            collisionRadius = 30,                   // 当前不用理会
            strikeRadius = 50,
            angleLimit = 900,

            targetDetectDistance = 500,            // client only
            targetDetectAngle = 90,                 // client only
            shootOffsets = new List<Cfg.Point>
                {
                    new Cfg.Point { x = 50, y = 0 }     // 当前这个还可以不需要用矩阵旋转来算
                },
            skillIds = new List<int>
                {
                    0,
                    1
                },

            evolutionIds = new List<int>            // 当前设计为  三种飞机可以相互进化
                {
                    1
                },
            evolutionEnergyCost = 2500,
            enhanceEnergyCosts = new List<int>
                {
                    50,
                    100,
                    200,
                    500
                },
            enhanceIds = new List<int>
                {
                    0,
                    1,
                    2
                },

            bornShieldLifespan = 5000,
            bornInterval = 10000,

            name = "坦型",
            vision = 120,                             // client only
        });

        // 坦型普攻
        cfgs.skills.Add(new Cfg.Skill {
            id = 0,
            properties = new List<Cfg.Property> {
                new Cfg.Property { type = Cfg.PropertyTypes.atkMoveSpdScaling, value = -100 },
                new Cfg.Property { type = Cfg.PropertyTypes.damage, value = 20 },
                new Cfg.Property { type = Cfg.PropertyTypes.startDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.endDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.range, value = 250 },
                new Cfg.Property { type = Cfg.PropertyTypes.bulletRadius, value = 5 },
                new Cfg.Property { type = Cfg.PropertyTypes.cd, value = 1100 },
            }
        });
        // 坦型大招: 普攻三连射
        cfgs.skills.Add(new Cfg.Skill
        {
            id = 1,
            properties = new List<Cfg.Property> {
                new Cfg.Property { type = Cfg.PropertyTypes.atkMoveSpdScaling, value = -100 },
                new Cfg.Property { type = Cfg.PropertyTypes.damage, value = 15 },
                new Cfg.Property { type = Cfg.PropertyTypes.startDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.endDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.range, value = 250 },
                new Cfg.Property { type = Cfg.PropertyTypes.bulletRadius, value = 5 },
                new Cfg.Property { type = Cfg.PropertyTypes.interval, value = 50 },
                new Cfg.Property { type = Cfg.PropertyTypes.numTotal, value = 3 },
                new Cfg.Property { type = Cfg.PropertyTypes.cd, value = 5000 },
            }
        });

        // 奶型普攻
        cfgs.skills.Add(new Cfg.Skill
        {
            id = 2,
            properties = new List<Cfg.Property> {
                new Cfg.Property { type = Cfg.PropertyTypes.atkMoveSpdScaling, value = -100 },
                new Cfg.Property { type = Cfg.PropertyTypes.damage, value = 15 },
                new Cfg.Property { type = Cfg.PropertyTypes.startDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.endDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.range, value = 250 },
                new Cfg.Property { type = Cfg.PropertyTypes.bulletRadius, value = 5 },
                new Cfg.Property { type = Cfg.PropertyTypes.cd, value = 1100 },
            }
        });
        // 奶型大招: 自己加满血
        cfgs.skills.Add(new Cfg.Skill
        {
            id = 3,
            properties = new List<Cfg.Property> {
                new Cfg.Property { type = Cfg.PropertyTypes.cd, value = 20000 },
            }
        });

        // 攻型普攻( 比奶坦强些远些快些 )
        cfgs.skills.Add(new Cfg.Skill
        {
            id = 4,
            properties = new List<Cfg.Property> {
                new Cfg.Property { type = Cfg.PropertyTypes.atkMoveSpdScaling, value = 0 },
                new Cfg.Property { type = Cfg.PropertyTypes.damage, value = 25 },
                new Cfg.Property { type = Cfg.PropertyTypes.startDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.endDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.range, value = 300 },
                new Cfg.Property { type = Cfg.PropertyTypes.bulletRadius, value = 7 },
                new Cfg.Property { type = Cfg.PropertyTypes.cd, value = 700 },
            }
        });
        // 攻型大招: 更粗更强的单次
        cfgs.skills.Add(new Cfg.Skill
        {
            id = 5,
            properties = new List<Cfg.Property> {
                new Cfg.Property { type = Cfg.PropertyTypes.atkMoveSpdScaling, value = 0 },
                new Cfg.Property { type = Cfg.PropertyTypes.damage, value = 30 },
                new Cfg.Property { type = Cfg.PropertyTypes.startDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.endDelay, value = 100 },
                new Cfg.Property { type = Cfg.PropertyTypes.range, value = 400 },
                new Cfg.Property { type = Cfg.PropertyTypes.bulletRadius, value = 20 },
                new Cfg.Property { type = Cfg.PropertyTypes.cd, value = 10000 },
            }
        });

        // 坦型
        cfgs.enhances.Add(new Cfg.Enhance { id = 0, name = "血", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraHp, value = 20 } } });
        cfgs.enhances.Add(new Cfg.Enhance { id = 1, name = "甲", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraArmor, value = 5 } } });
        cfgs.enhances.Add(new Cfg.Enhance { id = 2, name = "盾", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraShield, value = 50 } } });

        // 奶型
        cfgs.enhances.Add(new Cfg.Enhance { id = 3, name = "血", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraHp, value = 10 } } });
        cfgs.enhances.Add(new Cfg.Enhance { id = 4, name = "甲", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraArmor, value = 1 } } });
        cfgs.enhances.Add(new Cfg.Enhance { id = 5, name = "盾", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraShield, value = 10 } } });

        // 攻型
        cfgs.enhances.Add(new Cfg.Enhance { id = 6, name = "血", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraHp, value = 20 } } });
        cfgs.enhances.Add(new Cfg.Enhance { id = 7, name = "甲", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraArmor, value = 2 } } });
        cfgs.enhances.Add(new Cfg.Enhance { id = 8, name = "盾", properties = new List<Cfg.Property> { new Cfg.Property { type = Cfg.PropertyTypes.extraShield, value = 20 } } });


        // 计算填充附加字段
        foreach (var o in cfgs.maps) o.Init(cfgs);
        foreach (var o in cfgs.planes) o.Init(cfgs);
        // todo: more

        return cfgs;
    }
}

