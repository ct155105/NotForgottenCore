using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Table : BaseModel<int>
    {
        //Calculated via Database Trigger
        public int OpenSeats { get; set; } = 10;
        /*
            CREATE TRIGGER updateOpenSeats  
            ON Groups  
            AFTER INSERT, UPDATE, DELETE   
            AS  
              Update dbo.Tables Set OpenSeats = Case When NbrOpenSeats IS NULL THEN TotalSeats
													Else TotalSeats - NbrOpenSeats
													END
              FROM dbo.Tables 
              Left Join
                 (SELECT TableId, Sum(NumberSeats) AS NbrOpenSeats
                  FROM Groups 
                  GROUP BY TableId) B
              on Tables.Id = B.TableId
              GO
        */
        public int TotalSeats { get; set; }
        public string TableType { get; set; }
        public List<Group> Groups { get; set; }

        public List<SelectListItem> GetSelectLists()
        {
            List<SelectListItem> seatList = new List<SelectListItem> { };
            seatList.Add(new SelectListItem() { Text = "0", Value = "0" });

            if (OpenSeats >= 8)
            {
                seatList.Add(new SelectListItem() { Text = "8 Seats - $240", Value = "8" });
            }
            if (OpenSeats >= 10)
            {
                seatList.Add(new SelectListItem() { Text = "10 Seats - $280", Value = "10" });
            }
            if (OpenSeats >= 15)
            {
                seatList.Add(new SelectListItem() { Text = "15 Seats - $400", Value = "15" });
            }
            if (OpenSeats >= 20)
            {
                seatList.Add(new SelectListItem() { Text = "20 Seats - $500", Value = "20" });
            }
            return (seatList);
        }
    }
}

