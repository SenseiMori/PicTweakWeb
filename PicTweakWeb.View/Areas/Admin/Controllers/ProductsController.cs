using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using DataAccess.Data;
using Models.ViewModels;


namespace PicTweakWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            //Создает список на основе взятого класса (обращается к классу _db.Categories. и переводит в список)
            //Нужно передать список в html код, чтобы он считал код и принял модель Category
            // @model List<Category>

            //View-bag - подходит для передачи данных из контроллера в ситуациях, когда данные временные и их нет в модели
            //время жизни равно запросу

            //View-data когда временных данных нет в модели
            //ViewDataDictionary имеет тип словаря
            //данные должны быть явно приведены до использования
            //живут до текущего http запроса
            //ключ view и свойство viewBag не должны совпадать

            //TempData используется для хранения данных между двумя последовательными запросами
            //ВНутренне спользует хранение данных и имеет короткий lifetime
            //Должен быть приведен до использования и не может содержать null
            //Используется для однократного сообщения об ошибке или проверке или чего-нибудь другого быстрого. После обновления страницы исчезнет
            //используется при редиректах

            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            //Возможно стоит улучшить 
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.
            //    GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString(),
            //    });
            ////ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {

                Product = new Product()
            };
            if(id == null || id ==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file )
        {
            //Здесь можно самому выставлять проверки
            //Или же сделать их в html
            //<div asp-validation-summary="All"></div>

            //dependence injection. Берем картинки 
            //Этот код создает уникальный идентификатор (GUID) и преобразует его в строку.
            //GUID — это 128-битный уникальный идентификатор, который можно использовать для генерации уникальных имен файлов.

            string wwwRootPatch = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPatch, @"images\product");

                if (!string.IsNullOrEmpty(obj.Product.ImageURL))
                {
                    //delete image
                    var oldImagePath = Path.Combine(wwwRootPatch, obj.Product.ImageURL.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageURL = @"\images\product" + fileName;
                }

                if (obj.Product.ImageURL == null)
                {
                    _unitOfWork.Product.Add(obj.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
            }
            //Проверяет, заполнены ли все поля и все ли атрибуты соблюдены
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                //Использовать partial views для всех многоразовых компонентов, например, уведомлений 
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
               
                  return View(obj);

            }
        }
        /*public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
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
        public IActionResult Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product edit successfully";
                return RedirectToAction("Index");
            }

            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            //Category categoryFromDb = _db.Categories.Find(id);
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();



            return View();
        }
        */
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? cproductFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (cproductFromDb == null)
            {
                return NotFound();
            }
            return View(cproductFromDb);
            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();




        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            TempData["success"] = "Product deleted successfully";
            _unitOfWork.Save();

            return RedirectToAction("Index");


            //Для того, чтобы передать этот аргумент в html нужно в Edit добавить функцию
            // asp-route-id="@obj.Id"
            //Category categoryFromDb = _db.Categories.Find(id);
            // Category categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category category = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();




        }

    }
}
