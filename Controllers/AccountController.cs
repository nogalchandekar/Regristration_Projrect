using Microsoft.AspNetCore.Mvc;
using Regristration_Project.Models;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop.Implementation;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Http;

using System.Data;

namespace Regristration_Project.Controllers
{
    public class AccountController : Controller
    {
        string conn;

        public AccountController()
        {

            var dbconfig = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json").Build();
             conn = dbconfig["ConnectionStrings:constr"];
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Login(LoginModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con=new SqlConnection(conn))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_login", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmailId", obj.EmailID);
                        cmd.Parameters.AddWithValue("@Password", obj.Password);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            HttpContext.Session.SetString("Uname", dr["Name"].ToString());
                            HttpContext.Session.SetString("LoginTime", System.DateTime.Now.ToShortTimeString());
                            return RedirectToAction("Home", "Account");
                        }


                        else
                        {
                              ViewBag.error="EmailID or Password is not correct";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "something went wrong");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return View();
        }

        [HttpGet]

        public IActionResult Regrister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Regrister(RegristerModel obj)
        {

            try
              {

                if (ModelState.IsValid)
                {

                    using (SqlConnection con = new SqlConnection(conn)) 
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_insert", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", obj.Name);
                        cmd.Parameters.AddWithValue("@rollno", obj.RollNo);
                        cmd.Parameters.AddWithValue("@Age", obj.Age);
                        cmd.Parameters.AddWithValue("@dob", obj.DOB);
                        cmd.Parameters.AddWithValue("@fees", obj.Fees);
                        cmd.Parameters.AddWithValue("@status", obj.Status);
                        cmd.Parameters.AddWithValue("@role", obj.Role);
                        cmd.Parameters.AddWithValue("@password", obj.Password);
                        cmd.Parameters.AddWithValue("@emailid", obj.EmailId);
                        int x = cmd.ExecuteNonQuery();
                        if(x>0)
                        {
                            return RedirectToAction("Login","Account");
                        }
                        else
                        {
                            ModelState.AddModelError("", "something went wrong");
                            return View();
                        }
                    }


                }
                else
                {

                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
            }
            finally
            {

            }
            return View();


        }
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Displaydata()
        {
            List<DisplayModel>obj=getalldata();
            return View(obj);

            
        }

        public  List<DisplayModel> getalldata()
        {
            List<DisplayModel> display = new List<DisplayModel>();
            using (SqlConnection con=new SqlConnection(conn))
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_getalldata", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                                    display.Add(
    new DisplayModel
    {
        ID = Convert.ToInt32(dr["ID"].ToString()), // Add parentheses here
        Name = dr["name"].ToString(),
        RollNo = dr["rollno"].ToString(),
        Age = Convert.ToInt32(dr["Age"].ToString()),
        DOB = Convert.ToDateTime(dr["Dob"].ToString()),
        Fees = Convert.ToDecimal(dr["Fees"].ToString()),
        Status = dr["Status"].ToString(),
        Role = dr["role"].ToString(),
        Password = dr["Password"].ToString(),
        EmailId = dr["emailID"].ToString()
    }
                                             );



                }
            }
            return display;
        }


        [HttpGet]
        public IActionResult Edit(int? ID)
        {
            UpdateModel obj=getdatabyid((int) ID);
            return View(obj);

        }

        [HttpPost]
        public IActionResult Edit(UpdateModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_update", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", obj.Name);
                        cmd.Parameters.AddWithValue("@RollNo", obj.RollNo);
                        cmd.Parameters.AddWithValue("@Age", obj.Age);
                        cmd.Parameters.AddWithValue("@DOB", obj.DOB);
                        cmd.Parameters.AddWithValue("@Fees", obj.Fees);
                        cmd.Parameters.AddWithValue("@Status", obj.Status); // Correct parameter name
                        cmd.Parameters.AddWithValue("@Role", obj.Role);
                        cmd.Parameters.AddWithValue("@Password", obj.Password);
                        cmd.Parameters.AddWithValue("@EmailId", obj.EmailId);
                        cmd.Parameters.AddWithValue("@ID", obj.ID); // Make sure you have an ID property in your UpdateModel

                        int x = cmd.ExecuteNonQuery();

                        if (x >= 0)
                        {
                            return RedirectToAction("DisplayData", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Something went Wrong");
                            return View();
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
            }

            return View();
        }


        public IActionResult Delete(int ID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return View("Displaydata");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something went wrog");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public UpdateModel getdatabyid(int ID)
        {
            UpdateModel obj = null;
            try
            {
                if (ID == null)
                {

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_getdatabyid", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID",ID);
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            obj = new UpdateModel();
                            obj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                            obj.RollNo = ds.Tables[0].Rows[i]["RollNo"].ToString();

                            obj.Age =Convert.ToInt32(ds.Tables[0].Rows[i]["Age"].ToString());
                            obj.DOB = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dob"].ToString());
                            obj.Fees = Convert.ToDecimal(ds.Tables[0].Rows[i]["Fees"].ToString());
                            obj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                            obj.Role = ds.Tables[0].Rows[i]["Role"].ToString();
                            obj.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                            obj.EmailId = ds.Tables[0].Rows[i]["EmailID"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return obj;
        }
    }
}
