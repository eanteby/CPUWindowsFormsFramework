﻿using System.Data;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable targetdt, String tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            if (targetdt != null)
            {
                lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void SetControlBinding(Control ctrl, BindingSource bindsource)
        {
            string propertyname = "";
            string controlname = ctrl.Name.ToLower();
            string columnname = controlname.Substring(3);
            string controltype = controlname.Substring(0, 3);
            switch (controltype)
            {

                case "txt":
                case "lbl":
                case "num":
                    propertyname = "Text";
                    break;
                case "dtp":
                    propertyname = "Value";
                    break;
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, bindsource, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatGridForSearchResults(DataGridView grid, string tablename)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatGrid(grid, tablename);
        }

        public static void FormatGridForEdit(DataGridView grid, string tablename)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;            
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = true;
            DoFormatGrid(grid, tablename);
        }

        private static void DoFormatGrid(DataGridView grid, string tablename, bool fillgrid = false)
        {
            grid.AutoSizeColumnsMode = fillgrid == true ? DataGridViewAutoSizeColumnsMode.Fill : DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.HeaderText = System.Text.RegularExpressions.Regex.Replace(col.Name, "([A-Z])", " $1").Trim();
                if (col.Name.EndsWith("Id") || col.Name.EndsWith("Pic"))
                {
                    col.Visible = false;
                }
            }
            string pkname = tablename + "Id";
            if (grid.Columns.Contains(pkname))
            {
                grid.Columns[pkname].Visible = false;
            }
        
        }


        public static int GetIdFromGrid(DataGridView grid, int rowindex, string colname)
        {
            int id = 0;
            if (grid.Rows.Count > rowindex && grid.Columns.Contains(colname) && grid.Rows[rowindex].Cells[colname].Value != null) 
            {
                if (grid.Rows[rowindex].Cells[colname].Value is int)
                {
                    id = (int)grid.Rows[rowindex].Cells[colname].Value;
                }
            }
            return id;
        }

        public static int GetIdFromComboBox(ComboBox lst)
        {
            int value = 0;
            if (lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }
            return value;
        }

        public static void AddComboBoxToGrid(DataGridView grid, DataTable datasource, string tablename, string displaymember )
        {
            DataGridViewComboBoxColumn c = new();
            c.DataSource = datasource;
            c.DisplayMember = displaymember;
            c.ValueMember = tablename + "Id";
            c.DataPropertyName = c.ValueMember;
            c.Name = tablename;
            c.HeaderText = tablename;
            grid.Columns.Insert(0, c);


        }

        public static void AddDeleteButtonToGrid(DataGridView grid, string deletecolname)
        {
            grid.Columns.Add(new DataGridViewButtonColumn() { Text = "X", UseColumnTextForButtonValue = true, HeaderText = "Delete", Name = deletecolname });
        }
        public static bool IsFormOpen(Type formtype, int pkvalue = 0)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkvalue = 0;
                if (frm.Tag != null && frm.Tag is int)
                {
                    frmpkvalue = (int)frm.Tag;
                }

                if (frm.GetType() == formtype && frmpkvalue == pkvalue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }
        public static void SetupNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (f.IsMdiContainer == false)
                {
                    ToolStripButton btn = new();
                    btn.Text = f.Text;
                    btn.Tag = f;
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }
        private static void Btn_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is ToolStripButton)
            {
                ToolStripButton btn = (ToolStripButton)sender;
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                }
            }
        }

    }
}
