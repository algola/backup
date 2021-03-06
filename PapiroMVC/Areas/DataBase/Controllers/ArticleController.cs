﻿using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;
using Services;
using PapiroMVC.Validation;
using Newtonsoft.Json;
using Mvc.HtmlHelpers;
using System.Data.OleDb;
using System.Xml;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    [AuthorizeUser]
    public partial class ArticleController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IArticleRepository articleRepository;
        private readonly ICustomerSupplierRepository customerSupplierRepository;
        private readonly ITaskExecutorRepository taskExecutorRepository;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            articleRepository.SetDbName(CurrentDatabase);
            customerSupplierRepository.SetDbName(CurrentDatabase);
            warehouseRepository.SetDbName(CurrentDatabase);
            taskExecutorRepository.SetDbName(CurrentDatabase);
        }

        public ArticleController(IArticleRepository _articleDataRep,
            ICustomerSupplierRepository _dataRepCS,
            IWarehouseRepository _warehouseDataRep,
            ITaskExecutorRepository _taskExecutorRep)
        {
            articleRepository = _articleDataRep;
            customerSupplierRepository = _dataRepCS;
            warehouseRepository = _warehouseDataRep;
            taskExecutorRepository = _taskExecutorRep;

            this.Disposables.Add(articleRepository);
            this.Disposables.Add(customerSupplierRepository);
            this.Disposables.Add(warehouseRepository);
            this.Disposables.Add(taskExecutorRepository);
        }

        //
        // GET: /Article/
        public ActionResult Index()
        {
            return View(new ArticleAutoChangesViewModel());
        }



        [AuthorizeAlgola(Roles = "Warehouse")]
        public ActionResult IndexWarehouse()
        {
            return View();
        }



        [HttpPost]
        public ActionResult ExcelDie(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                    excelConnection1.Close();
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    var a = new DieFlexo();
                    //a.CodArticle = (ds.Tables[0].Rows[i][0].ToString()).PadLeft(6, '0');
                    //a.ArticleName = ds.Tables[0].Rows[i][1].ToString();
                    //a.CodDie = "Vuoto";

                    var str = ds.Tables[0].Rows[i][0].ToString();
                    string format = String.Empty;
                    var leftstr = str.IndexOf(" ");
                    format = str.Substring(0, leftstr);
                    try
                    {
                        format = ((Convert.ToDouble(format.GetSide1()) / 10).ToString("0.##")) + "x" + ((Convert.ToDouble(format.GetSide2()) / 10).ToString("0.##"));
                    }
                    catch (Exception)
                    {
                        format = "0x0";
                    }

                    a.Format = format;
                    a.Description = (ds.Tables[0].Rows[i][1].ToString()) + "-" + (ds.Tables[0].Rows[i][2].ToString());
                    a.ArticleName = a.Description;

                    try
                    {
                        a.Width = Convert.ToDouble(ds.Tables[0].Rows[i][3]) / 10;
                    }
                    catch (Exception)
                    {
                        a.Width = 0;
                    }

                    try
                    {
                        if ((ds.Tables[0].Rows[i][4].ToString()).Contains("Z")) a.Z = Convert.ToInt32((ds.Tables[0].Rows[i][4].ToString()).Remove(0, 1));
                        else a.Z = Convert.ToInt32(ds.Tables[0].Rows[i][4]);
                    }
                    catch (Exception)
                    {
                        a.Z = 0;
                    }

                    a.PrintingFormat = a.Width + "x" + a.GetCmFromZ(Convert.ToInt32(a.Z ?? 0));

                    a.MaxGain1 = Convert.ToInt32(ds.Tables[0].Rows[i][5]);
                    a.TaskExecutorName = "GALLUS";

                    int resatot = 0;
                    string str2 = ds.Tables[0].Rows[i][6].ToString();
                    try
                    {
                        if (str2.Contains(" "))
                        {
                            str2 = str2.Remove(str2.IndexOf(" "), str2.Length - str2.IndexOf(" "));
                            resatot = Convert.ToInt32(str2);
                        }

                        else
                        {
                            resatot = Convert.ToInt32(ds.Tables[0].Rows[i][6]);
                        }
                    }
                    catch (Exception)
                    {
                        resatot = 0;
                    }


                    a.MaxGain2 = resatot / a.MaxGain1;

                    if (ds.Tables[0].Rows[i][0].ToString().Contains("F"))
                    {
                        int index = (ds.Tables[0].Rows[i][0].ToString()).IndexOf("F");
                        a.CodDie = (ds.Tables[0].Rows[i][0].ToString()).Substring(index);
                    }
                    else
                    {
                        (ds.Tables[0].Rows[i][0].ToString()).Replace(" ", "");
                        a.CodDie = ds.Tables[0].Rows[i][0].ToString();
                    }
                    var typeFormat = ds.Tables[0].Rows[i][0].ToString();

                    if (typeFormat.ToLower().Contains("sag")) a.FormatType = 2;
                    if (typeFormat.ToLower().Contains("ret")) a.FormatType = 3;
                    if (typeFormat.ToLower().Contains("tonda") || typeFormat.ToLower().Contains("roto")) a.FormatType = 5;
                    if (typeFormat.ToLower().Contains("ova")) a.FormatType = 1;
                    if (typeFormat.ToLower().Contains("quad")) a.FormatType = 0;
                    if (typeFormat.ToLower().Contains("trian")) a.FormatType = 4;


                    a.CodArticle = articleRepository.GetNewCode(a, customerSupplierRepository, "Die", "Die");
                    var b = articleRepository.GetSingle(a.CodArticle);
                    if (b == null)
                    {
                        articleRepository.Add(a);
                    }
                    else
                    {
                        //b.ProductName = a.ProductName;
                        //b.CodMenuProduct = "Vuoto";
                        articleRepository.Edit(b);
                    }

                    articleRepository.Save();

                }

            }



            return View();
        }


        public ActionResult ExcelDie2(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                    excelConnection1.Close();
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                for (int i = 2; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    Die a;

                    if ((ds.Tables[0].Rows[i][6].ToString()).Equals(""))
                    {
                        a = new DieFlatRoll();
                    }
                    else
                    {
                        a = new DieFlexo();
                    }

                    if (a.TypeOfArticle.ToString().Equals("DieFlexo"))
                    {
                        string format = String.Empty;
                        try
                        {
                            format = ((Convert.ToDouble(ds.Tables[0].Rows[i][0]) / 10).ToString()) + "x" + ((Convert.ToDouble(ds.Tables[0].Rows[i][1]) / 10).ToString());
                        }
                        catch (Exception)
                        {
                            format = "0x0";
                        }
                        a.Format = format;
                        a.MaxGain1 = Convert.ToInt32(ds.Tables[0].Rows[i][2]);
                        a.MaxGain2 = Convert.ToInt32(ds.Tables[0].Rows[i][3]);

                        try
                        {
                            a.Width = Convert.ToDouble(ds.Tables[0].Rows[i][5]) / 10;
                        }
                        catch (Exception)
                        {
                            a.Width = 0;
                        }

                        a.Z = Convert.ToInt32(ds.Tables[0].Rows[i][6]);
                        a.PrintingFormat = a.Width + "x" + a.GetCmFromZ(Convert.ToInt32(a.Z ?? 0));
                        a.TaskExecutorName = ds.Tables[0].Rows[i][11].ToString();
                        a.CodDie = ds.Tables[0].Rows[i][13].ToString();
                        a.Description = ds.Tables[0].Rows[i][0].ToString() + "x" + ds.Tables[0].Rows[i][1].ToString();
                        //      a.ArticleName = a.Description;

                        var typeFormat = ds.Tables[0].Rows[i][0].ToString();

                        try
                        {
                            if (typeFormat.ToLower().Contains("tonda") || typeFormat.ToLower().Contains("roto"))
                            {
                                a.FormatType = 5;
                            }
                            else
                            {
                                if (typeFormat.ToLower().Contains("ova"))
                                {
                                    a.FormatType = 1;
                                }
                                else
                                {
                                    a.FormatType = 3;
                                }
                            }

                        }
                        catch (Exception)
                        {
                            a.FormatType = 3;
                        }
                    }
                    else
                    {
                        string format = String.Empty;
                        try
                        {
                            format = ((Convert.ToDouble(ds.Tables[0].Rows[i][0]) / 10).ToString()) + "x" + ((Convert.ToDouble(ds.Tables[0].Rows[i][1]) / 10).ToString());
                        }
                        catch (Exception)
                        {
                            format = "0x0";
                        }
                        a.Format = format;
                        a.MaxGain1 = Convert.ToInt32(ds.Tables[0].Rows[i][2]);
                        a.MaxGain2 = Convert.ToInt32(ds.Tables[0].Rows[i][3]);

                        try
                        {
                            a.Width = Convert.ToDouble(ds.Tables[0].Rows[i][5]) / 10;
                        }
                        catch (Exception)
                        {
                            a.Width = 0;
                        }
                        a.PrintingFormat = a.Width + "x" + (Convert.ToInt32(ds.Tables[0].Rows[i][7]) / 10);
                        a.TaskExecutorName = ds.Tables[0].Rows[i][11].ToString();
                        a.CodDie = ds.Tables[0].Rows[i][13].ToString();
                        a.Description = ds.Tables[0].Rows[i][0].ToString() + "x" + ds.Tables[0].Rows[i][1].ToString();
                        //      a.ArticleName = a.Description;

                        var typeFormat = ds.Tables[0].Rows[i][0].ToString();

                        try
                        {
                            if (typeFormat.ToLower().Contains("tonda") || typeFormat.ToLower().Contains("roto"))
                            {
                                a.FormatType = 5;
                            }
                            else
                            {
                                if (typeFormat.ToLower().Contains("ova"))
                                {
                                    a.FormatType = 1;
                                }
                                else
                                {
                                    a.FormatType = 3;
                                }
                            }

                        }
                        catch (Exception)
                        {
                            a.FormatType = 3;
                        }
                    }


                    a.CodArticle = articleRepository.GetNewCode(a, customerSupplierRepository, "Die", "Die");
                    var b = articleRepository.GetSingle(a.CodArticle);
                    if (b == null)
                    {
                        articleRepository.Add(a);
                    }
                    else
                    {
                        //b.ProductName = a.ProductName;
                        //b.CodMenuProduct = "Vuoto";
                        articleRepository.Edit(b);
                    }

                    articleRepository.Save();

                }

            }



            return View();
        }


        [HttpGet]
        [AuthorizeAlgola(Roles = "Warehouse")]
        public ActionResult EditArticleOnlyMov(string id)
        {
            //devo caricare nel prodotto anche tutte le informazioni per il magazzino
            var locations = warehouseRepository.GetWarehouseList();
            ViewBag.Locations = locations;

            var art = articleRepository.GetSingle(id);
            return View(art);
        }




        public ActionResult IndexSheetPrintableArticle()
        {
            return View(new SheetPrintableArticleAutoChanges());
        }

        public ActionResult IndexRollPrintableArticle()
        {
            return View(new RollPrintableArticleAutoChanges());
        }

        public ActionResult IndexRigidPrintableArticle()
        {
            return View(new RigidPrintableArticleAutoChanges());
        }

        public ActionResult IndexNoPrintable()
        {
            return View();
        }

        public ActionResult IndexInk()
        {
            return View();
        }

        public ActionResult IndexMesh()
        {
            return View();
        }


        public ActionResult IndexAnilox()
        {
            return View();
        }



        public ActionResult IndexFoil()
        {
            return View();
        }



        public ActionResult IndexDie()
        {
            return View();
        }

        [AuthorizeUser]
        [HttpGet]
        public ActionResult CreateSheetPrintableArticle()
        {
            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateSheetPrintableArticle";
            return View(new SheetPrintableArticleViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSheetPrintableArticle(SheetPrintableArticleViewModel c)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Add(c.Article);
                    //rigeneration name of article
                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index") });
                }
                catch (NoSupplierException)
                {
                    ModelState.AddModelError("PersError", "NoSupplierError");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }

            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateSheetPrintableArticle";
            return PartialView("_EditAndCreateSheetPrintableArticle", c);
        }

        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateRollPrintableArticle(string tags)
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateRollPrintableArticle";
            var vm = new RollPrintableArticleViewModel();
            vm.Article.Tags = tags;
            return View(vm);
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateRollPrintableArticle(RollPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Add(c.Article);

                    //rigeneration name of article
                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRollPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateRollPrintableArticle";
            return PartialView("_EditAndCreateRollPrintableArticle", c);

        }

        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateRigidPrintableArticle()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateRigidPrintableArticle";
            return View(new RigidPrintableArticleViewModel());
        }


        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateNoPrintable()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateNoPrintable";
            return View(new NoPrintableViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNoPrintable(NoPrintableViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexNoPrintable") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateNoPrintable";
            return PartialView("_EditAndCreateNoPrintable", c);

        }







        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditNoPrintable(NoPrintableViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    //                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexNoPrintable") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditNoPrintable";
            return PartialView("_EditAndCreateNoPrintable", c);
        }

        public ActionResult EditNoPrintable(string id)
        {
            NoPrintableViewModel viewModel = new NoPrintableViewModel();
            viewModel.Article = (NoPrintable)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditNoPrintable";
            return View("EditNoPrintable", viewModel);
        }



        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateMesh()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateMesh";
            return View(new MeshViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateMesh(MeshViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    //   c.Article.ArticleName =  c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexMesh") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateMesh";
            return PartialView("_EditAndCreateMesh", c);

        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditMesh(MeshViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    articleRepository.SincroSupplier(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexMesh") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditMesh";
            return PartialView("_EditAndCreateMesh", c);
        }

        public ActionResult EditMesh(string id)
        {
            MeshViewModel viewModel = new MeshViewModel();
            viewModel.Article = (Mesh)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditMesh";
            return View("EditMesh", viewModel);
        }

        public ActionResult MeshAutoComplete(string term)
        {
            Article[] typeOfSerigraphy = articleRepository.GetAll().OfType<Mesh>().ToArray();

            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);

            var filteredItems = typeOfSerigraphy.Where(
            item => item.ArticleName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ArticleName,
                                 label = art.ArticleName,
                                 value = art.ArticleName
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }







        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateAnilox()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateAnilox";
            return View(new AniloxViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAnilox(AniloxViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    //   c.Article.ArticleName =  c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexAnilox") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateAnilox";
            return PartialView("_EditAndCreateAnilox", c);

        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAnilox(AniloxViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    articleRepository.SincroSupplier(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexAnilox") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditAnilox";
            return PartialView("_EditAndCreateAnilox", c);
        }

        public ActionResult EditAnilox(string id)
        {
            AniloxViewModel viewModel = new AniloxViewModel();
            viewModel.Article = (Anilox)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditAnilox";
            return View("EditAnilox", viewModel);
        }

        public ActionResult AniloxAutoComplete(string term)
        {
            Article[] typeOfSerigraphy = articleRepository.GetAll().OfType<Anilox>().ToArray();

            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);

            var filteredItems = typeOfSerigraphy.Where(
            item => item.ArticleName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ArticleName,
                                 label = art.ArticleName,
                                 value = art.ArticleName
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }











        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateInk()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateInk";
            return View(new InkViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateInk(InkViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    //   c.Article.ArticleName =  c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexInk") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateInk";
            return PartialView("_EditAndCreateInk", c);

        }







        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditInk(InkViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    articleRepository.SincroSupplier(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexInk") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditInk";
            return PartialView("_EditAndCreateInk", c);
        }

        public ActionResult EditInk(string id)
        {
            InkViewModel viewModel = new InkViewModel();
            viewModel.Article = (Ink)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditInk";
            return View("EditInk", viewModel);
        }


        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateFoil()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateFoil";
            return View(new FoilViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateFoil(FoilViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    //   c.Article.ArticleName =  c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexFoil") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateFoil";
            return PartialView("_EditAndCreateFoil", c);

        }







        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditFoil(FoilViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    articleRepository.SincroSupplier(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexFoil") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditFoil";
            return PartialView("_EditAndCreateFoil", c);
        }

        public ActionResult EditFoil(string id)
        {
            FoilViewModel viewModel = new FoilViewModel();
            viewModel.Article = (Foil)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditFoil";
            return View("EditFoil", viewModel);
        }






        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateRigidPrintableArticle(RigidPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRigidPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateRigidPrintableArticle";
            return PartialView("_EditAndCreateRigidPrintableArticle", c);

        }

        [HttpGet]
        public ActionResult WizardRollPrintableArticle()
        {
            return View(new RollPrintableArticleViewModelWizard());
        }

        public ActionResult WizardRollPrintableArticle(RollPrintableArticleViewModelWizard c)
        {
            RollPrintableArticle a;

            if (ModelState.IsValid)
            {
                try
                {
                    //we have to count weights and widths and use 2 counter to 

                    //WeightS
                    foreach (var weight in c.Weights)
                    {
                        //control if weigth is valid
                        if (weight > 0)
                            foreach (var width in c.Widths)
                            {
                                if (width > 0)
                                {

                                    c.Article.Weight = (long)weight;
                                    c.Article.Width = width;

                                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                                    //c.RollPrintableArticleCuttedCost.TimeStampTable = DateTime.Now;
                                    c.RollPrintableArticleStandardCost.TimeStampTable = DateTime.Now;

                                    //                       c.RollPrintableArticleCuttedCost.CodArticle = c.Article.CodArticle;
                                    //                       c.RollPrintableArticleCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                                    c.RollPrintableArticleStandardCost.CodArticle = c.Article.CodArticle;
                                    c.RollPrintableArticleStandardCost.CodArticleCost = c.Article.CodArticle + "_STD";


                                    c.Article.ArticleName = c.Article.ToString();

                                    c.Article.TimeStampTable = DateTime.Now;

                                    a = (RollPrintableArticle)c.Article.Clone_();

                                    //cost = (RollPrintableArticleStandardCost)c.RollPrintableArticleStandardCost.Clone();
                                    //a.ArticleCosts.Clear();
                                    //a.ArticleCosts.Add(cost);

                                    articleRepository.Add(a);
                                    articleRepository.Save();
                                }
                            }
                    }

                    return Json(new { redirectUrl = Url.Action("IndexRollPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return PartialView("_WizardRollPrintableArticle", c);
        }

        [HttpGet]
        public ActionResult WizardSheetPrintableArticle()
        {
            return View(new SheetPrintableArticleViewModelWizard());
        }

        public ActionResult WizardSheetPrintableArticle(SheetPrintableArticleViewModelWizard c)
        {
            SheetPrintableArticle a;

            if (ModelState.IsValid)
            {
                try
                {
                    //we have to count weights and widths and use 2 counter to 

                    //WeightS
                    foreach (var weight in c.Weights)
                    {
                        //control if weigth is valid
                        if (weight > 0)
                            foreach (var format in c.Formats)
                            {
                                if (format != "")
                                {

                                    c.Article.Weight = (long)weight;
                                    c.Article.Format = format;

                                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                                    c.Article.ArticleName = c.Article.ToString();

                                    a = c.Article.Clone_();

                                    articleRepository.Add(a);
                                    articleRepository.Save();

                                }
                            }
                    }

                    return Json(new { redirectUrl = Url.Action("IndexSheetPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return PartialView("_WizardSheetPrintableArticle", c);
        }

        #region Edit

        //
        // GET: /Article/Edit/5
        public ActionResult Edit(string id)
        {
            //this is a common point where edit function is called
            //base on type we have to call right method

            //load article
            var article = articleRepository.GetSingle(id);
            ActionResult ret = null;

            //check type


            ret = RedirectToAction(article.GetEditMethod(), "Article", new { id = id });

            //switch (article.TypeOfArticle)
            //{
            //    case Article.ArticleType.SheetPrintableArticle:
            //        ret = RedirectToAction("EditSheetPrintableArticle", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.RollPrintableArticle:
            //        ret = RedirectToAction("EditRollPrintableArticle", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.RigidPrintableArticle:
            //        ret = RedirectToAction("EditRigidPrintableArticle", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.ObjectPrintableArticle:
            //        ret = RedirectToAction("EditObjectPrintableArticle", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.NoPrintable:
            //        ret = RedirectToAction("EditNoPrintable", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.DieFlexo:
            //        ret = RedirectToAction("EditDieFlexo", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.DieFlatRoll:
            //        ret = RedirectToAction("EditDieFlatRoll", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.DieSheet:
            //        ret = RedirectToAction("EditDieSheet", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.Ink:
            //        ret = RedirectToAction("EditInk", "Article", new { id = id });
            //        break;

            //    case Article.ArticleType.Foil:
            //        ret = RedirectToAction("EditFoil", "Article", new { id = id });
            //        break;
            //}

            return ret;
        }



        public ActionResult EditSheetPrintableArticle(string id)
        {
            SheetPrintableArticleViewModel viewModel = new SheetPrintableArticleViewModel();
            viewModel.Article = (SheetPrintableArticle)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            ViewBag.ActionMethod = "EditSheetPrintableArticle";
            return View("EditSheetPrintableArticle", viewModel);
        }


        public ActionResult EditRollPrintableArticle(string id)
        {
            RollPrintableArticleViewModel viewModel = new RollPrintableArticleViewModel();
            viewModel.Article = (RollPrintableArticle)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditRollPrintableArticle";
            return View("EditRollPrintableArticle", viewModel);
        }

        public ActionResult EditRigidPrintableArticle(string id)
        {
            RigidPrintableArticleViewModel viewModel = new RigidPrintableArticleViewModel();
            viewModel.Article = (RigidPrintableArticle)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditRigidPrintableArticle";
            return View("EditRigidPrintableArticle", viewModel);
        }


        #endregion

        //
        // POST: /Article/Edit/5
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSheetPrintableArticle(SheetPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexSheetPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      

            ViewBag.ActionMethod = "EditSheetPrintableArticle";
            return PartialView("_EditAndCreateSheetPrintableArticle", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRigidPrintableArticle(RigidPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    //                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRigidPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditRigidPrintableArticle";
            return PartialView("_EditAndCreateRigidPrintableArticle", c);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRollPrintableArticle(RollPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    //                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRollPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditRollPrintableArticle";
            return PartialView("_EditAndCreateRollPrintableArticle", c);
        }


        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return Json(new { redirectUrl = Url.Action("Index") });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Article/Delete/5

        public ActionResult Delete(string id)
        {
            return View();
        }

        //
        // POST: Database/Article/DeleteArticle/5,

        [HttpPost]
        public ActionResult DeleteArticle(string ids, string urlBack)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);
            foreach (var id in strings)
            {
                var c = articleRepository.GetSingle(id);
                articleRepository.Delete(c);
            }

            articleRepository.Save();

            return Json(new { redirectUrl = Url.Action(urlBack, "Article", new { area = "Database" }) });
        }


        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateDieFlexo()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateDieFlexo";
            return View(new DieFlexo());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateDieFlexo(DieFlexo c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    c.CodArticle = articleRepository.GetNewCode(c, customerSupplierRepository, c.SupplierMaker, c.SupplierMaker);
                    //c.PrintingFormat = c.Width + "x" + Math.Truncate(Convert.ToDouble((Convert.ToDouble(c.Z) / 8) * 2.54) * 100) / 100;

                    c.PrintingFormat = c.Width + "x" + c.GetCmFromZ(Convert.ToInt32(c.Z ?? 0));

                    //CHECK IF TASKEXECUTOR EXIST
                    var resTask = taskExecutorRepository.GetAll().FirstOrDefault(x => x.TaskExecutorName == c.TaskExecutorName);
                    if (resTask != null)
                    {
                        c.CodTaskExecutor = resTask.CodTaskExecutor;
                    }
                    else
                    {
                        c.CodTaskExecutor = null;
                    }

                    articleRepository.Add(c);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDie") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateDieFlexo";
            return PartialView("_EditAndCreateDieFlexo", c);

        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDieFlexo(DieFlexo c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    c.PrintingFormat = c.Width + "x" + Convert.ToDouble(c.Z) / 8 * 2.54;

                    //CHECK IF TASKEXECUTOR EXIST
                    var resTask = taskExecutorRepository.GetAll().FirstOrDefault(x => x.TaskExecutorName == c.TaskExecutorName);
                    if (resTask != null)
                    {
                        c.CodTaskExecutor = resTask.CodTaskExecutor;
                    }
                    else
                    {
                        c.CodTaskExecutor = null;
                    }



                    articleRepository.Edit(c);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDie") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditDieFlexo";
            return PartialView("_EditAndCreateDieFlexo", c);
        }

        public ActionResult EditDieFlexo(string id)
        {
            DieFlexo viewModel = new DieFlexo();
            viewModel = (DieFlexo)articleRepository.GetSingle(id);


            var tsk = taskExecutorRepository.GetSingle(viewModel.CodTaskExecutor);

            if (tsk != null)
            {
                viewModel.TaskExecutorName = tsk.TaskExecutorName;

            }

            //get producer and maker

            if (viewModel.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditDieFlexo";
            return View("EditDieFlexo", viewModel);
        }


        //--------- DieSheet

        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateDieSheet()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateDieSheet";
            return View(new DieSheet());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateDieSheet(DieSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.CodArticle = articleRepository.GetNewCode(c, customerSupplierRepository, c.SupplierMaker, c.SupplierMaker);


                    articleRepository.Add(c);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDie") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateDieSheet";
            return PartialView("_EditAndCreateDieSheet", c);

        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDieSheet(DieSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    articleRepository.Edit(c);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDie") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditDieSheet";
            return PartialView("_EditAndCreateDieSheet", c);
        }

        public ActionResult EditDieSheet(string id)
        {
            DieSheet viewModel = new DieSheet();
            viewModel = (DieSheet)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditDieSheet";
            return View("EditDieSheet", viewModel);
        }


        //---------DieFlatRoll

        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateDieFlatRoll()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateDieFlatRoll";
            return View(new DieFlatRoll());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateDieFlatRoll(DieFlatRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.CodArticle = articleRepository.GetNewCode(c, customerSupplierRepository, c.SupplierMaker, c.SupplierMaker);


                    articleRepository.Add(c);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDie") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateDieFlatRoll";
            return PartialView("_EditAndCreateDieFlatRoll", c);

        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDieFlatRoll(DieFlatRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    articleRepository.Edit(c);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDie") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditDieFlatRoll";
            return PartialView("_EditAndCreateDieFlatRoll", c);
        }

        public ActionResult EditDieFlatRoll(string id)
        {
            DieFlatRoll viewModel = new DieFlatRoll();
            viewModel = (DieFlatRoll)articleRepository.GetSingle(id);


            //get producer and maker

            if (viewModel.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditDieFlatRoll";
            return View("EditDieFlatRoll", viewModel);
        }





        public ActionResult InkAutoComplete(string term)
        {
            Article[] typeOfSerigraphy = articleRepository.GetAll().OfType<Ink>().ToArray();

            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);

            var filteredItems = typeOfSerigraphy.Where(
            item => item.ArticleName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ArticleName,
                                 label = art.ArticleName,
                                 value = art.ArticleName
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        #region Warehouse


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeAlgola(Roles = "Warehouse")]
        public ActionResult NewMovArticle(NewMovViewModel c)
        {

            //devo caricare nel prodotto anche tutte le informazioni per il magazzino
            var locations = warehouseRepository.GetWarehouseList();
            ViewBag.Locations = locations;

            if (ModelState.IsValid)
            {
                try
                {
                    //il codice del movimento lo prendo dal codice dell'articolo
                    c.Mov.CodWarehouseArticle = c.ArticleOrProduct.CodWarehouseArticle;
                    c.Mov.CodWarehouseArticleMov = warehouseRepository.GetNewMovCode(c.Mov);
                    warehouseRepository.AddMov(c.Mov);
                    warehouseRepository.Save();

                    warehouseRepository.UpdateArticle(warehouseRepository.GetSingle(c.Mov.CodWarehouseArticle));
                    warehouseRepository.Save();

                    var temp = warehouseRepository.GetSingle(c.Mov.CodWarehouseArticle);
                    var newMov = new NewMovViewModel();
                    newMov.ArticleOrProduct = temp;
                    newMov.Mov = new WarehouseArticleMov { WarehouseArticle = temp, CodWarehouseArticle = temp.CodWarehouseArticle };


                    return Json(new { redirectUrl = Url.Action("NewMovArticle", new { CodArticle = temp.CodArticle, CodWarehouse = temp.CodWarehouse }) });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            return PartialView("_NewMovArticle", c);
        }


        /// <summary>
        /// New movment of specific product in a specific Warehouse
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>

        [AuthorizeAlgola(Roles = "Warehouse")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult NewMovArticle(string codArticle, string codWarehouse)
        {

            //devo caricare nel prodotto anche tutte le informazioni per il magazzino
            var locations = warehouseRepository.GetWarehouseList();
            ViewBag.Locations = locations;

            //the new movment display warehouse information
            //plus new movment in accord to warehouse article specify

            var c = warehouseRepository.GetSingleArticle(codArticle, codWarehouse);
            var newMov = new NewMovViewModel();
            newMov.IsProduct = false;

            newMov.ArticleOrProduct = c;
            newMov.Mov = new WarehouseArticleMov { WarehouseArticle = c, CodWarehouseArticle = c == null ? "" : c.CodWarehouseArticle };

            return View("NewMovArticle", newMov);

        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewMovArticleOrder(NewMovViewModel c)
        {
            c.Mov.TypeOfMov = 2; //ordine!!!!
            return NewMovArticle(c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewMovArticleMov(NewMovViewModel c)
        {

            var oldWare = c.Mov.WarehouseArticle.CodWarehouse;
            var newWare = c.CodWarehouseTo;

            //devo caricare nel prodotto anche tutte le informazioni per il magazzino
            var locations = warehouseRepository.GetWarehouseList();
            ViewBag.Locations = locations;

            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.WarehouseItem art;
                    //  prod = warehouseRepository.GetSingleProduct(c.ArticleOrProduct.CodProduct, c.ArticleOrProduct.CodWarehouse);

                    c.Mov.TypeOfMov = 0; //scarico!!!!
                    c.Mov.CodWarehouseArticle = c.ArticleOrProduct.CodWarehouseArticle;
                    c.Mov.CodWarehouseArticleMov = warehouseRepository.GetNewMovCode(c.Mov);
                    warehouseRepository.AddMov(c.Mov);
                    warehouseRepository.Save();

                    warehouseRepository.UpdateArticle(warehouseRepository.GetSingle(c.ArticleOrProduct.CodWarehouseArticle));
                    warehouseRepository.Save();

                    art = warehouseRepository.GetSingle(c.ArticleOrProduct.CodWarehouseArticle);

                    warehouseRepository.SetDbName(CurrentDatabase);

                    c.Mov.TypeOfMov = 1; //scarico!!!!
                    c.Mov.CodWarehouseArticle = newWare + "P" + art.CodProduct;
                    c.ArticleOrProduct.CodWarehouseArticle = c.Mov.CodWarehouseArticle;
                    c.Mov.CodWarehouseArticleMov = warehouseRepository.GetNewMovCode(c.Mov);
                    warehouseRepository.AddMov(c.Mov);
                    warehouseRepository.Save();

                    warehouseRepository.UpdateArticle(warehouseRepository.GetSingle(c.ArticleOrProduct.CodWarehouseArticle));
                    warehouseRepository.Save();

                    return Json(new { redirectUrl = Url.Action("NewMovArticle", new { CodArticle = art.CodArticle, CodWarehouse = art.CodWarehouse }) });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            return PartialView("_NewMovArticle", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewMovArticleLoad(NewMovViewModel c)
        {
            c.Mov.TypeOfMov = 1; //carico!!!!
            return NewMovArticle(c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewMovArticleUnLoad(NewMovViewModel c)
        {
            c.Mov.TypeOfMov = 0; //scarico!!!!
            return NewMovArticle(c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewMovArticleReserve(NewMovViewModel c)
        {
            c.Mov.TypeOfMov = 3; //impegno!!!!
            return NewMovArticle(c);
        }

        [AuthorizeAlgola(Roles = "Warehouse")]
        public ActionResult UpdateWarehouseArticleInfo(string codWarehouseArticle, string codArticle, string codWarehouse)
        {

            //devo caricare nel prodotto anche tutte le informazioni per il magazzino
            var locations = warehouseRepository.GetWarehouseList();
            ViewBag.Locations = locations;

            var c = warehouseRepository.GetSingleArticle(codArticle, codWarehouse);
            return PartialView("_WarehouseArticleInfo", new NewMovViewModel { ArticleOrProduct = c });
        }


        [AuthorizeAlgola(Roles = "Warehouse")]
        public ActionResult UpdateWarehouseMinQuantity(string codWarehouseArticle, string minQuantity)
        {

            //devo caricare nel prodotto anche tutte le informazioni per il magazzino
            var locations = warehouseRepository.GetWarehouseList();
            ViewBag.Locations = locations;

            var c = warehouseRepository.GetSingle(codWarehouseArticle);
            c.MinQuantity = Convert.ToInt32(minQuantity);

            warehouseRepository.Edit(c);
            warehouseRepository.Save();

            return PartialView("_WarehouseArticleInfo", new NewMovViewModel { ArticleOrProduct = c });
        }

        [AuthorizeAlgola(Roles = "Warehouse")]
        public ActionResult ArticleWarehouseList(GridSettings gridSettings)
        {
            string codArticleFilter = string.Empty;
            string productNameFilter = string.Empty;
            string typeOfProductFilter = string.Empty;
            string warehouseNameFilter = string.Empty;
            string sottoScortaFilter = string.Empty;
            //read from validation's language file

            //LANGFILE
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);

            if (gridSettings.isSearch)
            {
                codArticleFilter = gridSettings.where.rules.Any(r => r.field == "CodArticle") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodArticle").data : string.Empty;

                productNameFilter = gridSettings.where.rules.Any(r => r.field == "ArticleName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ArticleName").data : string.Empty;

                typeOfProductFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfProduct") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfProduct").data : string.Empty;

                warehouseNameFilter = gridSettings.where.rules.Any(r => r.field == "WarehouseName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "WarehouseName").data : string.Empty;

                sottoScortaFilter = gridSettings.where.rules.Any(r => r.field == "MinQuantity") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "MinQuantity").data : string.Empty;
            }


            var fff = articleRepository.GetAll().ToArray();
            var q = warehouseRepository.GetAll();

            if (!string.IsNullOrEmpty(codArticleFilter))
            {
                q = q.Where(c => c.CodArticle.ToLower().Contains(codArticleFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(productNameFilter))
            {
                var names = productNameFilter.Split(' ');

                foreach (var name in names)
                {
                    q = q.Where(c => c.Article.ArticleName.ToLower().Contains(name.ToLower()));
                }

            }

            if (!string.IsNullOrEmpty(warehouseNameFilter))
            {
                q = q.Where(c => c.WarehouseSpec.WarehouseName.ToLower().Contains(warehouseNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sottoScortaFilter))
            {
                q = q.Where(a => ((a.PotentialAvailable ?? 0) < (a.MinQuantity ?? 0)));
            }


            switch (gridSettings.sortColumn)
            {
                case "CodArticle":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodArticle) : q.OrderBy(c => c.CodArticle);
                    break;
                case "ArticleName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Article.ArticleName) : q.OrderBy(c => c.Article.ArticleName);
                    break;
                case "WarehouseName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.WarehouseSpec.WarehouseName) : q.OrderBy(c => c.WarehouseSpec.WarehouseName);
                    break;
            }

            var q2 = q.ToList();

            var pp = q2.OfType<WarehouseArticle>();

            var q3 = pp.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize).ToList();

            int totalRecords = pp.Count();

            // create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;

            var jsonData = new
            {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
                rows =
                (
                    from a in q3
                    select new
                    {
                        id = a.CodWarehouseArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                            a.CodArticle,
                            a.WarehouseSpec.WarehouseName,
                            a.Article.ArticleName,
                            ((a.PotentialAvailable??0) < (a.MinQuantity??0))?"Sotto Scorta":"",
                            a.QuantityOnHand==null?"0":a.QuantityOnHand.ToString(),
                            a.Available==null?"0":a.Available.ToString(),
                            a.PotentialQuantityOnHand==null?"0":a.PotentialQuantityOnHand.ToString(),
                            a.PotentialAvailable==null?"0":a.PotentialAvailable.ToString()
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
