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
              Update dbo.Tables Set OpenSeats = IsNull(NoOpenSeats,10)
              FROM dbo.Tables 
              Left Join
                 (SELECT TableId, 10 - Sum(NumberSeats) AS NoOpenSeats
                  FROM Groups 
                  GROUP BY TableId) B
              on Tables.Id = B.TableId
              GO
        */
        public List<Group> Groups { get; set; }
    }
}
