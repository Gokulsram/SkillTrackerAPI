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
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly SkillTrackerDBContext _dbContext;
        private readonly ICacheHelper _cacheHelper;
        public UserProfileRepository(SkillTrackerDBContext dbContext, ICacheHelper cacheHelper)
        {
            _dbContext = dbContext;
            _cacheHelper = cacheHelper;
        }
        public async Task<BaseResponse> DeleteUserProfileById(int userID)
        {
            try
            {
                var userprofile = _dbContext.User.Where(s => s.UserID == userID).FirstOrDefault();
                if (userprofile != null)
                {
                    _dbContext.User.Remove(userprofile);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.NotExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.NotExistsMessage, userID)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in Delete UserProfileById()");
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE"))
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.ReferenceErrorCode,
                        StatusDescription = string.Format(HttpStatusDetail.ReferenceErrorMessage, "UserProfileID -" + userID.ToString())
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.InternalErrorCode,
                        StatusDescription = HttpStatusDetail.InternalErrorMessage
                    };
                }
            }
            return new BaseResponse
            {
                StatusCode = HttpStatusDetail.SuccessCode,
                StatusDescription = HttpStatusDetail.SuccessMessage
            };
        }
        public async Task<User> GetUserProfileByUserId(int userId)
        {
            try
            {
                return await _dbContext.User.AsNoTracking().Where(x => x.UserID == userId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetUserProfile()
        {
            try
            {
                return await _dbContext.User.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BaseResponse> SaveUserProfile(User userprofile)
        {
            try
            {
                var existsUserProfile = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userprofile.Name);
                if (existsUserProfile == null)
                {
                    _dbContext.User.Add(userprofile);
                    await _dbContext.SaveChangesAsync();
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.SuccessCode,
                        StatusDescription = HttpStatusDetail.SuccessMessage,
                        ID = userprofile.UserID
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.AlreadyExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.AlreadyExistsMessage, userprofile.Name)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in SaveUserProfile()");
                return new BaseResponse
                {
                    StatusCode = HttpStatusDetail.InternalErrorCode,
                    StatusDescription = HttpStatusDetail.InternalErrorMessage
                };
            }
        }
        public async Task<BaseResponse> UpdateUserProfile(User userprofile)
        {
            try
            {
                var existsUserProfile = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.UserID == userprofile.UserID);
                if (existsUserProfile != null)
                {
                    if (existsUserProfile.Name != userprofile.Name)
                    {
                        existsUserProfile = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.Name == userprofile.Name);
                        if (existsUserProfile != null)
                        {
                            return new BaseResponse
                            {
                                StatusCode = HttpStatusDetail.AlreadyExistsCode,
                                StatusDescription = string.Format(HttpStatusDetail.AlreadyExistsMessage, userprofile.Name)
                            };
                        }
                        else
                        {
                            _dbContext.User.UpdateRange(userprofile);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _dbContext.User.UpdateRange(userprofile);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {

                    return new BaseResponse
                    {
                        StatusCode = HttpStatusDetail.NotExistsCode,
                        StatusDescription = string.Format(HttpStatusDetail.NotExistsMessage, userprofile.UserID)
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in UpdateUserProfile()");
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

        public async Task<BaseResponse> EditUserProfile(int userId, List<Skills> editUserProfile)
        {
            try
            {
                var existsUserProfile = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.UserID == userId);
                if (existsUserProfile != null)
                {
                    DateTime dateTime = DateTime.Now;
                    if (existsUserProfile.ModifiedTime != null)
                        dateTime = (DateTime)existsUserProfile.ModifiedTime;

                    if (DateTime.Now > dateTime.AddDays(10))
                    {
                        foreach (var skill in editUserProfile)
                        {
                            var skillDetail = _cacheHelper.SkillDetail.Where(s => s.SkillName == skill.SkillName).FirstOrDefault();
                            if (skillDetail != null)
                            {
                                var existSkillMapping = await _dbContext.UserSkillMapping.AsNoTracking().
                                          FirstOrDefaultAsync(x => x.UserID == userId && x.SkillDetailID == skillDetail.SkillDetailID);
                                if (existSkillMapping != null)
                                {
                                    existSkillMapping.ExpertiseLevel = skill.ExpertiseLevel;
                                    existSkillMapping.ModifiedTime = DateTime.Now;
                                    _dbContext.UserSkillMapping.UpdateRange(existSkillMapping);
                                    await _dbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    //  return new BadRequestObjectResult(HttpStatusDetail.NoNewSkill);
                                    UserSkillMapping userSkillMapping = new UserSkillMapping
                                    {
                                        SkillDetailID = skillDetail.SkillDetailID,
                                        UserID = userId,
                                        ExpertiseLevel = skill.ExpertiseLevel,
                                        CreatedTime = DateTime.Now
                                    };

                                    _dbContext.UserSkillMapping.Add(userSkillMapping);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                Log.Information("Skill Name not found in the Databse " + skill.SkillName);
                                return new BaseResponse { StatusDescription = HttpStatusDetail.SkillNameNotFound, StatusCode = 400 };
                            }
                        }
                    }
                    else
                    {
                        Log.Information(HttpStatusDetail.ProfileUpdateAllowed);
                        return new BaseResponse { StatusDescription = HttpStatusDetail.ProfileUpdateAllowed, StatusCode = 400 };
                    }
                }
                else
                {
                    Log.Information(HttpStatusDetail.UserNotExistsMessage);
                    return new BaseResponse { StatusDescription = HttpStatusDetail.UserNotExistsMessage, StatusCode = 400 };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in UpdateUserProfile()");
            }
            return new BaseResponse { StatusDescription = "Success", StatusCode = 200 };
        }

        public async Task<List<UserProfileDetail>> GetAllUserProfile(SearchCrteria searchCrteria)
        {
            List<UserProfileDetail> userProfileDetails = await (from user in _dbContext.User.AsNoTracking().
                                                                Where(x => x.Name.Contains(searchCrteria.Name) && x.AssociateID.Contains(searchCrteria.AssociateID))
                                                                select new UserProfileDetail
                                                                {
                                                                    AssociateID = user.AssociateID,
                                                                    Name = user.Name,
                                                                    Email = user.Email,
                                                                    Mobile = user.Mobile,
                                                                    UserID = user.UserID,
                                                                    UserTechnicalSkillDetails = (from skillmap in _dbContext.UserSkillMapping.AsNoTracking().
                                                                                                             Where(x => x.UserID == user.UserID)
                                                                                                 join skill in _dbContext.SkillDetail.AsNoTracking() on skillmap.SkillDetailID equals skill.SkillDetailID
                                                                                                 select new UserTechnicalSkill
                                                                                                 {
                                                                                                     SkillName = skill.SkillName,
                                                                                                     SkillDetailID = skill.SkillDetailID,
                                                                                                     ExpertiseLevel = skillmap.ExpertiseLevel,
                                                                                                     IsTechnical = skill.IsTechnical
                                                                                                 }
                                                                                                            ).Where(x => x.SkillName.Contains(searchCrteria.SkillName) && x.ExpertiseLevel > 10).
                                                                                                                OrderByDescending(x => x.ExpertiseLevel).ToList()

                                                                }).ToListAsync();
            return userProfileDetails;


        }

        public async Task<BaseResponse> AddUserProfile(UserSkill userSkill)
        {
            try
            {
                var existsUserProfile = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.AssociateID == userSkill.AssociateID);
                if (existsUserProfile == null)
                {
                    List<UserSkillMapping> userSkillMappings = new List<UserSkillMapping>();
                    foreach (var skill in userSkill.SkillList)
                    {
                        var skillDetail = await _dbContext.SkillDetail.AsNoTracking().Where(x => x.SkillName == skill.SkillName).FirstOrDefaultAsync();
                        if (skillDetail != null)
                        {
                            userSkillMappings.Add(new UserSkillMapping
                            {
                                SkillDetailID = skillDetail.SkillDetailID,
                                ExpertiseLevel = skill.ExpertiseLevel,
                                CreatedBy = 0,
                                CreatedTime = DateTime.Now
                            });
                        }
                    }

                    User user = new User
                    {
                        AssociateID = userSkill.AssociateID,
                        Email = userSkill.Email,
                        Mobile = userSkill.Mobile,
                        Name = userSkill.Name,
                        CreatedTime = DateTime.Now,
                        CreatedBy = 0,
                        UserSkillMapping = userSkillMappings
                    };

                    _dbContext.User.Add(user);
                    await _dbContext.SaveChangesAsync();
                    return new BaseResponse
                    {
                        StatusDescription = HttpStatusDetail.SuccessMessage,
                        StatusCode = HttpStatusDetail.SuccessCode,
                        ID = user.UserID
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        StatusDescription = string.Format(HttpStatusDetail.AlreadyExistsMessage, userSkill.AssociateID),
                        StatusCode = HttpStatusDetail.SuccessCode,
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error Occured in AddUserProfile()");
                return new BaseResponse
                {
                    StatusDescription = HttpStatusDetail.InternalErrorMessage,
                    StatusCode = HttpStatusDetail.InternalErrorCode,
                };
            }
        }
    }
}

