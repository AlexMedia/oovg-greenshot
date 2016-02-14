using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Greenshot.IniFile;
using Greenshot.Plugin;
using GreenshotPlugin.Core;
using log4net;

[assembly: PluginAttribute("AlexMedia.GreenshotPlugins.Oovg.OovgPlugin", true)]
namespace AlexMedia.GreenshotPlugins.Oovg
{
    public class OovgPlugin : IGreenshotPlugin
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(OovgPlugin));

        private ToolStripMenuItem _ctxMenu;
        private OovgConfiguration _config;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            if (_ctxMenu == null)
            {
                return;
            }
            _ctxMenu.Dispose();
            _ctxMenu = null;
        }

        public bool Initialize(IGreenshotHost host, PluginAttribute pluginAttribute)
        {
            _config = IniConfig.GetIniSection<OovgConfiguration>();

            _ctxMenu = new ToolStripMenuItem();
            _ctxMenu.Text = "Configure oo.vg";
            _ctxMenu.Tag = host;
            _ctxMenu.Click += _ctxMenu_Click;

            PluginUtils.AddToContextMenu(host, _ctxMenu);

            return true;
        }

        private void _ctxMenu_Click(object sender, EventArgs e)
        {
            Configure();
        }

        public void Shutdown()
        {
            _log.Debug("oo.vg plugin is shutting down");
        }

        public void Configure()
        {
            _config.ShowConfigDialog();
        }

        public IEnumerable<IDestination> Destinations()
        {
            yield return new OovgDestination();
        }

        public IEnumerable<IProcessor> Processors()
        {
            yield break;
        }
    }
}
