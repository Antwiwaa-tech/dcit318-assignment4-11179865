using System;
using System.Windows.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        var btnDoctors = new Button { Text = "View Doctors", Left = 20, Top = 20, Width = 180 };
        var btnBook = new Button { Text = "Book Appointment", Left = 20, Top = 60, Width = 180 };
        var btnManage = new Button { Text = "Manage Appointments", Left = 20, Top = 100, Width = 180 };

        btnDoctors.Click += (s, e) => new DoctorListForm().ShowDialog();
        btnBook.Click += (s, e) => new AppointmentForm().ShowDialog();
        btnManage.Click += (s, e) => new ManageAppointmentsForm().ShowDialog();

        Controls.AddRange(new Control[] { btnDoctors, btnBook, btnManage });
        Text = "Medical Appointment Booking - Main";
        Width = 250; Height = 190;
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }
}
