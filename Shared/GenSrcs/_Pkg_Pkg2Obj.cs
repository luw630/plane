using System;
using System.Collections.Generic;
using xxlib;
using Pkg;
namespace Pkg
{
    public static partial class Pkg2Obj
    {
        public static T ConvertCore<T>(this ByteBuffer _bb_) where T : IBBReader, new()
        {
            var o = new T();
            _bb_.Read(ref o);
            return o;
        }

        public static IBBWriter Convert(this ByteBuffer _bb_)
        {
            try
            {
                switch(_bb_.ReadPackageId())
                {
                    case Pkg.CS.Join.packageId : return ConvertCore<Pkg.CS.Join>(_bb_);
                    case Pkg.CS.Quit.packageId : return ConvertCore<Pkg.CS.Quit>(_bb_);
                    case Pkg.CS.Rotate.packageId : return ConvertCore<Pkg.CS.Rotate>(_bb_);
                    case Pkg.CS.Move.packageId : return ConvertCore<Pkg.CS.Move>(_bb_);
                    case Pkg.CS.StopMove.packageId : return ConvertCore<Pkg.CS.StopMove>(_bb_);
                    case Pkg.CS.Cast.packageId : return ConvertCore<Pkg.CS.Cast>(_bb_);
                    case Pkg.CS.StopCast.packageId : return ConvertCore<Pkg.CS.StopCast>(_bb_);
                    case Pkg.CS.Aim.packageId : return ConvertCore<Pkg.CS.Aim>(_bb_);
                    case Pkg.CS.CancelAim.packageId : return ConvertCore<Pkg.CS.CancelAim>(_bb_);
                    case Pkg.CS.Evolution.packageId : return ConvertCore<Pkg.CS.Evolution>(_bb_);
                    case Pkg.CS.Enhance.packageId : return ConvertCore<Pkg.CS.Enhance>(_bb_);
                    case Pkg.SC.Join.packageId : return ConvertCore<Pkg.SC.Join>(_bb_);
                    case Pkg.SC.FullSync.packageId : return ConvertCore<Pkg.SC.FullSync>(_bb_);
                    case Pkg.SC.EventSync.packageId : return ConvertCore<Pkg.SC.EventSync>(_bb_);
                    case Pkg.Sim.Connect.packageId : return ConvertCore<Pkg.Sim.Connect>(_bb_);
                    case Pkg.Sim.Disconnect.packageId : return ConvertCore<Pkg.Sim.Disconnect>(_bb_);
                    case Pkg.Sim.Rotate.packageId : return ConvertCore<Pkg.Sim.Rotate>(_bb_);
                    default: return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static bool Convert<T>(ByteBuffer _bb_, List<T> output) where T : IBBWriter
        {
            output.Clear();
            while(_bb_.offset < _bb_.dataLen)
            {
                var o = Pkg.Pkg2Obj.Convert(_bb_);
                if (o != null) output.Add((T)o);
                else return false;
            }
            return true;
        }

    }
}