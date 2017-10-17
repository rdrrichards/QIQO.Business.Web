using QIQO.Business.Client.Entities;
using System;
using Xunit;

namespace QIQO.Business.Tests
{
    public class ModelCoreUnitTests
    {
        [Fact]
        public void AccountPersonUnitTests()
        {
            var now = DateTime.Now;
            var ap = new AccountPerson() {
                AddedDateTime = now,
                AddedUserID = "Test",
                Comment = "Test Comment",
                EndDate = DateTime.Today.AddYears(1),
                PersonCode = "TEST",
                PersonDOB = new DateTime(1966, 12, 3),
                PersonFirstName = "Test",
                PersonFullNameFL = "Test",
                PersonFullNameFML = "Test",
                PersonFullNameLF = "Test",
                PersonFullNameLFM = "Test",
                PersonKey = 100,
                PersonLastName = "Test",
                PersonMI = "T",
                RoleInCompany = "Test",
                StartDate = DateTime.Today,
                UpdateDateTime = now,
                UpdateUserID = "Test",
                EntityPersonKey = 162
            };
            Assert.NotNull(ap);
            Assert.NotNull(ap.Addresses);
            Assert.Empty(ap.Addresses);
            Assert.NotNull(ap.PersonAttributes);
            Assert.Empty(ap.PersonAttributes);
            Assert.NotNull(ap.PersonTypeData);
            Assert.Equal(0, ap.PersonTypeData.PersonTypeKey);
            Assert.Equal(QIQOPersonType.AccountEmployee, ap.CompanyRoleType);
            Assert.Equal(now, ap.AddedDateTime);
            Assert.Equal("Test", ap.AddedUserID);
            Assert.Equal("Test Comment", ap.Comment);
            Assert.Equal(DateTime.Today.AddYears(1), ap.EndDate);
            Assert.Equal("TEST", ap.PersonCode);
            Assert.Equal(new DateTime(1966, 12, 3), ap.PersonDOB);
            Assert.Equal("Test", ap.PersonFirstName);
            Assert.Equal("Test", ap.PersonFullNameFL);
            Assert.Equal("Test", ap.PersonFullNameFML);
            Assert.Equal("Test", ap.PersonFullNameLF);
            Assert.Equal("Test", ap.PersonFullNameLFM);
            Assert.Equal(100, ap.PersonKey);
            Assert.Equal("Test", ap.PersonLastName);
            Assert.Equal("T", ap.PersonMI);
            Assert.Equal("Test", ap.RoleInCompany);
            Assert.Equal(DateTime.Today, ap.StartDate);
            Assert.Equal(now, ap.UpdateDateTime);
            Assert.Equal("Test", ap.UpdateUserID);
            Assert.Equal(162, ap.EntityPersonKey);
        }
    }
}
