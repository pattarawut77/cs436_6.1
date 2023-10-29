using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AppSystem.Pages.Siam
{
    public class EditSiamModel : PageModel
    {
        public StockInfo StockInfo = new StockInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String itemid = Request.Query["itemid"];

            try
            {
                String connectionString = "Server=tcp:svapp.database.windows.net,1433;Initial Catalog=DBAPP;Persist Security Info=False;User ID=Nice2MU;Password=Owen1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM stocks WHERE itemid=@itemid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemid", itemid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                StockInfo.itemid = "" + reader.GetInt32(0);
                                StockInfo.item = reader.GetString(1);
                                StockInfo.storeid = reader.GetString(2);
                                StockInfo.supplier = reader.GetString(3);
                                StockInfo.amount = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            StockInfo.itemid = Request.Form["itemid"];
            StockInfo.item = Request.Form["item"];
            StockInfo.storeid = Request.Form["storeid"];
            StockInfo.supplier = Request.Form["supplier"];
            StockInfo.amount = Request.Form["amount"];

            if (StockInfo.item.Length == 0 || StockInfo.storeid.Length == 0 ||
                StockInfo.supplier.Length == 0 || StockInfo.amount.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Server=tcp:svapp.database.windows.net,1433;Initial Catalog=DBAPP;Persist Security Info=False;User ID=Nice2MU;Password=Owen1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE stocks " +
                                 "SET item=@item, storeid=@storeid, supplier=@supplier, amount=@amount " +
                                 "WHERE itemid=@itemid;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemid", StockInfo.itemid);
                        command.Parameters.AddWithValue("@item", StockInfo.item);
                        command.Parameters.AddWithValue("@storeid", StockInfo.storeid);
                        command.Parameters.AddWithValue("@supplier", StockInfo.supplier);
                        command.Parameters.AddWithValue("@amount", StockInfo.amount);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Siam/IndexSiam");
        }
    }
}
