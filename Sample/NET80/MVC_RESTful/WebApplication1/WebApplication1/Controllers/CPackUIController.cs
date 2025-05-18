using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class CPackUIController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly CAppDbContext _context;
        public CPackUIController(CAppDbContext context) => _context = context;

        public IActionResult Index() => View(_context.CPacks.ToList());

        /// <summary>
        /// 顯示新增頁面.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create() => View(); 

        /// <summary>
        /// 新增存檔後, 導向 Index 頁面.
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(CPack pack)
        {
            _context.CPacks.Add(pack);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 檢視編輯頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(long id) => View(_context.CPacks.Find(id));

        /// <summary>
        /// 編輯存檔後, 導向 Index 頁面.
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(CPack pack)
        {
            _context.CPacks.Update(pack);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 顯示刪除頁面.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(long id) => View(_context.CPacks.Find(id));

        /// <summary>
        /// 確認刪除後, 導向 Index 頁面.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm] long id)
        {
            /*
                若 UseInMemoryDatabase, 則要
    @* 確保 name="id"
    <input asp-for="_SeqNo" type="hidden" />
    <input type="hidden" name="id" value="@Model._Key" />
    *@
    <input type="hidden" name="id" value="@Model._SeqNo" />       

            否則無法正確收到 id 參數, 始終為 0.
            */


            var pack = _context.CPacks.Find(id);
            if (pack is not null)
            {
                _context.CPacks.Remove(pack);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
