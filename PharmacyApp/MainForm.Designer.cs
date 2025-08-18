using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PharmacyApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent(); // This is auto-generated in MainForm.Designer.cs
        }

        private void btnAddMedicine_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper.AddMedicine(txtName.Text, txtCategory.Text,
                    decimal.Parse(txtPrice.Text), int.Parse(txtQuantity.Text));
                MessageBox.Show("Medicine added successfully!");
                LoadMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding medicine: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<Medicine> results = DatabaseHelper.SearchMedicine(txtSearch.Text);
                dgvMedicines.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching medicines: " + ex.Message);
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            try
            {
                int medicineId = int.Parse(txtMedicineID.Text);
                int qty = int.Parse(txtQuantity.Text);
                DatabaseHelper.UpdateStock(medicineId, qty);
                MessageBox.Show("Stock updated successfully!");
                LoadMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating stock: " + ex.Message);
            }
        }

        private void btnRecordSale_Click(object sender, EventArgs e)
        {
            try
            {
                int medicineId = int.Parse(txtMedicineID.Text);
                int qtySold = int.Parse(txtQuantity.Text);
                DatabaseHelper.RecordSale(medicineId, qtySold);
                MessageBox.Show("Sale recorded successfully!");
                LoadMedicines();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error recording sale: " + ex.Message);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadMedicines();
        }

        private void LoadMedicines()
        {
            try
            {
                List<Medicine> medicines = DatabaseHelper.GetAllMedicines();
                dgvMedicines.DataSource = medicines;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicines: " + ex.Message);
            }
        }
    }
}
