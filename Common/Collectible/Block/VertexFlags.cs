﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common
{
    public enum EnumWindBitMode
    {
        /// <summary>
        /// Not affected by wind
        /// </summary>
        NoWind = 0,
        /// <summary>
        /// Slightly affected by wind. Wiggle + Height bend based on ground distance.
        /// </summary>
        WeakWind = 1,
        /// <summary>
        /// Normally affected by wind. Wiggle + Height bend based on ground distance.
        /// </summary>
        NormalWind = 2,
        /// <summary>
        /// Same as normal wind, but with some special behavior for leaves. Wiggle + Height bend based on ground distance.
        /// </summary>
        Leaves = 3,
        /// <summary>
        /// Same as normal wind, but no wiggle. Weak height bend based on ground distance.
        /// </summary>
        Bend = 4,
        /// <summary>
        /// Bend behavior for tall plants
        /// </summary>
        TallBend = 5,
        /// <summary>
        /// Vertical wiggle
        /// </summary>
        Water = 6,
        ExtraWeakWind = 7,
        Fruit = 8,
        WeakWindNoBend = 9,
        WeakWindInverseBend = 10
    }

    public static class EnumWindBitModeMask
    {
        /// <summary>
        /// Slightly affected by wind. Wiggle + Height bend based on ground distance.<br/>
        /// </summary>
        public const int WeakWind = 1 << VertexFlags.WindModeBitsPos;
        /// <summary>
        /// Normally affected by wind. Wiggle + Height bend based on ground distance.
        /// </summary>
        public const int NormalWind = 2 << VertexFlags.WindModeBitsPos;
        /// <summary>
        /// Same as normal wind, but with some special behavior for leaves. Wiggle + Height bend based on ground distance.
        /// </summary>
        public const int Leaves = 3 << VertexFlags.WindModeBitsPos;
        /// <summary>
        /// Same as weak wind, but no wiggle. Height bend based on ground distance.
        /// </summary>
        public const int Bend = 4 << VertexFlags.WindModeBitsPos;
        /// <summary>
        /// Bend behavior for tall plants
        /// </summary> 
        public const int TallBend = 5 << VertexFlags.WindModeBitsPos;
        /// <summary>
        /// Vertical wiggle
        /// </summary>
        public const int Water = 6 << VertexFlags.WindModeBitsPos;
        /// <summary>
        /// Vertical wiggle
        /// </summary>
        public const int ExtraWeakWind = 7 << VertexFlags.WindModeBitsPos;

        public const int Fruit = 8 << VertexFlags.WindModeBitsPos;

        public const int WeakWindNoBend = 9 << VertexFlags.WindModeBitsPos;
    }

    /// <summary>
    /// Special class to handle the vertex flagging in a very nicely compressed space.<br/>
    /// Bit 0-7: Glow level<br/>
    /// Bit 8-10: Z-Offset<br/>
    /// Bit 11: Reflective bit<br/>
    /// Bit 12: Lod 0 Bit<br/>
    /// Bit 13-24: X/Y/Z Normals<br/>
    /// Bit 25, 26, 27, 28: Wind mode<br/>
    /// Bit 29, 30, 31: Wind data<br/>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VertexFlags
    {
        /// <summary>
        /// Bit 0..7
        /// </summary>
        public const int GlowLevelBitMask = 0xFF;

        public const int ZOffsetBitPos = 8;
        /// <summary>
        /// Bit 8..10
        /// </summary>
        public const int ZOffsetBitMask = 0x7 << ZOffsetBitPos;

        /// <summary>
        /// Bit 11
        /// </summary>
        public const int ReflectiveBitMask = 1 << 11;
        /// <summary>
        /// Bit 12
        /// </summary>
        public const int Lod0BitMask = 1 << 12;

        public const int NormalBitPos = 13;
        /// <summary>
        /// Bit 13..25
        /// </summary>
        public const int NormalBitMask = 0xFFF << NormalBitPos;

        /// <summary>
        /// Bit 25..28
        /// </summary>
        public const int WindModeBitsMask = 0xF << 25;

        public const int WindModeBitsPos = 25;

        /// <summary>
        /// Bit 29..31
        /// </summary>
        public const int WindDataBitsMask = 0x7 << 29;

        public const int WindDataBitsPos = 29;

        /// <summary>
        /// Bit 26..31
        /// </summary>
        public const int WindBitsMask = WindModeBitsMask | WindDataBitsMask;


        // Bit 25..28
        public const int LiquidWaterModeBitMask = 0x7 << 25;
        // Bit 29
        public const int LiquidExposedToSkyBitMask = 1 << 29;



        public const int ClearWindBitsMask = ~WindBitsMask;
        public const int ClearWindModeBitsMask = ~WindModeBitsMask;
        public const int ClearWindDataBitsMask = ~WindDataBitsMask;
        public const int ClearZOffsetMask = ~ZOffsetBitMask;
        public const int ClearNormalBitMask = ~NormalBitMask;


        int all;

        byte glowLevel, zOffset;
        bool reflective;
        bool lod0;
        short normal;
        EnumWindBitMode windMode;
        byte windData;


        [JsonProperty]
        public int All
        {
            get
            {
                return all;
            }
            set
            {
                glowLevel = (byte)(value & 0xFF);
                zOffset = (byte)((value >> 8) & 0x7);
                reflective = ((value >> 11) & 1) != 0;
                lod0 = ((value >> 12) & 1) != 0;
                normal = (short)((value >> 13) & 0xFFF);
                windMode = (EnumWindBitMode)((value >> 25) & 0xF);
                windData = (byte)((value >> 29) & 0x7);
                all = value;
            }
        }

        #region Normal stuff

        /// <summary>
        /// Creates an already bit shifted normal
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static int PackNormal(Vec3d normal)
        {
            return PackNormal(normal.X, normal.Y, normal.Z);
        }

        /// <summary>
        /// Creates an already bit shifted normal
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static int PackNormal(double x, double y, double z)
        {
            int xN = (int)(x * 7.000001) * 2;  //a small bump up for rounding, as the x, y, z values may be 1/7th fractions rounded down in binary
            int yN = (int)(y * 7.000001) * 2;
            int zN = (int)(z * 7.000001) * 2;  //can be any even number between -14 and +14

            return
                (xN < 0 ? 1 - xN : xN) << NormalBitPos |
                (yN < 0 ? 1 - yN : yN) << (NormalBitPos + 4) |
                (zN < 0 ? 1 - zN : zN) << (NormalBitPos + 8)
            ;
        }

        /// <summary>
        /// Creates an already bit shifted normal
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static int PackNormal(Vec3f normal)
        {
            int xN = (int)(normal.X * 7.000001f) * 2;  //a small bump up for rounding, as the x, y, z values may be 1/7th fractions rounded down in binary
            int yN = (int)(normal.Y * 7.000001f) * 2;
            int zN = (int)(normal.Z * 7.000001f) * 2;  //can be any even number between -14 and +14

            return
                (xN < 0 ? 1 - xN : xN) << NormalBitPos |
                (yN < 0 ? 1 - yN : yN) << (NormalBitPos + 4) |
                (zN < 0 ? 1 - zN : zN) << (NormalBitPos + 8)
            ;
        }

        /// <summary>
        /// Creates an already bit shifted normal
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static int PackNormal(Vec3i normal)
        {
            int xN = (int)(normal.X * 7.000001f) * 2;  // A small bump up for rounding, as the x, y, z values may be 1/7th fractions rounded down in binary
            int yN = (int)(normal.Y * 7.000001f) * 2;
            int zN = (int)(normal.Z * 7.000001f) * 2;  //can be any even number between -14 and +14
            
            return
                (xN < 0 ? 1 - xN : xN) << NormalBitPos |
                (yN < 0 ? 1 - yN : yN) << (NormalBitPos+4) |
                (zN < 0 ? 1 - zN : zN) << (NormalBitPos+8)
            ;
        }


        const int nValueBitMask = 0b1110;
        const int nXValueBitMask = nValueBitMask << (NormalBitPos);
        const int nYValueBitMask = nValueBitMask << (NormalBitPos + 4);
        const int nZValueBitMask = nValueBitMask << (NormalBitPos + 8);

        const int nXSignBitPos = NormalBitPos - 1;
        const int nYSignBitPos = NormalBitPos + 3;
        const int nZSignBitPos = NormalBitPos + 7;

        
        public static void UnpackNormal(int vertexFlags, float[] intoFloats)
        {
            // Trick to save one multiplication: Instead of bitshifting x/y/z we divide it at the end, as we have to divide by 14 anyway.
            // To get the sign bit to be 0 or 2, we bit shift it to the right by one
            int x = vertexFlags & nXValueBitMask;
            int y = vertexFlags & nYValueBitMask;
            int z = vertexFlags & nZValueBitMask;

            int signx = 1 - ((vertexFlags >> nXSignBitPos) & 2);
            int signy = 1 - ((vertexFlags >> nYSignBitPos) & 2);
            int signz = 1 - ((vertexFlags >> nZSignBitPos) & 2);

            intoFloats[0] = signx * x / (14f * (1 << NormalBitPos));
            intoFloats[1] = signy * y / (14f * 16 * (1 << NormalBitPos));
            intoFloats[2] = signz * z / (14f * 256 * (1 << NormalBitPos));
        }

        public static void UnpackNormal(int vertexFlags, double[] intoDouble)
        {
            int x = vertexFlags & nXValueBitMask;
            int y = vertexFlags & nYValueBitMask;
            int z = vertexFlags & nZValueBitMask;

            int signx = 1 - ((vertexFlags >> nXSignBitPos) & 2);
            int signy = 1 - ((vertexFlags >> nYSignBitPos) & 2);
            int signz = 1 - ((vertexFlags >> nZSignBitPos) & 2);

            intoDouble[0] = signx * x / (14f * (1 << NormalBitPos));
            intoDouble[1] = signy * y / (14f * 16 * (1 << NormalBitPos));
            intoDouble[2] = signz * z / (14f * 256 * (1 << NormalBitPos));
        }

        #endregion


        [JsonProperty]
        public byte GlowLevel
        {
            get
            {
                return glowLevel;
            }
            set
            {
                glowLevel = value;
                UpdateAll();
            }
        }

        [JsonProperty]
        public byte ZOffset
        {
            get
            {
                return zOffset;
            }
            set
            {
                zOffset = value;
                UpdateAll();
            }
        }

        [JsonProperty]
        public bool Reflective
        {
            get
            {
                return reflective;
            }
            set
            {
                reflective = value;
                UpdateAll();
            }
        }

        [JsonProperty]
        public bool Lod0
        {
            get
            {
                return lod0;
            }
            set
            {
                lod0 = value;
                UpdateAll();
            }
        }

        [JsonProperty]
        public short Normal
        {
            get
            {
                return normal;
            }
            set
            {
                normal = value;
                UpdateAll();
            }
        }

        [JsonProperty]
        public EnumWindBitMode WindMode
        {
            get
            {
                return windMode;
            }
            set
            {
                windMode = value;
                UpdateAll();
            }
        }

        [JsonProperty]
        public byte WindData
        {
            get { return windData; }
            set { windData = value; UpdateAll(); }
        }




        public VertexFlags()
        {

        }

        public VertexFlags(int flags)
        {
            All = flags;
        }

        void UpdateAll()
        {
            all = glowLevel
                  | ((zOffset & 0x7) << 8)
                  | (reflective ? 1 : 0) << 11
                  | (Lod0 ? 1 : 0) << 12
                  | ((normal & 0xFFF) << 13)
                  | (((int)windMode & 0xF) << 25)
                  | ((windData & 0x7) << 29)
            ;
        }

        /// <summary>
        /// Clones this set of vertex flags.  
        /// </summary>
        /// <returns></returns>
        public VertexFlags Clone()
        {
            return new VertexFlags(All);
        }

        public override string ToString()
        {
            return string.Format(
                "Glow: {0}, ZOffset: {1}, Reflective: {2}, Lod0: {3}, Normal: {4}, WindMode: {5}, WindData: {6}", 
                glowLevel, ZOffset, reflective, lod0, normal, WindMode, windData
            );
        }
    }
}

