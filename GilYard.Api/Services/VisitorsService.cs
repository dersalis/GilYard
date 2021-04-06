using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GilYard.Api.Data;
using GilYard.Api.Models;
using Mapster;

namespace GilYard.Api.Services
{
    public interface IVisitorsService
    {
        /// <summary>
        /// Zwraca wszystkich gości
        /// </summary>
        /// <returns>Lista gości</returns>
        IEnumerable<VisitorForTable> GetAll();

        /// <summary>
        /// Zwraca gości po adresie id użytkownika
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <returns>Lista gości</returns>
        IEnumerable<ReportMyVisitors> GetByUserId(int id);

        /// <summary>
        /// Dodaje gościa
        /// </summary>
        /// <param name="visitor">Nowy gość</param>
        /// <returns>Id dodanego gościa</returns>
        int Add(VisitorAdd visitor);

        /// <summary>
        /// Aktualizuje gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <param name="visitor">Gość po aktualizacji</param>
        /// <returns>Id gościa</returns>
        int Update(int id, VisitorAdd visitor);

        /// <summary>
        /// Usuwa gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <returns>Id gościa</returns>
        int Delete(int id);

        /// <summary>
        /// Wpuszcza gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <returns>Id gościa</returns>
        int ComeIn(int id);

        /// <summary>
        /// Wypuszcza gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <returns>Id gościa</returns>
        int ComeOut(int id);
    }

    public class VisitorsService : IVisitorsService
    {
        private readonly GilYardContext _context;

        /// <summary>
        /// Serwis goście
        /// </summary>
        /// <param name="context">Kontekst</param>
        public VisitorsService(GilYardContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Zwraca wszystkich gości
        /// </summary>
        /// <returns>Lista gości</returns>
        public IEnumerable<VisitorForTable> GetAll()
        {
            var visitorsFromDB = _context.Visitors.Where(v => v.LeaveDate == null).ToList();
            TypeAdapterConfig<Visitor, VisitorForTable>.NewConfig().Map(dest => dest.UserName, src => src.Guardian.Name);
            var visitors = visitorsFromDB.Adapt<List<VisitorForTable>>();

            return visitors;
        }


        /// <summary>
        /// Zwraca gości po adresie id użytkownika
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <returns>Lista gości</returns>
        public IEnumerable<ReportMyVisitors> GetByUserId(int id)
        {
            var visitorsFromDB = _context.Visitors.Where(v => v.GuardianId == id).ToList();
            var visitors = visitorsFromDB.Adapt<List<ReportMyVisitors>>();

            return visitors;
        }


        /// <summary>
        /// Dodaje gościa
        /// </summary>
        /// <param name="visitor">Nowy gość</param>
        /// <returns>Id dodanego gościa</returns>
        public int Add(VisitorAdd visitor)
        {
            var newVisitor = visitor.Adapt<Visitor>();
            _context.Visitors.Add(newVisitor);
            var id = _context.SaveChanges();
            return id;
        }


        /// <summary>
        /// Aktualizuje gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <param name="visitor">Gość po aktualizacji</param>
        /// <returns>Id gościa</returns>
        public int Update(int id, VisitorAdd visitor)
        {
            var visitorsFromDB = _context.Visitors.Find(id);
            if (visitorsFromDB == null) throw new Exception("Nie można odnaleźć gościa.");

            if (!string.IsNullOrEmpty(visitor.Name)) visitorsFromDB.Name = visitor.Name;
            if (!string.IsNullOrEmpty(visitor.Phone)) visitorsFromDB.Phone = visitor.Phone;
            if (!string.IsNullOrEmpty(visitor.CarPlate)) visitorsFromDB.CarPlate = visitor.CarPlate;
            if (visitorsFromDB.GuardianId != visitor.GuardianId) visitorsFromDB.GuardianId = visitor.GuardianId;

            var uId = _context.SaveChanges();

            return uId;
        }


        /// <summary>
        /// Usuwa gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <returns>Id gościa</returns>
        public int Delete(int id)
        {
            var visitorsFromDB = _context.Visitors.Find(id);
            if (visitorsFromDB == null) throw new Exception("Nie można odnaleźć gościa.");

            _context.Visitors.Remove(visitorsFromDB);
            var uId = _context.SaveChanges();

            return uId;
        }


        /// <summary>
        /// Wpuszcza gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <returns>Id gościa</returns>
        public int ComeIn(int id)
        {
            var visitorsFromDB = _context.Visitors.Find(id);
            if (visitorsFromDB == null) throw new Exception("Nie można odnaleźć gościa.");

            visitorsFromDB.ArriveDate = DateTime.Now;

            var uId = _context.SaveChanges();

            return uId;
        }


        /// <summary>
        /// Wypuszcza gościa
        /// </summary>
        /// <param name="id">Id gościa</param>
        /// <returns>Id gościa</returns>
        public int ComeOut(int id)
        {
            var visitorsFromDB = _context.Visitors.Find(id);
            if (visitorsFromDB == null) throw new Exception("Nie można odnaleźć gościa.");

            visitorsFromDB.LeaveDate = DateTime.Now;

            var uId = _context.SaveChanges();

            return uId;
        }
    }
}