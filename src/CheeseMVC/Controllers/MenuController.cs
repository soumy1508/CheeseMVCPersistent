using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {

        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {

            AddMenuViewModel addmenuviewmodel = new AddMenuViewModel();
            return View(addmenuviewmodel);
        }


        [HttpPost]
        public IActionResult Add(AddMenuViewModel addmenuviewmodel)
        {
            if (ModelState.IsValid)
            {
                // Add the new menu to my existing menus
                Menu newMenu = new Menu
                {
                    Name = addmenuviewmodel.Name,
                    
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                //to do 
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addmenuviewmodel);
        }

        [Route("/Menu/ViewMenu/{id}")]
        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Where(m => m.ID == id).FirstOrDefault();

            List<CheeseMenu> items = context
                                    .CheeseMenus
                                    .Include(item => item.Cheese)
                                    .Where(cm => cm.MenuID == id)
                                    .ToList();

            ViewMenuViewModel viewMenu = new ViewMenuViewModel();
            viewMenu.Menu = menu;
            viewMenu.Items = items;

            return View(viewMenu);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Where(m => m.ID == id).FirstOrDefault();

            List<Cheese> cheeses = context.Cheeses.ToList();

            AddMenuItemViewModel addmenu = new AddMenuItemViewModel(cheeses, menu);

            return View(addmenu);

        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenu)
        {
            IList<CheeseMenu> existingItems = context.CheeseMenus
            .Where(cm => cm.CheeseID == addMenu.CheeseID)
            .Where(cm => cm.MenuID == addMenu.MenuID).ToList();

            if(existingItems.Count == 0)
            {
                CheeseMenu cheeseMenu = new CheeseMenu();
                cheeseMenu.MenuID = addMenu.MenuID;
                cheeseMenu.CheeseID = addMenu.CheeseID;
                cheeseMenu.MenuID = addMenu.MenuID;
                cheeseMenu.CheeseID = addMenu.CheeseID;

                context.CheeseMenus.Add(cheeseMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + addMenu.MenuID);
            }
            else
            {
                return Redirect("/Menu/AddItem/" + addMenu.MenuID);
            }
            //return View(addMenu);
        }
    }
}