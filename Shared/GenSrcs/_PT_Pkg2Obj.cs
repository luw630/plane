using System;
using System.Collections.Generic;
using xxlib;
using PT;
namespace PT
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
                    case PT.Fortress.packageId : return ConvertCore<PT.Fortress>(_bb_);
                    case PT.Gas.packageId : return ConvertCore<PT.Gas>(_bb_);
                    case PT.GasArea.packageId : return ConvertCore<PT.GasArea>(_bb_);
                    case PT.Map.packageId : return ConvertCore<PT.Map>(_bb_);
                    case PT.Player.packageId : return ConvertCore<PT.Player>(_bb_);
                    case PT.Scene.packageId : return ConvertCore<PT.Scene>(_bb_);
                    case PT.Plane.packageId : return ConvertCore<PT.Plane>(_bb_);
                    case PT.Plane_Laser.packageId : return ConvertCore<PT.Plane_Laser>(_bb_);
                    case PT.Plane0.packageId : return ConvertCore<PT.Plane0>(_bb_);
                    case PT.Plane1.packageId : return ConvertCore<PT.Plane1>(_bb_);
                    case PT.Plane2.packageId : return ConvertCore<PT.Plane2>(_bb_);
                    case PT.Plane3.packageId : return ConvertCore<PT.Plane3>(_bb_);
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
                var o = PT.Pkg2Obj.Convert(_bb_);
                if (o != null) output.Add((T)o);
                else return false;
            }
            return true;
        }

    }
}