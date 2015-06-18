using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace PapiroMVC.Helper
{
    /// <summary>
    /// Summary description for KeepSessionAlive
    /// </summary>
    public class KeepSessionAlive : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                context.Session["CodDocument"] = context.Session["CodDocument"];
            }
            catch (Exception)
            {
            }

            try
            {
                context.Session["TsksInPage"] = context.Session["TsksInPage"];
            }
            catch (Exception)
            {
            }    

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}