using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMovies.Shared
{
    /// <summary>
    /// Used to select client or Server
    /// </summary>
    public enum AppMode
    {
        WebAssembly = 1,
        ServerSide = 2
    }
}