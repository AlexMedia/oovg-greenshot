using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Greenshot.IniFile;
using GreenshotPlugin.Controls;

namespace AlexMedia.GreenshotPlugins.Oovg
{
    public partial class OovgConfigurationForm : Form
    {
        private readonly OovgConfiguration _oovgConfiguration;

        public OovgConfigurationForm(OovgConfiguration oovgConfiguration)
        {
            _oovgConfiguration = oovgConfiguration;
            InitializeComponent();

            this.UploadKey.Text = _oovgConfiguration.UploadKey;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _oovgConfiguration.UploadKey = UploadKey.Text;
            IniConfig.Save();

            DialogResult = DialogResult.OK;
        }

        private void lnkGenerate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var guidText = Guid.NewGuid().ToString("N").ToUpperInvariant();

            var key = guidText.Substring(0, 4) + guidText.Substring(13, 7) + guidText.Substring(23, 3);
            UploadKey.Text = key;
        }
    }
}
