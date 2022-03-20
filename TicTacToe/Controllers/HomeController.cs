using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicTacToe
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        readonly string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult InnerLogic(string allData, Int64 myid)
        {
            //int myid = allData[0] - '0'; //char to int
            int[] box = new int[9];
            for (int i = 0; i < 9; ++i) {
                switch (allData[i + 1])
                {
                    case 'X':
                        box[i] = 1;
                        break;
                    case '0':
                        box[i] = 2;
                        break;
                    case 'n':
                        box[i] = 0;
                        break;
                    default:
                        box[i] = -3;
                        break;
                }
            }
            int myflag = allData[10] - '0';
            int win = -1;
            string result_str = "";

            using (DbConnection db = new SqlConnection(connectionString))
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (box[i] * box[i + 3] * box[i + 6] == 1)
                    {
                        win = 1;
                        result_str = "X won";
                    }
                    if (box[i] * box[i + 3] * box[i + 6] == 8)
                    {
                        win = 2;
                        result_str = "0 won";
                    }
                }
                for (int j = 0; j < 3; ++j)
                {
                    if (box[3 * j] * box[3 * j + 1] * box[3 * j + 2] == 1)
                    {
                        win = 1;
                        result_str = "X won";
                    }
                    if (box[3 * j] * box[3 * j + 1] * box[3 * j + 2] == 8)
                    {
                        win = 2;
                        result_str = "0 won";
                    }
                }
                if (box[0] * box[4] * box[8] == 1 || box[2] * box[4] * box[6] == 1)
                {
                    win = 1;
                    result_str = "X won";
                }
                if (box[0] * box[4] * box[8] == 8 || box[2] * box[4] * box[6] == 8)
                {
                    win = 2;
                    result_str = "0 won";
                }
                //Match Tie?
                int temp_result = 1;
                for (int i = 0; i < 9; ++i) {
                    temp_result *= box[i];
                }
                if (temp_result != 0 && result_str == "") {
                    win = 0;
                    result_str = "Match tie";
                }

                //Match not finished.
                if (win >= 0)
                {
                    Console.WriteLine(myid);
                    //Console.Read();
                    dynamic cmdQuery = db.Query<dynamic>(string.Format("INSERT INTO master.[dbo].[MovesTicTacToe] values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11});",
                        myid, box[0], box[1], box[2], box[3], box[4], box[5], box[6], box[7], box[8], myflag, win));
                }
                else
                {
                    dynamic cmdQuery = db.Query<dynamic>(string.Format("INSERT INTO master.[dbo].[MovesTicTacToe] values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, NULL);",
                        myid, box[0], box[1], box[2], box[3], box[4], box[5], box[6], box[7], box[8], myflag));
                }

                if (result_str == "")
                {
                    result_str = "Continue";
                }
                else
                {
                    globalId += 1;
                }

                return Ok(result_str);
            }
        }

        public IActionResult OuterLogic([FromBody] string str)
        {
            using (DbConnection db = new SqlConnection(connectionString))
            {
                //TODO написать процедуру проверки на sql и вставить сюда.
            }
                return Ok();
        }

    }
}
