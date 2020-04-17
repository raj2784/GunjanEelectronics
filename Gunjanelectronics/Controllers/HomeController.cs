using Gunjanelectronics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gunjanelectronics.Controllers
{
	public class HomeController : Controller
	{
		GunjanelectronicsEntities db = new GunjanelectronicsEntities();

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		public List<country> GetCountriesList()
		{

			List<country> countries = db.countries.ToList();

			ViewBag.CountryList = new SelectList(countries, "countryid", "countryname");

			return countries;

		}
		public ActionResult GetStateList(int countryid)
		{
			List<state> states = db.states.Where(i => i.countryid == countryid).ToList();
			ViewBag.Slist = new SelectList(states, "stateid", "statename");
			return PartialView("/Views/Shared/_DisplayStates.cshtml");
		}
		public ActionResult GetCitiList(int stateid)
		{
			List<city> cities = db.cities.Where(i => i.stateid == stateid).ToList();
			ViewBag.Clist = new SelectList(cities, "cityid", "cityname");
			return PartialView("/Views/Shared/_DisplayCities.cshtml");
		}
		[HttpGet]
		public ActionResult Addcustomer()
		{
			ViewBag.Success = TempData["Success"];

			AddcustomerModel acm = new AddcustomerModel();

			List<country> countries = db.countries.ToList();
			ViewBag.CountryList = new SelectList(countries, "countryid", "countryname");

			return View(acm);
		}
		[HttpPost]
		public ActionResult Addcustomer(AddcustomerModel model)
		{
			Addcustomer ac = new Addcustomer();
			ac.Id = model.id;
			ac.Customerid = model.cityid;
			ac.Name = model.Name;
			ac.Dob = model.Dob;
			ac.countryid = model.countryid;
			ac.stateid = model.stateid;
			ac.cityid = model.cityid;
			ac.Address = model.Address;
			ac.Email = model.Email;
			ac.Mobile = model.Mobile;
			ac.Survey = model.wouldyouliketoparticipateinSurvey;
			db.Addcustomers.Add(ac);
			db.SaveChanges();

			TempData["Success"] = "Customer data added successfully.";
			return RedirectToAction("Addcustomer", "Home");
		}
		public ActionResult Customerlist()
		{
			//linq query start here

		        var customerlist = db.Addcustomers.Join(db.countries, i => i.countryid, j => j.countryid, (i, j) => new { add_cust = i, cust_ctr = j })
				.Join(db.states, i => i.add_cust.stateid, j => j.stateid, (i, j) => new {cust_state = i, tbl_state = j })
				.Join(db.cities, i => i.cust_state.add_cust.cityid, j => j.cityid, (i, j) => new { cust_city = i, tbl_city = j })
				.Select(i => new AddcustomerModel
				{
					id=i.cust_city.cust_state.add_cust.Id,
					Customerid = i.cust_city.cust_state.add_cust.Customerid,
					Name=i.cust_city.cust_state.add_cust.Name,
					Dob=i.cust_city.cust_state.add_cust.Dob,
					countryid=i.cust_city.cust_state.add_cust.countryid,
					countryname=i.cust_city.cust_state.cust_ctr.countryname,
					stateid=i.cust_city.cust_state.add_cust.stateid,
					statename=i.cust_city.tbl_state.statename,	
					cityid=i.cust_city.cust_state.add_cust.cityid,
					cityname=i.tbl_city.cityname,
					Address=i.cust_city.cust_state.add_cust.Address,
					Email=i.cust_city.cust_state.add_cust.Email,
					Mobile=i.cust_city.cust_state.add_cust.Mobile,
					wouldyouliketoparticipateinSurvey=i.cust_city.cust_state.add_cust.Survey,
									   		

				}).ToList();

			return View(customerlist);

		}
		[HttpGet]
		public ActionResult UpdateCustomer(int id)
		{
			List<country> countries = db.countries.ToList();
			ViewBag.CountryList = new SelectList(countries, "countryid", "countryname");

			ViewBag.Error = TempData["Error"];
			var ac = db.Addcustomers.Where(i => i.Id == id).FirstOrDefault();
			AddcustomerModel model = new AddcustomerModel();
			if (ac != null)
			{
				model.id = ac.Id;
				model.Customerid = ac.Customerid;
				model.Name = ac.Name;
				model.Dob = ac.Dob;
				model.countryid = ac.countryid;
				model.stateid = ac.stateid;
				model.cityid = ac.cityid;
				model.Address = ac.Address;
				model.Email = ac.Email;
				model.Mobile = ac.Mobile;
				model.wouldyouliketoparticipateinSurvey = ac.Survey;

			}
			else
			{
				ViewBag.Error = "No Record found!!";
			}
			return View(model);
		}
		[HttpPost]
		public ActionResult UpdateCustomer(AddcustomerModel model)
		{
			Addcustomer ac = db.Addcustomers.Where(i => i.Id == model.id).FirstOrDefault();
			ac.Id = model.id;
			ac.Customerid = model.cityid;
			ac.Name = model.Name;
			ac.Dob = model.Dob;
			ac.countryid = model.countryid;
			ac.stateid = model.stateid;
			ac.cityid = model.cityid;
			ac.Address = model.Address;
			ac.Email = model.Email;
			ac.Mobile = model.Mobile;
			ac.Survey = model.wouldyouliketoparticipateinSurvey;
			db.SaveChanges();

			return RedirectToAction("Customerlist", "Home");
		}
		[HttpGet]
		public ActionResult RemoveCustomer(int id)
		{
			Addcustomer ac = db.Addcustomers.Where(i => i.Id == id).FirstOrDefault();
			db.Addcustomers.Remove(ac);
			db.SaveChanges();
			return RedirectToAction("Customerlist", "Home");
		}

	}
}