using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Data;
using System.Collections.Generic;

namespace Services
{
    public class FormatsNameRepository : IFormatsNameRepository
    {
        public ProductFormatName[] GetAllById(string id)
        {

            ProductFormatName[] ret;

            switch (id)
            {
                case "Buste":
                    ret = new ProductFormatName[6] {  
                            new ProductFormatName {CodFormat= "11x23", FormatName="11x23"}, 
                            new ProductFormatName {CodFormat= "16x23", FormatName="16x23" },
                            new ProductFormatName {CodFormat= "22x23", FormatName="22x23" },
                            new ProductFormatName {CodFormat= "23x23", FormatName="23x23" },
                            new ProductFormatName {CodFormat= "19x26", FormatName="19x26" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};

                    break;
                case "EtichetteCartellini":
                    ret = new ProductFormatName[4] {  
                            new ProductFormatName {CodFormat= "5x5", FormatName="5x5"}, 
                            new ProductFormatName {CodFormat= "9,5x5", FormatName="9,5x5" },
                            new ProductFormatName {CodFormat= "8x5", FormatName="8x5"}, 
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0"}};
                    break;
                case "BigliettiVisita":
                    ret = new ProductFormatName[3] {  
                            new ProductFormatName {CodFormat= "8,5x5", FormatName="8,5x5"}, 
                            new ProductFormatName {CodFormat= "9x5", FormatName="9x5" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0"}};
                    break;
                case "CartolineInviti":
                    ret = new ProductFormatName[5] {  
                            new ProductFormatName {CodFormat= "10,5x14,8", FormatName="10,5x14,5"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21"}, 
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"}, 
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"}, 
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "Volantini":
                    ret = new ProductFormatName[5] {  
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10,5x14,8", FormatName="10,5x14,8"}, 
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"}, 
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "Pieghevoli":
                    ret = new ProductFormatName[5] {  
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"}, 
                            new ProductFormatName {CodFormat= "17x24", FormatName="17x24"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"}, 
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "CartaIntestata":
                    ret = new ProductFormatName[2] {  
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "Locandine":
                            ret = new ProductFormatName[4] {  
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "42x59,4", FormatName="42x59,4"},
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "CartolinePostali":
                    ret = new ProductFormatName[3] {  
                            new ProductFormatName {CodFormat= "16,5x11,5", FormatName="16,5x11,5"},
                            new ProductFormatName {CodFormat= "15x10,5", FormatName="15x10,5"},
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "FogliMacchina":
                    ret = new ProductFormatName[3] {  
                            new ProductFormatName {CodFormat= "64x88", FormatName="64x88"},
                            new ProductFormatName {CodFormat= "70x100", FormatName="70x100"},
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "AltriFormati":
                    ret = new ProductFormatName[16] {  
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "9x5", FormatName="9x5"},
                            new ProductFormatName {CodFormat= "8,5x5,5", FormatName="8,5x5,5"},
                            new ProductFormatName {CodFormat= "8,5x5,5", FormatName="8,5x5,5"},
                            new ProductFormatName {CodFormat= "8,8x5", FormatName="8,8x5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10,5x14,8", FormatName="10,5x14,8"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"},
                            new ProductFormatName {CodFormat= "17x24", FormatName="17x24"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "31x43", FormatName="31x43"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},                           
                            new ProductFormatName {CodFormat= "42x59,4", FormatName="42x59,4"},
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "PuntoMetallico":
                    ret = new ProductFormatName[12] {  
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x21", FormatName="29,7x21"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "21x14,8", FormatName="21x14,8"},
                            new ProductFormatName {CodFormat= "16,5x24", FormatName="16,5x24"},
                            new ProductFormatName {CodFormat= "21x28,5", FormatName="21x28,5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "SpiraleMetallica":
                    ret = new ProductFormatName[12] {  
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x21", FormatName="29,7x21"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "21x14,8", FormatName="21x14,8"},
                            new ProductFormatName {CodFormat= "16,5x24", FormatName="16,5x24"},
                            new ProductFormatName {CodFormat= "21x28,5", FormatName="21x28,5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "BrossuraFresata":
                    ret = new ProductFormatName[12] {  
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x21", FormatName="29,7x21"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "21x14,8", FormatName="21x14,8"},
                            new ProductFormatName {CodFormat= "16,5x24", FormatName="16,5x24"},
                            new ProductFormatName {CodFormat= "21x28,5", FormatName="21x28,5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "BrossuraCucitaFilo":
                    ret = new ProductFormatName[12] {  
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x21", FormatName="29,7x21"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "21x14,8", FormatName="21x14,8"},
                            new ProductFormatName {CodFormat= "16,5x24", FormatName="16,5x24"},
                            new ProductFormatName {CodFormat= "21x28,5", FormatName="21x28,5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "RivistePostalizzazione":
                    ret = new ProductFormatName[12] {  
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x21", FormatName="29,7x21"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "21x14,8", FormatName="21x14,8"},
                            new ProductFormatName {CodFormat= "16,5x24", FormatName="16,5x24"},
                            new ProductFormatName {CodFormat= "21x28,5", FormatName="21x28,5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;
                case "SchedeNonRilegate":
                    ret = new ProductFormatName[12] {  
                            new ProductFormatName {CodFormat= "15x21", FormatName="15x21"}, 
                            new ProductFormatName {CodFormat= "21x29,7", FormatName="21x29,7"},
                            new ProductFormatName {CodFormat= "29,7x21", FormatName="29,7x21"},
                            new ProductFormatName {CodFormat= "42x15", FormatName="42x15"},
                            new ProductFormatName {CodFormat= "29,7x42", FormatName="29,7x42"},
                            new ProductFormatName {CodFormat= "14,8x21", FormatName="14,8x21"},
                            new ProductFormatName {CodFormat= "21x14,8", FormatName="21x14,8"},
                            new ProductFormatName {CodFormat= "16,5x24", FormatName="16,5x24"},
                            new ProductFormatName {CodFormat= "21x28,5", FormatName="21x28,5"},
                            new ProductFormatName {CodFormat= "10x15", FormatName="10x15"},
                            new ProductFormatName {CodFormat= "10x21", FormatName="10x21" },
                            new ProductFormatName {CodFormat= "0x0", FormatName="0x0" }};
                    break;

           
                default :
                    ret = new ProductFormatName[0] { };
                    break;
            }

            return ret;
        }
    }
}



 
