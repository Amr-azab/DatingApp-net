using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikeRepository likeRepository) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
     {
      var sourceUserId = User.GetUserId();
      if (sourceUserId == targetUserId) return BadRequest("You cannot like youself");
      var existingLike = await likeRepository.GetUserLike(sourceUserId,targetUserId);
      if (existingLike == null) 
      {
        var like = new UserLike
        {
          SourceUserId = sourceUserId,
          TargetUserId = targetUserId
        };
        likeRepository.AddLike(like);
      }
      else
      {
        likeRepository.DeleteLike(existingLike);
      
      }
      if(await likeRepository.SaveChange()) return Ok();
      return BadRequest("Faild to update like");
     }
     [HttpGet("list")]
     public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
     {
      return Ok(await likeRepository.GetCurrentUserLikeIds(User.GetUserId()));
     }
    //  [HttpGet]
    //  public async Task<ActionResult<IEnumerable<MemberDto>>>GetUserLikes(string predicate)
    //  {
    //   var users = await likeRepository.GetUserLikes(predicate,User.GetUserId());
    //   return Ok(users);
    //  }
    [HttpGet]
     public async Task<ActionResult<IEnumerable<MemberDto>>>GetUserLikes([FromQuery]LikesParams likesParams)
     {
      likesParams.UserId = User.GetUserId();
      var users = await likeRepository.GetUserLikes(likesParams);
      Response.AddPaginationHeader(users);
      return Ok(users);
     }

}
