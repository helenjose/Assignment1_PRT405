using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_corporation.Models;

namespace E_corporation.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Report(String User_Name,string sort,string sortdir, string returnUrl)
        {
            try
            {
                if (User_Name == null)
                {
                    User_Name = Session["UserName"].ToString();
                }
                if (User_Name != null)
                {
                    if (sort == null)
                    {
                        sort = "id";
                    }
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
                    List<catalogue> lmd = new List<catalogue>();
                    con.Open();
                    SqlCommand cmd;
                    if (User_Name == "admin")
                    {
                        cmd = new SqlCommand("Select id,username, title, band, length from catalogue order by " + sort + " " + sortdir + " ", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("Select id,username, title, band, length from catalogue order by " + sort + " " + sortdir + " ", con);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();

                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        lmd.Add(new catalogue
                        {
                            Id = Convert.ToInt64(dr["Id"]), // adding data from dataset row in to list<modeldata>
                            Title = dr["Title"].ToString(),
                            Band = dr["Band"].ToString(),
                            Length = dr["Length"].ToString(),
                            Username = dr["username"].ToString()
                        });
                    }
                    Session["UserName"] = User_Name;
                    return View(lmd);
                }
                else
                {
                    ModelState.AddModelError("", "Session Expired");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return RedirectToAction("Logout", "Home");
            }         
        }
        public ActionResult Edit(String ID)
        {
            try
            {
                string User_Name = Session["UserName"].ToString();
                if (User_Name != null)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
                    List<catalogue> lmd = new List<catalogue>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select id,username,title, band, length from catalogue where ID = '" + ID + "' ", con);
                    SqlDataReader dr ;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lmd.Add(new catalogue
                        {
                            Id = Convert.ToInt64(dr["Id"]), // adding data from dataset row in to list<modeldata>
                            Title = dr["Title"].ToString(),
                            Band = dr["Band"].ToString(),
                            Length = dr["Length"].ToString(),
                            Username = dr["username"].ToString()
                        });
                    }
                    return View(lmd);
                }
                else
                {
                    ModelState.AddModelError("", "Session Expired");
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception e)
            {
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "Session Expired");
                return RedirectToAction("Logout", "Home");
            }
        }
        public ActionResult Add(String title, String length, String band)
        {
            try
            {
                string User_Name = Session["UserName"].ToString();
                if (User_Name != null)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into catalogue (username,title,length,band) values ('"+User_Name+"','"+title+"','"+length+"','"+band+"')", con);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Report", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Session Expired");
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Session Expired");
                return RedirectToAction("Logout", "Home");
            }
        }
        public ActionResult Save(String id, String title, String length, String band)
        {
            try
            {
                string User_Name = Session["UserName"].ToString();
                if (User_Name != null)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand("update catalogue set title='"+title+"',length='"+length+"',band='"+band+"' where ID = '" + id + "' ", con);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Report", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Session Expired");
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Session Expired");
                return RedirectToAction("Logout", "Home");
            }
        }
        public ActionResult Delete(String id)
        {
            try
            {
                string User_Name = Session["UserName"].ToString();
                if (User_Name != null)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from catalogue where ID = '" + id + "' ", con);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Report", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Session Expired");
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Session Expired");
                return RedirectToAction("Logout", "Home");
            }
        }
    }
}
