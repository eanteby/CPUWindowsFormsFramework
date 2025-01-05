using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable dt, String tablename)
        {
            lst.DataSource = dt;
            lst.ValueMember = tablename + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", dt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static void SetControlBinding(Control ctrl, DataTable dt)
        {
            string propertyname = "";
            string columnname = "";
            string controlname = ctrl.Name.ToLower();
            string controltype = columnname.Substring(0, 3);
            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyname = "Text";
                    break;
                case "dtp":
                    propertyname = "Value";
                    break;
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, dt, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
    }
}
