﻿using System;
using System.Runtime.InteropServices;

namespace Vintagestory.API.MathTools
{

    [StructLayout(LayoutKind.Explicit)]
    public struct VSColor
    {
        [FieldOffset(0)]
        public int AsInt;
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;
        [FieldOffset(3)]
        public byte A;


        public float Rn
        {
            get { return R / 255f; }
            set { R = (byte)GameMath.Clamp(value * 255, 0, 255); }
        }
        public float Gn
        {
            get { return G / 255f; }
            set { G = (byte)GameMath.Clamp(value * 255, 0, 255); }
        }
        public float Bn
        {
            get { return B / 255f; }
            set { B = (byte)GameMath.Clamp(value * 255, 0, 255); }
        }
        public float An
        {
            get { return A / 255f; }
            set { A = (byte)GameMath.Clamp(value * 255, 0, 255); }
        }


        public VSColor(int color) : this()
        {
            AsInt = color;
        }

        public VSColor(byte r, byte g, byte b, byte a) : this()
        {
            AsInt = r | (g << 8) | (b << 16) | (a << 24);
        }
    }

    /// <summary>
    /// Specifies types of per-pixel color blending.
    /// </summary>
    [DocumentAsJson]
    public enum EnumColorBlendMode
    {
        Normal = 0,
        Darken = 1,
        Lighten = 2,
        Multiply = 3,
        Screen = 4,
        ColorDodge = 5,
        ColorBurn = 6,
        Overlay = 7,
        OverlayCutout = 8
    }


    public static class ColorBlend
    {
        public delegate int ColorBlendDelegate(int col1, int col2);

        static ColorBlendDelegate[] Blenders;
        static ColorBlend()
        {
            Blenders = new ColorBlendDelegate[]
            {
                Normal,
                Darken,
                Lighten,
                Multiply,
                Screen,
                ColorDodge,
                ColorBurn,
                Overlay,
                OverlayCutout
            };
        }

        public static int Blend(EnumColorBlendMode blendMode, int colorBase, int colorOver)
        {
            return Blenders[(int)blendMode](colorBase, colorOver);
        }

        public static int Normal(int rgb1, int rgb2)
        {
            return ColorUtil.ColorOver(rgb2, rgb1);
        }


        // Autogenerated code copypasted from Paint.NET source code from 2008 (https://github.com/rivy/OpenPDN/blob/cca476b0df2a2f70996e6b9486ec45327631568c/src/Data/UserBlendOps.Generated.cs) which is under MIT license (https://github.com/rivy/OpenPDN/blob/master/src/Resources/Files/License.txt)
        // We should clean up this code and optimize it

        // i = z * 3;
        // (x / z) = ((x * masTable[i]) + masTable[i + 1]) >> masTable[i + 2)
        private static readonly uint[] masTable =
        {
            0x00000000, 0x00000000, 0,  // 0
            0x00000001, 0x00000000, 0,  // 1
            0x00000001, 0x00000000, 1,  // 2
            0xAAAAAAAB, 0x00000000, 33, // 3
            0x00000001, 0x00000000, 2,  // 4
            0xCCCCCCCD, 0x00000000, 34, // 5
            0xAAAAAAAB, 0x00000000, 34, // 6
            0x49249249, 0x49249249, 33, // 7
            0x00000001, 0x00000000, 3,  // 8
            0x38E38E39, 0x00000000, 33, // 9
            0xCCCCCCCD, 0x00000000, 35, // 10
            0xBA2E8BA3, 0x00000000, 35, // 11
            0xAAAAAAAB, 0x00000000, 35, // 12
            0x4EC4EC4F, 0x00000000, 34, // 13
            0x49249249, 0x49249249, 34, // 14
            0x88888889, 0x00000000, 35, // 15
            0x00000001, 0x00000000, 4,  // 16
            0xF0F0F0F1, 0x00000000, 36, // 17
            0x38E38E39, 0x00000000, 34, // 18
            0xD79435E5, 0xD79435E5, 36, // 19
            0xCCCCCCCD, 0x00000000, 36, // 20
            0xC30C30C3, 0xC30C30C3, 36, // 21
            0xBA2E8BA3, 0x00000000, 36, // 22
            0xB21642C9, 0x00000000, 36, // 23
            0xAAAAAAAB, 0x00000000, 36, // 24
            0x51EB851F, 0x00000000, 35, // 25
            0x4EC4EC4F, 0x00000000, 35, // 26
            0x97B425ED, 0x97B425ED, 36, // 27
            0x49249249, 0x49249249, 35, // 28
            0x8D3DCB09, 0x00000000, 36, // 29
            0x88888889, 0x00000000, 36, // 30
            0x42108421, 0x42108421, 35, // 31
            0x00000001, 0x00000000, 5,  // 32
            0x3E0F83E1, 0x00000000, 35, // 33
            0xF0F0F0F1, 0x00000000, 37, // 34
            0x75075075, 0x75075075, 36, // 35
            0x38E38E39, 0x00000000, 35, // 36
            0x6EB3E453, 0x6EB3E453, 36, // 37
            0xD79435E5, 0xD79435E5, 37, // 38
            0x69069069, 0x69069069, 36, // 39
            0xCCCCCCCD, 0x00000000, 37, // 40
            0xC7CE0C7D, 0x00000000, 37, // 41
            0xC30C30C3, 0xC30C30C3, 37, // 42
            0x2FA0BE83, 0x00000000, 35, // 43
            0xBA2E8BA3, 0x00000000, 37, // 44
            0x5B05B05B, 0x5B05B05B, 36, // 45
            0xB21642C9, 0x00000000, 37, // 46
            0xAE4C415D, 0x00000000, 37, // 47
            0xAAAAAAAB, 0x00000000, 37, // 48
            0x5397829D, 0x00000000, 36, // 49
            0x51EB851F, 0x00000000, 36, // 50
            0xA0A0A0A1, 0x00000000, 37, // 51
            0x4EC4EC4F, 0x00000000, 36, // 52
            0x9A90E7D9, 0x9A90E7D9, 37, // 53
            0x97B425ED, 0x97B425ED, 37, // 54
            0x94F2094F, 0x94F2094F, 37, // 55
            0x49249249, 0x49249249, 36, // 56
            0x47DC11F7, 0x47DC11F7, 36, // 57
            0x8D3DCB09, 0x00000000, 37, // 58
            0x22B63CBF, 0x00000000, 35, // 59
            0x88888889, 0x00000000, 37, // 60
            0x4325C53F, 0x00000000, 36, // 61
            0x42108421, 0x42108421, 36, // 62
            0x41041041, 0x41041041, 36, // 63
            0x00000001, 0x00000000, 6,  // 64
            0xFC0FC0FD, 0x00000000, 38, // 65
            0x3E0F83E1, 0x00000000, 36, // 66
            0x07A44C6B, 0x00000000, 33, // 67
            0xF0F0F0F1, 0x00000000, 38, // 68
            0x76B981DB, 0x00000000, 37, // 69
            0x75075075, 0x75075075, 37, // 70
            0xE6C2B449, 0x00000000, 38, // 71
            0x38E38E39, 0x00000000, 36, // 72
            0x381C0E07, 0x381C0E07, 36, // 73
            0x6EB3E453, 0x6EB3E453, 37, // 74
            0x1B4E81B5, 0x00000000, 35, // 75
            0xD79435E5, 0xD79435E5, 38, // 76
            0x3531DEC1, 0x00000000, 36, // 77
            0x69069069, 0x69069069, 37, // 78
            0xCF6474A9, 0x00000000, 38, // 79
            0xCCCCCCCD, 0x00000000, 38, // 80
            0xCA4587E7, 0x00000000, 38, // 81
            0xC7CE0C7D, 0x00000000, 38, // 82
            0x3159721F, 0x00000000, 36, // 83
            0xC30C30C3, 0xC30C30C3, 38, // 84
            0xC0C0C0C1, 0x00000000, 38, // 85
            0x2FA0BE83, 0x00000000, 36, // 86
            0x2F149903, 0x00000000, 36, // 87
            0xBA2E8BA3, 0x00000000, 38, // 88
            0xB81702E1, 0x00000000, 38, // 89
            0x5B05B05B, 0x5B05B05B, 37, // 90
            0x2D02D02D, 0x2D02D02D, 36, // 91
            0xB21642C9, 0x00000000, 38, // 92
            0xB02C0B03, 0x00000000, 38, // 93
            0xAE4C415D, 0x00000000, 38, // 94
            0x2B1DA461, 0x2B1DA461, 36, // 95
            0xAAAAAAAB, 0x00000000, 38, // 96
            0xA8E83F57, 0xA8E83F57, 38, // 97
            0x5397829D, 0x00000000, 37, // 98
            0xA57EB503, 0x00000000, 38, // 99
            0x51EB851F, 0x00000000, 37, // 100
            0xA237C32B, 0xA237C32B, 38, // 101
            0xA0A0A0A1, 0x00000000, 38, // 102
            0x9F1165E7, 0x9F1165E7, 38, // 103
            0x4EC4EC4F, 0x00000000, 37, // 104
            0x27027027, 0x27027027, 36, // 105
            0x9A90E7D9, 0x9A90E7D9, 38, // 106
            0x991F1A51, 0x991F1A51, 38, // 107
            0x97B425ED, 0x97B425ED, 38, // 108
            0x2593F69B, 0x2593F69B, 36, // 109
            0x94F2094F, 0x94F2094F, 38, // 110
            0x24E6A171, 0x24E6A171, 36, // 111
            0x49249249, 0x49249249, 37, // 112
            0x90FDBC09, 0x90FDBC09, 38, // 113
            0x47DC11F7, 0x47DC11F7, 37, // 114
            0x8E78356D, 0x8E78356D, 38, // 115
            0x8D3DCB09, 0x00000000, 38, // 116
            0x23023023, 0x23023023, 36, // 117
            0x22B63CBF, 0x00000000, 36, // 118
            0x44D72045, 0x00000000, 37, // 119
            0x88888889, 0x00000000, 38, // 120
            0x8767AB5F, 0x8767AB5F, 38, // 121
            0x4325C53F, 0x00000000, 37, // 122
            0x85340853, 0x85340853, 38, // 123
            0x42108421, 0x42108421, 37, // 124
            0x10624DD3, 0x00000000, 35, // 125
            0x41041041, 0x41041041, 37, // 126
            0x10204081, 0x10204081, 35, // 127
            0x00000001, 0x00000000, 7,  // 128
            0x0FE03F81, 0x00000000, 35, // 129
            0xFC0FC0FD, 0x00000000, 39, // 130
            0xFA232CF3, 0x00000000, 39, // 131
            0x3E0F83E1, 0x00000000, 37, // 132
            0xF6603D99, 0x00000000, 39, // 133
            0x07A44C6B, 0x00000000, 34, // 134
            0xF2B9D649, 0x00000000, 39, // 135
            0xF0F0F0F1, 0x00000000, 39, // 136
            0x077975B9, 0x00000000, 34, // 137
            0x76B981DB, 0x00000000, 38, // 138
            0x75DED953, 0x00000000, 38, // 139
            0x75075075, 0x75075075, 38, // 140
            0x3A196B1F, 0x00000000, 37, // 141
            0xE6C2B449, 0x00000000, 39, // 142
            0xE525982B, 0x00000000, 39, // 143
            0x38E38E39, 0x00000000, 37, // 144
            0xE1FC780F, 0x00000000, 39, // 145
            0x381C0E07, 0x381C0E07, 37, // 146
            0xDEE95C4D, 0x00000000, 39, // 147
            0x6EB3E453, 0x6EB3E453, 38, // 148
            0xDBEB61EF, 0x00000000, 39, // 149
            0x1B4E81B5, 0x00000000, 36, // 150
            0x36406C81, 0x00000000, 37, // 151
            0xD79435E5, 0xD79435E5, 39, // 152
            0xD62B80D7, 0x00000000, 39, // 153
            0x3531DEC1, 0x00000000, 37, // 154
            0xD3680D37, 0x00000000, 39, // 155
            0x69069069, 0x69069069, 38, // 156
            0x342DA7F3, 0x00000000, 37, // 157
            0xCF6474A9, 0x00000000, 39, // 158
            0xCE168A77, 0xCE168A77, 39, // 159
            0xCCCCCCCD, 0x00000000, 39, // 160
            0xCB8727C1, 0x00000000, 39, // 161
            0xCA4587E7, 0x00000000, 39, // 162
            0xC907DA4F, 0x00000000, 39, // 163
            0xC7CE0C7D, 0x00000000, 39, // 164
            0x634C0635, 0x00000000, 38, // 165
            0x3159721F, 0x00000000, 37, // 166
            0x621B97C3, 0x00000000, 38, // 167
            0xC30C30C3, 0xC30C30C3, 39, // 168
            0x60F25DEB, 0x00000000, 38, // 169
            0xC0C0C0C1, 0x00000000, 39, // 170
            0x17F405FD, 0x17F405FD, 36, // 171
            0x2FA0BE83, 0x00000000, 37, // 172
            0xBD691047, 0xBD691047, 39, // 173
            0x2F149903, 0x00000000, 37, // 174
            0x5D9F7391, 0x00000000, 38, // 175
            0xBA2E8BA3, 0x00000000, 39, // 176
            0x5C90A1FD, 0x5C90A1FD, 38, // 177
            0xB81702E1, 0x00000000, 39, // 178
            0x5B87DDAD, 0x5B87DDAD, 38, // 179
            0x5B05B05B, 0x5B05B05B, 38, // 180
            0xB509E68B, 0x00000000, 39, // 181
            0x2D02D02D, 0x2D02D02D, 37, // 182
            0xB30F6353, 0x00000000, 39, // 183
            0xB21642C9, 0x00000000, 39, // 184
            0x1623FA77, 0x1623FA77, 36, // 185
            0xB02C0B03, 0x00000000, 39, // 186
            0xAF3ADDC7, 0x00000000, 39, // 187
            0xAE4C415D, 0x00000000, 39, // 188
            0x15AC056B, 0x15AC056B, 36, // 189
            0x2B1DA461, 0x2B1DA461, 37, // 190
            0xAB8F69E3, 0x00000000, 39, // 191
            0xAAAAAAAB, 0x00000000, 39, // 192
            0x15390949, 0x00000000, 36, // 193
            0xA8E83F57, 0xA8E83F57, 39, // 194
            0x15015015, 0x15015015, 36, // 195
            0x5397829D, 0x00000000, 38, // 196
            0xA655C439, 0xA655C439, 39, // 197
            0xA57EB503, 0x00000000, 39, // 198
            0x5254E78F, 0x00000000, 38, // 199
            0x51EB851F, 0x00000000, 38, // 200
            0x028C1979, 0x00000000, 33, // 201
            0xA237C32B, 0xA237C32B, 39, // 202
            0xA16B312F, 0x00000000, 39, // 203
            0xA0A0A0A1, 0x00000000, 39, // 204
            0x4FEC04FF, 0x00000000, 38, // 205
            0x9F1165E7, 0x9F1165E7, 39, // 206
            0x27932B49, 0x00000000, 37, // 207
            0x4EC4EC4F, 0x00000000, 38, // 208
            0x9CC8E161, 0x00000000, 39, // 209
            0x27027027, 0x27027027, 37, // 210
            0x9B4C6F9F, 0x00000000, 39, // 211
            0x9A90E7D9, 0x9A90E7D9, 39, // 212
            0x99D722DB, 0x00000000, 39, // 213
            0x991F1A51, 0x991F1A51, 39, // 214
            0x4C346405, 0x00000000, 38, // 215
            0x97B425ED, 0x97B425ED, 39, // 216
            0x4B809701, 0x4B809701, 38, // 217
            0x2593F69B, 0x2593F69B, 37, // 218
            0x12B404AD, 0x12B404AD, 36, // 219
            0x94F2094F, 0x94F2094F, 39, // 220
            0x25116025, 0x25116025, 37, // 221
            0x24E6A171, 0x24E6A171, 37, // 222
            0x24BC44E1, 0x24BC44E1, 37, // 223
            0x49249249, 0x49249249, 38, // 224
            0x91A2B3C5, 0x00000000, 39, // 225
            0x90FDBC09, 0x90FDBC09, 39, // 226
            0x905A3863, 0x905A3863, 39, // 227
            0x47DC11F7, 0x47DC11F7, 38, // 228
            0x478BBCED, 0x00000000, 38, // 229
            0x8E78356D, 0x8E78356D, 39, // 230
            0x46ED2901, 0x46ED2901, 38, // 231
            0x8D3DCB09, 0x00000000, 39, // 232
            0x2328A701, 0x2328A701, 37, // 233
            0x23023023, 0x23023023, 37, // 234
            0x45B81A25, 0x45B81A25, 38, // 235
            0x22B63CBF, 0x00000000, 37, // 236
            0x08A42F87, 0x08A42F87, 35, // 237
            0x44D72045, 0x00000000, 38, // 238
            0x891AC73B, 0x00000000, 39, // 239
            0x88888889, 0x00000000, 39, // 240
            0x10FEF011, 0x00000000, 36, // 241
            0x8767AB5F, 0x8767AB5F, 39, // 242
            0x86D90545, 0x00000000, 39, // 243
            0x4325C53F, 0x00000000, 38, // 244
            0x85BF3761, 0x85BF3761, 39, // 245
            0x85340853, 0x85340853, 39, // 246
            0x10953F39, 0x10953F39, 36, // 247
            0x42108421, 0x42108421, 38, // 248
            0x41CC9829, 0x41CC9829, 38, // 249
            0x10624DD3, 0x00000000, 36, // 250
            0x828CBFBF, 0x00000000, 39, // 251
            0x41041041, 0x41041041, 38, // 252
            0x81848DA9, 0x00000000, 39, // 253
            0x10204081, 0x10204081, 36, // 254
            0x80808081, 0x00000000, 39  // 255
        };

        public static int Overlay(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            rhs.Rn *= rhs.An;
            rhs.Gn *= rhs.An;
            rhs.Bn *= rhs.An;

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    if (((lhs).B) < 128)
                    {
                        {
                            fB = ((2 * ((lhs).B)) * (((rhs).B)) + 0x80);
                            fB = ((((fB) >> 8) + (fB)) >> 8);
                        };
                    }
                    else
                    {
                        {
                            fB = ((2 * (255 - ((lhs).B))) * (255 - ((rhs).B)) + 0x80);
                            fB = ((((fB) >> 8) + (fB)) >> 8);
                        };
                        fB = 255 - fB;
                    }
                };
                {
                    if (((lhs).G) < 128)
                    {
                        {
                            fG = ((2 * ((lhs).G)) * (((rhs).G)) + 0x80);
                            fG = ((((fG) >> 8) + (fG)) >> 8);
                        };
                    }
                    else
                    {
                        {
                            fG = ((2 * (255 - ((lhs).G))) * (255 - ((rhs).G)) + 0x80);
                            fG = ((((fG) >> 8) + (fG)) >> 8);
                        };
                        fG = 255 - fG;
                    }
                };
                {
                    if (((lhs).R) < 128)
                    {
                        {
                            fR = ((2 * ((lhs).R)) * (((rhs).R)) + 0x80);
                            fR = ((((fR) >> 8) + (fR)) >> 8);
                        };
                    }
                    else
                    {
                        {
                            fR = ((2 * (255 - ((lhs).R))) * (255 - ((rhs).R)) + 0x80);
                            fR = ((((fR) >> 8) + (fR)) >> 8);
                        };
                        fR = 255 - fR;
                    }
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                };

                //ret = b + (g << 8) + (r << 16) + ((uint)a << 24);
                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };

            return (int)ret;
        }

        public static int Darken(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    fB = Math.Min((lhs).B, (rhs).B);
                };
                {
                    fG = Math.Min((lhs).G, (rhs).G);
                };
                {
                    fR = Math.Min((lhs).R, (rhs).R);
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                };

                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };

            return (int)ret;
        }

        public static int Lighten(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    fB = Math.Max((lhs).B, (rhs).B);
                };
                {
                    fG = Math.Max((lhs).G, (rhs).G);
                };
                {
                    fR = Math.Max((lhs).R, (rhs).R);
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                };
                //ret = b + (g << 8) + (r << 16) + ((uint)a << 24);
                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };
            
            //return ColorBgra.FromUInt32(ret);
            return (int)ret;
        }

        public static int Multiply(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    {
                        fB = ((((lhs).B)) * (((rhs).B)) + 0x80);
                        fB = ((((fB) >> 8) + (fB)) >> 8);
                    };
                };
                {
                    {
                        fG = ((((lhs).G)) * (((rhs).G)) + 0x80);
                        fG = ((((fG) >> 8) + (fG)) >> 8);
                    };
                };
                {
                    {
                        fR = ((((lhs).R)) * (((rhs).R)) + 0x80);
                        fR = ((((fR) >> 8) + (fR)) >> 8);
                    };
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                }; 
                
                //ret = b + (g << 8) + (r << 16) + ((uint)a << 24);
                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };

            //return ColorBgra.FromUInt32(ret);
            return (int)ret;
        }

        public static int Screen(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    {
                        fB = ((((rhs).B)) * (((lhs).B)) + 0x80);
                        fB = ((((fB) >> 8) + (fB)) >> 8);
                    };
                    fB = ((rhs).B) + ((lhs).B) - fB;
                };
                {
                    {
                        fG = ((((rhs).G)) * (((lhs).G)) + 0x80);
                        fG = ((((fG) >> 8) + (fG)) >> 8);
                    };
                    fG = ((rhs).G) + ((lhs).G) - fG;
                };
                {
                    {
                        fR = ((((rhs).R)) * (((lhs).R)) + 0x80);
                        fR = ((((fR) >> 8) + (fR)) >> 8);
                    };
                    fR = ((rhs).R) + ((lhs).R) - fR;
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                };
                //ret = b + (g << 8) + (r << 16) + ((uint)a << 24);
                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };

            //return ColorBgra.FromUInt32(ret);
            return (int)ret;
        }

        public static int ColorDodge(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    if (((rhs).B) == 255)
                    {
                        fB = 255;
                    }
                    else
                    {
                        {
                            int i = ((255 - ((rhs).B))) * 3;
                            uint M = masTable[i];
                            uint A = masTable[i + 1];
                            uint S = masTable[i + 2];
                            fB = (int)((((((lhs).B) * 255) * M) + A) >> (int)S);
                        };
                        fB = Math.Min(255, fB);
                    }
                };
                {
                    if (((rhs).G) == 255)
                    {
                        fG = 255;
                    }
                    else
                    {
                        {
                            int i = ((255 - ((rhs).G))) * 3;
                            uint M = masTable[i];
                            uint A = masTable[i + 1];
                            uint S = masTable[i + 2];
                            fG = (int)((((((lhs).G) * 255) * M) + A) >> (int)S);
                        };
                        fG = Math.Min(255, fG);
                    }
                };
                {
                    if (((rhs).R) == 255)
                    {
                        fR = 255;
                    }
                    else
                    {
                        {
                            int i = ((255 - ((rhs).R))) * 3;
                            uint M = masTable[i];
                            uint A = masTable[i + 1];
                            uint S = masTable[i + 2];
                            fR = (int)((((((lhs).R) * 255) * M) + A) >> (int)S);
                        };
                        fR = Math.Min(255, fR);
                    }
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                };

                //ret = b + (g << 8) + (r << 16) + ((uint)a << 24);
                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };

            return (int)ret;
        }

        public static int ColorBurn(int rgb1, int rgb2)
        {
            VSColor lhs = new VSColor(rgb1);
            VSColor rhs = new VSColor(rgb2);

            int lhsA;
            {
                lhsA = ((lhs).A);
            };
            int rhsA;
            {
                rhsA = ((rhs).A);
            };
            int y;
            {
                y = ((lhsA) * (255 - rhsA) + 0x80);
                y = ((((y) >> 8) + (y)) >> 8);
            };
            int totalA = y + rhsA;
            uint ret;
            if (totalA == 0)
            {
                ret = 0;
            }
            else
            {
                int fB;
                int fG;
                int fR;
                {
                    if (((rhs).B) == 0)
                    {
                        fB = 0;
                    }
                    else
                    {
                        {
                            int i = (((rhs).B)) * 3;
                            uint M = masTable[i];
                            uint A = masTable[i + 1];
                            uint S = masTable[i + 2];
                            fB = (int)(((((255 - ((lhs).B)) * 255) * M) + A) >> (int)S);
                        };
                        fB = 255 - fB;
                        fB = Math.Max(0, fB);
                    }
                };
                {
                    if (((rhs).G) == 0)
                    {
                        fG = 0;
                    }
                    else
                    {
                        {
                            int i = (((rhs).G)) * 3;
                            uint M = masTable[i];
                            uint A = masTable[i + 1];
                            uint S = masTable[i + 2];
                            fG = (int)(((((255 - ((lhs).G)) * 255) * M) + A) >> (int)S);
                        };
                        fG = 255 - fG;
                        fG = Math.Max(0, fG);
                    }
                };
                {
                    if (((rhs).R) == 0)
                    {
                        fR = 0;
                    }
                    else
                    {
                        {
                            int i = (((rhs).R)) * 3;
                            uint M = masTable[i];
                            uint A = masTable[i + 1];
                            uint S = masTable[i + 2];
                            fR = (int)(((((255 - ((lhs).R)) * 255) * M) + A) >> (int)S);
                        };
                        fR = 255 - fR;
                        fR = Math.Max(0, fR);
                    }
                };
                int x;
                {
                    x = ((lhsA) * (rhsA) + 0x80);
                    x = ((((x) >> 8) + (x)) >> 8);
                };
                int z = rhsA - x;
                int masIndex = totalA * 3;
                uint taM = masTable[masIndex];
                uint taA = masTable[masIndex + 1];
                uint taS = masTable[masIndex + 2];
                uint b = (uint)(((((long)((((lhs).B * y) + ((rhs).B * z) + (fB * x)))) * taM) + taA) >> (int)taS);
                uint g = (uint)(((((long)((((lhs).G * y) + ((rhs).G * z) + (fG * x)))) * taM) + taA) >> (int)taS);
                uint r = (uint)(((((long)((((lhs).R * y) + ((rhs).R * z) + (fR * x)))) * taM) + taA) >> (int)taS);
                int a;
                {
                    {
                        a = ((lhsA) * (255 - (rhsA)) + 0x80);
                        a = ((((a) >> 8) + (a)) >> 8);
                    };
                    a += (rhsA);
                };

                //ret = b + (g << 8) + (r << 16) + ((uint)a << 24);
                ret = r + (g << 8) + (b << 16) + ((uint)a << 24);
            };

            return (int)ret;
        }

        public static int OverlayCutout(int rgb1, int rgb2)
        {
            VSColor lhs = new(rgb1);
            VSColor rhs = new(rgb2);
            if (rhs.A != 0)
            {
                lhs.A = 0;
            }
            return lhs.AsInt;
        }


    }
}
