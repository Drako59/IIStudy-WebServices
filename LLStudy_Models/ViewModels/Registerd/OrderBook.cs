using LLStudy_Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLStudy_Models.ViewModels;
using LLStudy_Models.Models;

public class OrderBook: Book
{
   

    public int Amount { get; set; }
    public string Subject_name { get; set; }
}
