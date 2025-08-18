using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PharmacyApp
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;

        // Add a new medicine
        public static void AddMedicine(string name, string category, decimal price, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("AddMedicine", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Search medicines by name/category
        public static List<Medicine> SearchMedicine(string searchTerm)
        {
            List<Medicine> results = new List<Medicine>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SearchMedicine", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Medicine
                    {
                        MedicineID = Convert.ToInt32(reader["MedicineID"]),
                        Name = reader["Name"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
            }
            return results;
        }

        // Update stock
        public static void UpdateStock(int medicineId, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UpdateStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MedicineID", medicineId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Record sale
        public static void RecordSale(int medicineId, int quantitySold)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("RecordSale", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MedicineID", medicineId);
                cmd.Parameters.AddWithValue("@QuantitySold", quantitySold);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Get all medicines
        public static List<Medicine> GetAllMedicines()
        {
            List<Medicine> medicines = new List<Medicine>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllMedicines", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    medicines.Add(new Medicine
                    {
                        MedicineID = Convert.ToInt32(reader["MedicineID"]),
                        Name = reader["Name"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }
            }
            return medicines;
        }
    }
}
