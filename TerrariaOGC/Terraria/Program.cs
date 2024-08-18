using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Terraria
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Marshal.PrelinkAll(typeof(Main));
			ThreadPool.SetMinThreads(0, 0);
			ThreadPool.SetMaxThreads(0, 0);
#if !USE_ORIGINAL_CODE
			Environment.SetEnvironmentVariable("FNA3D_FORCE_DRIVER", "D3D11");
			Environment.SetEnvironmentVariable("FNA3D_BACKBUFFER_SCALE_NEAREST", "1");
#endif

			using (Main TheGame = new Main())
			{
				try
				{
					TheGame.Run();
				}
				catch (Exception Ex)
				{
					try
					{
						using (StreamWriter ErrorWriter = new StreamWriter("client-crashlog.txt", append: true))
						{
							ErrorWriter.WriteLine(DateTime.Now);
							ErrorWriter.WriteLine(Ex);
							ErrorWriter.WriteLine("");
#if !USE_ORIGINAL_CODE
							var Tracer = new StackTrace(Ex, true);
                            StackFrame frame = null;
                            for (int i = 0; i < Tracer.FrameCount; i++)
                            {
                                frame = Tracer.GetFrame(i);
                                ErrorWriter.WriteLine(frame.GetMethod());
                                ErrorWriter.WriteLine(frame.GetFileLineNumber());
                            }
                            ErrorWriter.WriteLine("");
#endif
						}
					}
					catch
					{
					}
				}
			}
		}
	}
}
