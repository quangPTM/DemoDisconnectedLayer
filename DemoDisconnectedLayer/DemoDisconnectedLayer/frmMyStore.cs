using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDisconnectedLayer
{
    public partial class frmMyStore : Form
    {
        public frmMyStore()
        {
            InitializeComponent();
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            var strConnection = config["ConnectionStrings:MyStoreDB"];

            return strConnection;

        }
        private void btnViewProducts_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = dsMyStore.Tables[0];
        }

        private void btnViewCategories_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = dsMyStore.Tables[1];

        }

        private void btnClose_Click(object sender, EventArgs e)=>this.Close();

        DataSet dsMyStore = new DataSet();
        private void frmMyStore_Load(object sender, EventArgs e)
        {

            DbProviderFactory factory = SqlClientFactory.Instance;

            using DbConnection connection = factory.CreateConnection();

            if (connection == null)
            {

                Console.WriteLine($"Unable to create the connection object.");
                return;

            }
            connection.ConnectionString = GetConnectionString();
            connection.Open();
            string SQL = "Select ProductID, ProductName, UnitsInStock From Products; select * from categories ";
            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(SQL, GetConnectionString());
                dataAdapter.Fill(dsMyStore);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Data From Database");
            }
        }
    }
}
