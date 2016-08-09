#pragma warning disable 0109
using System;
using System.Collections.Generic;
using xxlib;
namespace PT
{
public partial class Fortress : IBBReader, IBBWriter 
{
    public void OverrideTo(Fortress o)
    {
        o.cfgId = this.cfgId;
        o.startMoveTimeTicks = this.startMoveTimeTicks;
        o.firstAttackTimeTicks = this.firstAttackTimeTicks;
        o.atkIntervalTicks = this.atkIntervalTicks;
        o.restrictionTimeTicks = this.restrictionTimeTicks;
    }
    public string FindDiff(Fortress o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.cfgId != this.cfgId) return rootName + @"/cfgId is diff! 
this  = " + this.cfgId + @",
other = " + o.cfgId;
        if(o.startMoveTimeTicks != this.startMoveTimeTicks) return rootName + @"/startMoveTimeTicks is diff! 
this  = " + this.startMoveTimeTicks + @",
other = " + o.startMoveTimeTicks;
        if(o.firstAttackTimeTicks != this.firstAttackTimeTicks) return rootName + @"/firstAttackTimeTicks is diff! 
this  = " + this.firstAttackTimeTicks + @",
other = " + o.firstAttackTimeTicks;
        if(o.atkIntervalTicks != this.atkIntervalTicks) return rootName + @"/atkIntervalTicks is diff! 
this  = " + this.atkIntervalTicks + @",
other = " + o.atkIntervalTicks;
        if(o.restrictionTimeTicks != this.restrictionTimeTicks) return rootName + @"/restrictionTimeTicks is diff! 
this  = " + this.restrictionTimeTicks + @",
other = " + o.restrictionTimeTicks;
        return _rtv_;
    }

    public const short packageId = 11;
    public static readonly Fortress DefaultInstance = new Fortress();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Fortress());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(cfgId);
        _bb_.Write(startMoveTimeTicks);
        _bb_.Write(firstAttackTimeTicks);
        _bb_.Write(atkIntervalTicks);
        _bb_.Write(restrictionTimeTicks); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref cfgId);
        _bb_.Read(ref startMoveTimeTicks);
        _bb_.Read(ref firstAttackTimeTicks);
        _bb_.Read(ref atkIntervalTicks);
        _bb_.Read(ref restrictionTimeTicks);
    }

}
public partial class Gas : IBBReader, IBBWriter 
{
    public void OverrideTo(Gas o)
    {
        o.cfgId = this.cfgId;
        o.col = this.col;
        o.row = this.row;
    }
    public string FindDiff(Gas o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.cfgId != this.cfgId) return rootName + @"/cfgId is diff! 
this  = " + this.cfgId + @",
other = " + o.cfgId;
        if(o.col != this.col) return rootName + @"/col is diff! 
this  = " + this.col + @",
other = " + o.col;
        if(o.row != this.row) return rootName + @"/row is diff! 
this  = " + this.row + @",
other = " + o.row;
        return _rtv_;
    }

    public const short packageId = 4;
    public static readonly Gas DefaultInstance = new Gas();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Gas());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(cfgId);
        _bb_.Write(col);
        _bb_.Write(row); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref cfgId);
        _bb_.Read(ref col);
        _bb_.Read(ref row);
    }

}
public partial class GasArea : IBBReader, IBBWriter 
{
    public void OverrideTo(GasArea o)
    {
        o.gass = this.gass;
    }
    public string FindDiff(GasArea o, string rootName = "")
    {
        string _rtv_ = null;
        for (int _i_ = 0; _i_ < this.gass.Count; ++_i_)
        {
            _rtv_ = this.gass[_i_].FindDiff(o.gass[_i_], rootName + "/gass[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        return _rtv_;
    }

    public const short packageId = 5;
    public static readonly GasArea DefaultInstance = new GasArea();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new GasArea());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.Write(gass); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.Read(ref gass, 0, 0);
    }

}
public partial class Map : IBBReader, IBBWriter 
{
    public void OverrideTo(Map o)
    {
        o.cfgId = this.cfgId;
        o.areas = this.areas;
        o.areaSequence = this.areaSequence;
        o.currentAreaSequenceIndex = this.currentAreaSequenceIndex;
        o.gasScanIntervalTicks = this.gasScanIntervalTicks;
        o.gasSupplementIntervalTicks = this.gasSupplementIntervalTicks;
        o.gasNumSupplement = this.gasNumSupplement;
    }
    public string FindDiff(Map o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.cfgId != this.cfgId) return rootName + @"/cfgId is diff! 
this  = " + this.cfgId + @",
other = " + o.cfgId;
        for (int _i_ = 0; _i_ < this.areas.Count; ++_i_)
        {
            _rtv_ = this.areas[_i_].FindDiff(o.areas[_i_], rootName + "/areas[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.areaSequence.Count; ++_i_)
        {
            if(o.areaSequence[_i_] != this.areaSequence[_i_]) return rootName + @"/areaSequence[" + _i_ + @"] is diff! 
this  = " + this.areaSequence[_i_] + @",
other = " + o.areaSequence[_i_];
        }
        if(o.currentAreaSequenceIndex != this.currentAreaSequenceIndex) return rootName + @"/currentAreaSequenceIndex is diff! 
this  = " + this.currentAreaSequenceIndex + @",
other = " + o.currentAreaSequenceIndex;
        if(o.gasScanIntervalTicks != this.gasScanIntervalTicks) return rootName + @"/gasScanIntervalTicks is diff! 
this  = " + this.gasScanIntervalTicks + @",
other = " + o.gasScanIntervalTicks;
        if(o.gasSupplementIntervalTicks != this.gasSupplementIntervalTicks) return rootName + @"/gasSupplementIntervalTicks is diff! 
this  = " + this.gasSupplementIntervalTicks + @",
other = " + o.gasSupplementIntervalTicks;
        if(o.gasNumSupplement != this.gasNumSupplement) return rootName + @"/gasNumSupplement is diff! 
this  = " + this.gasNumSupplement + @",
other = " + o.gasNumSupplement;
        return _rtv_;
    }

    public const short packageId = 6;
    public static readonly Map DefaultInstance = new Map();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Map());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(cfgId);
        _bb_.Write(areas);
        _bb_.VarWrite(areaSequence);
        _bb_.VarWrite(currentAreaSequenceIndex);
        _bb_.VarWrite(gasScanIntervalTicks);
        _bb_.VarWrite(gasSupplementIntervalTicks);
        _bb_.VarWrite(gasNumSupplement); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref cfgId);
        _bb_.Read(ref areas, 0, 0);
        _bb_.VarRead(ref areaSequence, 0, 0);
        _bb_.VarRead(ref currentAreaSequenceIndex);
        _bb_.VarRead(ref gasScanIntervalTicks);
        _bb_.VarRead(ref gasSupplementIntervalTicks);
        _bb_.VarRead(ref gasNumSupplement);
    }

}
public partial class Player : IBBReader, IBBWriter 
{
    public void OverrideTo(Player o)
    {
        o.id = this.id;
        o.planeCfgId = this.planeCfgId;
        o.camp = this.camp;
        o.bornCDTicks = this.bornCDTicks;
        o.energy = this.energy;
        o.enhanceLevels = this.enhanceLevels;
        o.numKills = this.numKills;
        o.numDeads = this.numDeads;
        o.numHelpKill = this.numHelpKill;
    }
    public string FindDiff(Player o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.planeCfgId != this.planeCfgId) return rootName + @"/planeCfgId is diff! 
this  = " + this.planeCfgId + @",
other = " + o.planeCfgId;
        if(o.camp != this.camp) return rootName + @"/camp is diff! 
this  = " + this.camp + @",
other = " + o.camp;
        if(o.bornCDTicks != this.bornCDTicks) return rootName + @"/bornCDTicks is diff! 
this  = " + this.bornCDTicks + @",
other = " + o.bornCDTicks;
        if(o.energy != this.energy) return rootName + @"/energy is diff! 
this  = " + this.energy + @",
other = " + o.energy;
        for (int _i_ = 0; _i_ < this.enhanceLevels.Count; ++_i_)
        {
            if(o.enhanceLevels[_i_] != this.enhanceLevels[_i_]) return rootName + @"/enhanceLevels[" + _i_ + @"] is diff! 
this  = " + this.enhanceLevels[_i_] + @",
other = " + o.enhanceLevels[_i_];
        }
        if(o.numKills != this.numKills) return rootName + @"/numKills is diff! 
this  = " + this.numKills + @",
other = " + o.numKills;
        if(o.numDeads != this.numDeads) return rootName + @"/numDeads is diff! 
this  = " + this.numDeads + @",
other = " + o.numDeads;
        if(o.numHelpKill != this.numHelpKill) return rootName + @"/numHelpKill is diff! 
this  = " + this.numHelpKill + @",
other = " + o.numHelpKill;
        return _rtv_;
    }

    public const short packageId = 7;
    public static readonly Player DefaultInstance = new Player();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Player());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.VarWrite(planeCfgId);
        _bb_.WriteEnum(camp);
        _bb_.VarWrite(bornCDTicks);
        _bb_.VarWrite(energy);
        _bb_.VarWrite(enhanceLevels);
        _bb_.VarWrite(numKills);
        _bb_.VarWrite(numDeads);
        _bb_.VarWrite(numHelpKill); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.VarRead(ref planeCfgId);
        _bb_.ReadEnum(ref camp);
        _bb_.VarRead(ref bornCDTicks);
        _bb_.VarRead(ref energy);
        _bb_.VarRead(ref enhanceLevels, 0, 0);
        _bb_.VarRead(ref numKills);
        _bb_.VarRead(ref numDeads);
        _bb_.VarRead(ref numHelpKill);
    }

}
public partial class Scene : IBBReader, IBBWriter 
{
    public void OverrideTo(Scene o)
    {
        o.rnd = this.rnd;
        o.map = this.map;
        o.players = this.players;
        o.planes = this.planes;
        o.currentPlayerId = this.currentPlayerId;
        o.ticks = this.ticks;
    }
    public string FindDiff(Scene o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = this.rnd.FindDiff(o.rnd, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        _rtv_ = this.map.FindDiff(o.map, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        for (int _i_ = 0; _i_ < this.players.Count; ++_i_)
        {
            _rtv_ = this.players[_i_].FindDiff(o.players[_i_], rootName + "/players[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.planes.Count; ++_i_)
        {
            _rtv_ = this.planes[_i_].FindDiff(o.planes[_i_], rootName + "/planes[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        if(o.currentPlayerId != this.currentPlayerId) return rootName + @"/currentPlayerId is diff! 
this  = " + this.currentPlayerId + @",
other = " + o.currentPlayerId;
        if(o.ticks != this.ticks) return rootName + @"/ticks is diff! 
this  = " + this.ticks + @",
other = " + o.ticks;
        return _rtv_;
    }

    public const short packageId = 8;
    public static readonly Scene DefaultInstance = new Scene();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Scene());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.Write(rnd);
        _bb_.Write(map);
        _bb_.Write(players);
        _bb_.WriteLength(planes.Count);
        for (int i = 0; i < planes.Count; ++i)
        {
            _bb_.WritePackage(planes[i]);
        }
        _bb_.Write(currentPlayerId);
        _bb_.Write(ticks); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.Read(ref rnd);
        _bb_.Read(ref map);
        _bb_.Read(ref players, 0, 0);
        {
            int _len_ = _bb_.ReadLength(0, 0);
            for (int _i_ = 0; _i_ < _len_; _i_++)
            {
                switch(_bb_.ReadPackageId())
                {
                    case Plane_Laser.packageId: planes.Add(_bb_.Read<Plane_Laser>()); break;
                    case Plane0.packageId: planes.Add(_bb_.Read<Plane0>()); break;
                    case Plane1.packageId: planes.Add(_bb_.Read<Plane1>()); break;
                    case Plane2.packageId: planes.Add(_bb_.Read<Plane2>()); break;
                    case Plane3.packageId: planes.Add(_bb_.Read<Plane3>()); break;
                }
            }
        }

        _bb_.Read(ref currentPlayerId);
        _bb_.Read(ref ticks);
    }

}
public partial class Plane : IBBReader, IBBWriter 
{
    public void OverrideTo(Plane o)
    {
        o.id = this.id;
        o.playerId = this.playerId;
        o.cfgId = this.cfgId;
        o.xy = this.xy;
        o.angle = this.angle;
        o.angleTarget = this.angleTarget;
        o.hp = this.hp;
        o.shield = this.shield;
        o.bornShieldLifespanTicks = this.bornShieldLifespanTicks;
        o.shieldRecoverIntervalTicks = this.shieldRecoverIntervalTicks;
        o.shieldBeginRecoverIntervalTicks = this.shieldBeginRecoverIntervalTicks;
        o.moving = this.moving;
        o.skillSwitches = this.skillSwitches;
        o.targetId = this.targetId;
    }
    public string FindDiff(Plane o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.id != this.id) return rootName + @"/id is diff! 
this  = " + this.id + @",
other = " + o.id;
        if(o.playerId != this.playerId) return rootName + @"/playerId is diff! 
this  = " + this.playerId + @",
other = " + o.playerId;
        if(o.cfgId != this.cfgId) return rootName + @"/cfgId is diff! 
this  = " + this.cfgId + @",
other = " + o.cfgId;
        _rtv_ = this.xy.FindDiff(o.xy, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        if(o.angle != this.angle) return rootName + @"/angle is diff! 
this  = " + this.angle + @",
other = " + o.angle;
        if(o.angleTarget != this.angleTarget) return rootName + @"/angleTarget is diff! 
this  = " + this.angleTarget + @",
other = " + o.angleTarget;
        if(o.hp != this.hp) return rootName + @"/hp is diff! 
this  = " + this.hp + @",
other = " + o.hp;
        if(o.shield != this.shield) return rootName + @"/shield is diff! 
this  = " + this.shield + @",
other = " + o.shield;
        if(o.bornShieldLifespanTicks != this.bornShieldLifespanTicks) return rootName + @"/bornShieldLifespanTicks is diff! 
this  = " + this.bornShieldLifespanTicks + @",
other = " + o.bornShieldLifespanTicks;
        if(o.shieldRecoverIntervalTicks != this.shieldRecoverIntervalTicks) return rootName + @"/shieldRecoverIntervalTicks is diff! 
this  = " + this.shieldRecoverIntervalTicks + @",
other = " + o.shieldRecoverIntervalTicks;
        if(o.shieldBeginRecoverIntervalTicks != this.shieldBeginRecoverIntervalTicks) return rootName + @"/shieldBeginRecoverIntervalTicks is diff! 
this  = " + this.shieldBeginRecoverIntervalTicks + @",
other = " + o.shieldBeginRecoverIntervalTicks;
        if(o.moving != this.moving) return rootName + @"/moving is diff! 
this  = " + this.moving + @",
other = " + o.moving;
        for (int _i_ = 0; _i_ < this.skillSwitches.Count; ++_i_)
        {
            if(o.skillSwitches[_i_] != this.skillSwitches[_i_]) return rootName + @"/skillSwitches[" + _i_ + @"] is diff! 
this  = " + this.skillSwitches[_i_] + @",
other = " + o.skillSwitches[_i_];
        }
        if(o.targetId != this.targetId) return rootName + @"/targetId is diff! 
this  = " + this.targetId + @",
other = " + o.targetId;
        return _rtv_;
    }

    public const short packageId = 9;
    public static readonly Plane DefaultInstance = new Plane();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(id);
        _bb_.VarWrite(playerId);
        _bb_.VarWrite(cfgId);
        _bb_.Write(xy);
        _bb_.Write(angle);
        _bb_.Write(angleTarget);
        _bb_.VarWrite(hp);
        _bb_.VarWrite(shield);
        _bb_.Write(bornShieldLifespanTicks);
        _bb_.Write(shieldRecoverIntervalTicks);
        _bb_.Write(shieldBeginRecoverIntervalTicks);
        _bb_.Write(moving);
        _bb_.Write(skillSwitches);
        _bb_.VarWrite(targetId); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref id);
        _bb_.VarRead(ref playerId);
        _bb_.VarRead(ref cfgId);
        _bb_.Read(ref xy);
        _bb_.Read(ref angle);
        if (float.IsNaN(angle) || float.IsInfinity(angle)) throw new Exception("the angle can't be NaN or Infinity.");
        _bb_.Read(ref angleTarget);
        if (float.IsNaN(angleTarget) || float.IsInfinity(angleTarget)) throw new Exception("the angleTarget can't be NaN or Infinity.");
        _bb_.VarRead(ref hp);
        _bb_.VarRead(ref shield);
        _bb_.Read(ref bornShieldLifespanTicks);
        _bb_.Read(ref shieldRecoverIntervalTicks);
        _bb_.Read(ref shieldBeginRecoverIntervalTicks);
        _bb_.Read(ref moving);
        _bb_.Read(ref skillSwitches, 0, 0);
        _bb_.VarRead(ref targetId);
    }

}
public partial class Plane_Laser : IBBReader, IBBWriter 
{
    public new void OverrideTo(Plane_Laser o)
    {
        base.OverrideTo(o);
        o.fireTargetId = this.fireTargetId;
        o.fireBeforeTicks = this.fireBeforeTicks;
        o.fireAfterTicks = this.fireAfterTicks;
        o.fireCDTicks = this.fireCDTicks;
        o.fireCancel = this.fireCancel;
    }
    public new string FindDiff(Plane_Laser o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = base.FindDiff(o, rootName + "/../");
        if(_rtv_ != null) return _rtv_;
        if(o.fireTargetId != this.fireTargetId) return rootName + @"/fireTargetId is diff! 
this  = " + this.fireTargetId + @",
other = " + o.fireTargetId;
        if(o.fireBeforeTicks != this.fireBeforeTicks) return rootName + @"/fireBeforeTicks is diff! 
this  = " + this.fireBeforeTicks + @",
other = " + o.fireBeforeTicks;
        if(o.fireAfterTicks != this.fireAfterTicks) return rootName + @"/fireAfterTicks is diff! 
this  = " + this.fireAfterTicks + @",
other = " + o.fireAfterTicks;
        if(o.fireCDTicks != this.fireCDTicks) return rootName + @"/fireCDTicks is diff! 
this  = " + this.fireCDTicks + @",
other = " + o.fireCDTicks;
        if(o.fireCancel != this.fireCancel) return rootName + @"/fireCancel is diff! 
this  = " + this.fireCancel + @",
other = " + o.fireCancel;
        return _rtv_;
    }

    public new const short packageId = 10;
    public static readonly new Plane_Laser DefaultInstance = new Plane_Laser();
    public static readonly new byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane_Laser());

    public new short PackageId { get { return packageId; } }

    public new void WriteTo(ByteBuffer _bb_)
    {
        base.WriteTo(_bb_);
        _bb_.VarWrite(fireTargetId);
        _bb_.VarWrite(fireBeforeTicks);
        _bb_.VarWrite(fireAfterTicks);
        _bb_.VarWrite(fireCDTicks);
        _bb_.Write(fireCancel); 
    }

    public new void ReadFrom(ByteBuffer _bb_)
    {
        base.ReadFrom(_bb_);
        _bb_.VarRead(ref fireTargetId);
        _bb_.VarRead(ref fireBeforeTicks);
        _bb_.VarRead(ref fireAfterTicks);
        _bb_.VarRead(ref fireCDTicks);
        _bb_.Read(ref fireCancel);
    }

}
public partial class Plane0 : IBBReader, IBBWriter 
{
    public new void OverrideTo(Plane0 o)
    {
        base.OverrideTo(o);
    }
    public new string FindDiff(Plane0 o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = base.FindDiff(o, rootName + "/../");
        if(_rtv_ != null) return _rtv_;
        return _rtv_;
    }

    public new const short packageId = 1;
    public static readonly new Plane0 DefaultInstance = new Plane0();
    public static readonly new byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane0());

    public new short PackageId { get { return packageId; } }

    public new void WriteTo(ByteBuffer _bb_)
    {
        base.WriteTo(_bb_); 
    }

    public new void ReadFrom(ByteBuffer _bb_)
    {
        base.ReadFrom(_bb_);
    }

}
public partial class Plane1 : IBBReader, IBBWriter 
{
    public new void OverrideTo(Plane1 o)
    {
        base.OverrideTo(o);
        o.castCDTicks = this.castCDTicks;
    }
    public new string FindDiff(Plane1 o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = base.FindDiff(o, rootName + "/../");
        if(_rtv_ != null) return _rtv_;
        if(o.castCDTicks != this.castCDTicks) return rootName + @"/castCDTicks is diff! 
this  = " + this.castCDTicks + @",
other = " + o.castCDTicks;
        return _rtv_;
    }

    public new const short packageId = 2;
    public static readonly new Plane1 DefaultInstance = new Plane1();
    public static readonly new byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane1());

    public new short PackageId { get { return packageId; } }

    public new void WriteTo(ByteBuffer _bb_)
    {
        base.WriteTo(_bb_);
        _bb_.VarWrite(castCDTicks); 
    }

    public new void ReadFrom(ByteBuffer _bb_)
    {
        base.ReadFrom(_bb_);
        _bb_.VarRead(ref castCDTicks);
    }

}
public partial class Plane2 : IBBReader, IBBWriter 
{
    public new void OverrideTo(Plane2 o)
    {
        base.OverrideTo(o);
        o.casting = this.casting;
        o.castCDTicks = this.castCDTicks;
    }
    public new string FindDiff(Plane2 o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = base.FindDiff(o, rootName + "/../");
        if(_rtv_ != null) return _rtv_;
        if(o.casting != this.casting) return rootName + @"/casting is diff! 
this  = " + this.casting + @",
other = " + o.casting;
        if(o.castCDTicks != this.castCDTicks) return rootName + @"/castCDTicks is diff! 
this  = " + this.castCDTicks + @",
other = " + o.castCDTicks;
        return _rtv_;
    }

    public new const short packageId = 3;
    public static readonly new Plane2 DefaultInstance = new Plane2();
    public static readonly new byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane2());

    public new short PackageId { get { return packageId; } }

    public new void WriteTo(ByteBuffer _bb_)
    {
        base.WriteTo(_bb_);
        _bb_.Write(casting);
        _bb_.VarWrite(castCDTicks); 
    }

    public new void ReadFrom(ByteBuffer _bb_)
    {
        base.ReadFrom(_bb_);
        _bb_.Read(ref casting);
        _bb_.VarRead(ref castCDTicks);
    }

}
public partial class Plane3 : IBBReader, IBBWriter 
{
    public new void OverrideTo(Plane3 o)
    {
        base.OverrideTo(o);
        o.castLeftFireTimes = this.castLeftFireTimes;
        o.castCDTicks = this.castCDTicks;
    }
    public new string FindDiff(Plane3 o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = base.FindDiff(o, rootName + "/../");
        if(_rtv_ != null) return _rtv_;
        if(o.castLeftFireTimes != this.castLeftFireTimes) return rootName + @"/castLeftFireTimes is diff! 
this  = " + this.castLeftFireTimes + @",
other = " + o.castLeftFireTimes;
        if(o.castCDTicks != this.castCDTicks) return rootName + @"/castCDTicks is diff! 
this  = " + this.castCDTicks + @",
other = " + o.castCDTicks;
        return _rtv_;
    }

    public new const short packageId = 12;
    public static readonly new Plane3 DefaultInstance = new Plane3();
    public static readonly new byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Plane3());

    public new short PackageId { get { return packageId; } }

    public new void WriteTo(ByteBuffer _bb_)
    {
        base.WriteTo(_bb_);
        _bb_.VarWrite(castLeftFireTimes);
        _bb_.VarWrite(castCDTicks); 
    }

    public new void ReadFrom(ByteBuffer _bb_)
    {
        base.ReadFrom(_bb_);
        _bb_.VarRead(ref castLeftFireTimes);
        _bb_.VarRead(ref castCDTicks);
    }

}
} // PT
