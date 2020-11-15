using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowers.Controllers
{
    public class FlowersController : Controller
    {
        // GET: Flowers
        FlowersDBDataContext _context = new FlowersDBDataContext(); 

        public ActionResult Index()
        {
            return View();
        }

        //được gọi ghi thực thi submit form “$("#UploadForm").submit();”   trong Index.cshtml. 
        //Hành động này sẽ làm việc là upload file được gửi cho server vào thư mục Assets\images 
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
                file.SaveAs(destFile);
            }
            return View();
        }

        //hiển thị một danh sách Movies thi ajax yêu cầu
        public ActionResult ListFlowers()
        {
            var flowers = _context.FlowersShops.ToList();
            return Json(flowers, JsonRequestBehavior.AllowGet);
        }

        //khi ajax yêu cầu tạo mới một Flowers
        //vì Id là số tự tăng nên ta có thể bỏ qua nó bằng Bind(Exclude = “Id”).
        //Trong hành động này ta sẽ Insert thêm một Flowers mới vào database. 
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] FlowersShop flower)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(flower.ImagePath);

                flower.ImagePath = "images/" + fileName;
                _context.FlowersShops.InsertOnSubmit(flower);
                _context.SubmitChanges();
            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var flower = _context.FlowersShops.ToList().Find(m => m.Id == id);
            if (flower != null)
            {
                _context.FlowersShops.DeleteOnSubmit(flower);
                _context.SubmitChanges();
            }
            return Json(flower , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int id)
        {
            var flower = _context.FlowersShops.ToList().Find(m => m.Id == id);

            string rootPath = Server.MapPath("~/");
            string fileName = System.IO.Path.GetFileName(flower.ImagePath);
            string destFile = System.IO.Path.Combine(rootPath, "Assets\\images\\" + fileName);
            flower.ImagePath = destFile;

            return Json(flower, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(FlowersShop flower)
        {
            if (ModelState.IsValid)
            {
                string rootPath = Server.MapPath("~/");
                string fileName = System.IO.Path.GetFileName(flower.ImagePath);
                flower.ImagePath = "images/" + fileName;

                FlowersShop fs = (from p in _context.FlowersShops
                                  where p.Id == flower.Id
                                  select p).SingleOrDefault();

                fs.FlowersName = flower.FlowersName;
                fs.ImagePath = flower.ImagePath;
                fs.TagContent = flower.TagContent;
                fs.TypeColumn = flower.TypeColumn;
                _context.SubmitChanges();
            }
            return Json(flower, JsonRequestBehavior.AllowGet);
        }
    }
}