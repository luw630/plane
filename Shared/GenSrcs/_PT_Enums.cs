using System;
namespace PT
{
    public enum FortressStates : byte
    {
        WaitMove = 0, 
        Move = 1, 
        Stop = 2, 
    }
    /// <summary>
    /// 阵营
    /// </summary>
    public enum Camps : byte
    {
        /// <summary>
        /// 联盟
        /// </summary>
        Alliance = 0, 
        /// <summary>
        /// 部落
        /// </summary>
        Horde = 1, 
        /// <summary>
        /// 中立
        /// </summary>
        Neutral = 2, 
    }
}
