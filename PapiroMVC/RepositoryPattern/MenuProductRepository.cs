using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class MenuProductRepository : GenericRepository<dbEntities, MenuProduct>, IMenuProductRepository
    {

        public override IQueryable<MenuProduct> GetAll()
        {

            //load each menuproduct
            var c = Context.MenuProducts;
            var tbCode = new List<MenuProduct>();

            tbCode.Add(new MenuProduct { CodCategory = "Description", IndexOf = 0, CodMenuProduct = "Vuoto", IndexOfCategory = 0 });

            ////Fogli Singoli
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 0, CodMenuProduct = "Buste", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 1, CodMenuProduct = "EtichetteCartellini", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 2, CodMenuProduct = "BigliettiVisita", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 3, CodMenuProduct = "CartolineInviti", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 4, CodMenuProduct = "Volantini", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 5, CodMenuProduct = "Pieghevoli", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 6, CodMenuProduct = "CartaIntestata", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 7, CodMenuProduct = "Locandine", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 8, CodMenuProduct = "CartolinePostali", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 9, CodMenuProduct = "FogliMacchina", IndexOfCategory = 1 });
            //tbCode.Add(new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 10, CodMenuProduct = "AltriFormati", IndexOfCategory = 1 });

            //Grande formato
            tbCode.Add(new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 1, CodMenuProduct = "Manifesti", IndexOfCategory = 1 });
            tbCode.Add(new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 2, CodMenuProduct = "PVC", IndexOfCategory = 1 });
            tbCode.Add(new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 3, CodMenuProduct = "Fotoquadri", IndexOfCategory = 1 });
            tbCode.Add(new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 4, CodMenuProduct = "Striscioni", IndexOfCategory = 1 });
            tbCode.Add(new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 5, CodMenuProduct = "SuppRigidi", IndexOfCategory = 1 });
            tbCode.Add(new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 6, CodMenuProduct = "Poster", IndexOfCategory = 1 });

            ////Riviste , Cataloghi e libri
            //tbCode.Add(new MenuProduct { CodCategory = "Book", IndexOf = 0, CodMenuProduct = "PuntoMetallico", IndexOfCategory = 2 });
            //tbCode.Add(new MenuProduct { CodCategory = "Book", IndexOf = 1, CodMenuProduct = "SpiraleMetallica", IndexOfCategory = 2 });
            //tbCode.Add(new MenuProduct { CodCategory = "Book", IndexOf = 2, CodMenuProduct = "BrossuraFresata", IndexOfCategory = 2 });
            //tbCode.Add(new MenuProduct { CodCategory = "Book", IndexOf = 3, CodMenuProduct = "BrossuraCucitaFilo", IndexOfCategory = 2 });
            //tbCode.Add(new MenuProduct { CodCategory = "Book", IndexOf = 4, CodMenuProduct = "RivistePostalizzazione", IndexOfCategory = 2 });
            //tbCode.Add(new MenuProduct { CodCategory = "Book", IndexOf = 5, CodMenuProduct = "SchedeNonRilegate", IndexOfCategory = 2 });


            //etichette in rotolo
            tbCode.Add(new MenuProduct { CodCategory = "Rotoli", IndexOf = 0, CodMenuProduct = "EtichetteRotolo", IndexOfCategory = 3 });
            tbCode.Add(new MenuProduct { CodCategory = "Rotoli", IndexOf = 0, CodMenuProduct = "EtichetteSagRotolo", IndexOfCategory = 3 });
            tbCode.Add(new MenuProduct { CodCategory = "Rotoli", IndexOf = 0, CodMenuProduct = "FasceGommateRotolo", IndexOfCategory = 3 });
            tbCode.Add(new MenuProduct { CodCategory = "Rotoli", IndexOf = 0, CodMenuProduct = "FasceGommateRotolo2", IndexOfCategory = 3 });
            tbCode.Add(new MenuProduct { CodCategory = "Rotoli", IndexOf = 0, CodMenuProduct = "EtichetteRotoloDouble", IndexOfCategory = 3 });


            ////Cliche
            //tbCode.Add(new MenuProduct { CodCategory = "Cliche", IndexOf = 1, CodMenuProduct = "Inciso", IndexOfCategory = 4 });
            //tbCode.Add(new MenuProduct { CodCategory = "Cliche", IndexOf = 2, CodMenuProduct = "Fotopolimero", IndexOfCategory = 4 });

            foreach (var item in tbCode)
            {
                //cerco nel 
                var trv = c.FirstOrDefault(x => x.CodMenuProduct == item.CodMenuProduct);

                if (trv == null)
                {
                    var x = new MenuProduct
                    {
                        CodMenuProduct = item.CodMenuProduct,
                        TimeStampTable = DateTime.Now,
                        CodCategory = item.CodCategory,
                        Enabled = true,
                        IndexOf = item.IndexOf,
                        IndexOfCategory = item.IndexOfCategory,
                        Hidden = false,

                    };

                    trv = x;
                    this.Add(trv);
                }
                else
                {
                    trv.CodCategory = item.CodCategory;
                    trv.IndexOfCategory = item.IndexOfCategory;
                    trv.IndexOf = item.IndexOf;
                    this.Edit(trv);
                }
                this.Save();
            }
                return Context.MenuProducts;

        //    return Context.MenuProducts.Where(x => x.IndexOfCategory == 1 || x.IndexOfCategory == 3 || x.IndexOfCategory == 4);
        }

        public IQueryable<MenuProduct> GetAll(string codMenuProduct)
        {

            return Context.MenuProducts.Where(o => o.CodMenuProduct == codMenuProduct);
        
        }

        public MenuProduct GetSingle(string codMenuProduct)
        {
            var query = Context.MenuProducts.FirstOrDefault(x => x.CodMenuProduct == codMenuProduct);
            return query;
        }
    }
}