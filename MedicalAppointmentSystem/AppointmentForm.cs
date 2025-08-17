using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public partial class AppointmentForm : Form
{
    private ComboBox cboDoctor, cboPatient;
    private DateTimePicker dtp;
    private TextBox txtNotes;
    private Button btnBook;

    public AppointmentForm()
    {
        InitializeComponent();

        cboDoctor = new ComboBox { Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
        cboPatient = new ComboBox { Left = 20, Top = 60, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
        dtp = new DateTimePicker { Left = 20, Top = 100, Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
        txtNotes = new TextBox { Left = 20, Top = 140, Width = 300 };
        btnBook = new Button { Left = 20, Top = 180, Width = 160, Text = "Book Appointment" };

        btnBook.Click += BtnBook_Click;

        Controls.AddRange(new Control[] { cboDoctor, cboPatient, dtp, txtNotes, btnBook });

        Text = "Book Appointment";
        Width = 380; Height = 270;
        StartPosition = FormStartPosition.CenterParent;

        LoadDoctors();
        LoadPatients();
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    private void LoadDoctors()
    {
        try
        {
            using (var conn = Db.GetOpenConnection())
            using (var cmd = new SqlCommand("SELECT DoctorID, FullName FROM Doctors WHERE Availability=1 ORDER BY FullName", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                var t = new DataTable();
                t.Load(rdr);
                cboDoctor.DisplayMember = "FullName";
                cboDoctor.ValueMember = "DoctorID";
                cboDoctor.DataSource = t;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load doctors.\n{ex.Message}", "Error");
        }
    }

    private void LoadPatients()
    {
        try
        {
            using (var conn = Db.GetOpenConnection())
            using (var cmd = new SqlCommand("SELECT PatientID, FullName FROM Patients ORDER BY FullName", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                var t = new DataTable();
                t.Load(rdr);
                cboPatient.DisplayMember = "FullName";
                cboPatient.ValueMember = "PatientID";
                cboPatient.DataSource = t;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load patients.\n{ex.Message}", "Error");
        }
    }

    private void BtnBook_Click(object sender, EventArgs e)
    {
        if (cboDoctor.SelectedValue == null || cboPatient.SelectedValue == null)
        {
            MessageBox.Show("Select a doctor and a patient.", "Validation");
            return;
        }

        int doctorId = (int)cboDoctor.SelectedValue;
        int patientId = (int)cboPatient.SelectedValue;
        DateTime apptDate = dtp.Value;
        string notes = txtNotes.Text?.Trim() ?? "";

        try
        {
            using (var conn = Db.GetOpenConnection())
            {
                // Availability check (same date)
                using (var chk = new SqlCommand(
                    @"SELECT COUNT(*) FROM Appointments
                      WHERE DoctorID=@D AND CONVERT(date, AppointmentDate)=CONVERT(date, @Dt)", conn))
                {
                    chk.Parameters.Add("@D", SqlDbType.Int).Value = doctorId;
                    chk.Parameters.Add("@Dt", SqlDbType.DateTime).Value = apptDate;
                    int count = (int)chk.ExecuteScalar();
                    if (count > 0)
                    {
                        var r = MessageBox.Show("Doctor has bookings that day. Continue?", "Warning",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (r == DialogResult.No) return;
                    }
                }

                using (var cmd = new SqlCommand(
                    @"INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes)
                      VALUES (@DoctorID, @PatientID, @AppointmentDate, @Notes)", conn))
                {
                    cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorId;
                    cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientId;
                    cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = apptDate;
                    cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 400).Value = (object)notes ?? DBNull.Value;

                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show(rows > 0 ? "Booked!" : "Nothing inserted.", "Result");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Booking failed.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
