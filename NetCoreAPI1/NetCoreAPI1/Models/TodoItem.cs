using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI1.Models
{
   /// <summary>
   /// model that defines a todo item
   /// </summary>
   public class TodoItem
   {
      /// <summary>
      /// id todo item
      /// </summary>
      public long Id { get; set; }
      /// <summary>
      /// name todo item
      /// </summary>
      [Required]
      public string name { get; set; }
      /// <summary>
      /// status todo item
      /// </summary>
      [DefaultValue(false)]
      public bool isComplete { get; set; }
   }
}
