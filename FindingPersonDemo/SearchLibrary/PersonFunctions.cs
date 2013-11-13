/* 
 * Finding a Person demonstration project
 * 
 * Released under the MIT license
 * Copyright (c) 2013 Rahn Lieberman, http://RahnsWorld.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;

namespace SearchLibrary
{
    public class PersonFunctions
    {
        public List<int> Results = new List<int>();

        public static StringBuilder SearchResultDetails = new StringBuilder();   //Store details of search, if it's desired for other programs.

        public static int FullSearch(int PersonId, string LastName, string FirstName, DateTime Birthdate, string SocialSecurity, int CustomerID, string AccountNumber, string MedicalRecordNumber, string StreetAddress, string City, string State, string ZipCode)
        {
            PersonWeight w = new PersonWeight();
            SearchResultDetails.Clear();
            w.Clear();

            #region Internal variables
            DateTime dob;
            if (Birthdate > DateTime.MinValue)
                dob = Birthdate;
            else
                dob = DateTime.MinValue;

            if (ZipCode.Length > 5)
                ZipCode = ZipCode.Substring(0, 5);
            if (ZipCode.Length < 5)
                ZipCode = ZipCode.PadLeft(5, '0');
            #endregion

            #region Searches
            try
            {
                if (PersonId > 0)
                {
                    //int r = Search(PersonID: PersonId, LastName: null, FirstName: null, MiddleName: null, Birthdate: null, Social: null, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null);
                    //if (r > 0)
                    //{
                    //    w.Add(r);   //add more than 1, to increase weight of this.
                    //    w.Add(r);
                    //    SearchResultDetails.AppendFormat("Pace PersonID search returned {0}\r\n", r);
                    //}
                }

                #region all MRN searches
                if (!string.IsNullOrEmpty(MedicalRecordNumber))
                {
                    if (!string.IsNullOrEmpty(LastName) && dob > DateTime.MinValue)
                        SearchResultDetails.AppendFormat("MRN + DOB + Last search returned {0}\r\n",
                            w.Add(Search(PersonID: null, LastName: LastName, FirstName: null, MiddleName: null, Birthdate: dob, Social: null, CustomerID: null, MedicalRecordNumber: MedicalRecordNumber, AccountNumber: null)));

                    if (!string.IsNullOrEmpty(FirstName) && dob > DateTime.MinValue)
                        SearchResultDetails.AppendFormat("MRN + DOB + First search returned {0}\r\n",
                            w.Add(Search(PersonID: null, LastName: null, FirstName: FirstName, MiddleName: null, Birthdate: dob, Social: null, CustomerID: null, MedicalRecordNumber: MedicalRecordNumber, AccountNumber: null)));

                    if (CustomerID > 0)
                        SearchResultDetails.AppendFormat("MRN + CustomerID returned {0}\r\n",
                            w.Add(Search(PersonID: null, LastName: null, FirstName: null, MiddleName: null, Birthdate: dob, Social: null, CustomerID: CustomerID, MedicalRecordNumber: MedicalRecordNumber, AccountNumber: null)));
                }
                #endregion

                #region all AccountNumber searches
                if (!string.IsNullOrEmpty(AccountNumber))
                {
                    //if (CustomerID > 0)
                    //    SearchResultDetails.AppendFormat("Account Number + CustomerID search returned {0}\r\n",
                    //       w.Add(Search(PersonID: null, LastName: null, FirstName: null, MiddleName: null, Birthdate: null, Social: null, CustomerID: CustomerID, MedicalRecordNumber: null, AccountNumber: AccountNumber)));

                    //if (CustomerID > 0)
                    //    SearchResultDetails.AppendFormat("Account Number + CustomerID + Last Name search returned {0}\r\n",
                    //        w.Add(Search(PersonID: null, LastName: LastName, FirstName: null, MiddleName: null, Birthdate: null, Social: SocialSecurity, CustomerID: CustomerID, MedicalRecordNumber: null, AccountNumber: AccountNumber)));
                }
                #endregion

                #region all SSN searches
                if (!string.IsNullOrWhiteSpace(SocialSecurity))
                {
                    //if (!string.IsNullOrEmpty(MedicalRecordNumber))
                    //    SearchResultDetails.AppendFormat("MRN + SSN search returned {0}\r\n",
                    //        w.Add(Search(PersonID: null, LastName: null, FirstName: null, MiddleName: null, Birthdate: null, Social: SocialSecurity, CustomerID: null, MedicalRecordNumber: MedicalRecordNumber, AccountNumber: null)));

                    if (dob > DateTime.MinValue)
                    {
                        if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                            SearchResultDetails.AppendFormat("SSN + First + Last + DOB search returned {0}\r\n", w.Add(Search(PersonID: null, LastName: LastName, FirstName: FirstName, MiddleName: null, Birthdate: dob, Social: SocialSecurity, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null)));

                        SearchResultDetails.AppendFormat("SSN + DOB search returned {0}\r\n",
                            w.Add(Search(PersonID: null, LastName: null, FirstName: null, MiddleName: null, Birthdate: dob, Social: SocialSecurity, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null)));
                    }

                    //if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                    //    SearchResultDetails.AppendFormat("SSN + First + Last search returned {0}\r\n", w.Add(Search(PersonID: null, LastName: LastName, FirstName: FirstName, MiddleName: null, Birthdate: null, Social: SocialSecurity, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null)));

                    //if (!string.IsNullOrEmpty(LastName))
                    //    SearchResultDetails.AppendFormat("(I) SSN + Last search returned {0}\r\n", w.Add(Search(PersonID: null, LastName: LastName, FirstName: null, MiddleName: null, Birthdate: null, Social: SocialSecurity, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null)));

                    //SSN+F+State -- Put this in to handle changing last name
                    if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(State))
                    {
                        int tmpSearch = Search(PersonID: null, LastName: null, FirstName: FirstName, MiddleName: null, Birthdate: dob, Social: null, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null);
                        if (tmpSearch > 0)
                        {
                            //PaceLib.Person p = new PaceLib.Person(tmpSearch);
                            //if (p.Address.StateCode.ToLower() == StateCode.ToLower())
                            //    SearchResultDetails.AppendFormat("(J) SSN + First + State search returned {0}\r\n", w.Add(tmpSearch));
                        }
                    }
                }
                #endregion


                #region FIRST + LAST + DOB variations
                int tmpSearch3 = Search(PersonID: null, LastName: LastName, FirstName: FirstName, MiddleName: null, Birthdate: dob, Social: null, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null);
                int tmpSearch2 = Search(PersonID: null, LastName: LastName, FirstName: FirstName, MiddleName: null, Birthdate: dob, Social: null, CustomerID: null, MedicalRecordNumber: null, AccountNumber: null);
                #endregion
            }
            catch
            { }
            #endregion

            return w.Weigh();
        }

        /// <summary>
        /// Perform a single search against the dataset
        /// </summary>
        public static int Search(object PersonID, object LastName, object FirstName, object MiddleName, DateTime Birthdate, object Social, object CustomerID, object MedicalRecordNumber, object AccountNumber)
        {
            int result = 0;

            int personID;
            if (PersonID == null)
                personID = 0;
            else
                int.TryParse(PersonID.ToString(), out personID);

            string lastName = LastName == null ? "" : LastName.ToString();
            string firstName = FirstName == null ? "" : FirstName.ToString();
            string middleName = MiddleName == null ? "" : MiddleName.ToString();
            DateTime birthdate;
            DateTime.TryParse(Birthdate.ToString(), out birthdate);
            string social = Social == null ? "" : Social.ToString();
            int customerID;
            if (CustomerID == null)
                customerID = 0;
            else
                int.TryParse(CustomerID.ToString(), out customerID);

            string medicalRecordNumber = MedicalRecordNumber == null ? "" : MedicalRecordNumber.ToString();
            string accountNumber = AccountNumber == null ? "" : AccountNumber.ToString();

            try
            {
                result = SingleSearch(personID, lastName, firstName, middleName, birthdate, social, customerID, medicalRecordNumber, accountNumber);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception hit in Search: {0}\r\n{1}", e.Message, e.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// Abstracted database routine.  
        /// Connect here to SQL Server, or whereever the data to search is stored.
        /// Use stored procedures or prepared statements in reality!
        /// This is for demonstration purposes and works with the SQL CE DB included in the project.
        /// </summary>
        /// <returns></returns>
        private static int SingleSearch(int PersonID, string LastName, string FirstName, string MiddleName
            , DateTime Birthdate, string Social, int CustomerID, string MedicalRecordNumber, string AccountNumber)
        {
            string connString = @"Data Source='Findingaperson_sampledatabase.sdf';";
            int result = 0;

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Select p.PersonID ");
            sql.AppendFormat("From Person p ");
            sql.AppendFormat("Left Join Visit v on p.PersonId=v.PersonId ");

            StringBuilder whereClause = new StringBuilder();

            if (PersonID > 0)
                whereClause.AppendFormat("p.PersonID = {0} ", PersonID);

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                if (whereClause.Length > 1)
                    whereClause.AppendFormat("and LastName='{0}' ", LastName);
                else
                    whereClause.AppendFormat("LastName='{0}' ", LastName);
            }

            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                if (whereClause.Length > 1)
                    whereClause.AppendFormat("and FirstName='{0}' ", FirstName);
                else
                    whereClause.AppendFormat("FirstName='{0}' ", FirstName);
            }

            if (!string.IsNullOrWhiteSpace(MiddleName))
            {
                if (whereClause.Length > 1)
                    whereClause.AppendFormat("and MiddleName='{0}' ", MiddleName);
                else
                    whereClause.AppendFormat("MiddleName='{0}' ", MiddleName);
            }

            //ADD the rest of the parameters as needed for your situation.

            if (whereClause.Length > 1)
            {
                sql.Append("where ");
                sql.Append(whereClause);
            }
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(connString))
                {
                    conn.Open();
                    SqlCeCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql.ToString();
                    SqlCeDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}

