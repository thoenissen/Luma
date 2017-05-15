using System;

namespace Seth.Luma.Configuration.Data
{
    /// <summary>
    /// Serializable assembly location
    /// </summary>
    [Serializable]
    public class SerializableAssemblyLocation
    {
        /// <summary>
        /// Description
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public String Path { get; set; }
    }
}