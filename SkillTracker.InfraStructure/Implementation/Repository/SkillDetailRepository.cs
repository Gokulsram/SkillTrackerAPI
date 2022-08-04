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
    public class SkillDetailRepository : ISkillDetailRepository
    {
        private readonly SkillTrackerDBContext _dbContext;
        private readonly ICacheHelper _cacheHelper;
        public SkillDetailRepository(SkillTrackerDBContext dbContext, ICacheHelper cacheHelper)
        {
            _dbContext = dbContext;
            _cacheHelper = cacheHelper;
        }
        public async Task<BaseResponse> DeleteSkillDetailById(int skilldetailId)
        {
            try
            {
                var skilldetail = _dbContext.SkillDetail.Where(s => s.SkillDetailID == skilldetailId).FirstOrDefault<SkillDetail>();
                if (skilldetail != null)
                {
                    _dbContext.SkillDetail.Remove(skilldetail);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new BaseResponse { StatusCode = HttpStatusDetail.NotExistsCode, StatusDescription = string.Format(HttpStatusDetail.NotExistsMessage, skilldetailId) };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in Delete SkillDetailById()");
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE"))
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.ReferenceErrorCode,
                        StatusDescription = string.Format(HttpStatusDetail.ReferenceErrorMessage, "SkillDetailID -" + skilldetailId.ToString())
                    };
                }
                else
                {
                    return new BaseResponse { StatusCode = HttpStatusDetail.InternalErrorCode, StatusDescription = HttpStatusDetail.InternalErrorMessage };
                }
            }
            return new BaseResponse { StatusCode = HttpStatusDetail.SuccessCode, StatusDescription = HttpStatusDetail.SuccessMessage };
        }
        public async Task<SkillDetail> GetSkillDetailById(int skilldetailId)
        {
            try
            {
                return await _dbContext.SkillDetail.AsNoTracking().Where(x => x.SkillDetailID == skilldetailId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SkillDetail>> GetSkillDetail()
        {
            try
            {
                return await _dbContext.SkillDetail.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BaseResponse> SaveSkillDetail(SkillDetail skilldetail)
        {
            try
            {
                var existsSkillDetail = await _dbContext.SkillDetail.AsNoTracking().FirstOrDefaultAsync(x => x.SkillName == skilldetail.SkillName);
                if (existsSkillDetail == null)
                {
                    _dbContext.SkillDetail.Add(skilldetail);
                    await _dbContext.SaveChangesAsync();
                    _cacheHelper.SkillDetail = await _dbContext.SkillDetail.AsNoTracking().ToListAsync();
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.SuccessCode,
                        StatusDescription = HttpStatusDetail.SuccessMessage,
                        ID = skilldetail.SkillDetailID
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.AlreadyExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.AlreadyExistsMessage, skilldetail.SkillName)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in SaveSkillDetail()");
                return new BaseResponse
                {
                    StatusCode = HttpStatusDetail.InternalErrorCode,
                    StatusDescription = HttpStatusDetail.InternalErrorMessage
                };
            }
        }
        public async Task<BaseResponse> UpdateSkillDetail(SkillDetail skilldetail)
        {
            try
            {
                var existsSkillDetail = await _dbContext.SkillDetail.AsNoTracking().FirstOrDefaultAsync(x => x.SkillDetailID == skilldetail.SkillDetailID);
                if (existsSkillDetail != null)
                {
                    if (existsSkillDetail.SkillName != skilldetail.SkillName)
                    {
                        existsSkillDetail = await _dbContext.SkillDetail.AsNoTracking().FirstOrDefaultAsync(x => x.SkillName == skilldetail.SkillName);
                        if (existsSkillDetail != null)
                        {
                            return new BaseResponse
                            {
                                StatusCode = HttpStatusDetail.AlreadyExistsCode,
                                StatusDescription = string.Format(HttpStatusDetail.AlreadyExistsMessage, skilldetail.SkillName)
                            };
                        }
                        else
                        {
                            _dbContext.SkillDetail.UpdateRange(skilldetail);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _dbContext.SkillDetail.UpdateRange(skilldetail);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.NotExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.NotExistsMessage, skilldetail.SkillDetailID)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in UpdateSkillDetail()");
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

