using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Login;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{
    public class AccountProvider : BaseProvider
    {
        private static AccountProvider _instance = new AccountProvider();

        public static AccountProvider Getprovider()
        {
            return _instance;
        }

        public bool IsAuthentication(LoginViewModel model)
        {
            var account = AccountRepository.GetSingle(model.Username);
            if (account != null)
            {
                string hashPass = account.Password;
                bool verify = BCrypt.Net.BCrypt.Verify(model.Password, hashPass);
                if (verify)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public string GetRoleName(string username)
        {
            return AccountRepository.GetRole(username);
        }

        private IEnumerable<RegisterViewModel> GetDataIndex()
        {

            var account = AccountRepository.GetAll();
            var accounts = (from acc in account
                            select new RegisterViewModel
                            {
                                Username = acc.Username,
                                Role = acc.Role
                            }).ToList();

            return accounts;

        }

        public List<RegisterViewModel> GetIndex()
        {
            var model = GetDataIndex().ToList();
            return model;
        }

        public RegisterViewModel GetUpdate(string username)
        {
            var model = new RegisterViewModel();
            var oldAccount = AccountRepository.GetSingle(username);
            MapingModel(model, oldAccount);
            model.DropDownRole = GetRole();
            return model;
        }

        public RegisterViewModel GetAdd()
        {
            var model = new RegisterViewModel();
            model.DropDownRole = GetRole();
            return model;
        }

        public void PostAdd(RegisterViewModel model)
        {          
            try
            {              
                var hashPass = BCrypt.Net.BCrypt.HashPassword(model.Password = "indocyber");
                var account = new Account();
                account.Username = model.Username;
                account.Password = hashPass;
                account.Role = model.Role;

                AccountRepository.Insert(account);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PostUpdate(RegisterViewModel model)
        {
            try
            {
                var oldPassword = AccountRepository.GetSingle(model.Username);
                var hashPass = BCrypt.Net.BCrypt.HashPassword(model.Password = "indocyber");
                oldPassword.Username = model.Username;
                oldPassword.Password = hashPass;
                oldPassword.Role = model.Role;

                AccountRepository.Update(oldPassword);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DropDownListViewModel> GetRole()
        {
            var account = new List<DropDownListViewModel>
            {
                new DropDownListViewModel{StringValue = "Administrator", Text = "Administrator"},
                new DropDownListViewModel{StringValue = "Customer", Text = "Customer"},
                new DropDownListViewModel{StringValue = "Finance", Text = "Finance"},
                new DropDownListViewModel{StringValue = "Salesman", Text = "Salesman"},
            };

            return account;

        }

    }
}
