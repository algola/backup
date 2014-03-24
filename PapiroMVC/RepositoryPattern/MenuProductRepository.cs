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

            var tbCode = new MenuProduct[24];

            //Fogli Singoli
            tbCode[0] = new MenuProduct { CodCategory="FogliSingoli", IndexOf = 0, CodMenuProduct="Buste", IndexOfCategory = 0};
            tbCode[1] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 1, CodMenuProduct = "EtichetteCartellini", IndexOfCategory = 0 };
            tbCode[2] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 2, CodMenuProduct = "BigliettiVisita", IndexOfCategory = 0 };
            tbCode[3] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 3, CodMenuProduct = "CartolineInviti", IndexOfCategory = 0 };
            tbCode[4] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 4, CodMenuProduct = "Volantini", IndexOfCategory = 0 };
            tbCode[5] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 5, CodMenuProduct = "Pieghevoli", IndexOfCategory = 0 };
            tbCode[6] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 6, CodMenuProduct = "CartaIntestata", IndexOfCategory = 0 };
            tbCode[7] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 7, CodMenuProduct = "Locandine", IndexOfCategory = 0 };
            tbCode[8] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 8, CodMenuProduct = "CartolinePostali", IndexOfCategory = 0 };
            tbCode[9] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 9, CodMenuProduct = "FogliMacchina", IndexOfCategory = 0 };
            tbCode[10] = new MenuProduct { CodCategory = "FogliSingoli", IndexOf = 10, CodMenuProduct = "AltriFormati", IndexOfCategory = 0 };

            //Grande formato
            tbCode[11] = new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 1, CodMenuProduct = "Manifesti", IndexOfCategory = 1 };
            tbCode[12] = new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 2, CodMenuProduct = "PVC", IndexOfCategory = 1 };
            tbCode[13] = new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 3, CodMenuProduct = "Fotoquadri", IndexOfCategory = 1 };
            tbCode[14] = new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 4, CodMenuProduct = "Striscioni", IndexOfCategory = 1 };
            tbCode[15] = new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 5, CodMenuProduct = "SuppRigidi", IndexOfCategory = 1 };
            tbCode[16] = new MenuProduct { CodCategory = "GrandeFormato", IndexOf = 6, CodMenuProduct = "Poster", IndexOfCategory = 1 };

            //Riviste , Cataloghi e libri
            tbCode[17] = new MenuProduct { CodCategory = "Book", IndexOf = 0, CodMenuProduct = "PuntoMetallico", IndexOfCategory = 2 };
            tbCode[18] = new MenuProduct { CodCategory = "Book", IndexOf = 1, CodMenuProduct = "SpiraleMetallica", IndexOfCategory = 2 };
            tbCode[19] = new MenuProduct { CodCategory = "Book", IndexOf = 2, CodMenuProduct = "BrossuraFresata", IndexOfCategory = 2 };
            tbCode[20] = new MenuProduct { CodCategory = "Book", IndexOf = 3, CodMenuProduct = "BrossuraCucitaFilo", IndexOfCategory = 2 };
            tbCode[21] = new MenuProduct { CodCategory = "Book", IndexOf = 4, CodMenuProduct = "RivistePostalizzazione", IndexOfCategory = 2 };
            tbCode[22] = new MenuProduct { CodCategory = "Book", IndexOf = 5, CodMenuProduct = "SchedeNonRilegate", IndexOfCategory = 2 };


            //etichette in rotolo
            tbCode[23] = new MenuProduct { CodCategory = "Rotoli", IndexOf = 0, CodMenuProduct = "EtichetteRotolo", IndexOfCategory = 3 };


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