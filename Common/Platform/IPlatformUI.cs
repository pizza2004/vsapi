﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common
{
    /// <summary>
    /// The platform interface for various controls.  Used by the game to handle various properties.
    /// </summary>
    public interface IXPlatformInterface
    {

        event EventHandler Exit;

        void SetClipboardText(string text);
        string GetClipboardText();
        void ShowMessageBox(string title, string text);

        Size2i GetScreenSize();

        IAviWriter GetAviWriter(int recordingBufferSize, double framerate, string codeccode);
        AvailableCodec[] AvailableCodecs();

        void DeleteFileToRecyclebin(string filepath);

        /// <summary>
        /// Total disk space in bytes
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        long GetFreeDiskSpace(string filepath);

        /// <summary>
        /// Total system ram in bytes
        /// </summary>
        /// <returns></returns>
        long GetRamCapacity();

        void FocusWindow();
    }

}
