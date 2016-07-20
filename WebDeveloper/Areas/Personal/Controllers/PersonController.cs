using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebDeveloper.DataAccess;
using WebDeveloper.Model;


namespace WebDeveloper.Areas.Personal.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly PersonRepository _personRepository;
        public PersonController(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        [OutputCache(Duration = 0)]
        public ActionResult Index()
        {
            return View(_personRepository.GetListDto());
        }

        public PartialViewResult EmailList(int? id)
        {
            if (!id.HasValue) return null;
            return PartialView("_EmailList", _personRepository.EmailList(id.Value));
        }

        public PartialViewResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            if (!ModelState.IsValid) return PartialView("_Create", person);
            person.rowguid = Guid.NewGuid();
            person.BusinessEntity = new BusinessEntity
            {
                rowguid = person.rowguid,
                ModifiedDate = person.ModifiedDate
            };
            _personRepository.Add(person);
            return RedirectToAction("Index");
        }

    }
}