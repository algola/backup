using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;
using Services;

namespace PapiroMVC.Areas.CustomerSupplier.Controllers
{
    public class CustomerSupplierBaseController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        private readonly ITypeOfBaseRepository typeOfBaseRepository;
        private readonly ICustomerSupplierRepository customerSupplierRepository;
        private readonly ICustomerSupplierBaseRepository customerSupplierBaseRepository;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            typeOfBaseRepository.SetDbName(CurrentDatabase);
            customerSupplierRepository.SetDbName(CurrentDatabase);
            customerSupplierBaseRepository.SetDbName(CurrentDatabase);
        }

        public CustomerSupplierBaseController(ICustomerSupplierBaseRepository _clisupBaseDataRep, ICustomerSupplierRepository _clisupDataRep, ITypeOfBaseRepository _typeOfBaseDataRep)
        { 
            typeOfBaseRepository = _typeOfBaseDataRep;
            customerSupplierRepository = _clisupDataRep;
            customerSupplierBaseRepository = _clisupBaseDataRep;

            this.Disposables.Add(typeOfBaseRepository);
            this.Disposables.Add(customerSupplierRepository);
            this.Disposables.Add(customerSupplierBaseRepository);
        }
    }
}
