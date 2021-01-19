using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetImageForProduct
{
 
    public partial class Form1 : Form
    {
        string ConnStr = @"Data Source=localhost;Initial Catalog=BOOKSTOREWEB;Integrated Security=True";
        SqlConnection conn = null;
  
        SqlCommand cmd;
        string imageURL = null;
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(ConnStr);
            cmd = conn.CreateCommand();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    imageURL = ofd.FileName;
                    pictureBox.Image= Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(imageURL);
            byte[] arr;
            ImageConverter converter = new ImageConverter();
            arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
          try
          {
                int idProduct = Convert.ToInt32(txtIdProduct.Text);
                if (idProduct > 0)
                {
                using (SqlConnection openCon = new SqlConnection(ConnStr))
                {
                    string saveStaff = "INSERT into PhotoProduct(idProduct, photo) values (@idProduct, @image)";

                    using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                    {
                        querySaveStaff.Connection = openCon;
                        querySaveStaff.Parameters.AddWithValue("@idProduct", idProduct);
                        querySaveStaff.Parameters.AddWithValue("@image", arr);
                        
                        openCon.Open();

                        querySaveStaff.ExecuteNonQuery();
                    }
                }

                using (SqlConnection openCon = new SqlConnection(ConnStr))
                {
                    string saveStaff = "select * from PhotoProduct";

                    using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                    {
                        querySaveStaff.Connection = openCon;
                       
                        openCon.Open();

             
                        DataTable dt = new DataTable();
                        dt.Load(querySaveStaff.ExecuteReader());
                        dtgvListProduct.DataSource = dt;
                    }
                }

               }
          }
           catch
          {
              MessageBox.Show("You are missing out something!!!");
          }
           

        }
    }
}
