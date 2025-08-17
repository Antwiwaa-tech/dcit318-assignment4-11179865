using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public partial class DoctorListForm : Form
{
    private TextBox txtSearch;
    private DataGridView grid;
    private DataTable table;

    public DoctorListForm()
    {
        InitializeComponent();

        txtSearch = new TextBox { Left = 10, Top = 10, Width = 300, PlaceholderText = "Search name/specialty..." };
        grid = new DataGridView { Left = 10, Top = 40, Width = 560, Height = 320, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        txtSearch.TextChanged += (s, e) => ApplyFilter();

        Controls.AddRange(new Control[] { txtSearch, grid });
        Text = "Doctors";
        Width = 600; Height = 420;
        StartPosition = FormStartPosition.CenterParent;

        LoadDoctors();
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    private void LoadDoctors()
    {
        table = new DataTable();
        try
        {
            using (var conn = Db.GetOpenConnection())
            using (var cmd = new SqlCommand("SELECT DoctorID, FullName, Specialty, Availability FROM Doctors", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                table.Load(rdr);
                grid.DataSource = table;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load doctors.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ApplyFilter()
    {
        if (table == null) return;
        var q = txtSearch.Text.Replace("'", "''");
        table.DefaultView.RowFilter = $"FullName LIKE '%{q}%' OR Specialty LIKE '%{q}%'";
    }
}
