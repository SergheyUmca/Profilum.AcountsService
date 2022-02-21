using Microsoft.EntityFrameworkCore;
using Profilum.AccountService.Common;
using Profilum.AccountService.DAL.EF.Contexts;
using Profilum.AccountService.DAL.EF.Entities;
using Profilum.AccountService.DAL.Models;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.DAL.EF.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BaseContext _db;

        public AccountRepository(BaseContext db)
        {
            _db = db;
        }
        
        public async Task<Response<AccountResponse>> Get(long id)
        {
            try
            {
                var accountEntities = await _db.AccountEntities.Where(a => a.Id == id)
                    .Select(a => new AccountEntity
                    {
                       Id = a.Id,
                       UserId = a.UserId,
                       AccountNumber = a.AccountNumber
                    }).FirstOrDefaultAsync();
                if (accountEntities == null)
                    return new ErrorResponse<AccountResponse>("Not found", ResponseCodes.NOT_FOUND_RECORDS);
                

                return new Response<AccountResponse>(new AccountResponse
                {
                    Id = accountEntities.Id,
                    UserId = accountEntities.UserId,
                    AccountNumber = accountEntities.AccountNumber
                });
            }
            catch (Exception e)
            {
                var exceptionMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new ErrorResponse<AccountResponse>(exceptionMessage, 
                    ResponseCodes.DATABASE_ERROR);
            }
        }
        
        public async Task<Response<List<AccountResponse>>> GetAll()
        {
            try
            {
                var accountEntities = await _db.AccountEntities
                    .Select(a => new AccountEntity
                    {
                        Id = a.Id,
                        UserId = a.UserId,
                        AccountNumber = a.AccountNumber
                    }).AsNoTracking().ToListAsync();
                
                if (accountEntities.Count == 0)
                    return new ErrorResponse<List<AccountResponse>>("Not found", ResponseCodes.NOT_FOUND_RECORDS);
                

                return new Response<List<AccountResponse>>( accountEntities.Select(a => new AccountResponse
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    AccountNumber = a.AccountNumber
                }).ToList());
            }
            catch (Exception e)
            {
                var exceptionMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new ErrorResponse<List<AccountResponse>>(exceptionMessage, 
                    ResponseCodes.DATABASE_ERROR);
            }
        }
        
        public async IAsyncEnumerable<AccountResponse> GetAllAsyncEnumerable()
        {
            
            var enumerator = _db.AccountEntities.GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                yield return  new AccountResponse
                {
                    Id = enumerator.Current.Id,
                    UserId = enumerator.Current.UserId,
                    AccountNumber = enumerator.Current.AccountNumber
                };
            }
        }
        
        public async Task<Response<long>> Create(AccountRequest request)
        {
            try
            {
                var accountEntity = new AccountEntity
                {
                    UserId = request.UserId,
                    AccountNumber = request.AccountNumber
                };

                await _db.AccountEntities.AddAsync(accountEntity);
                await _db.SaveChangesAsync();
                
                return new Response<long>(accountEntity.Id);
            }
            catch (Exception e)
            {
                var exceptionMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new ErrorResponse<long>(exceptionMessage, ResponseCodes.DATABASE_ERROR);
            }
        }
        
        public async Task<Response> Update(AccountRequest entity)
        {
            try
            {
                var entityToBeUpdated = new AccountEntity
                {
                   Id = entity.Id,
                   UserId = entity.UserId,
                   AccountNumber = entity.AccountNumber
                };
                
                var local = _db.Set<AccountEntity>().Local.FirstOrDefault(d => d.Id.Equals(entityToBeUpdated.Id));
                if (local != null)
                    _db.Entry(local).State = EntityState.Detached;

                _db.Entry(entityToBeUpdated).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return new Response();
            }
            catch (Exception e)
            {
                var exceptionMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new ErrorResponse(exceptionMessage, ResponseCodes.DATABASE_ERROR);
            }
        }
        
        public async Task<Response> Delete(long id)
        {
            try
            {
                var entity = await _db.AccountEntities
                    .Where(d => d.Id.Equals(id))
                    .Select(d => new AccountEntity
                    {
                        Id = d.Id
                    }).AsNoTracking().FirstOrDefaultAsync();
                
                if (entity == null)
                    return new Response();

                _db.Remove(entity);
                await _db.SaveChangesAsync();
                
                return new Response();
            }
            catch (Exception e)
            {
                var exceptionMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new ErrorResponse(exceptionMessage, ResponseCodes.DATABASE_ERROR);
            }
        }
        
        public async Task<Response> DeleteAll()
        {
            try
            {
                var entity = await _db.AccountEntities
                    .Select(d => new AccountEntity
                    {
                        Id = d.Id
                    }).AsNoTracking().ToListAsync();

                _db.RemoveRange(entity);
                await _db.SaveChangesAsync();
                
                return new Response();
            }
            catch (Exception e)
            {
                var exceptionMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new ErrorResponse(exceptionMessage, ResponseCodes.DATABASE_ERROR);
            }
        }
    }
}