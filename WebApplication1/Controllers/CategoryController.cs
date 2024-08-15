using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
        // Обязательно в названии класса должно быть слово Controller, чтобы компилятор узнал его как контроллер
    public class CategoryController : Controller
    {

        //Обращается к классу и присвает экземпляру название

        // Конструктор, чтобы нельзя было изменить переменную извне 
        private readonly ApplicationDbContext _db; 
        public CategoryController(ApplicationDbContext db)
        {
                _db = db;
        }
        public IActionResult Index()
        {
            //Создает список на основе взятого класса (обращается к классу _db.Categories. и переводит в список)
            //Нужно передать список в html код, чтобы он считал код и принял модель Category
            // @model List<Category>
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {

            return View(); 

        }

        //Этот перегруженный метод нужен для того, чтобы обрабатывалось нажатие кнопки create category 
        //и возвращался новый объект в список category
        //Такой же post есть и в Category/Create

        //Вместо возврата представления, он возвращает перенаправление на действие. Можно указать имя файла и название папки.

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Здесь можно самому выставлять проверки
            //Или же сделать их в html
            //<div asp-validation-summary="All"></div>
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Custom error");
            }

            //Проверяет, заполнены ли все поля и все ли атрибуты соблюдены
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                //Использовать partial views для всех многоразовых компонентов, например, уведомлений 
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();



            return View();
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj); 
                _db.SaveChanges();
                TempData["success"] = "Category edit successfully";
                return RedirectToAction("Index");
            }
            
            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            //Category categoryFromDb = _db.Categories.Find(id);
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();
            


            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();



            
        }

        [HttpPost, ActionName ("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            TempData["success"] = "Category deleted successfully";
            _db.SaveChanges();
            
             return RedirectToAction("Index");

           
            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            //Category categoryFromDb = _db.Categories.Find(id);
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();




        }
    }
}
