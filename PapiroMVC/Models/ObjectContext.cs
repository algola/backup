using System;
using System.Data.Objects.DataClasses;

namespace PapiroMVC.Models
{



public partial class ObjectContext
{
    /// <summary>
    ///     This method exists for use in LINQ queries,
    ///     as a stub that will be converted to a SQL CAST statement.
    /// </summary>
    [EdmFunction("dbModel", "ParseDouble")]
    public static double ParseDouble(string stringvalue)
    {
        return Double.Parse(stringvalue);
    }
}}