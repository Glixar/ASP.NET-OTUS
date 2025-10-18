using System;
using System.Collections.Generic;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static IEnumerable<Employee> EmployeeSeeds => new List<Employee>
        {
            new Employee
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                RoleId = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                AppliedPromocodesCount = 5
            },
            new Employee
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                RoleId = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                AppliedPromocodesCount = 10
            },
        };

        public static IEnumerable<Role> Roles => new List<Role>
        {
            new Role
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

        public static IEnumerable<Preference> Preferences => new List<Preference>
        {
            new Preference
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                return new List<Customer>
                {
                    new Customer
                    {
                        Id = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                    },
                    new Customer
                    {
                        Id = Guid.Parse("9c0cbb87-6a1d-45f5-a6e8-9f5d7a2e4b10"),
                        Email = "maria_ivanova@mail.ru",
                        FirstName = "Мария",
                        LastName = "Иванова",
                    }
                };
            }
        }

        public static IEnumerable<CustomerPreference> CustomerPreferences
        {
            get
            {
                return new List<CustomerPreference>
                {
                    new CustomerPreference
                    {
                        CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                        PreferenceId = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84")
                    },
                    new CustomerPreference
                    {
                        CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                        PreferenceId = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd")
                    },
                    new CustomerPreference
                    {
                        CustomerId = Guid.Parse("9c0cbb87-6a1d-45f5-a6e8-9f5d7a2e4b10"),
                        PreferenceId = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c")
                    }
                };
            }
        }

        public static IEnumerable<PromoCode> PromoCodes
        {
            get
            {
                var begin = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                var end   = new DateTime(2026, 01, 11, 0, 0, 0, DateTimeKind.Utc);

                return new List<PromoCode>
                {
                    new PromoCode
                    {
                        Id = Guid.Parse("5d5e9df5-823f-42d1-a9dd-52e5fef6b8e1"),
                        Code = "BOOK15",
                        ServiceInfo = "Скидка 15% на книги",
                        PartnerName = "ReadCity",
                        BeginDate = begin,
                        EndDate = end,
                        PreferenceId = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                        CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0")
                    },
                    new PromoCode
                    {
                        Id = Guid.Parse("d52a3a5f-2e0a-4b83-9f5c-1f9c2b5e8fa1"),
                        Code = "THEATRE10",
                        ServiceInfo = "Скидка 10% на театр",
                        PartnerName = "CultureCity",
                        BeginDate = begin,
                        EndDate = end,
                        PreferenceId = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                        CustomerId = Guid.Parse("9c0cbb87-6a1d-45f5-a6e8-9f5d7a2e4b10")
                    }
                };
            }
        }
    }
}
