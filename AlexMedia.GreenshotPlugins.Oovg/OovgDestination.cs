using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Greenshot.IniFile;
using Greenshot.Plugin;
using GreenshotPlugin.Core;
using log4net;

namespace AlexMedia.GreenshotPlugins.Oovg
{
    public class OovgDestination : AbstractDestination
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(OovgDestination));

        public override ExportInformation ExportCapture(bool manuallyInitiated, ISurface surface, ICaptureDetails captureDetails)
        {
            _log.Debug("Start capture export to oo.vg");

            try
            {
                string uploadUrl;

                var exportInformation = new ExportInformation(this.Designation, this.Description)
                {
                    ExportMade = UploadImage(captureDetails, surface, out uploadUrl),
                    Uri = uploadUrl
                };

                ProcessExport(exportInformation, surface);

                if (exportInformation.ExportMade)
                {
                    Clipboard.SetText(uploadUrl);
                }

                return exportInformation;
            }
            finally
            {
                _log.Debug("Export to oo.vg complete");
            }
        }

        private bool UploadImage(ICaptureDetails captureDetails, ISurface surface, out string url)
        {
            string path = null;

            try
            {
                _log.Debug("Exporting file to oo.vg");

                var uploadUrl = "https://oo.vg/a/upload";

                var config = IniConfig.GetIniSection<OovgConfiguration>();

                if (!string.IsNullOrEmpty(config.UploadKey))
                {
                    uploadUrl += $"?key={HttpUtility.UrlEncode(config.UploadKey)}";
                }
                
                // Temporarily store the file somewhere
                path = ImageOutput.SaveNamedTmpFile(surface, captureDetails, new SurfaceOutputSettings(OutputFormat.png));

                ServicePointManager.Expect100Continue = false;
                var webClient = new WebClient();
                var response = webClient.UploadFile(uploadUrl, "POST", path);

                url = Encoding.ASCII.GetString(response);

                _log.InfoFormat("Upload of {0} to {1} complete", captureDetails.Filename, url);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while uploading file:" + Environment.NewLine + ex.Message, "oo.vg Screenshot Share");

                _log.Fatal("Uploading failed", ex);

                url = null;
                return false;
            }
            finally
            {
                // clean up after ourselves
                if (!string.IsNullOrEmpty(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception e)
                    {
                        _log.Warn("Could not delete temporary file", e);
                    }
                }
            }
        }


        public override string Designation => "oo.vg";

        public override string Description => "Upload to oo.vg";
    }
}
