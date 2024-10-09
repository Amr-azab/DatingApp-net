using System;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
  void Update(AppUser user);
  Task<bool>SaveAllAsync();
  Task<IEnumerable<AppUser>>GetUserAsync();
  Task<AppUser?> GetUserByIdAsync(int id);
  Task<AppUser?> GetUserByUsernameAsync(string username);
  // Task<IEnumerable<MemberDto>> GetMembersAsync();
  Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

  Task<MemberDto?>GetMemberAsync(string username);

}
