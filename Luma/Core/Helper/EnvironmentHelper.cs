using System;

namespace Seth.Luma.Core.Helper
{
    /// <summary>
    /// Useful method for accessing Environment
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Replaces all environment variables (e.g "$(PATH))
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String ExpandEnvironmentVariables(String data)
        {
            var replaced = data;

            if (String.IsNullOrWhiteSpace(replaced) == false)
             {
                 var startIndex = replaced.IndexOf("$(", StringComparison.Ordinal);
                 while (startIndex != -1)
                 {
                     var endIndex = replaced.IndexOf(")", startIndex, StringComparison.Ordinal);
                     if (endIndex != -1)
                     {
                         var environmentVariable = Environment.GetEnvironmentVariable(replaced.Substring(startIndex + 2, endIndex - startIndex - 2));
                         if (String.IsNullOrWhiteSpace(environmentVariable) == false)
                         {
                             replaced = replaced.Substring(0, startIndex) + environmentVariable + replaced.Substring(endIndex + 1);
                         }
                         else
                         {
                             startIndex++;
                         }
                     }
                     else
                     {
                         startIndex++;
                     }

                     startIndex = replaced.IndexOf("$(", startIndex, StringComparison.Ordinal);
                 }
             }

            return replaced;
        }
    }
}
