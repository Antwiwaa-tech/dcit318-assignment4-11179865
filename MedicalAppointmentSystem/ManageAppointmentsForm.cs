using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public partial class ManageAppointmentsForm : Form
{
    private DataGridView grid;
    private TextBox txtFilter;
    private Button btnRefresh, btnSave, btnDelete;
    private DataSet ds;
    private SqlDataAdapter adapter;
    private string baseSelect =
        @"SELECT A.AppointmentID, A.DoctorID, D.FullName AS DoctorName,
                 A.PatientID, P.FullName AS PatientName,
                 A.AppointmentDate, A.Notes
          FROM Appointments A
          JOIN Doctors D  ON D.DoctorID = A.DoctorID
          JOIN Patients P ON P.PatientID = A.PatientID";

    public ManageAppointmentsForm()
    {
        InitializeComponent();

        txtFilter = new TextBox { Left = 10, Top = 10, Width = 300, PlaceholderText = "Filter by patient/doctor..." };
        grid = new DataGridView { Left = 10, Top = 40, Width = 780, Height = 340, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        btnRefresh = new Button { Left = 10, Top = 390, Width = 120, Text = "Refresh" };
        btnSave = new Button { Left = 140, Top = 390, Width = 140, Text = "Save Changes" };
        btnDelete = new Button { Left = 290, Top = 390, Width = 160, Text = "Delete Selected" };

        txtFilter.TextChanged += (s, e) => ApplyFilter();
        btnRefresh.Click += (s, e) => LoadData();
        btnSave.Click += (s, e) => SaveChanges();
        btnDelete.Click += (s, e) => DeleteSelected();

        Controls.AddRange(new Control[] { txtFilter, grid, btnRefresh, btnSave, btnDelete });
        Text = "Manage Appointments";
        Width = 820; Height = 470;
        StartPosition = FormStartPosition.CenterParent;

        LoadData();
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    private void LoadData()
    {
        ds = new DataSet();

        try
        {
            using (var conn = Db.GetOpenConnection())
            {
                adapter = new SqlDataAdapter(baseSelect + " ORDER BY A.AppointmentDate DESC", conn);

                adapter.UpdateCommand = new SqlCommand(
                    @"UPDATE Appointments
                      SET AppointmentDate=@Date, Notes=@Notes
                      WHERE AppointmentID=@Id", conn);
                adapter.UpdateCommand.Parameters.Add("@Date", SqlDbType.DateTime, 0, "AppointmentDate");
                adapter.UpdateCommand.Parameters.Add("@Notes", SqlDbType.VarChar, 400, "Notes");
                adapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "AppointmentID");

                adapter.Fill(ds, "Appointments");
                grid.DataSource = ds.Tables["Appointments"];

                grid.Columns["AppointmentID"].ReadOnly = true;
                grid.Columns["DoctorID"].ReadOnly = true;
                grid.Columns["DoctorName"].ReadOnly = true;
                grid.Columns["PatientID"].ReadOnly = true;
                grid.Columns["PatientName"].ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Load failed.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ApplyFilter()
    {
        if (ds == null) return;
        var dt = ds.Tables["Appointments"];
        if (dt == null) return;

        var q = txtFilter.Text.Replace("'", "''");
        dt.DefaultView.RowFilter = $"DoctorName LIKE '%{q}%' OR PatientName LIKE '%{q}%'";
    }

    private void SaveChanges()
    {
        try
        {
            using (var conn = Db.GetOpenConnection())
            {
                adapter.UpdateCommand.Connection = conn;
                int rows = adapter.Update(ds, "Appointments");
                MessageBox.Show($"Saved {rows} change(s).", "Success");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Save failed.\n{ex.Message}", "Error");
        }
    }

    private void DeleteSelected()
    {
        if (grid.CurrentRow == null)
        {
            MessageBox.Show("Select a row first.", "Info");
            return;
        }

        int id = Convert.ToInt32(grid.CurrentRow.Cells["AppointmentID"].Value);
        var confirm = MessageBox.Show("Delete selected appointment?", "Confirm",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            using (var conn = Db.GetOpenConnection())
            using (var cmd = new SqlCommand("DELETE FROM Appointments WHERE AppointmentID=@Id", conn))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show(rows > 0 ? "Deleted." : "No rows deleted.", "Result");
                LoadData();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Delete failed.\n{ex.Message}", "Error");
        }
    }
}
