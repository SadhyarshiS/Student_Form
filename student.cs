using System;
using System.ComponentModel.DataAnnotations;

public class Student
{
    public int sid { get; set; }

    [Required]
    public string fname { get; set; }

    public string mname { get; set; }

    [Required]
    public string lname { get; set; }

    [Required]
    public DateTime dob { get; set; }

    [Required]
    public string pob { get; set; }

    [Required]
    public int cntct { get; set; }

    [Required]
    public string ntlnty { get; set; }

    [Required]
    public string religion { get; set; }

    [Required]
    public string gender { get; set; }

    public string q1 { get; set; }
    public string noi1 { get; set; }
    public string nobu1 { get; set; }
    public int yoc1 { get; set; }
    public float mob1 { get; set; }
    public string ds1 { get; set; }

    public string q2 { get; set; }
    public string noi2 { get; set; }
    public string nobu2 { get; set; }
    public int yoc2 { get; set; }
    public float mob2 { get; set; }
    public string ds2 { get; set; }

    public string q3 { get; set; }
    public string noi3 { get; set; }
    public string nobu3 { get; set; }
    public int yoc3 { get; set; }
    public float mob3 { get; set; }
    public string ds3 { get; set; }

    public string q4 { get; set; }
    public string noi4 { get; set; }
    public string nobu4 { get; set; }
    public int? yoc4 { get; set; }
    public float? mob4 { get; set; }
    public string ds4 { get; set; }

    public string q5 { get; set; }
    public string noi5 { get; set; }
    public string nobu5 { get; set; }
    public int? yoc5 { get; set; }
    public float? mob5 { get; set; }
    public string ds5 { get; set; }

    [Required]
    public string acc { get; set; }

    [Required]
    public string sp { get; set; }

    public string l1 { get; set; }
    public string l2 { get; set; }
    public string l3 { get; set; }
    public string l4 { get; set; }
    public string l5 { get; set; }
    public string l6 { get; set; }

    [Required]
    public string image { get; set; }
}