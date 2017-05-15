using System;
using System.Diagnostics;

namespace Seth.Luma.Core.Diagnostics
{
    /// <summary>
    /// Tracing Debug and Trace output
    /// </summary>
    public class DebugListener : TraceListener
    {
        /// <summary>
        /// Writing trace data
        /// </summary>
        /// <param name="message">Message</param>
        public override void Write(String message)
        {
            Console.Write(message);
        }

        /// <summary>
        /// Writing trace data
        /// </summary>
        /// <param name="message">Message</param>
        public override void WriteLine(String message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Write an exception to Trace
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void WriteToTrace(Exception ex)
        {
            var traceLine = "----------------------------------------\n"
                          + "--- Start: " + DateTime.Now + " ---------\n"
                          + "----------------------------------------\n"
                          + "----------------------------------------\n"
                          + ex.ToString()
                          + "----------------------------------------\n"
                          + "--- End: " + DateTime.Now + " -----------\n"
                          + "----------------------------------------\n";

            Trace.WriteLine(traceLine);
        }
    }
}
