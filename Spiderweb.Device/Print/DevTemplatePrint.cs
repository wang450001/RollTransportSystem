using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderweb.Device.Print
{
    public class DevTemplatePrint : CPrint
    {
        XtraReport report;

        public DevTemplatePrint() : base()
        {

        }

        public DevTemplatePrint(string connStr) : base(connStr)
        {

        }

        protected override void Init()
        {
            base.Init();

            report = new XtraReport()
            {
                DisplayName = "纸卷标签"
            };
        }

        public override bool Write(params object[] obj)
        {
            if (obj == null || obj.Length < 2)
            {
                OnSendMessage("纸卷标签数据异常，请检查");
                return false;
            }

            string labelTemplate = obj[1].ToString();
            if (!System.IO.File.Exists(labelTemplate))
            {
                OnSendMessage($"打印模板文件<{labelTemplate}>不存在，请检查");
                return false;
            }

            try
            {
                report.LoadLayout(labelTemplate);                

                report.DataSource = obj[0];
                // 无条码数据时，设置条码控件不可见
                XRControl ctrlBar = report.FindControl("label16", true);
                if (ctrlBar != null) ctrlBar.Visible = true;

                //report.Print(PrinterName);
                report.PrintAsync(PrinterName);
            }
            catch (Exception ex)
            {
                OnSendMessage($"打印纸卷标签失败，{ex.Message}");
            }
            return true;
        }
    }
}
