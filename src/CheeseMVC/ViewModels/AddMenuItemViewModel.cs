using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public int MenuID { get; set; }
        public Menu Menu { get; set; }

        public int CheeseID { get; set; }
        public List<SelectListItem> Cheeses { get; set; } = new List<SelectListItem>();

        public AddMenuItemViewModel()
        { }

        public AddMenuItemViewModel(IEnumerable<Cheese> cheeses, Menu menu)
        {
            this.Menu = menu;

            foreach(Cheese cheese in cheeses)
            {
                this.Cheeses.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name
                });
            }
            
        }
    }
}
