﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common
{
    /// <summary>
    /// The arguments from a client or sever command
    /// </summary>
    public class CmdArgs
    {
        List<string> args = new List<string>();

        /// <summary>
        /// Creates a new instance of the CmdArgs util with no arguments
        /// </summary>
        public CmdArgs()
        {
        }

        /// <summary>
        /// Creates a new instance of the CmdArgs util
        /// </summary>
        /// <param name="joinedargs"></param>
        public CmdArgs(string joinedargs)
        {
            Push(joinedargs);
        }

        public void Push(string joinedargs)
        {
            string[] args = new string[0];

            if (joinedargs.Length > 0)
            {
                args = joinedargs.Split(' ');
            }

            this.args.AddRange(args);
        }


        /// <summary>
        /// Creates a new instance of the CmdArgs util
        /// </summary>
        /// <param name="args"></param>
        public CmdArgs(string[] args)
        {
            this.args = new List<string>(args);
        }

        /// <summary>
        /// Returns the n-th arugment
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get { return args[index]; }
            set { args[index] = value; }
        }

        /// <summary>
        /// Amount of arguments passed
        /// </summary>
        public int Length
        {
            get { return args.Count; }
        }



        /// <summary>
        /// Returns all remaining arguments as single merged string, concatenated with spaces
        /// </summary>
        /// <returns></returns>
        public string PopAll()
        {
            var str = string.Join(" ", args.ToArray(), 0, args.Count);
            args.Clear();
            return str;
        }


        /// <summary>
        /// Returns the first char of the first argument
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public char? PeekChar(char? defaultValue = null)
        {
            if (args.Count == 0) return defaultValue;
            if (args[0].Length == 0) return (char)0;

            return args[0][0];
        }

        /// <summary>
        /// Remove the first character from the first argument and returns it
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public char? PopChar(char? defaultValue = null)
        {
            if (args.Count == 0) return defaultValue;
            string first = args[0];

            args[0] = args[0].Substring(1);

            return first[0];
        }

        /// <summary>
        /// Removes the first argument and returns it, scans until it encounters a white space
        /// </summary>
        public string PopWord(string defaultValue = null)
        {
            if (args.Count == 0) return defaultValue;
            string first = args[0];

            args.RemoveAt(0);

            return first;
        }


        /// <summary>
        /// Removes the first argument and returns it
        /// </summary>
        public string PeekWord(string defaultValue = null)
        {
            if (args.Count == 0) return defaultValue;
            string first = args[0];

            return first;
        }


        public string PopUntil(char endChar)
        {
            StringBuilder sb = new StringBuilder();
            string all = PopAll();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] == endChar) break;
                sb.Append(all[i]);
            }

            Push(all.Substring(sb.Length).TrimStart());
            return sb.ToString();
        }

        public string PopCodeBlock(char blockOpenChar, char blockCloseChar, out string parseErrorMsg)
        {
            parseErrorMsg = null;

            string all = PopAll();
            StringBuilder sb = new StringBuilder();
            int depth = 0;
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] == blockOpenChar) depth++;
                if (depth == 0)
                {
                    parseErrorMsg = Lang.Get("First character is not " + blockOpenChar + ". Please consume all input until the block open char");
                    return null;
                }

                if (depth > 0) sb.Append(all[i]);

                if (all[i] == blockCloseChar)
                {
                    depth--;
                    if (depth <= 0)
                    {
                        break;
                    }
                }
            }

            if (depth > 0)
            {
                parseErrorMsg = Lang.Get("Incomplete block. At least one " + blockCloseChar + " is missing");
                return null;
            }

            Push(all.Substring(sb.Length).TrimStart());
            return sb.ToString();
        }


        /// <summary>
        /// Adds an arg to the beginning
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public void PushSingle(string arg)
        {
            args.Insert(0, arg);
        }

        /// <summary>
        /// Adds an arg to the end
        /// </summary>
        /// <param name="arg"></param>
        public void AppendSingle(string arg)
        {
            args.Add(arg);
        }

        /// <summary>
        /// Tries to retrieve arg at given index as enum value or default if not enough arguments or not part of the enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T PopEnum<T>(T defaultValue = default(T))
        { 
            string arg = PopWord();
            if (arg == null) return defaultValue;

            int val;
            if (int.TryParse(arg, out val))
            {
                if (Enum.IsDefined(typeof(T), val))
                {
                    return (T)Enum.ToObject(typeof(T), val);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Tries to retrieve arg at given index as int, or null if not enough arguments or not an integer
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int? PopInt(int? defaultValue = null)
        {
            string arg = PopWord();
            if (arg == null) return defaultValue;

            int val;
            if (int.TryParse(arg, out val))
            {
                return val;
            }

            return defaultValue;
        }

        /// <summary>
        /// Tries to retrieve arg at given index as long, or null if not enough arguments or not a long
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long? PopLong(long? defaultValue = null)
        {
            string arg = PopWord();
            if (arg == null) return defaultValue;

            long val;
            if (long.TryParse(arg, out val))
            {
                return val;
            }

            return defaultValue;

        }


        /// <summary>
        /// Tries to retrieve arg at given index as boolean, or null if not enough arguments or not an integer
        /// <br/>'true', 'yes' and '1' will all be interpreted as true.  Parameter trueAlias (with default value 'on') allows one additional word to be used to signify true.  Anything else will return false.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool? PopBool(bool? defaultValue = null, string trueAlias = "on")
        {
            string arg = PopWord()?.ToLowerInvariant();
            if (arg == null) return defaultValue;

            return arg == "1" || arg == "yes" || arg == "true" || arg == trueAlias;
        }

        
        /// <summary>
        /// Tries to retrieve arg at given index as int, or null if not enough arguments or not an integer
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double? PopDouble(double? defaultValue = null)
        {
            string arg = PopWord();
            if (arg == null) return defaultValue;

            return arg.ToDoubleOrNull(defaultValue);
        }

        /// <summary>
        /// Tries to retrieve arg at given index as float, or null if not enough arguments or not a float
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float? PopFloat(float? defaultValue = null)
        {
            string arg = PopWord();
            if (arg == null) return defaultValue;

            return arg.ToFloatOrNull(defaultValue);
        }

        /// <summary>
        /// Tries to retrieve 3 int coordinates from the next 3 arguments
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Vec3i PopVec3i(Vec3i defaultValue = null)
        {
            int? x = PopInt(defaultValue?.X);
            int? y = PopInt(defaultValue?.Y);
            int? z = PopInt(defaultValue?.Z);

            if (x == null || y == null || z == null) return defaultValue;
            return new Vec3i((int)x, (int)y, (int)z);
        }


        public Vec3d PopVec3d(Vec3d defaultValue = null)
        {
            double? x = PopDouble(defaultValue?.X);
            double? y = PopDouble(defaultValue?.Y);
            double? z = PopDouble(defaultValue?.Z);

            if (x == null || y == null || z == null) return defaultValue;
            return new Vec3d((double)x, (double)y, (double)z);
        }


        /// <summary>
        /// Retrieves a player position with following syntax:
        /// [coord] [coord] [coord]
        /// whereas 
        /// [coord] may be ~[decimal]  or =[decimal] or [decimal]
        /// ~ denotes a position relative to the player
        /// = denotes an absolute position
        /// no prefix denots a position relative to the map middle
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="mapMiddle"></param>
        /// <returns></returns>
        public Vec3d PopFlexiblePos(Vec3d playerPos, Vec3d mapMiddle)
        {
            if (args.Count < 3) return null;

            Vec3d outPos = new Vec3d();
            double? val;

            for (int i = 0; i < 3; i++)
            {
                char modifier = (char)PeekChar();

                if (modifier == '~')
                {
                    PopChar();
                    if (PeekChar() != (char)0)
                    {
                        val = PopDouble();
                        if (val == null) return null;
                    }
                    else
                    {
                        val = 0;
                        PopWord();
                    }

                    outPos[i] = (double)val + playerPos[i];
                    continue;
                }

                if (modifier == '=')
                {
                    PopChar();
                    val = PopDouble();
                    if (val == null) return null;
                    outPos[i] = (double)val;
                    continue;
                }

                val = PopDouble();
                if (val == null) return null;
                outPos[i] = (double)val + mapMiddle[i];
            }

            return outPos;
        }


        /// <summary>
        /// Retrieves a player position with following syntax:
        /// [coord] [coord] [coord]
        /// whereas 
        /// [coord] may be ~[decimal]  or =[decimal] or [decimal]
        /// ~ denotes a position relative to the player
        /// = denotes an absolute position
        /// no prefix denots a position relative to the map middle
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="mapMiddle"></param>
        /// <returns></returns>
        public Vec2i PopFlexiblePos2D(Vec3d playerPos, Vec3d mapMiddle)
        {
            if (args.Count < 2) return null;

            Vec2i outPos = new Vec2i();
            double? val;

            for (int i = 0; i < 2; i++)
            {
                char modifier = (char)PeekChar();

                if (modifier == '~')
                {
                    PopChar();
                    if (PeekChar() != (char)0)
                    {
                        val = PopDouble();
                        if (val == null) return null;
                    }
                    else
                    {
                        val = 0;
                        PopWord();
                    }

                    outPos[i] = (int)((double)val + playerPos[i * 2]);   // * 2 because we want index 1 to be offset from the playerPos Z position
                    continue;
                }

                if (modifier == '=')
                {
                    PopChar();
                    val = PopDouble();
                    if (val == null) return null;
                    outPos[i] = (int)(double)val;
                    continue;
                }

                if (modifier == '+') PopChar();
                val = PopDouble();
                if (val == null) return null;
                outPos[i] = (int)((double)val + mapMiddle[i * 2]);   // * 2 because we want index 1 to be offset from the mapMiddle Z position
            }

            return outPos;
        }



        public CmdArgs Clone()
        {
            return new CmdArgs(args.ToArray());
        }

    }


}