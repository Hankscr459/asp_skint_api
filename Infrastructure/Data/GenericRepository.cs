using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity {
        private readonly StoreContext _context;
        public GenericRepository (StoreContext context) {
            _context = context;
        }

        public async Task<T> GetByIdAsync (int id) {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync () {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            var objFormDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (objFormDb != null)
            {
                if (product.PictureUrl != null)
                {
                    objFormDb.PictureUrl = product.PictureUrl;
                }
                objFormDb.Name = product.Name;
                objFormDb.Description = product.Description;
                objFormDb.Price = product.Price;
                objFormDb.ProductType = product.ProductType;
                objFormDb.ProductTypeId = product.ProductTypeId;
                objFormDb.ProductBrand = product.ProductBrand;
                objFormDb.ProductBrandId = product.ProductBrandId;
                objFormDb.ProductType = product.ProductType;
                objFormDb.ProductType = product.ProductType;
            }
        }
    }
}