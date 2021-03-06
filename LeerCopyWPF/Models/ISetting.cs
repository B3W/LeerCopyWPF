using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Models
{
    public interface ISetting
    {
        /// <summary>
        /// Unique identifier for setting
        /// </summary>
        string SettingID { get; }

        /// <summary>
        /// Flag indicating if the setting's value has changed
        /// </summary>
        bool ValueModified { get; }

        /// <summary>
        /// The result of the latest 'Validate' call
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Error message describing why setting is in invalid or empty if setting valid
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Validates the setting
        /// </summary>
        /// <returns>true if valid, false otherwise</returns>
        bool Validate();

        /// <summary>
        /// Saves the setting
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        bool Save();
    }
}
