using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Greenshot.IniFile;

namespace AlexMedia.GreenshotPlugins.Oovg
{
    [IniSection("Oovg", Description = "oo.vg plugin configuration")]
    public class OovgConfiguration : IniSection
    {
        [IniProperty("UploadKey", Description = "oo.vg upload key")]
        public string UploadKey;

        /// <summary>
		/// A form for token
		/// </summary>
		/// <returns>bool true if OK was pressed, false if cancel</returns>
		public bool ShowConfigDialog()
        {
            using (var oovgConfigurationForm = new OovgConfigurationForm(this))
            {
                var result = oovgConfigurationForm.ShowDialog();

                return result == DialogResult.OK;
            }
        }
    }
}
