using System;
using System.Collections.Generic;
using System.Text;
using tuszcom.dao.Repository;
using tuszcom.models.Interfaces.Services;

namespace tuszcom.services
{
    public class UserService : IServiceUser
    {
        private readonly UserRepository repository;

        public UserService()
        {
            repository = new UserRepository();
        }
    }
}
