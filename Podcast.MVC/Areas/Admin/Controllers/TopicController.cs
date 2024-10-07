using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.ViewModels.TopicViewModels;
using Podcast.DAL.DataContext;

namespace Podcast.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
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
        public async Task<IActionResult> Create(TopicCreateViewModel vm)
        {

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

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _topicService.RemoveAsync(id);
           return RedirectToAction(nameof(Index));
        }
    }
}
