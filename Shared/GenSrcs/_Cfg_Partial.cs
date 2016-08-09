#pragma warning disable 0109
using System;
using System.Collections.Generic;
using xxlib;
namespace Cfg
{
public partial class Map : IBBReader, IBBWriter 
{
    public void OverrideTo(Map o)
    {
        o.id = this.id;
        o.size = this.size;
        o.gasAreaSize = this.gasAreaSize;
        o.gasNumInit = this.gasNumInit;
        o.gasScanInterval = this.gasScanInterval;
        o.gasScanThreshold = this.gasScanThreshold;
        o.gasSupplementInterval = this.gasSupplementInterval;
        o.gasNumSupplement = this.gasNumSupplement;
        o.gasWeights = this.gasWeights;
        o.fortressMoveType = this.fortressMoveType;
        o.focusPoints = this.focusPoints;
        o.radius = this.radius;
        o.fortressId = this.fortressId;
        o.gasholeId = this.gasholeId;
    }
    public string FindDiff(Map o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        _rtv_ = this.size.FindDiff(o.size, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        _rtv_ = this.gasAreaSize.FindDiff(o.gasAreaSize, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        _rtv_ = this.gasNumInit.FindDiff(o.gasNumInit, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        if(o.gasScanInterval != this.gasScanInterval) return rootName + @"/gasScanInterval is diff! 
this  = " + this.gasScanInterval + @",
other = " + o.gasScanInterval;
        if(o.gasScanThreshold != this.gasScanThreshold) return rootName + @"/gasScanThreshold is diff! 
this  = " + this.gasScanThreshold + @",
other = " + o.gasScanThreshold;
        if(o.gasSupplementInterval != this.gasSupplementInterval) return rootName + @"/gasSupplementInterval is diff! 
this  = " + this.gasSupplementInterval + @",
other = " + o.gasSupplementInterval;
        _rtv_ = this.gasNumSupplement.FindDiff(o.gasNumSupplement, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        for (int _i_ = 0; _i_ < this.gasWeights.Count; ++_i_)
        {
            _rtv_ = this.gasWeights[_i_].FindDiff(o.gasWeights[_i_], rootName + "/gasWeights[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        if(o.fortressMoveType != this.fortressMoveType) return rootName + @"/fortressMoveType is diff! 
this  = " + this.fortressMoveType + @",
other = " + o.fortressMoveType;
        for (int _i_ = 0; _i_ < this.focusPoints.Count; ++_i_)
        {
            _rtv_ = this.focusPoints[_i_].FindDiff(o.focusPoints[_i_], rootName + "/focusPoints[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        if(o.radius != this.radius) return rootName + @"/radius is diff! 
this  = " + this.radius + @",
other = " + o.radius;
        if(o.fortressId != this.fortressId) return rootName + @"/fortressId is diff! 
this  = " + this.fortressId + @",
other = " + o.fortressId;
        if(o.gasholeId != this.gasholeId) return rootName + @"/gasholeId is diff! 
this  = " + this.gasholeId + @",
other = " + o.gasholeId;
        return _rtv_;
    }

    public const short packageId = 5;
    public static readonly Map DefaultInstance = new Map();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Map());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.Write(size);
        _bb_.Write(gasAreaSize);
        _bb_.Write(gasNumInit);
        _bb_.VarWrite(gasScanInterval);
        _bb_.VarWrite(gasScanThreshold);
        _bb_.VarWrite(gasSupplementInterval);
        _bb_.Write(gasNumSupplement);
        _bb_.Write(gasWeights);
        _bb_.VarWrite(fortressMoveType);
        _bb_.Write(focusPoints);
        _bb_.VarWrite(radius);
        _bb_.VarWrite(fortressId);
        _bb_.VarWrite(gasholeId); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.Read(ref size);
        _bb_.Read(ref gasAreaSize);
        _bb_.Read(ref gasNumInit);
        _bb_.VarRead(ref gasScanInterval);
        _bb_.VarRead(ref gasScanThreshold);
        _bb_.VarRead(ref gasSupplementInterval);
        _bb_.Read(ref gasNumSupplement);
        _bb_.Read(ref gasWeights, 1, 3);
        _bb_.VarRead(ref fortressMoveType);
        _bb_.Read(ref focusPoints, 0, 2);
        _bb_.VarRead(ref radius);
        _bb_.VarRead(ref fortressId);
        _bb_.VarRead(ref gasholeId);
    }

}
public partial struct Point : IBBReader, IBBWriter 
{
    public void OverrideTo(Point o)
    {
        o.x = this.x;
        o.y = this.y;
    }
    public string FindDiff(Point o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.x != this.x) return rootName + @"/x is diff! 
this  = " + this.x + @",
other = " + o.x;
        if(o.y != this.y) return rootName + @"/y is diff! 
this  = " + this.y + @",
other = " + o.y;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(x);
        _bb_.VarWrite(y); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref x);
        _bb_.VarRead(ref y);
    }

}
public partial struct Size : IBBReader, IBBWriter 
{
    public void OverrideTo(Size o)
    {
        o.width = this.width;
        o.height = this.height;
    }
    public string FindDiff(Size o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.width != this.width) return rootName + @"/width is diff! 
this  = " + this.width + @",
other = " + o.width;
        if(o.height != this.height) return rootName + @"/height is diff! 
this  = " + this.height + @",
other = " + o.height;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(width);
        _bb_.VarWrite(height); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref width);
        _bb_.VarRead(ref height);
    }

}
public partial struct Range : IBBReader, IBBWriter 
{
    public void OverrideTo(Range o)
    {
        o.min = this.min;
        o.max = this.max;
    }
    public string FindDiff(Range o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.min != this.min) return rootName + @"/min is diff! 
this  = " + this.min + @",
other = " + o.min;
        if(o.max != this.max) return rootName + @"/max is diff! 
this  = " + this.max + @",
other = " + o.max;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(min);
        _bb_.VarWrite(max); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref min);
        _bb_.VarRead(ref max);
    }

}
public partial class Fire : IBBReader, IBBWriter 
{
    public void OverrideTo(Fire o)
    {
        o.id = this.id;
        o.properties = this.properties;
    }
    public string FindDiff(Fire o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        for (int _i_ = 0; _i_ < this.properties.Count; ++_i_)
        {
            _rtv_ = this.properties[_i_].FindDiff(o.properties[_i_], rootName + "/properties[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        return _rtv_;
    }

    public const short packageId = 2;
    public static readonly Fire DefaultInstance = new Fire();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Fire());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.Write(properties); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.Read(ref properties, 0, 20);
    }

}
public partial class Enhance : IBBReader, IBBWriter 
{
    public void OverrideTo(Enhance o)
    {
        o.id = this.id;
        o.name = this.name;
        o.properties = this.properties;
    }
    public string FindDiff(Enhance o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.name != this.name) return rootName + @"/name is diff! 
this  = " + this.name + @",
other = " + o.name;
        for (int _i_ = 0; _i_ < this.properties.Count; ++_i_)
        {
            _rtv_ = this.properties[_i_].FindDiff(o.properties[_i_], rootName + "/properties[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        return _rtv_;
    }

    public const short packageId = 4;
    public static readonly Enhance DefaultInstance = new Enhance();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Enhance());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.Write(name);
        _bb_.Write(properties); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.Read(ref name, 0, 0);
        _bb_.Read(ref properties, 0, 10);
    }

}
public partial class Fortress : IBBReader, IBBWriter 
{
    public void OverrideTo(Fortress o)
    {
        o.id = this.id;
        o.isBuilding = this.isBuilding;
        o.hp = this.hp;
        o.moveSpeed = this.moveSpeed;
        o.startMoveTime = this.startMoveTime;
        o.firstAttackTime = this.firstAttackTime;
        o.atkInterval = this.atkInterval;
        o.restrictionTime = this.restrictionTime;
        o.damage = this.damage;
        o.buildingDamageFactor = this.buildingDamageFactor;
        o.atkHpThreshold = this.atkHpThreshold;
        o.ceasefireHpThreshold = this.ceasefireHpThreshold;
        o.killPlaneLoss = this.killPlaneLoss;
        o.name = this.name;
        o.resourceImg = this.resourceImg;
    }
    public string FindDiff(Fortress o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.isBuilding != this.isBuilding) return rootName + @"/isBuilding is diff! 
this  = " + this.isBuilding + @",
other = " + o.isBuilding;
        if(o.hp != this.hp) return rootName + @"/hp is diff! 
this  = " + this.hp + @",
other = " + o.hp;
        if(o.moveSpeed != this.moveSpeed) return rootName + @"/moveSpeed is diff! 
this  = " + this.moveSpeed + @",
other = " + o.moveSpeed;
        if(o.startMoveTime != this.startMoveTime) return rootName + @"/startMoveTime is diff! 
this  = " + this.startMoveTime + @",
other = " + o.startMoveTime;
        if(o.firstAttackTime != this.firstAttackTime) return rootName + @"/firstAttackTime is diff! 
this  = " + this.firstAttackTime + @",
other = " + o.firstAttackTime;
        if(o.atkInterval != this.atkInterval) return rootName + @"/atkInterval is diff! 
this  = " + this.atkInterval + @",
other = " + o.atkInterval;
        if(o.restrictionTime != this.restrictionTime) return rootName + @"/restrictionTime is diff! 
this  = " + this.restrictionTime + @",
other = " + o.restrictionTime;
        if(o.damage != this.damage) return rootName + @"/damage is diff! 
this  = " + this.damage + @",
other = " + o.damage;
        if(o.buildingDamageFactor != this.buildingDamageFactor) return rootName + @"/buildingDamageFactor is diff! 
this  = " + this.buildingDamageFactor + @",
other = " + o.buildingDamageFactor;
        if(o.atkHpThreshold != this.atkHpThreshold) return rootName + @"/atkHpThreshold is diff! 
this  = " + this.atkHpThreshold + @",
other = " + o.atkHpThreshold;
        if(o.ceasefireHpThreshold != this.ceasefireHpThreshold) return rootName + @"/ceasefireHpThreshold is diff! 
this  = " + this.ceasefireHpThreshold + @",
other = " + o.ceasefireHpThreshold;
        if(o.killPlaneLoss != this.killPlaneLoss) return rootName + @"/killPlaneLoss is diff! 
this  = " + this.killPlaneLoss + @",
other = " + o.killPlaneLoss;
        if(o.name != this.name) return rootName + @"/name is diff! 
this  = " + this.name + @",
other = " + o.name;
        if(o.resourceImg != this.resourceImg) return rootName + @"/resourceImg is diff! 
this  = " + this.resourceImg + @",
other = " + o.resourceImg;
        return _rtv_;
    }

    public const short packageId = 6;
    public static readonly Fortress DefaultInstance = new Fortress();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Fortress());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.Write(isBuilding);
        _bb_.VarWrite(hp);
        _bb_.VarWrite(moveSpeed);
        _bb_.VarWrite(startMoveTime);
        _bb_.VarWrite(firstAttackTime);
        _bb_.VarWrite(atkInterval);
        _bb_.VarWrite(restrictionTime);
        _bb_.VarWrite(damage);
        _bb_.VarWrite(buildingDamageFactor);
        _bb_.VarWrite(atkHpThreshold);
        _bb_.VarWrite(ceasefireHpThreshold);
        _bb_.VarWrite(killPlaneLoss);
        _bb_.Write(name);
        _bb_.Write(resourceImg); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.Read(ref isBuilding);
        _bb_.VarRead(ref hp);
        _bb_.VarRead(ref moveSpeed);
        _bb_.VarRead(ref startMoveTime);
        _bb_.VarRead(ref firstAttackTime);
        _bb_.VarRead(ref atkInterval);
        _bb_.VarRead(ref restrictionTime);
        _bb_.VarRead(ref damage);
        _bb_.VarRead(ref buildingDamageFactor);
        _bb_.VarRead(ref atkHpThreshold);
        _bb_.VarRead(ref ceasefireHpThreshold);
        _bb_.VarRead(ref killPlaneLoss);
        _bb_.Read(ref name, 0, 0);
        _bb_.Read(ref resourceImg, 0, 0);
    }

}
public partial class Property : IBBReader, IBBWriter 
{
    public void OverrideTo(Property o)
    {
        o.type = this.type;
        o.value = this.value;
    }
    public string FindDiff(Property o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.type != this.type) return rootName + @"/type is diff! 
this  = " + this.type + @",
other = " + o.type;
        if(o.value != this.value) return rootName + @"/value is diff! 
this  = " + this.value + @",
other = " + o.value;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.WriteEnum(type);
        _bb_.VarWrite(value); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.ReadEnum(ref type);
        _bb_.VarRead(ref value);
    }

}
public partial class GasWeight : IBBReader, IBBWriter 
{
    public void OverrideTo(GasWeight o)
    {
        o.id = this.id;
        o.weight = this.weight;
    }
    public string FindDiff(GasWeight o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.weight != this.weight) return rootName + @"/weight is diff! 
this  = " + this.weight + @",
other = " + o.weight;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.VarWrite(weight); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.VarRead(ref weight);
    }

}
public partial class Gas : IBBReader, IBBWriter 
{
    public void OverrideTo(Gas o)
    {
        o.id = this.id;
        o.radius = this.radius;
        o.energy = this.energy;
        o.name = this.name;
        o.resourceImg = this.resourceImg;
    }
    public string FindDiff(Gas o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.radius != this.radius) return rootName + @"/radius is diff! 
this  = " + this.radius + @",
other = " + o.radius;
        if(o.energy != this.energy) return rootName + @"/energy is diff! 
this  = " + this.energy + @",
other = " + o.energy;
        if(o.name != this.name) return rootName + @"/name is diff! 
this  = " + this.name + @",
other = " + o.name;
        if(o.resourceImg != this.resourceImg) return rootName + @"/resourceImg is diff! 
this  = " + this.resourceImg + @",
other = " + o.resourceImg;
        return _rtv_;
    }

    public const short packageId = 7;
    public static readonly Gas DefaultInstance = new Gas();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Gas());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.VarWrite(radius);
        _bb_.VarWrite(energy);
        _bb_.Write(name);
        _bb_.Write(resourceImg); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.VarRead(ref radius);
        _bb_.VarRead(ref energy);
        _bb_.Read(ref name, 0, 0);
        _bb_.Read(ref resourceImg, 0, 0);
    }

}
public partial class SkillOffset : IBBReader, IBBWriter 
{
    public void OverrideTo(SkillOffset o)
    {
        o.id = this.id;
        o.point = this.point;
    }
    public string FindDiff(SkillOffset o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        for (int _i_ = 0; _i_ < this.point.Count; ++_i_)
        {
            _rtv_ = this.point[_i_].FindDiff(o.point[_i_], rootName + "/point[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        return _rtv_;
    }

    public const short packageId = 8;
    public static readonly SkillOffset DefaultInstance = new SkillOffset();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new SkillOffset());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.Write(point); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.Read(ref point, 1, 5);
    }

}
public partial class Skill : IBBReader, IBBWriter 
{
    public void OverrideTo(Skill o)
    {
        o.id = this.id;
        o.properties = this.properties;
    }
    public string FindDiff(Skill o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        for (int _i_ = 0; _i_ < this.properties.Count; ++_i_)
        {
            _rtv_ = this.properties[_i_].FindDiff(o.properties[_i_], rootName + "/properties[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        return _rtv_;
    }

    public const short packageId = 3;
    public static readonly Skill DefaultInstance = new Skill();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Skill());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.Write(properties); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.Read(ref properties, 0, 20);
    }

}
public partial class Plane : IBBReader, IBBWriter 
{
    public void OverrideTo(Plane o)
    {
        o.id = this.id;
        o.hp = this.hp;
        o.armor = this.armor;
        o.shield = this.shield;
        o.shieldRecoverInterval = this.shieldRecoverInterval;
        o.shieldBeginRecoverInterval = this.shieldBeginRecoverInterval;
        o.isBuilding = this.isBuilding;
        o.rank = this.rank;
        o.moveSpeed = this.moveSpeed;
        o.colletRadius = this.colletRadius;
        o.collisionRadius = this.collisionRadius;
        o.strikeRadius = this.strikeRadius;
        o.angleLimit = this.angleLimit;
        o.targetDetectDistance = this.targetDetectDistance;
        o.targetDetectAngle = this.targetDetectAngle;
        o.shootOffsets = this.shootOffsets;
        o.skillIds = this.skillIds;
        o.evolutionIds = this.evolutionIds;
        o.evolutionEnergyCost = this.evolutionEnergyCost;
        o.enhanceEnergyCosts = this.enhanceEnergyCosts;
        o.enhanceIds = this.enhanceIds;
        o.bornShieldLifespan = this.bornShieldLifespan;
        o.bornInterval = this.bornInterval;
        o.name = this.name;
        o.vision = this.vision;
    }
    public string FindDiff(Plane o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.hp != this.hp) return rootName + @"/hp is diff! 
this  = " + this.hp + @",
other = " + o.hp;
        if(o.armor != this.armor) return rootName + @"/armor is diff! 
this  = " + this.armor + @",
other = " + o.armor;
        if(o.shield != this.shield) return rootName + @"/shield is diff! 
this  = " + this.shield + @",
other = " + o.shield;
        if(o.shieldRecoverInterval != this.shieldRecoverInterval) return rootName + @"/shieldRecoverInterval is diff! 
this  = " + this.shieldRecoverInterval + @",
other = " + o.shieldRecoverInterval;
        if(o.shieldBeginRecoverInterval != this.shieldBeginRecoverInterval) return rootName + @"/shieldBeginRecoverInterval is diff! 
this  = " + this.shieldBeginRecoverInterval + @",
other = " + o.shieldBeginRecoverInterval;
        if(o.isBuilding != this.isBuilding) return rootName + @"/isBuilding is diff! 
this  = " + this.isBuilding + @",
other = " + o.isBuilding;
        if(o.rank != this.rank) return rootName + @"/rank is diff! 
this  = " + this.rank + @",
other = " + o.rank;
        if(o.moveSpeed != this.moveSpeed) return rootName + @"/moveSpeed is diff! 
this  = " + this.moveSpeed + @",
other = " + o.moveSpeed;
        if(o.colletRadius != this.colletRadius) return rootName + @"/colletRadius is diff! 
this  = " + this.colletRadius + @",
other = " + o.colletRadius;
        if(o.collisionRadius != this.collisionRadius) return rootName + @"/collisionRadius is diff! 
this  = " + this.collisionRadius + @",
other = " + o.collisionRadius;
        if(o.strikeRadius != this.strikeRadius) return rootName + @"/strikeRadius is diff! 
this  = " + this.strikeRadius + @",
other = " + o.strikeRadius;
        if(o.angleLimit != this.angleLimit) return rootName + @"/angleLimit is diff! 
this  = " + this.angleLimit + @",
other = " + o.angleLimit;
        if(o.targetDetectDistance != this.targetDetectDistance) return rootName + @"/targetDetectDistance is diff! 
this  = " + this.targetDetectDistance + @",
other = " + o.targetDetectDistance;
        if(o.targetDetectAngle != this.targetDetectAngle) return rootName + @"/targetDetectAngle is diff! 
this  = " + this.targetDetectAngle + @",
other = " + o.targetDetectAngle;
        for (int _i_ = 0; _i_ < this.shootOffsets.Count; ++_i_)
        {
            _rtv_ = this.shootOffsets[_i_].FindDiff(o.shootOffsets[_i_], rootName + "/shootOffsets[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.skillIds.Count; ++_i_)
        {
            if(o.skillIds[_i_] != this.skillIds[_i_]) return rootName + @"/skillIds[" + _i_ + @"] is diff! 
this  = " + this.skillIds[_i_] + @",
other = " + o.skillIds[_i_];
        }
        for (int _i_ = 0; _i_ < this.evolutionIds.Count; ++_i_)
        {
            if(o.evolutionIds[_i_] != this.evolutionIds[_i_]) return rootName + @"/evolutionIds[" + _i_ + @"] is diff! 
this  = " + this.evolutionIds[_i_] + @",
other = " + o.evolutionIds[_i_];
        }
        if(o.evolutionEnergyCost != this.evolutionEnergyCost) return rootName + @"/evolutionEnergyCost is diff! 
this  = " + this.evolutionEnergyCost + @",
other = " + o.evolutionEnergyCost;
        for (int _i_ = 0; _i_ < this.enhanceEnergyCosts.Count; ++_i_)
        {
            if(o.enhanceEnergyCosts[_i_] != this.enhanceEnergyCosts[_i_]) return rootName + @"/enhanceEnergyCosts[" + _i_ + @"] is diff! 
this  = " + this.enhanceEnergyCosts[_i_] + @",
other = " + o.enhanceEnergyCosts[_i_];
        }
        for (int _i_ = 0; _i_ < this.enhanceIds.Count; ++_i_)
        {
            if(o.enhanceIds[_i_] != this.enhanceIds[_i_]) return rootName + @"/enhanceIds[" + _i_ + @"] is diff! 
this  = " + this.enhanceIds[_i_] + @",
other = " + o.enhanceIds[_i_];
        }
        if(o.bornShieldLifespan != this.bornShieldLifespan) return rootName + @"/bornShieldLifespan is diff! 
this  = " + this.bornShieldLifespan + @",
other = " + o.bornShieldLifespan;
        if(o.bornInterval != this.bornInterval) return rootName + @"/bornInterval is diff! 
this  = " + this.bornInterval + @",
other = " + o.bornInterval;
        if(o.name != this.name) return rootName + @"/name is diff! 
this  = " + this.name + @",
other = " + o.name;
        if(o.vision != this.vision) return rootName + @"/vision is diff! 
this  = " + this.vision + @",
other = " + o.vision;
        return _rtv_;
    }

    public const short packageId = 1;
    public static readonly Plane DefaultInstance = new Plane();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.VarWrite(hp);
        _bb_.VarWrite(armor);
        _bb_.VarWrite(shield);
        _bb_.VarWrite(shieldRecoverInterval);
        _bb_.VarWrite(shieldBeginRecoverInterval);
        _bb_.Write(isBuilding);
        _bb_.VarWrite(rank);
        _bb_.VarWrite(moveSpeed);
        _bb_.VarWrite(colletRadius);
        _bb_.VarWrite(collisionRadius);
        _bb_.VarWrite(strikeRadius);
        _bb_.VarWrite(angleLimit);
        _bb_.VarWrite(targetDetectDistance);
        _bb_.VarWrite(targetDetectAngle);
        _bb_.Write(shootOffsets);
        _bb_.VarWrite(skillIds);
        _bb_.VarWrite(evolutionIds);
        _bb_.VarWrite(evolutionEnergyCost);
        _bb_.VarWrite(enhanceEnergyCosts);
        _bb_.VarWrite(enhanceIds);
        _bb_.VarWrite(bornShieldLifespan);
        _bb_.VarWrite(bornInterval);
        _bb_.Write(name);
        _bb_.VarWrite(vision); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.VarRead(ref hp);
        _bb_.VarRead(ref armor);
        _bb_.VarRead(ref shield);
        _bb_.VarRead(ref shieldRecoverInterval);
        _bb_.VarRead(ref shieldBeginRecoverInterval);
        _bb_.Read(ref isBuilding);
        _bb_.VarRead(ref rank);
        _bb_.VarRead(ref moveSpeed);
        _bb_.VarRead(ref colletRadius);
        _bb_.VarRead(ref collisionRadius);
        _bb_.VarRead(ref strikeRadius);
        _bb_.VarRead(ref angleLimit);
        _bb_.VarRead(ref targetDetectDistance);
        _bb_.VarRead(ref targetDetectAngle);
        _bb_.Read(ref shootOffsets, 1, 5);
        _bb_.VarRead(ref skillIds, 2, 3);
        _bb_.VarRead(ref evolutionIds, 0, 3);
        _bb_.VarRead(ref evolutionEnergyCost);
        _bb_.VarRead(ref enhanceEnergyCosts, 4, 4);
        _bb_.VarRead(ref enhanceIds, 3, 3);
        _bb_.VarRead(ref bornShieldLifespan);
        _bb_.VarRead(ref bornInterval);
        _bb_.Read(ref name, 0, 0);
        _bb_.VarRead(ref vision);
    }

}
} // Cfg
