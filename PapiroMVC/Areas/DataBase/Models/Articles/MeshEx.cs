using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Mesh_MetaData))]
    public partial class Mesh : NoPrintable
    {
        public Mesh()
        {
            TypeOfArticle = ArticleType.Mesh;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return this.ArticleName;
        }

        public override string GetEditMethod()
        {
            return "EditMesh";
        }
    }
}

