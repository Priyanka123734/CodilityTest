using CodilityTest.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=697860

namespace CodilityTest.Services
{
    //[Route("api/[controller]")]
    [Route("api/Patient-groups/calculate")]
    [ApiController]
    public class PatientGroups : ControllerBase
    {
        public int Row = 0;
        public int Column = 0;
        public bool[,] patientVisited ;

        [Produces("application/json")]
        [HttpPost]
        public object Post([FromBody] dynamic json)
        {
            //Parse Json value to get metrix
            PatientJsonModel patientJsonModel = JsonConvert.DeserializeObject<PatientJsonModel>(json.ToString());
            if (patientJsonModel.matrix == null || patientJsonModel.matrix.Count()==0)
            {
                return "Matrix is not present";
            }

            IEnumerable<IEnumerable<int>> value = patientJsonModel.matrix;

            int[,] arr = new int[Row, Column];

            //Get 2D Array from Json
            arr = getArrayFromJson(value);
            if (arr != null)
            {
                int groupCount = countPatientGroup(arr);
                return groupCount;
            }

            return "Matrix is not passed or Passed matrix is not appropriate";
        }

        public int[,] getArrayFromJson(IEnumerable<IEnumerable<int>> value)
        {
            Row = value.Count(); Column = value.First().Count();
            int[,] arr = new int[Row, Column];
            if (Row != 0 && Column != 0)
            {
                int i = 0;
                foreach (var item in value)
                {
                    int j = 0;
                    foreach (var it in item)
                    {
                        arr[i, j] = it;
                        j++;
                    }
                    i++;
                }
                return arr;
            }
            return null;
        }

       
        public int countPatientGroup(int[,] M)
        {
            // To handle visited patient from matrix
            patientVisited = new bool[Row, Column];

            int groupCount = 0;
            for (int i = 0; i < Row; ++i)
                for (int j = 0; j < Column; ++j)
                    if (M[i, j] == 1 && !patientVisited[i, j])
                    {
                        //If patient is 1 and not visited then start find group of patient
                        findPatientsGroup(M, i, j);
                        ++groupCount;
                    }

            return groupCount;
        }

        private void findPatientsGroup(int[,] M, int row,
                    int col)
        {
            //find same group patients in adjacent 8 cells
            int[] adjacentRow = new int[] { -1, -1, -1, 0,
                                   0, 1, 1, 1 };
            int[] adjacentCol = new int[] { -1, 0, 1, -1,
                                   1, -1, 0, 1 };

            //mark current patient visited
            patientVisited[row, col] = true;

            for (int m = 0; m < 8; ++m)
            {
                if (visitIfPatient(M, row + adjacentRow[m], col + adjacentCol[m]))
                {
                    findPatientsGroup(M, row + adjacentRow[m],
                        col + adjacentCol[m]);
                }
            }
        }

        private bool visitIfPatient(int[,] Matrix, int row,
                       int col)
        {
            //Check, Cell to be visit is valid or not and It contains patient and patient is not visited
            bool validCell= (row >= 0) && (row < Row) && (col >= 0) && (col < Column) && (Matrix[row, col] == 1 && !patientVisited[row, col]);
            return validCell;
        }

    }
    }
