using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.ViewModels.TopicViewModels;
using Podcast.DAL.DataContext;
using Podcast.MVC.Helpers.Extensions;

namespace Podcast.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TopicController(ITopicService topicService, IWebHostEnvironment webHostEnvironment)
        {
            _topicService = topicService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var topics = await _topicService.GetListAsync();
            return View(topics);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var topics = await _topicService.GetAsync(x => x.Id == id);

            if (topics == null) return NotFound();

            return View(topics);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TopicCreateViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(!vm.CoverFile.IsImage())
            {
                ModelState.AddModelError("CoverFile", "You should choose image");

                return View();
            }

            if(!vm.CoverFile.CheckSize(2))
            {
                ModelState.AddModelError("CoverFile", "You should choose true image size");

                return View();
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "topics");
            var imageName = await vm.CoverFile.GenerateFileAsync(path);

            vm.CoverUrl = imageName;

            await _topicService.CreateAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var topic = await _topicService.GetAsync(id);
            if (topic == null) return NotFound();

            var vm = new TopicUpdateViewModel
            {
                Id = topic.Id,
                Name = topic.Name,
                 CoverUrl = topic.CoverUrl,
                  
                
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TopicUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm); 
            }

            await _topicService.UpdateAsync(vm);
            return RedirectToAction(nameof(Index)); 
        }

        

       public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();

            await _topicService.RemoveAsync(id);

            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            if(id==null) return NotFound();

            await _topicService.RemoveAsync(id);
            
           return RedirectToAction(nameof(Index));
        }
    }
}
