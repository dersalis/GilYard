using System;
using System.Collections.Generic;
using System.Linq;
using GilYard.Api.Data;
using GilYard.Api.Models;
using Mapster;

namespace GilYard.Api.Services
{
    /// <summary>
    /// Serwis użytkownik
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Dodaje użytkownika
        /// </summary>
        /// <param name="user">Użytkownik</param>
        /// <param name="hashedPassword">Hasło</param>
        /// <returns>Id dodanego użytkownika</returns>
        int Add(UserRegister user, string hashedPassword);

        /// <summary>
        /// Zwraca uzytkownika na podstawie adresu email
        /// </summary>
        /// <param name="email">Adres email</param>
        /// <returns>Uzytkownik</returns>
        User GetByEmail(string email);

        /// <summary>
        /// Zwraca użytkownika na podstawie id
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <returns>Użytkownik</returns>
        User GetById(int id);

        /// <summary>
        /// Zwraca listę użytkowników
        /// </summary>
        /// <returns>Lista użytkowników</returns>
        IEnumerable<UserForList> GetForList();

        /// <summary>
        /// Zmienia hasło użytkownika
        /// </summary>
        /// <param name="email">Email użytkownika</param>
        /// <param name="newHashedPassword">Nowe hasło</param>
        /// <returns>Id użytkownika</returns>
        int ChangePassword(string email, string newHashedPassword);

        /// <summary>
        /// Zmiania status użytkownika
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <param name="setActive">Status</param>
        /// <returns>Id użytkownika</returns>
        int ChangeStatus(int id, bool setActive);
    }

    /// <summary>
    /// Serwis użytkownik
    /// </summary>
    public class UserService : IUserService
    {
        private readonly GilYardContext _context;

        /// <summary>
        /// Serwis uzytkownik
        /// </summary>
        /// <param name="context">Kontekst</param>
        public UserService(GilYardContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Dodaje użytkownika
        /// </summary>
        /// <param name="user">Użytkownik</param>
        /// <param name="hashedPassword">Hasło</param>
        /// <returns>Id dodanego użytkownika</returns>
        public int Add(UserRegister user, string hashedPassword)
        {
            TypeAdapterConfig<UserRegister, User>.NewConfig()
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.Password, src => hashedPassword);

            var newUser = user.Adapt<User>();
            // newUser.Password = SecurePasswordHasherHelper.Hash(request.Password);

            _context.Users.Add(newUser);
            var id = _context.SaveChanges();

            return id;
        }


        /// <summary>
        /// Zwraca uzytkownika na podstawie adresu email
        /// </summary>
        /// <param name="email">Adres email</param>
        /// <returns>Uzytkownik</returns>
        public User GetByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }


        /// <summary>
        /// Zwraca użytkownika na podstawie id
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <returns>Użytkownik</returns>
        public User GetById(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }


        /// <summary>
        /// Zwraca listę użytkowników
        /// </summary>
        /// <returns>Lista użytkowników</returns>
        public IEnumerable<UserForList> GetForList()
        {
            var usersFromDB = _context.Users.Where(u => u.IsActive == true).ToList();
            var users = usersFromDB.Adapt<List<UserForList>>();
            return users;
        }


        /// <summary>
        /// Zmienia hasło użytkownika
        /// </summary>
        /// <param name="email">Email użytkownika</param>
        /// <param name="newHashedPassword">Nowe hasło</param>
        /// <returns>Id użytkownika</returns>
        public int ChangePassword(string email, string newHashedPassword)
        {
            var user = GetByEmail(email);
            if (user == null) throw new Exception($"Uzytkonwnik {email} juz istnieje.");
            user.Password = newHashedPassword;
            int id = _context.SaveChanges();
            return id;
        }


        /// <summary>
        /// Zmiania status użytkownika
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <param name="setActive">Status</param>
        /// <returns>Id użytkownika</returns>
        public int ChangeStatus(int id, bool setActive)
        {
            var user = GetById(id);
            if (user == null) throw new System.Exception($"Uzytkonwnik o id {id} juz istnieje.");
            user.IsActive = setActive;
            int userId = _context.SaveChanges();
            return userId;
        }
    }
}