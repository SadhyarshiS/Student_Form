using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics; 

public class StudentController : Controller
{
    private DatabaseHelper db = new DatabaseHelper();

    
    public ActionResult Index()
    {
        var model = new Student
        {
            q1 = "Secondary",
            q2 = "Sr. Secondary",
            q3 = "Graduation",
            q4 = "Post-graduation",
            q5 = "Professional Degree"
        };
        ViewBag.Nationalities = new SelectList(GetNationalities(), "Value", "Text");
        ViewBag.Religions = new SelectList(new[] { "Hindu", "Muslim", "Christian", "Other" });
        return View(model);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateStudent(Student m, HttpPostedFileBase file)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    m.image = "/Images/" + fileName;
                }
                else
                {
                    ModelState.AddModelError("", "Please upload an image.");
                    ViewBag.Nationalities = new SelectList(GetNationalities(), "Value", "Text");
                    ViewBag.Religions = new SelectList(new[] { "Hindu", "Muslim", "Christian", "Other" });
                    return View("Index", m);
                }

                var selectedLanguages = Request.Form["Languages"]?.Split(',');
                if (selectedLanguages != null)
                {
                    for (int i = 0; i < selectedLanguages.Length && i < 6; i++)
                    {
                        switch (i)
                        {
                            case 0: m.l1 = selectedLanguages[i]; break;
                            case 1: m.l2 = selectedLanguages[i]; break;
                            case 2: m.l3 = selectedLanguages[i]; break;
                            case 3: m.l4 = selectedLanguages[i]; break;
                            case 4: m.l5 = selectedLanguages[i]; break;
                            case 5: m.l6 = selectedLanguages[i]; break;
                        }
                    }
                }

                db.InsertStudent(
                    m.fname, m.mname, m.lname, m.dob, m.pob, m.cntct, m.ntlnty, m.religion, m.gender,
                    m.q1, m.noi1, m.nobu1, m.yoc1, m.mob1, m.ds1,
                    m.q2, m.noi2, m.nobu2, m.yoc2, m.mob2, m.ds2,
                    m.q3, m.noi3, m.nobu3, m.yoc3, m.mob3, m.ds3,
                    m.q4, m.noi4, m.nobu4, m.yoc4, m.mob4, m.ds4,
                    m.q5, m.noi5, m.nobu5, m.yoc5, m.mob5, m.ds5,
                    m.acc, m.sp,
                    m.l1, m.l2, m.l3, m.l4, m.l5, m.l6,
                    m.image
                );

                TempData["Message"] = "Student added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
        }

        ViewBag.Nationalities = new SelectList(GetNationalities(), "Value", "Text");
        ViewBag.Religions = new SelectList(new[] { "Hindu", "Muslim", "Christian", "Other" });
        return View("Index", m);
    }

    // GET: View all students
    public ActionResult ViewStudents()
    {
        var students = db.GetStudents();
        return View(students);
    }

    // GET: Display edit form for a specific student
    public ActionResult EditStudent(int id)
    {
        Debug.WriteLine($"EditStudent called with id: {id}");

        var studentData = db.GetStudentById(id);
        Debug.WriteLine($"GetStudentById returned: {(studentData != null ? "data" : "null")} with count: {studentData?.Count ?? 0}");

        if (studentData == null)
        {
            TempData["Message"] = $"Student not found for id: {id}";
            return RedirectToAction("ViewStudents");
        }

        var student = new Student
        {
            sid = id,
            fname = studentData.ContainsKey("fname") && studentData["fname"] != null ? studentData["fname"].ToString() : "",
            mname = studentData.ContainsKey("mname") && studentData["mname"] != null ? studentData["mname"].ToString() : "",
            lname = studentData.ContainsKey("lname") && studentData["lname"] != null ? studentData["lname"].ToString() : "",
            dob = studentData.ContainsKey("dob") && studentData["dob"] != null ? DateTime.Parse(studentData["dob"].ToString()) : DateTime.MinValue,
            pob = studentData.ContainsKey("pob") && studentData["pob"] != null ? studentData["pob"].ToString() : "",
            cntct = studentData.ContainsKey("cntct") && studentData["cntct"] != null ? Convert.ToInt32(studentData["cntct"]) : 0,
            ntlnty = studentData.ContainsKey("ntlnty") && studentData["ntlnty"] != null ? studentData["ntlnty"].ToString() : "",
            religion = studentData.ContainsKey("religion") && studentData["religion"] != null ? studentData["religion"].ToString() : "",
            gender = studentData.ContainsKey("gender") && studentData["gender"] != null ? studentData["gender"].ToString() : "",
            q1 = studentData.ContainsKey("q1") && studentData["q1"] != null ? studentData["q1"].ToString() : "",
            noi1 = studentData.ContainsKey("noi1") && studentData["noi1"] != null ? studentData["noi1"].ToString() : "",
            nobu1 = studentData.ContainsKey("nobu1") && studentData["nobu1"] != null ? studentData["nobu1"].ToString() : "",
            yoc1 = studentData.ContainsKey("yoc1") && studentData["yoc1"] != null ? Convert.ToInt32(studentData["yoc1"]) : 0,
            mob1 = studentData.ContainsKey("mob1") && studentData["mob1"] != null ? Convert.ToSingle(studentData["mob1"]) : 0,
            ds1 = studentData.ContainsKey("ds1") && studentData["ds1"] != null ? studentData["ds1"].ToString() : "",
            q2 = studentData.ContainsKey("q2") && studentData["q2"] != null ? studentData["q2"].ToString() : "",
            noi2 = studentData.ContainsKey("noi2") && studentData["noi2"] != null ? studentData["noi2"].ToString() : "",
            nobu2 = studentData.ContainsKey("nobu2") && studentData["nobu2"] != null ? studentData["nobu2"].ToString() : "",
            yoc2 = studentData.ContainsKey("yoc2") && studentData["yoc2"] != null ? Convert.ToInt32(studentData["yoc2"]) : 0,
            mob2 = studentData.ContainsKey("mob2") && studentData["mob2"] != null ? Convert.ToSingle(studentData["mob2"]) : 0,
            ds2 = studentData.ContainsKey("ds2") && studentData["ds2"] != null ? studentData["ds2"].ToString() : "",
            q3 = studentData.ContainsKey("q3") && studentData["q3"] != null ? studentData["q3"].ToString() : "",
            noi3 = studentData.ContainsKey("noi3") && studentData["noi3"] != null ? studentData["noi3"].ToString() : "",
            nobu3 = studentData.ContainsKey("nobu3") && studentData["nobu3"] != null ? studentData["nobu3"].ToString() : "",
            yoc3 = studentData.ContainsKey("yoc3") && studentData["yoc3"] != null ? Convert.ToInt32(studentData["yoc3"]) : 0,
            mob3 = studentData.ContainsKey("mob3") && studentData["mob3"] != null ? Convert.ToSingle(studentData["mob3"]) : 0,
            ds3 = studentData.ContainsKey("ds3") && studentData["ds3"] != null ? studentData["ds3"].ToString() : "",
            q4 = studentData.ContainsKey("q4") && studentData["q4"] != null ? studentData["q4"].ToString() : "",
            noi4 = studentData.ContainsKey("noi4") && studentData["noi4"] != null ? studentData["noi4"].ToString() : "",
            nobu4 = studentData.ContainsKey("nobu4") && studentData["nobu4"] != null ? studentData["nobu4"].ToString() : "",
            yoc4 = studentData.ContainsKey("yoc4") && studentData["yoc4"] != null ? (int?)Convert.ToInt32(studentData["yoc4"]) : null,
            mob4 = studentData.ContainsKey("mob4") && studentData["mob4"] != null ? (float?)Convert.ToSingle(studentData["mob4"]) : null,
            ds4 = studentData.ContainsKey("ds4") && studentData["ds4"] != null ? studentData["ds4"].ToString() : "",
            q5 = studentData.ContainsKey("q5") && studentData["q5"] != null ? studentData["q5"].ToString() : "",
            noi5 = studentData.ContainsKey("noi5") && studentData["noi5"] != null ? studentData["noi5"].ToString() : "",
            nobu5 = studentData.ContainsKey("nobu5") && studentData["nobu5"] != null ? studentData["nobu5"].ToString() : "",
            yoc5 = studentData.ContainsKey("yoc5") && studentData["yoc5"] != null ? (int?)Convert.ToInt32(studentData["yoc5"]) : null,
            mob5 = studentData.ContainsKey("mob5") && studentData["mob5"] != null ? (float?)Convert.ToSingle(studentData["mob5"]) : null,
            ds5 = studentData.ContainsKey("ds5") && studentData["ds5"] != null ? studentData["ds5"].ToString() : "",
            acc = studentData.ContainsKey("acc") && studentData["acc"] != null ? studentData["acc"].ToString() : "",
            sp = studentData.ContainsKey("sp") && studentData["sp"] != null ? studentData["sp"].ToString() : "",
            l1 = studentData.ContainsKey("l1") && studentData["l1"] != null ? studentData["l1"].ToString() : "",
            l2 = studentData.ContainsKey("l2") && studentData["l2"] != null ? studentData["l2"].ToString() : "",
            l3 = studentData.ContainsKey("l3") && studentData["l3"] != null ? studentData["l3"].ToString() : "",
            l4 = studentData.ContainsKey("l4") && studentData["l4"] != null ? studentData["l4"].ToString() : "",
            l5 = studentData.ContainsKey("l5") && studentData["l5"] != null ? studentData["l5"].ToString() : "",
            l6 = studentData.ContainsKey("l6") && studentData["l6"] != null ? studentData["l6"].ToString() : "",
            image = studentData.ContainsKey("image") && studentData["image"] != null ? studentData["image"].ToString() : ""
        };

        ViewBag.Nationalities = new SelectList(GetNationalities(), "Value", "Text", student.ntlnty);
        ViewBag.Religions = new SelectList(new[] { "Hindu", "Muslim", "Christian", "Other" }, student.religion);
        return View(student);
    }

    // POST: Save edited student
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditStudent(Student m, HttpPostedFileBase file)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                    m.image = "/Images/" + fileName;
                }

                var selectedLanguages = Request.Form["Languages"]?.Split(',');
                if (selectedLanguages != null)
                {
                    for (int i = 0; i < selectedLanguages.Length && i < 6; i++)
                    {
                        switch (i)
                        {
                            case 0: m.l1 = selectedLanguages[i]; break;
                            case 1: m.l2 = selectedLanguages[i]; break;
                            case 2: m.l3 = selectedLanguages[i]; break;
                            case 3: m.l4 = selectedLanguages[i]; break;
                            case 4: m.l5 = selectedLanguages[i]; break;
                            case 5: m.l6 = selectedLanguages[i]; break;
                        }
                    }
                }

                db.UpdateStudent(
                    m.sid, m.fname, m.mname, m.lname, m.dob, m.pob, m.cntct, m.ntlnty, m.religion, m.gender,
                    m.q1, m.noi1, m.nobu1, m.yoc1, m.mob1, m.ds1,
                    m.q2, m.noi2, m.nobu2, m.yoc2, m.mob2, m.ds2,
                    m.q3, m.noi3, m.nobu3, m.yoc3, m.mob3, m.ds3,
                    m.q4, m.noi4, m.nobu4, m.yoc4, m.mob4, m.ds4,
                    m.q5, m.noi5, m.nobu5, m.yoc5, m.mob5, m.ds5,
                    m.acc, m.sp,
                    m.l1, m.l2, m.l3, m.l4, m.l5, m.l6,
                    m.image
                );

                TempData["Message"] = "Student updated successfully!";
                return RedirectToAction("ViewStudents");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
        }

        ViewBag.Nationalities = new SelectList(GetNationalities(), "Value", "Text", m.ntlnty);
        ViewBag.Religions = new SelectList(new[] { "Hindu", "Muslim", "Christian", "Other" }, m.religion);
        return View(m);
    }

    // POST: Delete a student
    [HttpPost]
    public ActionResult DeleteStudent(int id)
    {
        try
        {
            db.DeleteStudent(id);
            TempData["Message"] = "Student deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"An error occurred: {ex.Message}";
        }
        return RedirectToAction("ViewStudents");
    }

    private List<SelectListItem> GetNationalities()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        var nationalities = cultures
            .Select(c => new { c.Name, Region = new RegionInfo(c.LCID) })
            .GroupBy(r => r.Region.EnglishName)
            .Select(g => g.First())
            .OrderBy(r => r.Region.EnglishName)
            .Select(r => new SelectListItem { Text = r.Region.EnglishName, Value = r.Region.EnglishName })
            .ToList();
        return nationalities;
    }
}
