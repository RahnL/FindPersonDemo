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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchLibrary;
using System.Text;

namespace SearchLibraryTests
{
    [TestClass]
    public class PersonFunctionTests
    {
        [TestMethod]
        public void SearchForSingleKnownPersonByPersonID_ReturnsCorrectPersonID()
        {
            int personIdActual = SearchLibrary.PersonFunctions.Search(
                PersonID: 1,
                LastName: null,
                FirstName: null,
                MiddleName: null,
                Birthdate: DateTime.MinValue,
                Social: null,
                CustomerID: null,
                MedicalRecordNumber: null,
                AccountNumber: null);

            int expected = 1;

            Assert.AreEqual(expected, personIdActual, "Found the wrong person");
        }

        [TestMethod]
        public void SearchForSingleKnownPersonByNameDOB_ReturnsCorrectPersonID()
        {
            int personIdActual = SearchLibrary.PersonFunctions.Search(
                PersonID: null,
                LastName: "Hingst",
                FirstName: "Cara",
                MiddleName: "N",
                Birthdate: DateTime.Parse("2/1/1980"),
                Social: null,
                CustomerID: null,
                MedicalRecordNumber: null,
                AccountNumber: null);

            int expected = 15;

            Assert.AreEqual(expected, personIdActual, "Found the wrong person");
        }

        #region Build Breaking demo tests
        /*
         * Tests that don't work with this particular build, but I wanted to include to show the concept of how to do.
         * Keeping them commented out, so they don't break the build.
         * Functions aren't identical to what's in this library.
         */
        /*
        [TestMethod]
        public void SearchSingleKnownPersonBySSN()
        {
            int personIDActual = SearchLibrary.PersonFunctions.Search(
                PacePersonID: 0
                , LastName: "Smith"
                , FirstName: "John"
                , MiddleName: ""
                , Birthdate: DateTime.Parse("12/23/41")
                , SSN: "555-66-7777"
                , CustomerID: 76
                , MedicalRecordNumber: "009401"
                , CustomerGroup: "GenericHospital"
                , CustomerAccountNumber: "98591134"
                , StateCode: "KY"
                , ZipCode: "41101"
                , SearchToSkip: null
                );

            //This isn't active with this version of the library, but essentially it captures a log of all
            //the searches done so you can use it for troubleshooting.
            Console.WriteLine(SearchLibrary.PersonFunctions.SearchResultDetails);

            Assert.AreEqual(666, personIDActual, "Person found that should not have been found.");
        }

        //This is an obvious search to do.  Can go all-out and do random data, but this is a little easier to read. :)
        [TestMethod]
        public void SearchPersonKnownDoesntExist()
        {
            int personIDactual = SearchLibrary.PersonFunctions.Search(
               PacePersonID: 0
               , LastName: "John"
               , FirstName: "Smith"
               , MiddleName: ""
               , Birthdate: DateTime.Parse("1/1/1970")
               , SSN: "000-000-0000"
               , CustomerID: 123456
               , MedicalRecordNumber: null
               , CustomerGroup: "test group"
               , CustomerAccountNumber: "1234567890"
               , StateCode: "GA"
               , ZipCode: "30305"
               , SearchToSkip: null
               );

            Console.WriteLine(SearchLibrary.PersonFunctions.SearchResultDetails);

            Assert.AreEqual(-1, personIDactual, "Person found that should not have been found.");
        }

        //This is one of the gotcha's mentioned in the presentation.
        [TestMethod]
        public void SearchSingleKnownPerson_BabyNameWasChangedToRealName()
        {
            int personIDActual = SearchLibrary.PersonFunctions.FullSearch(
                 PacePersonID: 0
                , LastName: "Walker"
                , FirstName: "Boy Samantha"
                , MiddleName: ""
                , Birthdate: DateTime.Parse("11/22/2012")
                , SSN: ""
                , CustomerID: 501273
                , MedicalRecordNumber: "4483456"
                , CustomerGroup: "Some Health System"
                , CustomerAccountNumber: "7646423"
                , StateCode: "FL"
                , ZipCode: "25702"
                , SearchToSkip: null
                );

            Console.WriteLine(SearchLibrary.PersonFunctions.SearchResultDetails);

            //This ID below is one from the usual DB I run against. It doesn't relate to the demo database.
            Assert.AreEqual(4278036, personIDActual, "Person found that should not have been found.");
        }

        //This is one of the places where do a bulk extract of data, then re-search for it.
        //SQLCE doesn't allow TOP, so can't use here...otherwise, these are my favorite tests. :)
        //ExtractData(sql) is just a helper function to run a query and return a dataset.
        [TestMethod]
        public void SearchFor10PersonByPersonID()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select top 10 p.PersonID,FirstName,MiddleName,LastName,BirthDate,ssn, Address1, StateCode,ZipCode");
            sql.Append(" ,CustomerAccountNumber, MedicalRecordNumber, v.CustomerID");
            sql.Append(" , cg.Name as GroupName");
            sql.Append(" from Person p");
            sql.Append(" inner join Visit v (nolock) on p.PersonID=v.PersonID");
            sql.Append(" inner join Address a (nolock) on p.AddressID=a.AddressID");
            sql.Append(" inner join Customer c (nolock) on v.CustomerID=c.CustomerID");
            sql.Append(" inner join CustomerFacility cf (nolock) on c.CustomerFacilityID=cf.CustomerFacilityID");
            sql.Append(" inner join CustomerGroup cg (nolock) on cf.CustomerGroupID=cg.CustomerGroupID");
            sql.Append(" where referraldate>'1/1/2012'");

            DataSet peopleToSearch = ExtractData(sql);
            int recordsFound = 0;
            int recorsNotFound = 0;
            int recordsProcessed = 0;

            StringBuilder records = new StringBuilder();

            foreach (DataTable t in peopleToSearch.Tables)
            {
                foreach (DataRow r in t.Rows)
                {
                    recordsProcessed++;

                    int personIDActual = SearchLibrary.PersonFunctions.FullSearch(
                         PacePersonID: int.Parse(r["PersonID"].ToString())
                        , LastName: ""
                        , FirstName: ""
                        , MiddleName: ""
                        , Birthdate: DateTime.Parse("1/1/0001")
                        , SSN: ""
                        , CustomerID: 0
                        , MedicalRecordNumber: ""
                        , CustomerGroup: ""
                        , CustomerAccountNumber: ""
                        , StreetAddress: ""
                        , StateCode: ""
                        , ZipCode: ""
                        , SearchToSkip: null);

                    Console.WriteLine(SearchLibrary.PersonFunctions.SearchResultDetails);

                    if (personIDActual == int.Parse(r["PersonID"].ToString()))
                    {
                        recordsFound++;
                        records.AppendFormat("{0}: {1} matched to {2}\r\n", recordsProcessed, r["PersonID"], personIDActual);
                    }
                    else
                    {
                        recorsNotFound++;
                        records.AppendFormat("{0}: ** {1} matched to {2} - THIS IS UNEXPECTED **\r\n", recordsProcessed, r["PersonID"], personIDActual);
                    }
                }
            }

            Assert.AreEqual(10, recordsFound, "Error matching on Pace ID: \r\n" + records.ToString());
        }

        //Another bulk test, for demo purposes
        //Where some of the elements from the data is through out, and we know the existing PersonID's we should get back
        [TestMethod]
        public void SearchFor10PersonByNonPersonIDCriteria()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select top 100 p.PersonID,FirstName,MiddleName,LastName,BirthDate,ssn, Address1, StateCode,ZipCode ");
            sql.Append(" ,CustomerAccountNumber, MedicalRecordNumber, v.CustomerID ");
            sql.Append(" ,cg.Name as GroupName ");
            sql.Append(" from Person p");
            sql.Append(" inner join Visit v (nolock) on p.PersonID=v.PersonID");
            sql.Append(" inner join Address a (nolock) on p.AddressID=a.AddressID");
            sql.Append(" inner join Customer c (nolock) on v.CustomerID=c.CustomerID");
            sql.Append(" where referraldate>'1/1/2012'");

            DataSet peopleToSearch = ExtractData(sql);
            int recordsFound = 0;
            int recorsNotFound = 0;
            int recordsProcessed = 0;

            StringBuilder records = new StringBuilder();

            foreach (DataTable t in peopleToSearch.Tables)
            {
                foreach (DataRow r in t.Rows)
                {
                    recordsProcessed++;

                    int personIDActual = SearchLibrary.PersonFunctions.FullSearch(
                         PacePersonID: 0
                        , LastName: r["LastName"].ToString()
                        , FirstName: r["FirstName"].ToString()
                        , MiddleName: ""
                        , Birthdate: DateTime.Parse(r["BirthDate"].ToString())
                        , SSN: r["SSN"].ToString()
                        , CustomerID: int.Parse(r["CustomerID"].ToString())
                        , MedicalRecordNumber: r["MedicalRecordNumber"].ToString()
                        , CustomerGroup: ""
                        , CustomerAccountNumber: ""
                        , StateCode: r["StateCode"].ToString()
                        , ZipCode: r["ZipCode"].ToString()
                        , SearchToSkip: null);

                    Console.WriteLine(SearchLibrary.PersonFunctions.SearchResultDetails);
                    if (personIDActual == int.Parse(r["PersonID"].ToString()))
                    {
                        recordsFound++;
                    }
                    else
                    {
                        recorsNotFound++;
                        records.AppendFormat("{0}: ** {1} matched to {2} - THIS IS UNEXPECTED **\r\n", recordsProcessed, r["PersonID"], personIDActual);
                    }
                }
            }

            Assert.AreEqual(10, recordsFound, "Error matching on Pace ID: \r\n" + records.ToString());
        }

                */
        #endregion
    }
}
