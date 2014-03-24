﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntities : DbContext
    {
        public dbEntities()
            : base("name=dbEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<TaskExecutor> taskexecutors { get; set; }
        public DbSet<TaskEstimatedOn> taskexecutorestimatedon { get; set; }
        public DbSet<Step> steps { get; set; }
        public DbSet<ArticleCost> articlecost { get; set; }
        public DbSet<Article> articles { get; set; }
        public DbSet<CustomerSupplierBase> customersupplierbases { get; set; }
        public DbSet<CustomerSupplier> customersuppliers { get; set; }
        public DbSet<TypeOfBase> typeofbase { get; set; }
        public DbSet<ProductPart> ProductParts { get; set; }
        public DbSet<ProductPartsPrintableArticle> ProductPartsPrintableArticles { get; set; }
        public DbSet<ProductPartTask> ProductPartTasks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTask> ProductTasks { get; set; }
        public DbSet<TaskExecutorTypeOfTask> TaskExecutorTypeOfTasks { get; set; }
        public DbSet<TypeOfTask> TypeOfTasks { get; set; }
        public DbSet<OptionTypeOfTask> OptionTypeOfTasks { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<DocumentProduct> DocumentProducts { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<productpartstoproducttask> ProductPartsToProductTasks { get; set; }
        public DbSet<CostDetail> CostDetail { get; set; }
        public DbSet<ProductPartPrinting> ProductPartPrinting { get; set; }
        public DbSet<ProductPartPrintingGain> ProductPartPrintingGain { get; set; }
        public DbSet<Makeready> Makereadies { get; set; }
        public DbSet<TaskExecutorCylinder> TaskExecutorCylinders { get; set; }
    }
}
