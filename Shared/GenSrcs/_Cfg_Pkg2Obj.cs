using System;
using System.Collections.Generic;
using xxlib;
using Cfg;
namespace Cfg
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
                    case Cfg.Map.packageId : return ConvertCore<Cfg.Map>(_bb_);
                    case Cfg.Fire.packageId : return ConvertCore<Cfg.Fire>(_bb_);
                    case Cfg.Enhance.packageId : return ConvertCore<Cfg.Enhance>(_bb_);
                    case Cfg.Fortress.packageId : return ConvertCore<Cfg.Fortress>(_bb_);
                    case Cfg.Gas.packageId : return ConvertCore<Cfg.Gas>(_bb_);
                    case Cfg.SkillOffset.packageId : return ConvertCore<Cfg.SkillOffset>(_bb_);
                    case Cfg.Skill.packageId : return ConvertCore<Cfg.Skill>(_bb_);
                    case Cfg.Plane.packageId : return ConvertCore<Cfg.Plane>(_bb_);
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
                var o = Cfg.Pkg2Obj.Convert(_bb_);
                if (o != null) output.Add((T)o);
                else return false;
            }
            return true;
        }

    }
}