using Microsoft.EntityFrameworkCore;
using Serilog;
using SkillTracker.Core;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillTracker.InfraStructure
{
    public class UserSkillMappingRepository : IUserSkillMappingRepository
    {
        private readonly SkillTrackerDBContext _dbContext;
        public UserSkillMappingRepository(SkillTrackerDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<BaseResponse> DeleteUserSkillMappingById(int userskillmappingId)
        {
            try
            {
                var userskillmapping = _dbContext.UserSkillMapping.Where(s => s.UserSkillMappingID == userskillmappingId).FirstOrDefault<UserSkillMapping>();
                if (userskillmapping != null)
                {
                    _dbContext.UserSkillMapping.Remove(userskillmapping);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.NotExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.NotExistsMessage, userskillmappingId)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in Delete UserSkillMappingById()");
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE"))
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.ReferenceErrorCode,
                        StatusDescription = string.Format(HttpStatusDetail.ReferenceErrorMessage, "UserSkillMappingID -" + userskillmappingId.ToString())
                    };
                }
                else
                {
                    return new BaseResponse { StatusCode = HttpStatusDetail.InternalErrorCode, StatusDescription = HttpStatusDetail.InternalErrorMessage };
                }
            }
            return new BaseResponse { StatusCode = HttpStatusDetail.SuccessCode, StatusDescription = HttpStatusDetail.SuccessMessage };
        }
        public async Task<UserSkillMapping> GetUserSkillMappingById(int userskillmappingId)
        {
            try
            {
                return await _dbContext.UserSkillMapping.AsNoTracking().Where(x => x.UserSkillMappingID == userskillmappingId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserSkillMapping>> GetUserSkillMapping()
        {
            try
            {
                return await _dbContext.UserSkillMapping.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BaseResponse> SaveUserSkillMapping(UserSkillMapping userskillmapping)
        {
            try
            {
                var existsUserSkillMapping = await _dbContext.UserSkillMapping.AsNoTracking().
                    FirstOrDefaultAsync(x => x.SkillDetailID == userskillmapping.SkillDetailID);
                if (existsUserSkillMapping == null)
                {
                    _dbContext.UserSkillMapping.Add(userskillmapping);
                    await _dbContext.SaveChangesAsync();
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.SuccessCode,
                        StatusDescription = HttpStatusDetail.SuccessMessage,
                        ID = userskillmapping.UserSkillMappingID
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.AlreadyExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.AlreadyExistsMessage, userskillmapping.SkillDetailID)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in SaveUserSkillMapping()");
                return new BaseResponse
                {
                    StatusCode = HttpStatusDetail.InternalErrorCode,
                    StatusDescription = HttpStatusDetail.InternalErrorMessage
                };
            }
        }
        public async Task<BaseResponse> UpdateUserSkillMapping(UserSkillMapping userskillmapping)
        {
            try
            {
                var existsUserSkillMapping = await _dbContext.UserSkillMapping.AsNoTracking().FirstOrDefaultAsync(x => x.UserSkillMappingID == userskillmapping.UserSkillMappingID);
                if (existsUserSkillMapping != null)
                {
                    _dbContext.UserSkillMapping.UpdateRange(userskillmapping);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.NotExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.NotExistsMessage, userskillmapping.UserSkillMappingID)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in UpdateUserSkillMapping()");
                return new BaseResponse
                {
                    StatusCode = HttpStatusDetail.InternalErrorCode,
                    StatusDescription = HttpStatusDetail.InternalErrorMessage
                };
            }
            return new BaseResponse
            {
                StatusCode = HttpStatusDetail.SuccessCode,
                StatusDescription = HttpStatusDetail.SuccessMessage
            };
        }
    }
}

