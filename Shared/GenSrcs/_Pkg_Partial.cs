#pragma warning disable 0109
using System;
using System.Collections.Generic;
using xxlib;
namespace Pkg
{
namespace CS
{
public partial class Join : IBBReader, IBBWriter 
{
    public void OverrideTo(Join o)
    {
    }
    public string FindDiff(Join o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 3;
    public static readonly Join DefaultInstance = new Join();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Join());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // CS
namespace CS
{
public partial class Quit : IBBReader, IBBWriter 
{
    public void OverrideTo(Quit o)
    {
    }
    public string FindDiff(Quit o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 4;
    public static readonly Quit DefaultInstance = new Quit();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Quit());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // CS
namespace CS
{
public partial class Rotate : IBBReader, IBBWriter 
{
    public void OverrideTo(Rotate o)
    {
        o.angle = this.angle;
    }
    public string FindDiff(Rotate o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.angle != this.angle) return rootName + @"/angle is diff! 
this  = " + this.angle + @",
other = " + o.angle;
        return _rtv_;
    }

    public const short packageId = 5;
    public static readonly Rotate DefaultInstance = new Rotate();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Rotate());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.Write(angle); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.Read(ref angle);
        if (float.IsNaN(angle) || float.IsInfinity(angle)) throw new Exception("the angle can't be NaN or Infinity.");
    }

}
} // CS
namespace CS
{
public partial class Move : IBBReader, IBBWriter 
{
    public void OverrideTo(Move o)
    {
    }
    public string FindDiff(Move o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 6;
    public static readonly Move DefaultInstance = new Move();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Move());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // CS
namespace CS
{
public partial class StopMove : IBBReader, IBBWriter 
{
    public void OverrideTo(StopMove o)
    {
    }
    public string FindDiff(StopMove o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 7;
    public static readonly StopMove DefaultInstance = new StopMove();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new StopMove());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // CS
namespace CS
{
public partial class Cast : IBBReader, IBBWriter 
{
    public void OverrideTo(Cast o)
    {
        o.index = this.index;
    }
    public string FindDiff(Cast o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.index != this.index) return rootName + @"/index is diff! 
this  = " + this.index + @",
other = " + o.index;
        return _rtv_;
    }

    public const short packageId = 1;
    public static readonly Cast DefaultInstance = new Cast();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Cast());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(index); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref index);
    }

}
} // CS
namespace CS
{
public partial class StopCast : IBBReader, IBBWriter 
{
    public void OverrideTo(StopCast o)
    {
        o.index = this.index;
    }
    public string FindDiff(StopCast o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.index != this.index) return rootName + @"/index is diff! 
this  = " + this.index + @",
other = " + o.index;
        return _rtv_;
    }

    public const short packageId = 2;
    public static readonly StopCast DefaultInstance = new StopCast();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new StopCast());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(index); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref index);
    }

}
} // CS
namespace CS
{
public partial class Aim : IBBReader, IBBWriter 
{
    public void OverrideTo(Aim o)
    {
        o.planeId = this.planeId;
    }
    public string FindDiff(Aim o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.planeId != this.planeId) return rootName + @"/planeId is diff! 
this  = " + this.planeId + @",
other = " + o.planeId;
        return _rtv_;
    }

    public const short packageId = 11;
    public static readonly Aim DefaultInstance = new Aim();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Aim());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(planeId); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref planeId);
    }

}
} // CS
namespace CS
{
public partial class CancelAim : IBBReader, IBBWriter 
{
    public void OverrideTo(CancelAim o)
    {
    }
    public string FindDiff(CancelAim o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 12;
    public static readonly CancelAim DefaultInstance = new CancelAim();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new CancelAim());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // CS
namespace CS
{
public partial class Evolution : IBBReader, IBBWriter 
{
    public void OverrideTo(Evolution o)
    {
        o.index = this.index;
    }
    public string FindDiff(Evolution o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.index != this.index) return rootName + @"/index is diff! 
this  = " + this.index + @",
other = " + o.index;
        return _rtv_;
    }

    public const short packageId = 8;
    public static readonly Evolution DefaultInstance = new Evolution();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Evolution());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(index); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref index);
    }

}
} // CS
namespace CS
{
public partial class Enhance : IBBReader, IBBWriter 
{
    public void OverrideTo(Enhance o)
    {
        o.index = this.index;
    }
    public string FindDiff(Enhance o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.index != this.index) return rootName + @"/index is diff! 
this  = " + this.index + @",
other = " + o.index;
        return _rtv_;
    }

    public const short packageId = 9;
    public static readonly Enhance DefaultInstance = new Enhance();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Enhance());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(index); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref index);
    }

}
} // CS
namespace SC
{
public partial class Join : IBBReader, IBBWriter 
{
    public void OverrideTo(Join o)
    {
        o.selfId = this.selfId;
    }
    public string FindDiff(Join o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.selfId != this.selfId) return rootName + @"/selfId is diff! 
this  = " + this.selfId + @",
other = " + o.selfId;
        return _rtv_;
    }

    public const short packageId = 13;
    public static readonly Join DefaultInstance = new Join();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Join());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(selfId); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref selfId);
    }

}
} // SC
namespace SC
{
public partial class FullSync : IBBReader, IBBWriter 
{
    public void OverrideTo(FullSync o)
    {
        o.scene = this.scene;
    }
    public string FindDiff(FullSync o, string rootName = "")
    {
        string _rtv_ = null;
        _rtv_ = this.scene.FindDiff(o.scene, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        return _rtv_;
    }

    public const short packageId = 14;
    public static readonly FullSync DefaultInstance = new FullSync();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new FullSync());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.Write(scene); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.Read(ref scene);
    }

}
} // SC
namespace SC
{
public partial class EventSync : IBBReader, IBBWriter 
{
    public void OverrideTo(EventSync o)
    {
        o.ticks = this.ticks;
        o.joins = this.joins;
        o.quits = this.quits;
        o.rotates = this.rotates;
        o.casts = this.casts;
        o.stopCasts = this.stopCasts;
        o.moves = this.moves;
        o.stopMoves = this.stopMoves;
        o.aims = this.aims;
        o.enhances = this.enhances;
        o.evolutions = this.evolutions;
        o.debugInfo = this.debugInfo;
    }
    public string FindDiff(EventSync o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.ticks != this.ticks) return rootName + @"/ticks is diff! 
this  = " + this.ticks + @",
other = " + o.ticks;
        if(o.joins != this.joins) return rootName + @"/joins is diff! 
this  = " + this.joins + @",
other = " + o.joins;
        for (int _i_ = 0; _i_ < this.quits.Count; ++_i_)
        {
            if(o.quits[_i_] != this.quits[_i_]) return rootName + @"/quits[" + _i_ + @"] is diff! 
this  = " + this.quits[_i_] + @",
other = " + o.quits[_i_];
        }
        for (int _i_ = 0; _i_ < this.rotates.Count; ++_i_)
        {
            _rtv_ = this.rotates[_i_].FindDiff(o.rotates[_i_], rootName + "/rotates[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.casts.Count; ++_i_)
        {
            _rtv_ = this.casts[_i_].FindDiff(o.casts[_i_], rootName + "/casts[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.stopCasts.Count; ++_i_)
        {
            _rtv_ = this.stopCasts[_i_].FindDiff(o.stopCasts[_i_], rootName + "/stopCasts[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.moves.Count; ++_i_)
        {
            if(o.moves[_i_] != this.moves[_i_]) return rootName + @"/moves[" + _i_ + @"] is diff! 
this  = " + this.moves[_i_] + @",
other = " + o.moves[_i_];
        }
        for (int _i_ = 0; _i_ < this.stopMoves.Count; ++_i_)
        {
            if(o.stopMoves[_i_] != this.stopMoves[_i_]) return rootName + @"/stopMoves[" + _i_ + @"] is diff! 
this  = " + this.stopMoves[_i_] + @",
other = " + o.stopMoves[_i_];
        }
        for (int _i_ = 0; _i_ < this.aims.Count; ++_i_)
        {
            _rtv_ = this.aims[_i_].FindDiff(o.aims[_i_], rootName + "/aims[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.enhances.Count; ++_i_)
        {
            _rtv_ = this.enhances[_i_].FindDiff(o.enhances[_i_], rootName + "/enhances[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        for (int _i_ = 0; _i_ < this.evolutions.Count; ++_i_)
        {
            _rtv_ = this.evolutions[_i_].FindDiff(o.evolutions[_i_], rootName + "/evolutions[" + _i_ + "]/");
            if(_rtv_ != null) return _rtv_;
        }
        _rtv_ = this.debugInfo.FindDiff(o.debugInfo, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        return _rtv_;
    }

    public const short packageId = 15;
    public static readonly EventSync DefaultInstance = new EventSync();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new EventSync());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(ticks);
        _bb_.VarWrite(joins);
        _bb_.VarWrite(quits);
        _bb_.Write(rotates);
        _bb_.Write(casts);
        _bb_.Write(stopCasts);
        _bb_.VarWrite(moves);
        _bb_.VarWrite(stopMoves);
        _bb_.Write(aims);
        _bb_.Write(enhances);
        _bb_.Write(evolutions);
        _bb_.Write(debugInfo); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref ticks);
        _bb_.VarRead(ref joins);
        _bb_.VarRead(ref quits, 0, 0);
        _bb_.Read(ref rotates, 0, 0);
        _bb_.Read(ref casts, 0, 0);
        _bb_.Read(ref stopCasts, 0, 0);
        _bb_.VarRead(ref moves, 0, 0);
        _bb_.VarRead(ref stopMoves, 0, 0);
        _bb_.Read(ref aims, 0, 0);
        _bb_.Read(ref enhances, 0, 0);
        _bb_.Read(ref evolutions, 0, 0);
        _bb_.Read(ref debugInfo, 0, 0);
    }

}
} // SC
namespace SC.Events
{
public partial class Rotate : IBBReader, IBBWriter 
{
    public void OverrideTo(Rotate o)
    {
        o.playerId = this.playerId;
        o.angle = this.angle;
        o.xyIncBase = this.xyIncBase;
    }
    public string FindDiff(Rotate o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.playerId != this.playerId) return rootName + @"/playerId is diff! 
this  = " + this.playerId + @",
other = " + o.playerId;
        if(o.angle != this.angle) return rootName + @"/angle is diff! 
this  = " + this.angle + @",
other = " + o.angle;
        _rtv_ = this.xyIncBase.FindDiff(o.xyIncBase, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(playerId);
        _bb_.Write(angle);
        _bb_.Write(xyIncBase); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref playerId);
        _bb_.Read(ref angle);
        if (float.IsNaN(angle) || float.IsInfinity(angle)) throw new Exception("the angle can't be NaN or Infinity.");
        _bb_.Read(ref xyIncBase);
    }

}
} // SC.Events
namespace SC.Events
{
public partial class Aim : IBBReader, IBBWriter 
{
    public void OverrideTo(Aim o)
    {
        o.playerId = this.playerId;
        o.planeId = this.planeId;
    }
    public string FindDiff(Aim o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.playerId != this.playerId) return rootName + @"/playerId is diff! 
this  = " + this.playerId + @",
other = " + o.playerId;
        if(o.planeId != this.planeId) return rootName + @"/planeId is diff! 
this  = " + this.planeId + @",
other = " + o.planeId;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(playerId);
        _bb_.VarWrite(planeId); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref playerId);
        _bb_.VarRead(ref planeId);
    }

}
} // SC.Events
namespace SC.Events
{
public partial class PlayerAction : IBBReader, IBBWriter 
{
    public void OverrideTo(PlayerAction o)
    {
        o.playerId = this.playerId;
        o.index = this.index;
    }
    public string FindDiff(PlayerAction o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.playerId != this.playerId) return rootName + @"/playerId is diff! 
this  = " + this.playerId + @",
other = " + o.playerId;
        if(o.index != this.index) return rootName + @"/index is diff! 
this  = " + this.index + @",
other = " + o.index;
        return _rtv_;
    }


    public short PackageId { get { throw new Exception("struct has no package id"); } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.VarWrite(playerId);
        _bb_.VarWrite(index); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.VarRead(ref playerId);
        _bb_.VarRead(ref index);
    }

}
} // SC.Events
namespace Sim
{
public partial class Connect : IBBReader, IBBWriter 
{
    public void OverrideTo(Connect o)
    {
    }
    public string FindDiff(Connect o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 16;
    public static readonly Connect DefaultInstance = new Connect();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Connect());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // Sim
namespace Sim
{
public partial class Disconnect : IBBReader, IBBWriter 
{
    public void OverrideTo(Disconnect o)
    {
    }
    public string FindDiff(Disconnect o, string rootName = "")
    {
        string _rtv_ = null;
        return _rtv_;
    }

    public const short packageId = 17;
    public static readonly Disconnect DefaultInstance = new Disconnect();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Disconnect());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    { 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
    }

}
} // Sim
namespace Sim
{
public partial class Rotate : IBBReader, IBBWriter 
{
    public void OverrideTo(Rotate o)
    {
        o.angle = this.angle;
        o.xyIncBase = this.xyIncBase;
    }
    public string FindDiff(Rotate o, string rootName = "")
    {
        string _rtv_ = null;
        if(o.angle != this.angle) return rootName + @"/angle is diff! 
this  = " + this.angle + @",
other = " + o.angle;
        _rtv_ = this.xyIncBase.FindDiff(o.xyIncBase, rootName + "/");
        if(_rtv_ != null) return _rtv_;
        return _rtv_;
    }

    public const short packageId = 10;
    public static readonly Rotate DefaultInstance = new Rotate();
    public static readonly byte[] DefaultPackageData = ByteBuffer.MakePackageData(new Rotate());

    public short PackageId { get { return packageId; } }

    public void WriteTo(ByteBuffer _bb_)
    {
        _bb_.Write(angle);
        _bb_.Write(xyIncBase); 
    }

    public void ReadFrom(ByteBuffer _bb_)
    {
        _bb_.Read(ref angle);
        if (float.IsNaN(angle) || float.IsInfinity(angle)) throw new Exception("the angle can't be NaN or Infinity.");
        _bb_.Read(ref xyIncBase);
    }

}
} // Sim
} // Pkg
