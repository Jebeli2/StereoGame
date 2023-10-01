namespace StereoGame.Framework.Platform.SDL
{
    using StereoGame.Framework.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO.Pipes;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class Sdl
    {
        private const string nativeLibName = "SDL2";

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_RWFromFile(string file, string mode);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_AllocRW();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_FreeRW(IntPtr area);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_RWFromMem(IntPtr mem, int size);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_RWFromMem([In()][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] mem, int size);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_RWFromConstMem(IntPtr mem, int size);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern long SDL_RWsize(IntPtr context);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_SetMainReady();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_Init(uint flags);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_InitSubSystem(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_QuitSubSystem(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint SDL_WasInit(uint flags);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetPlatform();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_ClearHints();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetHint(string name);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SDL_SetHint(string name, string value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SDL_SetHintWithPriority(string name, string value, SDL_HintPriority priority);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SDL_GetHintBoolean(string name, bool default_value);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_ClearError();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetError();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_SetError(string fmtAndArglist);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetErrorMsg(IntPtr errstr, int maxlength);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_GetVersion(out SDL_version ver);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetRevision();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_GetRevisionNumber();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_PumpEvents();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SDL_HasEvent(EventType type);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SDL_HasEvents(EventType minType, EventType maxType);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_FlushEvent(EventType type);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_FlushEvents(EventType min, EventType max);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_PollEvent(out Event _event);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_WaitEvent(out Event _event);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_WaitEventTimeout(out Event _event, int timeout);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_PushEvent(ref Event _event);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_DisableScreenSaver();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_EnableScreenSaver();
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_FreeSurface(IntPtr surface);


        public static IntPtr RWFromFile(string file, string mode) => SDL_RWFromFile(file, mode);
        public static IntPtr AllocRW() => SDL_AllocRW();
        public static void FreeRW(IntPtr area) => SDL_FreeRW(area);
        public static IntPtr RWFromMem(IntPtr mem, int size) => SDL_RWFromMem(mem, size);
        public static IntPtr RWFromMem(byte[] mem, int size) => SDL_RWFromMem(mem, size);
        public static IntPtr RWFromConstMem(IntPtr mem, int size) => SDL_RWFromConstMem(mem, size);
        public static long RWSize(IntPtr context) => SDL_RWsize(context);
        public static void SetMainReady() => SDL_SetMainReady();
        public static int Init(uint flags) => SDL_Init(flags);
        public static int InitSubSystem(uint flags) => SDL_InitSubSystem(flags);
        public static void Quit() => SDL_Quit();
        public static void QuitSubSystem(uint flags) => SDL_QuitSubSystem(flags);
        public static uint WasInit(uint flags) => SDL_WasInit(flags);
        public static string? GetPlatform() => InteropHelpers.Utf8ToString(SDL_GetPlatform());


        public static void ClearHints() => SDL_ClearHints();
        public static string? GetHint(string name) => InteropHelpers.Utf8ToString(SDL_GetHint(name));
        public static bool SetHint(string name, string value) => SDL_SetHint(name, value);
        public static bool SetHint(string name, int value) => SDL_SetHint(name, value.ToString());
        public static bool SetHintWithPriority(string name, string value, SDL_HintPriority priority) => SDL_SetHintWithPriority(name, value, priority);
        public static bool GetHintBoolean(string name, bool default_value) => SDL_GetHintBoolean(name, default_value);
        public static void ClearError() => SDL_ClearError();
        public static string? GetError() => InteropHelpers.Utf8ToString(SDL_GetError());
        public static void SetError(string fmtAndArglist) => SDL_SetError(fmtAndArglist);
        public static void GetVersion(out SDL_version ver) => SDL_GetVersion(out ver);
        public static string? GetRevision() => InteropHelpers.Utf8ToString(SDL_GetRevision());
        public static int GetRevisionNumber() => SDL_GetRevisionNumber();

        public static int PollEvent(out Event _event) => SDL_PollEvent(out _event);
        public static void PumpEvents() => SDL_PumpEvents();
        public static void DisableScreenSaver() => SDL_DisableScreenSaver();
        public static void EnableScreenSaver() => SDL_EnableScreenSaver();
        public static void FreeSurface(IntPtr surface) => SDL_FreeSurface(surface);

        public static IntPtr NativeLibrary = GetNativeLibrary();

        private static IntPtr GetNativeLibrary()
        {
            if (CurrentPlatform.OS == OS.Windows)
                return FuncLoader.LoadLibraryExt("SDL2.dll");
            else if (CurrentPlatform.OS == OS.Linux)
                return FuncLoader.LoadLibraryExt("libSDL2-2.0.so.0");
            else if (CurrentPlatform.OS == OS.MacOSX)
                return FuncLoader.LoadLibraryExt("libSDL2.dylib");
            else
                return FuncLoader.LoadLibraryExt("sdl2");
        }

        public static int Major;
        public static int Minor;
        public static int Patch;

        [Flags]
        public enum InitFlags
        {
            Audio = 0x00000010,
            Video = 0x00000020,
            Joystick = 0x00000200,
            Haptic = 0x00001000,
            GameController = 0x00002000,
        }

        public enum SDL_HintPriority
        {
            SDL_HINT_DEFAULT,
            SDL_HINT_NORMAL,
            SDL_HINT_OVERRIDE
        }

        public enum EventType : uint
        {
            First = 0,

            Quit = 0x100,

            WindowEvent = 0x200,
            SysWM = 0x201,

            KeyDown = 0x300,
            KeyUp = 0x301,
            TextEditing = 0x302,
            TextInput = 0x303,

            MouseMotion = 0x400,
            MouseButtonDown = 0x401,
            MouseButtonup = 0x402,
            MouseWheel = 0x403,

            JoyAxisMotion = 0x600,
            JoyBallMotion = 0x601,
            JoyHatMotion = 0x602,
            JoyButtonDown = 0x603,
            JoyButtonUp = 0x604,
            JoyDeviceAdded = 0x605,
            JoyDeviceRemoved = 0x606,

            ControllerAxisMotion = 0x650,
            ControllerButtonDown = 0x651,
            ControllerButtonUp = 0x652,
            ControllerDeviceAdded = 0x653,
            ControllerDeviceRemoved = 0x654,
            ControllerDeviceRemapped = 0x655,

            FingerDown = 0x700,
            FingerUp = 0x701,
            FingerMotion = 0x702,

            DollarGesture = 0x800,
            DollarRecord = 0x801,
            MultiGesture = 0x802,

            ClipboardUpdate = 0x900,

            DropFile = 0x1000,
            DropText = 0x1001,
            DropBegin = 0x1002,
            DropComplete = 0x1003,

            AudioDeviceAdded = 0x1100,
            AudioDeviceRemoved = 0x1101,

            RenderTargetsReset = 0x2000,
            RenderDeviceReset = 0x2001,

            UserEvent = 0x8000,

            Last = 0xFFFF
        }

        public enum EventAction
        {
            AddEvent = 0x0,
            PeekEvent = 0x1,
            GetEvent = 0x2,
        }

        [StructLayout(LayoutKind.Explicit, Size = 56)]
        public struct Event
        {
            [FieldOffset(0)]
            public EventType Type;
            [FieldOffset(0)]
            public Window.Event Window;
            [FieldOffset(0)]
            public Keyboard.Event Key;
            [FieldOffset(0)]
            public Mouse.MotionEvent Motion;
            [FieldOffset(0)]
            public Mouse.ButtonEvent Button;
            [FieldOffset(0)]
            public Keyboard.TextEditingEvent Edit;
            [FieldOffset(0)]
            public Keyboard.TextInputEvent Text;
            [FieldOffset(0)]
            public Mouse.WheelEvent Wheel;
            [FieldOffset(0)]
            public Joystick.DeviceEvent JoystickDevice;
            [FieldOffset(0)]
            public GameController.DeviceEvent ControllerDevice;
            [FieldOffset(0)]
            public Drop.Event Drop;
        }

        //public struct Rectangle
        //{
        //    public int X;
        //    public int Y;
        //    public int Width;
        //    public int Height;
        //}

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_version
        {
            public byte Major;
            public byte Minor;
            public byte Patch;
        }

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_init(int flags);
        //public static readonly d_sdl_init SDL_Init = FuncLoader.LoadFunction<d_sdl_init>(NativeLibrary, "SDL_Init");

        //public static void Init(int flags)
        //{
        //    GetError(SDL_Init(flags));
        //}

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_setmainready();
        //public static readonly d_sdl_setmainready SetMainReady = FuncLoader.LoadFunction<d_sdl_setmainready>(NativeLibrary, "SDL_SetMainReady");


        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_disablescreensaver();
        //public static readonly d_sdl_disablescreensaver DisableScreenSaver = FuncLoader.LoadFunction<d_sdl_disablescreensaver>(NativeLibrary, "SDL_DisableScreenSaver");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_getversion(out Version version);
        //public static readonly d_sdl_getversion GetVersion = FuncLoader.LoadFunction<d_sdl_getversion>(NativeLibrary, "SDL_GetVersion");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_pollevent([Out] out Event _event);
        //public static readonly d_sdl_pollevent PollEvent = FuncLoader.LoadFunction<d_sdl_pollevent>(NativeLibrary, "SDL_PollEvent");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_pumpevents();
        //public static readonly d_sdl_pumpevents PumpEvents = FuncLoader.LoadFunction<d_sdl_pumpevents>(NativeLibrary, "SDL_PumpEvents");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate IntPtr d_sdl_creatergbsurfacefrom(IntPtr pixels, int width, int height, int depth, int pitch, uint rMask, uint gMask, uint bMask, uint aMask);
        //private static readonly d_sdl_creatergbsurfacefrom SDL_CreateRGBSurfaceFrom = FuncLoader.LoadFunction<d_sdl_creatergbsurfacefrom>(NativeLibrary, "SDL_CreateRGBSurfaceFrom");

        //public static IntPtr CreateRGBSurfaceFrom(byte[] pixels, int width, int height, int depth, int pitch, uint rMask, uint gMask, uint bMask, uint aMask)
        //{
        //    var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
        //    try
        //    {
        //        return SDL_CreateRGBSurfaceFrom(handle.AddrOfPinnedObject(), width, height, depth, pitch, rMask, gMask, bMask, aMask);
        //    }
        //    finally
        //    {
        //        handle.Free();
        //    }
        //}

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_freesurface(IntPtr surface);
        //public static readonly d_sdl_freesurface FreeSurface = FuncLoader.LoadFunction<d_sdl_freesurface>(NativeLibrary, "SDL_FreeSurface");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate IntPtr d_sdl_geterror();
        //private static readonly d_sdl_geterror SDL_GetError = FuncLoader.LoadFunction<d_sdl_geterror>(NativeLibrary, "SDL_GetError");

        //public static string? GetError()
        //{
        //    return InteropHelpers.Utf8ToString(SDL_GetError());
        //}

        public static int GetError(int value)
        {
            if (value < 0)
                Debug.WriteLine(GetError());

            return value;
        }

        public static IntPtr GetError(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
                Debug.WriteLine(GetError());

            return pointer;
        }

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_clearerror();
        //public static readonly d_sdl_clearerror ClearError = FuncLoader.LoadFunction<d_sdl_clearerror>(NativeLibrary, "SDL_ClearError");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate IntPtr d_sdl_gethint(string name);
        //public static readonly d_sdl_gethint SDL_GetHint = FuncLoader.LoadFunction<d_sdl_gethint>(NativeLibrary, "SDL_GetHint");

        //public static string GetHint(string name)
        //{
        //    return InteropHelpers.Utf8ToString(SDL_GetHint(name)) ?? "";
        //}

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate IntPtr d_sdl_loadbmp_rw(IntPtr src, int freesrc);
        //private static readonly d_sdl_loadbmp_rw SDL_LoadBMP_RW = FuncLoader.LoadFunction<d_sdl_loadbmp_rw>(NativeLibrary, "SDL_LoadBMP_RW");

        //public static IntPtr LoadBMP_RW(IntPtr src, int freesrc)
        //{
        //    return GetError(SDL_LoadBMP_RW(src, freesrc));
        //}

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_quit();
        //public static readonly d_sdl_quit Quit = FuncLoader.LoadFunction<d_sdl_quit>(NativeLibrary, "SDL_Quit");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void d_sdl_freerw(IntPtr rw);
        //public static readonly d_sdl_freerw FreeRW = FuncLoader.LoadFunction<d_sdl_freerw>(NativeLibrary, "SDL_FreeRW");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate IntPtr d_sdl_rwfrommem(byte[] mem, int size);
        //private static readonly d_sdl_rwfrommem SDL_RWFromMem = FuncLoader.LoadFunction<d_sdl_rwfrommem>(NativeLibrary, "SDL_RWFromMem");

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate IntPtr d_sdl_rwfrommem2(IntPtr mem, int size);
        //private static readonly d_sdl_rwfrommem2 SDL_RWFromMem2 = FuncLoader.LoadFunction<d_sdl_rwfrommem2>(NativeLibrary, "SDL_RWFromMem");

        //public static IntPtr RwFromMem(byte[] mem, int size)
        //{
        //    return GetError(SDL_RWFromMem(mem, size));
        //}

        //public static IntPtr RwFromMem(IntPtr mem, int size)
        //{
        //    return GetError(SDL_RWFromMem2(mem, size));
        //}

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate int d_sdl_sethint(string name, string value);
        //public static readonly d_sdl_sethint SetHint = FuncLoader.LoadFunction<d_sdl_sethint>(NativeLibrary, "SDL_SetHint");

        public static class Window
        {
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SDL_CreateWindow(string? title, int x, int y, int w, int h, int flags);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_DestroyWindow(IntPtr window);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern uint SDL_GetWindowID(IntPtr window);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetWindowDisplayIndex(IntPtr window);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_GetWindowPosition(IntPtr window, out int x, out int y);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_GetWindowSize(IntPtr window, out int w, out int h);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetWindowPosition(IntPtr window, int x, int y);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetWindowSize(IntPtr window, int w, int h);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetWindowBordered(IntPtr window, bool bordered);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetWindowResizable(IntPtr window, bool resizable);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetWindowAlwaysOnTop(IntPtr window, bool on_top);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetWindowTitle(IntPtr window, string? title);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_ShowWindow(IntPtr window);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetWindowBordersSize(IntPtr window, out int top, out int left, out int bottom, out int right);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetWindowFullscreen(IntPtr window, uint flags);


            public const int PosUndefined = 0x1FFF0000;
            public const int PosCentered = 0x2FFF0000;

            public enum EventId : byte
            {
                None,
                Shown,
                Hidden,
                Exposed,
                Moved,
                Resized,
                SizeChanged,
                Minimized,
                Maximized,
                Restored,
                Enter,
                Leave,
                FocusGained,
                FocusLost,
                Close,
            }

            public static class State
            {
                public const int Fullscreen = 0x00000001;
                public const int OpenGL = 0x00000002;
                public const int Shown = 0x00000004;
                public const int Hidden = 0x00000008;
                public const int Borderless = 0x00000010;
                public const int Resizable = 0x00000020;
                public const int Minimized = 0x00000040;
                public const int Maximized = 0x00000080;
                public const int Grabbed = 0x00000100;
                public const int InputFocus = 0x00000200;
                public const int MouseFocus = 0x00000400;
                public const int FullscreenDesktop = 0x00001001;
                public const int Foreign = 0x00000800;
                public const int AllowHighDPI = 0x00002000;
                public const int MouseCapture = 0x00004000;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Event
            {
                public EventType Type;
                public uint TimeStamp;
                public uint WindowID;
                public EventId EventID;
                private readonly byte padding1;
                private readonly byte padding2;
                private readonly byte padding3;
                public int Data1;
                public int Data2;
            }

            public enum SysWMType
            {
                Unknow,
                Windows,
                X11,
                Directfb,
                Cocoa,
                UiKit,
                Wayland,
                Mir,
                WinRt,
                Android
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct SDL_SysWMinfo
            {
                public Version version;
                public SysWMType subsystem;
                public IntPtr window;
            }

            public static IntPtr CreateWindow(string? title, int x, int y, int w, int h, int flags) => SDL_CreateWindow(title, x, y, w, h, flags);
            public static void DestroyWindow(IntPtr window) => SDL_DestroyWindow(window);
            public static uint GetWindowId(IntPtr window) => SDL_GetWindowID(window);
            public static int GetWindowDisplayIndex(IntPtr window) => SDL_GetWindowDisplayIndex(window);
            public static void GetWindowPosition(IntPtr window, out int x, out int y) => SDL_GetWindowPosition(window, out x, out y);
            public static void GetWindowSize(IntPtr window, out int w, out int h) => SDL_GetWindowSize(window, out w, out h);
            public static void SetWindowPosition(IntPtr window, int x, int y) => SDL_SetWindowPosition(window, x, y);
            public static void SetWindowSize(IntPtr window, int w, int h) => SDL_SetWindowSize(window, w, h);
            public static void SetWindowBordered(IntPtr window, bool bordered) => SDL_SetWindowBordered(window, bordered);
            public static void SetWindowResizable(IntPtr window, bool resizable) => SDL_SetWindowResizable(window, resizable);
            public static void SetWindowAlwaysOnTop(IntPtr window, bool alwaysOnTop) => SDL_SetWindowAlwaysOnTop(window, alwaysOnTop);
            public static void SetWindowTitle(IntPtr window, string? title) => SDL_SetWindowTitle(window, title);
            public static void ShowWindow(IntPtr window) => SDL_ShowWindow(window);
            public static int GetWindowBordersSize(IntPtr window, out int top, out int left, out int bottom, out int right) => SDL_GetWindowBordersSize(window, out top, out left, out bottom, out right);
            public static int SetWindowFullscreen(IntPtr window, uint flags) => SDL_SetWindowFullscreen(window, flags);

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_createwindow(string title, int x, int y, int w, int h, int flags);
            //private static readonly d_sdl_createwindow SDL_CreateWindow = FuncLoader.LoadFunction<d_sdl_createwindow>(NativeLibrary, "SDL_CreateWindow");

            //public static IntPtr Create(string title, int x, int y, int w, int h, int flags)
            //{
            //    return GetError(SDL_CreateWindow(title, x, y, w, h, flags));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_destroywindow(IntPtr window);
            //public static readonly d_sdl_destroywindow Destroy = FuncLoader.LoadFunction<d_sdl_destroywindow>(NativeLibrary, "SDL_DestroyWindow");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate uint d_sdl_getwindowid(IntPtr window);
            //public static readonly d_sdl_getwindowid GetWindowId = FuncLoader.LoadFunction<d_sdl_getwindowid>(NativeLibrary, "SDL_GetWindowID");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getwindowdisplayindex(IntPtr window);
            //private static readonly d_sdl_getwindowdisplayindex SDL_GetWindowDisplayIndex = FuncLoader.LoadFunction<d_sdl_getwindowdisplayindex>(NativeLibrary, "SDL_GetWindowDisplayIndex");

            //public static int GetDisplayIndex(IntPtr window)
            //{
            //    return GetError(SDL_GetWindowDisplayIndex(window));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_getwindowflags(IntPtr window);
            //public static readonly d_sdl_getwindowflags GetWindowFlags = FuncLoader.LoadFunction<d_sdl_getwindowflags>(NativeLibrary, "SDL_GetWindowFlags");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_setwindowicon(IntPtr window, IntPtr icon);
            //public static readonly d_sdl_setwindowicon SetIcon = FuncLoader.LoadFunction<d_sdl_setwindowicon>(NativeLibrary, "SDL_SetWindowIcon");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_getwindowposition(IntPtr window, out int x, out int y);
            //public static readonly d_sdl_getwindowposition GetPosition = FuncLoader.LoadFunction<d_sdl_getwindowposition>(NativeLibrary, "SDL_GetWindowPosition");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_getwindowsize(IntPtr window, out int w, out int h);
            //public static readonly d_sdl_getwindowsize GetSize = FuncLoader.LoadFunction<d_sdl_getwindowsize>(NativeLibrary, "SDL_GetWindowSize");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_setwindowbordered(IntPtr window, int bordered);
            //public static readonly d_sdl_setwindowbordered SetBordered = FuncLoader.LoadFunction<d_sdl_setwindowbordered>(NativeLibrary, "SDL_SetWindowBordered");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_setwindowfullscreen(IntPtr window, int flags);
            //private static readonly d_sdl_setwindowfullscreen SDL_SetWindowFullscreen = FuncLoader.LoadFunction<d_sdl_setwindowfullscreen>(NativeLibrary, "SDL_SetWindowFullscreen");

            //public static void SetFullscreen(IntPtr window, int flags)
            //{
            //    GetError(SDL_SetWindowFullscreen(window, flags));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_setwindowposition(IntPtr window, int x, int y);
            //public static readonly d_sdl_setwindowposition SetPosition = FuncLoader.LoadFunction<d_sdl_setwindowposition>(NativeLibrary, "SDL_SetWindowPosition");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_setwindowresizable(IntPtr window, bool resizable);
            //public static readonly d_sdl_setwindowresizable SetResizable = FuncLoader.LoadFunction<d_sdl_setwindowresizable>(NativeLibrary, "SDL_SetWindowResizable");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_setwindowsize(IntPtr window, int w, int h);
            //public static readonly d_sdl_setwindowsize SetSize = FuncLoader.LoadFunction<d_sdl_setwindowsize>(NativeLibrary, "SDL_SetWindowSize");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate void d_sdl_setwindowtitle(IntPtr window, ref byte value);
            //private static readonly d_sdl_setwindowtitle SDL_SetWindowTitle = FuncLoader.LoadFunction<d_sdl_setwindowtitle>(NativeLibrary, "SDL_SetWindowTitle");

            //public static void SetTitle(IntPtr handle, string title)
            //{
            //    var bytes = Encoding.UTF8.GetBytes(title);
            //    SDL_SetWindowTitle(handle, ref bytes[0]);
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_showwindow(IntPtr window);
            //public static readonly d_sdl_showwindow Show = FuncLoader.LoadFunction<d_sdl_showwindow>(NativeLibrary, "SDL_ShowWindow");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate bool d_sdl_getwindowwminfo(IntPtr window, ref SDL_SysWMinfo sysWMinfo);
            //public static readonly d_sdl_getwindowwminfo GetWindowWMInfo = FuncLoader.LoadFunction<d_sdl_getwindowwminfo>(NativeLibrary, "SDL_GetWindowWMInfo");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_getwindowborderssize(IntPtr window, out int top, out int left, out int right, out int bottom);
            //public static readonly d_sdl_getwindowborderssize GetBorderSize = FuncLoader.LoadFunction<d_sdl_getwindowborderssize>(NativeLibrary, "SDL_GetWindowBordersSize");
        }

        public static class Renderer
        {
            [Flags]
            public enum SDL_RendererFlags : uint
            {
                SDL_RENDERER_SOFTWARE = 0x00000001,
                SDL_RENDERER_ACCELERATED = 0x00000002,
                SDL_RENDERER_PRESENTVSYNC = 0x00000004,
                SDL_RENDERER_TARGETTEXTURE = 0x00000008
            }

            public const int SDL_TEXTUREACCESS_STATIC = 0;
            public const int SDL_TEXTUREACCESS_STREAMING = 1;
            public const int SDL_TEXTUREACCESS_TARGET = 2;

            [StructLayout(LayoutKind.Sequential)]
            public struct SDL_Vertex
            {
                public PointF position;
                public int color;
                public PointF tex_coord;
            }
            public enum SDL_ScaleMode
            {
                SDL_ScaleModeNearest,
                SDL_ScaleModeLinear,
                SDL_ScaleModeBest
            }


            [Flags]
            public enum SDL_BlendMode
            {
                SDL_BLENDMODE_NONE = 0x00000000,
                SDL_BLENDMODE_BLEND = 0x00000001,
                SDL_BLENDMODE_ADD = 0x00000002,
                SDL_BLENDMODE_MOD = 0x00000004,
                SDL_BLENDMODE_MUL = 0x00000008, /* >= 2.0.11 */
                SDL_BLENDMODE_INVALID = 0x7FFFFFFF
            }
            public enum SDL_PixelType
            {
                SDL_PIXELTYPE_UNKNOWN,
                SDL_PIXELTYPE_INDEX1,
                SDL_PIXELTYPE_INDEX4,
                SDL_PIXELTYPE_INDEX8,
                SDL_PIXELTYPE_PACKED8,
                SDL_PIXELTYPE_PACKED16,
                SDL_PIXELTYPE_PACKED32,
                SDL_PIXELTYPE_ARRAYU8,
                SDL_PIXELTYPE_ARRAYU16,
                SDL_PIXELTYPE_ARRAYU32,
                SDL_PIXELTYPE_ARRAYF16,
                SDL_PIXELTYPE_ARRAYF32
            }

            public enum SDL_BitmapOrder
            {
                SDL_BITMAPORDER_NONE,
                SDL_BITMAPORDER_4321,
                SDL_BITMAPORDER_1234
            }
            public enum SDL_PackedOrder
            {
                SDL_PACKEDORDER_NONE,
                SDL_PACKEDORDER_XRGB,
                SDL_PACKEDORDER_RGBX,
                SDL_PACKEDORDER_ARGB,
                SDL_PACKEDORDER_RGBA,
                SDL_PACKEDORDER_XBGR,
                SDL_PACKEDORDER_BGRX,
                SDL_PACKEDORDER_ABGR,
                SDL_PACKEDORDER_BGRA
            }

            public enum SDL_ArrayOrder
            {
                SDL_ARRAYORDER_NONE,
                SDL_ARRAYORDER_RGB,
                SDL_ARRAYORDER_RGBA,
                SDL_ARRAYORDER_ARGB,
                SDL_ARRAYORDER_BGR,
                SDL_ARRAYORDER_BGRA,
                SDL_ARRAYORDER_ABGR
            }

            public enum SDL_PackedLayout
            {
                SDL_PACKEDLAYOUT_NONE,
                SDL_PACKEDLAYOUT_332,
                SDL_PACKEDLAYOUT_4444,
                SDL_PACKEDLAYOUT_1555,
                SDL_PACKEDLAYOUT_5551,
                SDL_PACKEDLAYOUT_565,
                SDL_PACKEDLAYOUT_8888,
                SDL_PACKEDLAYOUT_2101010,
                SDL_PACKEDLAYOUT_1010102
            }
            public static uint SDL_DEFINE_PIXELFORMAT(SDL_PixelType type,
                uint order,
                SDL_PackedLayout layout,
                byte bits,
                byte bytes
            )
            {
                return (uint)(
                    (1 << 28) |
                    (((byte)type) << 24) |
                    (((byte)order) << 20) |
                    (((byte)layout) << 16) |
                    (bits << 8) |
                    (bytes)
                );
            }

            public static readonly uint SDL_PIXELFORMAT_UNKNOWN = 0;
            public static readonly uint SDL_PIXELFORMAT_INDEX1LSB =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_INDEX1,
                    (uint)SDL_BitmapOrder.SDL_BITMAPORDER_4321,
                    0,
                    1, 0
                );
            public static readonly uint SDL_PIXELFORMAT_INDEX1MSB =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_INDEX1,
                    (uint)SDL_BitmapOrder.SDL_BITMAPORDER_1234,
                    0,
                    1, 0
                );
            public static readonly uint SDL_PIXELFORMAT_INDEX4LSB =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_INDEX4,
                    (uint)SDL_BitmapOrder.SDL_BITMAPORDER_4321,
                    0,
                    4, 0
                );
            public static readonly uint SDL_PIXELFORMAT_INDEX4MSB =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_INDEX4,
                    (uint)SDL_BitmapOrder.SDL_BITMAPORDER_1234,
                    0,
                    4, 0
                );
            public static readonly uint SDL_PIXELFORMAT_INDEX8 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_INDEX8,
                    0,
                    0,
                    8, 1
                );
            public static readonly uint SDL_PIXELFORMAT_RGB332 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED8,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_332,
                    8, 1
                );
            public static readonly uint SDL_PIXELFORMAT_XRGB444 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
                    12, 2
                );
            public static readonly uint SDL_PIXELFORMAT_RGB444 =
                SDL_PIXELFORMAT_XRGB444;
            public static readonly uint SDL_PIXELFORMAT_XBGR444 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XBGR,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
                    12, 2
                );
            public static readonly uint SDL_PIXELFORMAT_BGR444 =
                SDL_PIXELFORMAT_XBGR444;
            public static readonly uint SDL_PIXELFORMAT_XRGB1555 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
                    15, 2
                );
            public static readonly uint SDL_PIXELFORMAT_RGB555 =
                SDL_PIXELFORMAT_XRGB1555;
            public static readonly uint SDL_PIXELFORMAT_XBGR1555 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_INDEX1,
                    (uint)SDL_BitmapOrder.SDL_BITMAPORDER_4321,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
                    15, 2
                );
            public static readonly uint SDL_PIXELFORMAT_BGR555 =
                SDL_PIXELFORMAT_XBGR1555;
            public static readonly uint SDL_PIXELFORMAT_ARGB4444 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_RGBA4444 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_RGBA,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_ABGR4444 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ABGR,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_BGRA4444 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_BGRA,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_ARGB1555 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_RGBA5551 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_RGBA,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_5551,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_ABGR1555 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ABGR,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_BGRA5551 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_BGRA,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_5551,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_RGB565 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_565,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_BGR565 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED16,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XBGR,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_565,
                    16, 2
                );
            public static readonly uint SDL_PIXELFORMAT_RGB24 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_ARRAYU8,
                    (uint)SDL_ArrayOrder.SDL_ARRAYORDER_RGB,
                    0,
                    24, 3
                );
            public static readonly uint SDL_PIXELFORMAT_BGR24 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_ARRAYU8,
                    (uint)SDL_ArrayOrder.SDL_ARRAYORDER_BGR,
                    0,
                    24, 3
                );
            public static readonly uint SDL_PIXELFORMAT_XRGB888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    24, 4
                );
            public static readonly uint SDL_PIXELFORMAT_RGB888 =
                SDL_PIXELFORMAT_XRGB888;
            public static readonly uint SDL_PIXELFORMAT_RGBX8888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_RGBX,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    24, 4
                );
            public static readonly uint SDL_PIXELFORMAT_XBGR888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_XBGR,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    24, 4
                );
            public static readonly uint SDL_PIXELFORMAT_BGR888 =
                SDL_PIXELFORMAT_XBGR888;
            public static readonly uint SDL_PIXELFORMAT_BGRX8888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_BGRX,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    24, 4
                );
            public static readonly uint SDL_PIXELFORMAT_ARGB8888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    32, 4
                );
            public static readonly uint SDL_PIXELFORMAT_RGBA8888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_RGBA,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    32, 4
                );
            public static readonly uint SDL_PIXELFORMAT_ABGR8888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ABGR,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    32, 4
                );
            public static readonly uint SDL_PIXELFORMAT_BGRA8888 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_BGRA,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
                    32, 4
                );
            public static readonly uint SDL_PIXELFORMAT_ARGB2101010 =
                SDL_DEFINE_PIXELFORMAT(
                    SDL_PixelType.SDL_PIXELTYPE_PACKED32,
                    (uint)SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
                    SDL_PackedLayout.SDL_PACKEDLAYOUT_2101010,
                    32, 4
                );


            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SDL_CreateRenderer(IntPtr window, int index, SDL_RendererFlags flags);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_DestroyRenderer(IntPtr renderer);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_RenderPresent(IntPtr renderer);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderClear(IntPtr renderer);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_DestroyTexture(IntPtr texture);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_QueryTexture(IntPtr texture, out uint format, out int access, out int w, out int h);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetTextureAlphaMod(IntPtr texture, byte alpha);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetTextureAlphaMod(IntPtr texture, out byte alpha);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetRenderDrawBlendMode(IntPtr renderer, int blendMode);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, ref Rectangle dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref Rectangle dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, IntPtr dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, ref RectangleF dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref RectangleF dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, IntPtr dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderCopyF(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderDrawRect(IntPtr renderer, ref Rectangle rect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderFillRect(IntPtr renderer, ref Rectangle rect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderDrawLine(IntPtr renderer, int x1, int y1, int x2, int y2);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SDL_GetRenderTarget(IntPtr renderer);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetRenderTarget(IntPtr renderer, IntPtr texture);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetTextureScaleMode(IntPtr texture, out SDL_ScaleMode scaleMode);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetTextureScaleMode(IntPtr texture, SDL_ScaleMode scaleMode);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetTextureColorMod(IntPtr texture, out byte r, out byte g, out byte b);

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetTextureColorMod(IntPtr texture, byte r, byte g, byte b);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetTextureBlendMode(IntPtr texture, out SDL_BlendMode blendMode);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_SetTextureBlendMode(IntPtr texture, SDL_BlendMode blendMode);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_RenderGeometry(IntPtr renderer, IntPtr texture, [In] SDL_Vertex[] vertices, int num_vertices, [In] int[] indices, int num_indices);

            public static IntPtr CreateRenderer(IntPtr window, int index, SDL_RendererFlags flags) => SDL_CreateRenderer(window, index, flags);
            public static void DestroyRenderer(IntPtr renderer) => SDL_DestroyRenderer(renderer);
            public static void RenderPresent(IntPtr renderer) => SDL_RenderPresent(renderer);
            public static int RenderClear(IntPtr renderer) => SDL_RenderClear(renderer);
            public static IntPtr CreateTexture(IntPtr renderer, uint format, int access, int w, int h) => SDL_CreateTexture(renderer, format, access, w, h);
            public static IntPtr CreateTextureFromSurface(IntPtr renderer, IntPtr surface) => SDL_CreateTextureFromSurface(renderer, surface);
            public static void DestroyTexture(IntPtr texture) => SDL_DestroyTexture(texture);
            public static int QueryTexture(IntPtr texture, out uint format, out int access, out int w, out int h) => SDL_QueryTexture(texture, out format, out access, out w, out h);
            public static int SetTextureAlphaMod(IntPtr texture, byte alpha) => SDL_SetTextureAlphaMod(texture, alpha);
            public static int GetTextureAlphaMod(IntPtr texture, out byte alpha) => SDL_GetTextureAlphaMod(texture, out alpha);
            public static int SetRenderDrawBlendMode(IntPtr renderer, int blendMode) => SDL_SetRenderDrawBlendMode(renderer, blendMode);
            public static int SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a) => SDL_SetRenderDrawColor(renderer, r, g, b, a);

            public static int RenderCopy(IntPtr renderer, IntPtr texture)
            {
                return SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
            }

            public static int RenderCopy(IntPtr renderer, IntPtr texture, Rectangle src, int x, int y)
            {
                Rectangle dst = new Rectangle(x, y, src.Width, src.Height);
                return SDL_RenderCopy(renderer, texture, ref src, ref dst);
            }

            public static int RenderCopy(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, ref Rectangle dstrect)
            {
                if (srcrect.IsEmpty && dstrect.IsEmpty)
                {
                    return SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
                }
                else if (srcrect.IsEmpty && !dstrect.IsEmpty)
                {
                    return SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref dstrect);
                }
                else if (!srcrect.IsEmpty && dstrect.IsEmpty)
                {
                    return SDL_RenderCopy(renderer, texture, ref srcrect, IntPtr.Zero);
                }
                else
                {
                    return SDL_RenderCopy(renderer, texture, ref srcrect, ref dstrect);
                }
            }

            public static int RenderCopyF(IntPtr renderer, IntPtr texture)
            {
                return SDL_RenderCopyF(renderer, texture, IntPtr.Zero, IntPtr.Zero);
            }
            public static int RenderCopyF(IntPtr renderer, IntPtr texture, Rectangle src, float x, float y)
            {
                RectangleF dst = new RectangleF(x, y, src.Width, src.Height);
                return SDL_RenderCopyF(renderer, texture, ref src, ref dst);
            }
            public static int RenderCopyF(IntPtr renderer, IntPtr texture, float x, float y, float w, float h)
            {
                RectangleF dst = new RectangleF(x, y, w, h);
                return SDL_RenderCopyF(renderer, texture, IntPtr.Zero, ref dst);
            }

            public static int RenderCopyF(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, ref RectangleF dstrect)
            {
                if (srcrect.IsEmpty && dstrect.IsEmpty)
                {
                    return SDL_RenderCopyF(renderer, texture, IntPtr.Zero, IntPtr.Zero);
                }
                else if (srcrect.IsEmpty && !dstrect.IsEmpty)
                {
                    return SDL_RenderCopyF(renderer, texture, IntPtr.Zero, ref dstrect);
                }
                else if (!srcrect.IsEmpty && dstrect.IsEmpty)
                {
                    return SDL_RenderCopyF(renderer, texture, ref srcrect, IntPtr.Zero);
                }
                else
                {
                    return SDL_RenderCopyF(renderer, texture, ref srcrect, ref dstrect);
                }
            }
            public static int RenderDrawRect(IntPtr renderer, ref Rectangle rect) => SDL_RenderDrawRect(renderer, ref rect);
            public static int RenderFillRect(IntPtr renderer, ref Rectangle rect) => SDL_RenderFillRect(renderer, ref rect);
            public static int RenderDrawLine(IntPtr renderer, int x1, int y1, int x2, int y2) => SDL_RenderDrawLine(renderer, x1, y1, x2, y2);
            public static IntPtr GetRenderTarget(IntPtr renderer) => SDL_GetRenderTarget(renderer);
            public static int SetRenderTarget(IntPtr renderer, IntPtr texture) => SDL_SetRenderTarget(renderer, texture);

            public static int GetTextureScaleMode(IntPtr texture, out SDL_ScaleMode scaleMode) => SDL_GetTextureScaleMode(texture, out scaleMode);
            public static int SetTextureScaleMode(IntPtr texture, SDL_ScaleMode scaleMode) => SDL_SetTextureScaleMode(texture, scaleMode);
            public static int GetTextureColorMod(IntPtr texture, out byte r, out byte g, out byte b) => SDL_GetTextureColorMod(texture, out r, out g, out b);
            public static int SetTextureColorMod(IntPtr texture, byte r, byte g, byte b) => SDL_SetTextureColorMod(texture, r, g, b);
            public static int GetTextureBlendMode(IntPtr texture, out SDL_BlendMode blendMode) => SDL_GetTextureBlendMode(texture, out blendMode);
            public static int SetTextureBlendMode(IntPtr texture, SDL_BlendMode blendMode) => SDL_SetTextureBlendMode(texture, blendMode);
            public static int RenderGeometry(IntPtr renderer, IntPtr texture, [In] SDL_Vertex[] vertices, int num_vertices, [In] int[] indices, int num_indices) => SDL_RenderGeometry(renderer, texture, vertices, num_vertices, indices, num_indices);




            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_createrenderer(IntPtr window, int index, SDL_RendererFlags flags);
            //private static readonly d_sdl_createrenderer SDL_CreateRenderer = FuncLoader.LoadFunction<d_sdl_createrenderer>(NativeLibrary, "SDL_CreateRenderer");

            //public static IntPtr Create(IntPtr window, int index, SDL_RendererFlags flags)
            //{
            //    return GetError(SDL_CreateRenderer(window, index, flags));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_destroyrenderer(IntPtr renderer);
            //public static readonly d_sdl_destroyrenderer Destroy = FuncLoader.LoadFunction<d_sdl_destroyrenderer>(NativeLibrary, "SDL_DestroyRenderer");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_renderpresent(IntPtr renderer);
            //public static readonly d_sdl_renderpresent Present = FuncLoader.LoadFunction<d_sdl_renderpresent>(NativeLibrary, "SDL_RenderPresent");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_renderclear(IntPtr renderer);
            //public static readonly d_sdl_renderclear Clear = FuncLoader.LoadFunction<d_sdl_renderclear>(NativeLibrary, "SDL_RenderClear");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate IntPtr d_sdl_createtexture(IntPtr renderer, uint format, int access, int w, int h);
            //public static readonly d_sdl_createtexture CreateTexture = FuncLoader.LoadFunction<d_sdl_createtexture>(NativeLibrary, "SDL_CreateTexture");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_querytexture(IntPtr renderer, out uint format, out int access, out int w, out int h);
            //public static readonly d_sdl_querytexture QueryTexture = FuncLoader.LoadFunction<d_sdl_querytexture>(NativeLibrary, "SDL_QueryTexture");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_destroytexture(IntPtr texture);
            //public static readonly d_sdl_destroytexture DestroyTexture = FuncLoader.LoadFunction<d_sdl_destroytexture>(NativeLibrary, "SDL_DestroyTexture");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate IntPtr d_sdl_cleartexturefromsurface(IntPtr renderer, IntPtr surface);
            //public static readonly d_sdl_cleartexturefromsurface CreateTextureFromSurface = FuncLoader.LoadFunction<d_sdl_cleartexturefromsurface>(NativeLibrary, "SDL_CreateTextureFromSurface");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_settexturealphamod(IntPtr renderer, byte alpha);
            //public static readonly d_sdl_settexturealphamod SetTextureAlphaMod = FuncLoader.LoadFunction<d_sdl_settexturealphamod>(NativeLibrary, "SDL_SetTextureAlphaMod");


            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopy_0(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, ref Rectangle dstRect);
            //private static readonly d_sdl_rendercopy_0 RenderCopy0 = FuncLoader.LoadFunction<d_sdl_rendercopy_0>(NativeLibrary, "SDL_RenderCopy");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopy_1(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, IntPtr dstRect);
            //private static readonly d_sdl_rendercopy_1 RenderCopy1 = FuncLoader.LoadFunction<d_sdl_rendercopy_1>(NativeLibrary, "SDL_RenderCopy");
            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopy_2(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref Rectangle dstRect);
            //private static readonly d_sdl_rendercopy_2 RenderCopy2 = FuncLoader.LoadFunction<d_sdl_rendercopy_2>(NativeLibrary, "SDL_RenderCopy");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopy_3(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstRect);
            //private static readonly d_sdl_rendercopy_3 RenderCopy3 = FuncLoader.LoadFunction<d_sdl_rendercopy_3>(NativeLibrary, "SDL_RenderCopy");


            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopyF_0(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, ref RectangleF dstRect);
            //private static readonly d_sdl_rendercopyF_0 RenderCopyF0 = FuncLoader.LoadFunction<d_sdl_rendercopyF_0>(NativeLibrary, "SDL_RenderCopyF");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopyF_1(IntPtr renderer, IntPtr texture, IntPtr srcrect, ref RectangleF dstRect);
            //private static readonly d_sdl_rendercopyF_1 RenderCopyF1 = FuncLoader.LoadFunction<d_sdl_rendercopyF_1>(NativeLibrary, "SDL_RenderCopyF");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopyF_2(IntPtr renderer, IntPtr texture, ref Rectangle srcrect, IntPtr dstRect);
            //private static readonly d_sdl_rendercopyF_2 RenderCopyF2 = FuncLoader.LoadFunction<d_sdl_rendercopyF_2>(NativeLibrary, "SDL_RenderCopyF");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendercopyF_3(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstRect);
            //private static readonly d_sdl_rendercopyF_3 RenderCopyF3 = FuncLoader.LoadFunction<d_sdl_rendercopyF_3>(NativeLibrary, "SDL_RenderCopyF");


            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendersetdrawcolor(IntPtr renderer, byte r, byte g, byte b, byte a);
            //public static readonly d_sdl_rendersetdrawcolor SetDrawColor = FuncLoader.LoadFunction<d_sdl_rendersetdrawcolor>(NativeLibrary, "SDL_SetRenderDrawColor");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_rendersetdrawblendmode(IntPtr renderer, int blendMode);
            //public static readonly d_sdl_rendersetdrawblendmode SetDrawBlendMode = FuncLoader.LoadFunction<d_sdl_rendersetdrawblendmode>(NativeLibrary, "SDL_SetRenderDrawBlendMode");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_renderdrawrect(IntPtr renderer, ref Rectangle rect);
            //public static readonly d_sdl_renderdrawrect DrawRect = FuncLoader.LoadFunction<d_sdl_renderdrawrect>(NativeLibrary, "SDL_RenderDrawRect");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_renderfillrect(IntPtr renderer, ref Rectangle rect);
            //public static readonly d_sdl_renderfillrect FillRect = FuncLoader.LoadFunction<d_sdl_renderfillrect>(NativeLibrary, "SDL_RenderFillRect");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_renderdrawline(IntPtr renderer, int x1, int y1, int x2, int y2);
            //public static readonly d_sdl_renderdrawline DrawLine = FuncLoader.LoadFunction<d_sdl_renderdrawline>(NativeLibrary, "SDL_RenderDrawLine");


        }

        public static class Display
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Mode
            {
                public uint Format;
                public int Width;
                public int Height;
                public int RefreshRate;
                public IntPtr DriverData;
            }

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetDisplayBounds(int displayIndex, out Rectangle rect);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern int SDL_GetCurrentDisplayMode(int displayIndex, out Mode mode);

            public static int GetDisplayBounds(int displayIndex, out Rectangle rect) => SDL_GetDisplayBounds(displayIndex, out rect);
            public static int GetCurrentDisplayMode(int displayIndex, out Mode mode) => SDL_GetCurrentDisplayMode(displayIndex, out mode);


            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getdisplaybounds(int displayIndex, out Rectangle rect);
            //private static readonly d_sdl_getdisplaybounds SDL_GetDisplayBounds = FuncLoader.LoadFunction<d_sdl_getdisplaybounds>(NativeLibrary, "SDL_GetDisplayBounds");

            //public static void GetBounds(int displayIndex, out Rectangle rect)
            //{
            //    GetError(SDL_GetDisplayBounds(displayIndex, out rect));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getcurrentdisplaymode(int displayIndex, out Mode mode);
            //private static readonly d_sdl_getcurrentdisplaymode SDL_GetCurrentDisplayMode = FuncLoader.LoadFunction<d_sdl_getcurrentdisplaymode>(NativeLibrary, "SDL_GetCurrentDisplayMode");

            //public static void GetCurrentDisplayMode(int displayIndex, out Mode mode)
            //{
            //    GetError(SDL_GetCurrentDisplayMode(displayIndex, out mode));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getdisplaymode(int displayIndex, int modeIndex, out Mode mode);
            //private static readonly d_sdl_getdisplaymode SDL_GetDisplayMode = FuncLoader.LoadFunction<d_sdl_getdisplaymode>(NativeLibrary, "SDL_GetDisplayMode");

            //public static void GetDisplayMode(int displayIndex, int modeIndex, out Mode mode)
            //{
            //    GetError(SDL_GetDisplayMode(displayIndex, modeIndex, out mode));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getclosestdisplaymode(int displayIndex, Mode mode, out Mode closest);
            //private static readonly d_sdl_getclosestdisplaymode SDL_GetClosestDisplayMode = FuncLoader.LoadFunction<d_sdl_getclosestdisplaymode>(NativeLibrary, "SDL_GetClosestDisplayMode");

            //public static void GetClosestDisplayMode(int displayIndex, Mode mode, out Mode closest)
            //{
            //    GetError(SDL_GetClosestDisplayMode(displayIndex, mode, out closest));
            //}



            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getnumdisplaymodes(int displayIndex);
            //private static readonly d_sdl_getnumdisplaymodes SDL_GetNumDisplayModes = FuncLoader.LoadFunction<d_sdl_getnumdisplaymodes>(NativeLibrary, "SDL_GetNumDisplayModes");

            //public static int GetNumDisplayModes(int displayIndex)
            //{
            //    return GetError(SDL_GetNumDisplayModes(displayIndex));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getnumvideodisplays();
            //private static readonly d_sdl_getnumvideodisplays SDL_GetNumVideoDisplays = FuncLoader.LoadFunction<d_sdl_getnumvideodisplays>(NativeLibrary, "SDL_GetNumVideoDisplays");

            //public static int GetNumVideoDisplays()
            //{
            //    return GetError(SDL_GetNumVideoDisplays());
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_getwindowdisplayindex(IntPtr window);
            //private static readonly d_sdl_getwindowdisplayindex SDL_GetWindowDisplayIndex = FuncLoader.LoadFunction<d_sdl_getwindowdisplayindex>(NativeLibrary, "SDL_GetWindowDisplayIndex");

            //public static int GetWindowDisplayIndex(IntPtr window)
            //{
            //    return GetError(SDL_GetWindowDisplayIndex(window));
            //}
        }

        //public static class GL
        //{
        //    public enum Attribute
        //    {
        //        RedSize,
        //        GreenSize,
        //        BlueSize,
        //        AlphaSize,
        //        BufferSize,
        //        DoubleBuffer,
        //        DepthSize,
        //        StencilSize,
        //        AccumRedSize,
        //        AccumGreenSize,
        //        AccumBlueSize,
        //        AccumAlphaSize,
        //        Stereo,
        //        MultiSampleBuffers,
        //        MultiSampleSamples,
        //        AcceleratedVisual,
        //        RetainedBacking,
        //        ContextMajorVersion,
        //        ContextMinorVersion,
        //        ContextEgl,
        //        ContextFlags,
        //        ContextProfileMAsl,
        //        ShareWithCurrentContext,
        //        FramebufferSRGBCapable,
        //        ContextReleaseBehaviour,
        //    }

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    private delegate IntPtr d_sdl_gl_createcontext(IntPtr window);
        //    private static readonly d_sdl_gl_createcontext SDL_GL_CreateContext = FuncLoader.LoadFunction<d_sdl_gl_createcontext>(NativeLibrary, "SDL_GL_CreateContext");

        //    public static IntPtr CreateContext(IntPtr window)
        //    {
        //        return GetError(SDL_GL_CreateContext(window));
        //    }

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    public delegate void d_sdl_gl_deletecontext(IntPtr context);
        //    public static readonly d_sdl_gl_deletecontext DeleteContext = FuncLoader.LoadFunction<d_sdl_gl_deletecontext>(NativeLibrary, "SDL_GL_DeleteContext");

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    private delegate IntPtr d_sdl_gl_getcurrentcontext();
        //    private static readonly d_sdl_gl_getcurrentcontext SDL_GL_GetCurrentContext = FuncLoader.LoadFunction<d_sdl_gl_getcurrentcontext>(NativeLibrary, "SDL_GL_GetCurrentContext");

        //    public static IntPtr GetCurrentContext()
        //    {
        //        return GetError(SDL_GL_GetCurrentContext());
        //    }

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    public delegate IntPtr d_sdl_gl_getprocaddress(string proc);
        //    public static readonly d_sdl_gl_getprocaddress GetProcAddress = FuncLoader.LoadFunction<d_sdl_gl_getprocaddress>(NativeLibrary, "SDL_GL_GetProcAddress");

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    public delegate int d_sdl_gl_getswapinterval();
        //    public static readonly d_sdl_gl_getswapinterval GetSwapInterval = FuncLoader.LoadFunction<d_sdl_gl_getswapinterval>(NativeLibrary, "SDL_GL_GetSwapInterval");

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    public delegate int d_sdl_gl_makecurrent(IntPtr window, IntPtr context);
        //    public static readonly d_sdl_gl_makecurrent MakeCurrent = FuncLoader.LoadFunction<d_sdl_gl_makecurrent>(NativeLibrary, "SDL_GL_MakeCurrent");

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    private delegate int d_sdl_gl_setattribute(Attribute attr, int value);
        //    private static readonly d_sdl_gl_setattribute SDL_GL_SetAttribute = FuncLoader.LoadFunction<d_sdl_gl_setattribute>(NativeLibrary, "SDL_GL_SetAttribute");

        //    public static int SetAttribute(Attribute attr, int value)
        //    {
        //        return GetError(SDL_GL_SetAttribute(attr, value));
        //    }

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    public delegate int d_sdl_gl_setswapinterval(int interval);
        //    public static readonly d_sdl_gl_setswapinterval SetSwapInterval = FuncLoader.LoadFunction<d_sdl_gl_setswapinterval>(NativeLibrary, "SDL_GL_SetSwapInterval");

        //    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //    public delegate void d_sdl_gl_swapwindow(IntPtr window);
        //    public static readonly d_sdl_gl_swapwindow SwapWindow = FuncLoader.LoadFunction<d_sdl_gl_swapwindow>(NativeLibrary, "SDL_GL_SwapWindow");
        //}

        public static class Mouse
        {
            [Flags]
            public enum Button
            {
                Left = 1 << 0,
                Middle = 1 << 1,
                Right = 1 << 2,
                X1Mask = 1 << 3,
                X2Mask = 1 << 4
            }

            public enum SystemCursor
            {
                Arrow,
                IBeam,
                Wait,
                Crosshair,
                WaitArrow,
                SizeNWSE,
                SizeNESW,
                SizeWE,
                SizeNS,
                SizeAll,
                No,
                Hand
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MotionEvent
            {
                public EventType Type;
                public uint Timestamp;
                public uint WindowID;
                public uint Which;
                public byte State;
                private readonly byte padding1;
                private readonly byte padding2;
                private readonly byte padding3;
                public int X;
                public int Y;
                public int Xrel;
                public int Yrel;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ButtonEvent
            {
                public EventType type;
                public uint Timestamp;
                public uint WindowID;
                public uint Which;
                public byte Button;
                public byte State;
                public byte Clicks;
                private readonly byte padding1;
                public int X;
                public int Y;
            }


            [StructLayout(LayoutKind.Sequential)]
            public struct WheelEvent
            {
                public EventType Type;
                public uint TimeStamp;
                public uint WindowId;
                public uint Which;
                public int X;
                public int Y;
                public uint Direction;
            }

            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_SetCursor(IntPtr cursor);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr SDL_CreateSystemCursor(SystemCursor cursor);
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void SDL_FreeCursor(IntPtr cursor);

            public static void SetCursor(IntPtr cursor) => SDL_SetCursor(cursor);
            public static IntPtr CreateSystemCursor(SystemCursor cursor) => SDL_CreateSystemCursor(cursor);
            public static void FreeCursor(IntPtr cursor) => SDL_FreeCursor(cursor);

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_createcolorcursor(IntPtr surface, int x, int y);
            //private static readonly d_sdl_createcolorcursor SDL_CreateColorCursor = FuncLoader.LoadFunction<d_sdl_createcolorcursor>(NativeLibrary, "SDL_CreateColorCursor");

            //public static IntPtr CreateColorCursor(IntPtr surface, int x, int y)
            //{
            //    return GetError(SDL_CreateColorCursor(surface, x, y));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_createsystemcursor(SystemCursor id);
            //private static readonly d_sdl_createsystemcursor SDL_CreateSystemCursor = FuncLoader.LoadFunction<d_sdl_createsystemcursor>(NativeLibrary, "SDL_CreateSystemCursor");

            //public static IntPtr CreateSystemCursor(SystemCursor id)
            //{
            //    return GetError(SDL_CreateSystemCursor(id));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_freecursor(IntPtr cursor);
            //public static readonly d_sdl_freecursor FreeCursor = FuncLoader.LoadFunction<d_sdl_freecursor>(NativeLibrary, "SDL_FreeCursor");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate Button d_sdl_getglobalmousestate(out int x, out int y);
            //public static readonly d_sdl_getglobalmousestate GetGlobalState = FuncLoader.LoadFunction<d_sdl_getglobalmousestate>(NativeLibrary, "SDL_GetGlobalMouseState");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate Button d_sdl_getmousestate(out int x, out int y);
            //public static readonly d_sdl_getmousestate GetState = FuncLoader.LoadFunction<d_sdl_getmousestate>(NativeLibrary, "SDL_GetMouseState");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_setcursor(IntPtr cursor);
            //public static readonly d_sdl_setcursor SetCursor = FuncLoader.LoadFunction<d_sdl_setcursor>(NativeLibrary, "SDL_SetCursor");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_showcursor(int toggle);
            //public static readonly d_sdl_showcursor ShowCursor = FuncLoader.LoadFunction<d_sdl_showcursor>(NativeLibrary, "SDL_ShowCursor");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_warpmouseinwindow(IntPtr window, int x, int y);
            //public static readonly d_sdl_warpmouseinwindow WarpInWindow = FuncLoader.LoadFunction<d_sdl_warpmouseinwindow>(NativeLibrary, "SDL_WarpMouseInWindow");
        }

        public static class Keyboard
        {
            [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
            private static extern Keymod SDL_GetModState();

            public struct Keysym
            {
                public int Scancode;
                public int Sym;
                public Keymod Mod;
                public uint Unicode;
            }

            [Flags]
            public enum Keymod : ushort
            {
                None = 0x0000,
                LeftShift = 0x0001,
                RightShift = 0x0002,
                LeftCtrl = 0x0040,
                RightCtrl = 0x0080,
                LeftAlt = 0x0100,
                RightAlt = 0x0200,
                LeftGui = 0x0400,
                RightGui = 0x0800,
                NumLock = 0x1000,
                CapsLock = 0x2000,
                AltGr = 0x4000,
                Reserved = 0x8000,
                Ctrl = (LeftCtrl | RightCtrl),
                Shift = (LeftShift | RightShift),
                Alt = (LeftAlt | RightAlt),
                Gui = (LeftGui | RightGui)
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Event
            {
                public EventType Type;
                public uint TimeStamp;
                public uint WindowId;
                public byte State;
                public byte Repeat;
                private readonly byte padding2;
                private readonly byte padding3;
                public Keysym Keysym;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct TextEditingEvent
            {
                public EventType Type;
                public uint Timestamp;
                public uint WindowId;
                public uint Text1;
                public uint Text2;
                public uint Text3;
                public uint Text4;
                public uint Text5;
                public uint Text6;
                public uint Text7;
                public uint Text8;
                public int Start;
                public int Length;
            }
            //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            //    public byte[] Text;
            //    public int Start;
            //    public int Length;
            //}

            [StructLayout(LayoutKind.Sequential)]
            public struct TextInputEvent
            {
                public EventType Type;
                public uint Timestamp;
                public uint WindowId;
                public uint Text1;
                public uint Text2;
                public uint Text3;
                public uint Text4;
                public uint Text5;
                public uint Text6;
                public uint Text7;
                public uint Text8;
                //    [MarshalAs(UnmanagedType.ByValArray,SizeConst = 32)]
                //    public byte[] Text;
            }

            public static Keymod GetModState() => SDL_GetModState();

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate Keymod d_sdl_getmodstate();
            //public static readonly d_sdl_getmodstate GetModState = FuncLoader.LoadFunction<d_sdl_getmodstate>(NativeLibrary, "SDL_GetModState");
        }

        public static class Joystick
        {
            [Flags]
            public enum Hat : byte
            {
                Centered = 0,
                Up = 1 << 0,
                Right = 1 << 1,
                Down = 1 << 2,
                Left = 1 << 3
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DeviceEvent
            {
                public EventType Type;
                public uint TimeStamp;
                public int Which;
            }

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_joystickclose(IntPtr joystick);
            //public static d_sdl_joystickclose Close = FuncLoader.LoadFunction<d_sdl_joystickclose>(NativeLibrary, "SDL_JoystickClose");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_joystickfrominstanceid(int joyid);
            //private static readonly d_sdl_joystickfrominstanceid SDL_JoystickFromInstanceID = FuncLoader.LoadFunction<d_sdl_joystickfrominstanceid>(NativeLibrary, "SDL_JoystickFromInstanceID");

            //public static IntPtr FromInstanceID(int joyid)
            //{
            //    return GetError(SDL_JoystickFromInstanceID(joyid));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate short d_sdl_joystickgetaxis(IntPtr joystick, int axis);
            //public static readonly d_sdl_joystickgetaxis GetAxis = FuncLoader.LoadFunction<d_sdl_joystickgetaxis>(NativeLibrary, "SDL_JoystickGetAxis");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate byte d_sdl_joystickgetbutton(IntPtr joystick, int button);
            //public static readonly d_sdl_joystickgetbutton GetButton = FuncLoader.LoadFunction<d_sdl_joystickgetbutton>(NativeLibrary, "SDL_JoystickGetButton");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_joystickname(IntPtr joystick);
            //private static readonly d_sdl_joystickname JoystickName = FuncLoader.LoadFunction<d_sdl_joystickname>(NativeLibrary, "SDL_JoystickName");

            //public static string GetJoystickName(IntPtr joystick)
            //{
            //    return InteropHelpers.Utf8ToString(JoystickName(joystick)) ?? "";
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate Guid d_sdl_joystickgetguid(IntPtr joystick);
            //public static readonly d_sdl_joystickgetguid GetGUID = FuncLoader.LoadFunction<d_sdl_joystickgetguid>(NativeLibrary, "SDL_JoystickGetGUID");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate Hat d_sdl_joystickgethat(IntPtr joystick, int hat);
            //public static readonly d_sdl_joystickgethat GetHat = FuncLoader.LoadFunction<d_sdl_joystickgethat>(NativeLibrary, "SDL_JoystickGetHat");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_joystickinstanceid(IntPtr joystick);
            //public static readonly d_sdl_joystickinstanceid InstanceID = FuncLoader.LoadFunction<d_sdl_joystickinstanceid>(NativeLibrary, "SDL_JoystickInstanceID");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_joystickopen(int deviceIndex);
            //private static readonly d_sdl_joystickopen SDL_JoystickOpen = FuncLoader.LoadFunction<d_sdl_joystickopen>(NativeLibrary, "SDL_JoystickOpen");

            //public static IntPtr Open(int deviceIndex)
            //{
            //    return GetError(SDL_JoystickOpen(deviceIndex));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_joysticknumaxes(IntPtr joystick);
            //private static readonly d_sdl_joysticknumaxes SDL_JoystickNumAxes = FuncLoader.LoadFunction<d_sdl_joysticknumaxes>(NativeLibrary, "SDL_JoystickNumAxes");

            //public static int NumAxes(IntPtr joystick)
            //{
            //    return GetError(SDL_JoystickNumAxes(joystick));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_joysticknumbuttons(IntPtr joystick);
            //private static readonly d_sdl_joysticknumbuttons SDL_JoystickNumButtons = FuncLoader.LoadFunction<d_sdl_joysticknumbuttons>(NativeLibrary, "SDL_JoystickNumButtons");

            //public static int NumButtons(IntPtr joystick)
            //{
            //    return GetError(SDL_JoystickNumButtons(joystick));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_joysticknumhats(IntPtr joystick);
            //private static readonly d_sdl_joysticknumhats SDL_JoystickNumHats = FuncLoader.LoadFunction<d_sdl_joysticknumhats>(NativeLibrary, "SDL_JoystickNumHats");

            //public static int NumHats(IntPtr joystick)
            //{
            //    return GetError(SDL_JoystickNumHats(joystick));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_numjoysticks();
            //private static readonly d_sdl_numjoysticks SDL_NumJoysticks = FuncLoader.LoadFunction<d_sdl_numjoysticks>(NativeLibrary, "SDL_NumJoysticks");

            //public static int NumJoysticks()
            //{
            //    return GetError(SDL_NumJoysticks());
            //}
        }

        public static class GameController
        {
            public enum Axis
            {
                Invalid = -1,
                LeftX,
                LeftY,
                RightX,
                RightY,
                TriggerLeft,
                TriggerRight,
                Max,
            }

            public enum Button
            {
                Invalid = -1,
                A,
                B,
                X,
                Y,
                Back,
                Guide,
                Start,
                LeftStick,
                RightStick,
                LeftShoulder,
                RightShoulder,
                DpadUp,
                DpadDown,
                DpadLeft,
                DpadRight,
                Max,
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DeviceEvent
            {
                public EventType Type;
                public uint TimeStamp;
                public int Which;
            }

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_free(IntPtr ptr);
            //public static readonly d_sdl_free SDL_Free = FuncLoader.LoadFunction<d_sdl_free>(NativeLibrary, "SDL_free");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_gamecontrolleraddmapping(string mappingString);
            //public static readonly d_sdl_gamecontrolleraddmapping AddMapping = FuncLoader.LoadFunction<d_sdl_gamecontrolleraddmapping>(NativeLibrary, "SDL_GameControllerAddMapping");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_gamecontrolleraddmappingsfromrw(IntPtr rw, int freew);
            //public static readonly d_sdl_gamecontrolleraddmappingsfromrw AddMappingFromRw = FuncLoader.LoadFunction<d_sdl_gamecontrolleraddmappingsfromrw>(NativeLibrary, "SDL_GameControllerAddMappingsFromRW");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_gamecontrollerclose(IntPtr gamecontroller);
            //public static readonly d_sdl_gamecontrollerclose Close = FuncLoader.LoadFunction<d_sdl_gamecontrollerclose>(NativeLibrary, "SDL_GameControllerClose");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_joystickfrominstanceid(int joyid);
            //private static readonly d_sdl_joystickfrominstanceid SDL_GameControllerFromInstanceID = FuncLoader.LoadFunction<d_sdl_joystickfrominstanceid>(NativeLibrary, "SDL_JoystickFromInstanceID");

            //public static IntPtr FromInstanceID(int joyid)
            //{
            //    return GetError(SDL_GameControllerFromInstanceID(joyid));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate short d_sdl_gamecontrollergetaxis(IntPtr gamecontroller, Axis axis);
            //public static readonly d_sdl_gamecontrollergetaxis GetAxis = FuncLoader.LoadFunction<d_sdl_gamecontrollergetaxis>(NativeLibrary, "SDL_GameControllerGetAxis");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate byte d_sdl_gamecontrollergetbutton(IntPtr gamecontroller, Button button);
            //public static readonly d_sdl_gamecontrollergetbutton GetButton = FuncLoader.LoadFunction<d_sdl_gamecontrollergetbutton>(NativeLibrary, "SDL_GameControllerGetButton");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_gamecontrollergetjoystick(IntPtr gamecontroller);
            //private static readonly d_sdl_gamecontrollergetjoystick SDL_GameControllerGetJoystick = FuncLoader.LoadFunction<d_sdl_gamecontrollergetjoystick>(NativeLibrary, "SDL_GameControllerGetJoystick");

            //public static IntPtr GetJoystick(IntPtr gamecontroller)
            //{
            //    return GetError(SDL_GameControllerGetJoystick(gamecontroller));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate byte d_sdl_isgamecontroller(int joystickIndex);
            //public static readonly d_sdl_isgamecontroller IsGameController = FuncLoader.LoadFunction<d_sdl_isgamecontroller>(NativeLibrary, "SDL_IsGameController");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_gamecontrollermapping(IntPtr gamecontroller);
            //private static readonly d_sdl_gamecontrollermapping SDL_GameControllerMapping = FuncLoader.LoadFunction<d_sdl_gamecontrollermapping>(NativeLibrary, "SDL_GameControllerMapping");

            //public static string GetMapping(IntPtr gamecontroller)
            //{
            //    IntPtr nativeStr = SDL_GameControllerMapping(gamecontroller);
            //    if (nativeStr == IntPtr.Zero)
            //        return string.Empty;

            //    string mappingStr = InteropHelpers.Utf8ToString(nativeStr) ?? "";

            //    //The mapping string returned by SDL is owned by us and thus must be freed
            //    SDL_Free(nativeStr);

            //    return mappingStr;
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_gamecontrolleropen(int joystickIndex);
            //private static readonly d_sdl_gamecontrolleropen SDL_GameControllerOpen = FuncLoader.LoadFunction<d_sdl_gamecontrolleropen>(NativeLibrary, "SDL_GameControllerOpen");

            //public static IntPtr Open(int joystickIndex)
            //{
            //    return GetError(SDL_GameControllerOpen(joystickIndex));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_gamecontrollername(IntPtr gamecontroller);
            //private static readonly d_sdl_gamecontrollername SDL_GameControllerName = FuncLoader.LoadFunction<d_sdl_gamecontrollername>(NativeLibrary, "SDL_GameControllerName");

            //public static string GetName(IntPtr gamecontroller)
            //{
            //    return InteropHelpers.Utf8ToString(SDL_GameControllerName(gamecontroller)) ?? "";
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_gamecontrollerrumble(IntPtr gamecontroller, ushort left, ushort right, uint duration);
            //public static readonly d_sdl_gamecontrollerrumble Rumble = FuncLoader.LoadFunction<d_sdl_gamecontrollerrumble>(NativeLibrary, "SDL_GameControllerRumble");
            //public static readonly d_sdl_gamecontrollerrumble RumbleTriggers = FuncLoader.LoadFunction<d_sdl_gamecontrollerrumble>(NativeLibrary, "SDL_GameControllerRumbleTriggers");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate byte d_sdl_gamecontrollerhasrumble(IntPtr gamecontroller);
            //public static readonly d_sdl_gamecontrollerhasrumble HasRumble = FuncLoader.LoadFunction<d_sdl_gamecontrollerhasrumble>(NativeLibrary, "SDL_GameControllerHasRumble");
            //public static readonly d_sdl_gamecontrollerhasrumble HasRumbleTriggers = FuncLoader.LoadFunction<d_sdl_gamecontrollerhasrumble>(NativeLibrary, "SDL_GameControllerHasRumbleTriggers");
        }

        public static class Haptic
        {
            // For some reason, different game controllers have different maximum value supported
            // Also, the more the value is close to their limit, the more they tend to randomly ignore it
            // Hence, we're setting an abitrary safe value as a maximum
            public const uint Infinity = 1000000U;

            public enum EffectId : ushort
            {
                LeftRight = (1 << 2),
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct LeftRight
            {
                public EffectId Type;
                public uint Length;
                public ushort LargeMagnitude;
                public ushort SmallMagnitude;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct Effect
            {
                [FieldOffset(0)] public EffectId type;
                [FieldOffset(0)] public LeftRight leftright;
            }

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_hapticclose(IntPtr haptic);
            //public static readonly d_sdl_hapticclose Close = FuncLoader.LoadFunction<d_sdl_hapticclose>(NativeLibrary, "SDL_HapticClose");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_hapticeffectsupported(IntPtr haptic, ref Effect effect);
            //public static readonly d_sdl_hapticeffectsupported EffectSupported = FuncLoader.LoadFunction<d_sdl_hapticeffectsupported>(NativeLibrary, "SDL_HapticEffectSupported");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate int d_sdl_joystickishaptic(IntPtr joystick);
            //public static readonly d_sdl_joystickishaptic IsHaptic = FuncLoader.LoadFunction<d_sdl_joystickishaptic>(NativeLibrary, "SDL_JoystickIsHaptic");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticneweffect(IntPtr haptic, ref Effect effect);
            //private static readonly d_sdl_hapticneweffect SDL_HapticNewEffect = FuncLoader.LoadFunction<d_sdl_hapticneweffect>(NativeLibrary, "SDL_HapticNewEffect");

            //public static void NewEffect(IntPtr haptic, ref Effect effect)
            //{
            //    GetError(SDL_HapticNewEffect(haptic, ref effect));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate IntPtr d_sdl_hapticopen(int device_index);
            //public static readonly d_sdl_hapticopen Open = FuncLoader.LoadFunction<d_sdl_hapticopen>(NativeLibrary, "SDL_HapticOpen");

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate IntPtr d_sdl_hapticopenfromjoystick(IntPtr joystick);
            //private static readonly d_sdl_hapticopenfromjoystick SDL_HapticOpenFromJoystick = FuncLoader.LoadFunction<d_sdl_hapticopenfromjoystick>(NativeLibrary, "SDL_HapticOpenFromJoystick");

            //public static IntPtr OpenFromJoystick(IntPtr joystick)
            //{
            //    return GetError(SDL_HapticOpenFromJoystick(joystick));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticrumbleinit(IntPtr haptic);
            //private static readonly d_sdl_hapticrumbleinit SDL_HapticRumbleInit = FuncLoader.LoadFunction<d_sdl_hapticrumbleinit>(NativeLibrary, "SDL_HapticRumbleInit");

            //public static void RumbleInit(IntPtr haptic)
            //{
            //    GetError(SDL_HapticRumbleInit(haptic));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticrumbleplay(IntPtr haptic, float strength, uint length);
            //private static readonly d_sdl_hapticrumbleplay SDL_HapticRumblePlay = FuncLoader.LoadFunction<d_sdl_hapticrumbleplay>(NativeLibrary, "SDL_HapticRumblePlay");

            //public static void RumblePlay(IntPtr haptic, float strength, uint length)
            //{
            //    GetError(SDL_HapticRumblePlay(haptic, strength, length));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticrumblesupported(IntPtr haptic);
            //private static readonly d_sdl_hapticrumblesupported SDL_HapticRumbleSupported = FuncLoader.LoadFunction<d_sdl_hapticrumblesupported>(NativeLibrary, "SDL_HapticRumbleSupported");

            //public static int RumbleSupported(IntPtr haptic)
            //{
            //    return GetError(SDL_HapticRumbleSupported(haptic));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticruneffect(IntPtr haptic, int effect, uint iterations);
            //private static readonly d_sdl_hapticruneffect SDL_HapticRunEffect = FuncLoader.LoadFunction<d_sdl_hapticruneffect>(NativeLibrary, "SDL_HapticRunEffect");

            //public static void RunEffect(IntPtr haptic, int effect, uint iterations)
            //{
            //    GetError(SDL_HapticRunEffect(haptic, effect, iterations));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticstopall(IntPtr haptic);
            //private static readonly d_sdl_hapticstopall SDL_HapticStopAll = FuncLoader.LoadFunction<d_sdl_hapticstopall>(NativeLibrary, "SDL_HapticStopAll");

            //public static void StopAll(IntPtr haptic)
            //{
            //    GetError(SDL_HapticStopAll(haptic));
            //}

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //private delegate int d_sdl_hapticupdateeffect(IntPtr haptic, int effect, ref Effect data);
            //private static readonly d_sdl_hapticupdateeffect SDL_HapticUpdateEffect = FuncLoader.LoadFunction<d_sdl_hapticupdateeffect>(NativeLibrary, "SDL_HapticUpdateEffect");

            //public static void UpdateEffect(IntPtr haptic, int effect, ref Effect data)
            //{
            //    GetError(SDL_HapticUpdateEffect(haptic, effect, ref data));
            //}
        }

        public static class Drop
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Event
            {
                public EventType Type;
                public uint TimeStamp;
                public IntPtr File;
                public uint WindowId;
            }

            //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            //public delegate void d_sdl_free(IntPtr ptr);
            //public static readonly d_sdl_free SDL_Free = FuncLoader.LoadFunction<d_sdl_free>(NativeLibrary, "SDL_free");
        }
    }
}